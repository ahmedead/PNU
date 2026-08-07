// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionUtils.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   Reflection Utilities
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Reflection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Reflection Utilities
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        ///     Binding Flags constant to be reused for all Reflection access methods.
        /// </summary>
        public const BindingFlags MemberAccess =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance
            | BindingFlags.IgnoreCase;

        /// <summary>
        /// Gets the System.Type object with the specified name from assembly file
        /// </summary>
        /// <param name="assemblyFile">
        /// The name or path of the file that contains the manifest of the assembly
        /// </param>
        /// <param name="typeName">
        /// The full name of the type
        /// </param>
        /// <returns>
        /// A System.Type object that represents the specified class
        /// </returns>
        public static Type GetType(string assemblyFile, string typeName)
        {
            if (string.IsNullOrEmpty(assemblyFile))
            {
                throw new ArgumentNullException(nameof(assemblyFile));
            }

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException(nameof(typeName));
            }

            if (!File.Exists(assemblyFile))
            {
                throw new ArgumentException("The file does not exist.", assemblyFile);
            }

            var assembly = Assembly.LoadFrom(assemblyFile);
            return assembly.GetType(typeName);
        }

        /// <summary>
        /// Retrieve a property value from an object dynamically. This is a simple version
        ///     that uses Reflection calls directly. It doesn't support indexers.
        /// </summary>
        /// <param name="instance">
        /// Object to make the call on
        /// </param>
        /// <param name="property">
        /// Property to retrieve
        /// </param>
        /// <returns>
        /// Object - cast to proper type
        /// </returns>
        public static object GetProperty(object instance, string property)
        {
            return instance.GetType().GetProperty(property, MemberAccess).GetValue(instance, null);
        }

        /// <summary>
        /// Retrieve a field dynamically from an object. This is a simple implementation that's
        ///     straight Reflection and doesn't support indexers.
        /// </summary>
        /// <param name="Object">
        /// Object to retreve Field from
        /// </param>
        /// <param name="property">
        /// name of the field to retrieve
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="TargetException">
        /// In the .NET for Windows Store apps or the Portable Class Library, catch
        ///     <see cref="T:System.Exception"/> instead.The field is non-static and <paramref name="obj"/> is null.
        /// </exception>
        /// <exception cref="FieldAccessException">
        /// In the .NET for Windows Store apps or the Portable Class Library, catch the base
        ///     class exception, <see cref="T:System.MemberAccessException"/>, instead.The caller does not have permission to
        ///     access this field.
        /// </exception>
        public static object GetField(object obj, string property)
        {
            var fieldInfo = obj.GetType().GetField(property, MemberAccess);
            return fieldInfo?.GetValue(obj);
        }

        /// <summary>
        /// Parses Properties and Fields including Array and Collection references.
        ///     Used internally for the 'Ex' Reflection methods.
        /// </summary>
        /// <param name="parent">
        /// </param>
        /// <param name="property">
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        private static object GetPropertyInternal(object parent, string property)
        {
            if (property == "this" || property == "me")
            {
                return parent;
            }

            var pureProperty = property;
            string indexes = null;
            var isArrayOrCollection = false;

            // Deal with Array Property
            if (property.IndexOf("[", StringComparison.Ordinal) > -1)
            {
                pureProperty = property.Substring(0, property.IndexOf("[", StringComparison.Ordinal));
                indexes = property.Substring(property.IndexOf("[", StringComparison.Ordinal));
                isArrayOrCollection = true;
            }

            // Get the member
            var member = parent.GetType().GetMember(pureProperty, MemberAccess)[0];
            var result = member.MemberType == MemberTypes.Property
                             ? ((PropertyInfo)member).GetValue(parent, null)
                             : ((FieldInfo)member).GetValue(parent);

            if (isArrayOrCollection)
            {
                indexes = indexes.Replace("[", string.Empty).Replace("]", string.Empty);

                if (result is Array)
                {
                    int index;
                    int.TryParse(indexes, out index);
                    result = CallMethod(result, "GetValue", index);
                }
                else if (result is ICollection)
                {
                    if (indexes.StartsWith("\""))
                    {
                        // String Index
                        indexes = indexes.Trim('\"');
                        result = CallMethod(result, "get_Item", indexes);
                    }
                    else
                    {
                        // assume numeric index
                        int index;
                        int.TryParse(indexes, out index);
                        result = CallMethod(result, "get_Item", index);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parses Properties and Fields including Array and Collection references.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        private static object SetPropertyInternal(object parent, string property, object value)
        {
            if (property == "this" || property == "me")
            {
                return parent;
            }

            object result;
            var pureProperty = property;
            string indexes = null;
            var isArrayOrCollection = false;

            // Deal with Array Property
            if (property.IndexOf("[", StringComparison.Ordinal) > -1)
            {
                pureProperty = property.Substring(0, property.IndexOf("[", StringComparison.Ordinal));
                indexes = property.Substring(property.IndexOf("[", StringComparison.Ordinal));
                isArrayOrCollection = true;
            }

            if (!isArrayOrCollection)
            {
                // Get the member
                var member = parent.GetType().GetMember(pureProperty, MemberAccess)[0];
                if (member.MemberType == MemberTypes.Property)
                {
                    ((PropertyInfo)member).SetValue(parent, value, null);
                }
                else
                {
                    ((FieldInfo)member).SetValue(parent, value);
                }

                return null;
            }
            else
            {
                // Get the member
                var member = parent.GetType().GetMember(pureProperty, MemberAccess)[0];
                result = member.MemberType == MemberTypes.Property
                             ? ((PropertyInfo)member).GetValue(parent, null)
                             : ((FieldInfo)member).GetValue(parent);
            }

            indexes = indexes.Replace("[", string.Empty).Replace("]", string.Empty);

            if (result is Array)
            {
                int index;
                int.TryParse(indexes, out index);
                result = CallMethod(result, "SetValue", value, index);
            }
            else if (result is ICollection)
            {
                if (indexes.StartsWith("\""))
                {
                    // String Index
                    indexes = indexes.Trim('\"');
                    result = CallMethod(result, "set_Item", indexes, value);
                }
                else
                {
                    // assume numeric index
                    int index;
                    int.TryParse(indexes, out index);
                    result = CallMethod(result, "set_Item", index, value);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a property or field value using a base object and sub members including . syntax.
        ///     For example, you can access: oCustomer.oData.Company with (this,"oCustomer.oData.Company")
        ///     This method also supports indexers in the Property value such as:
        ///     Customer.DataSet.Tables["Customers"].Rows[0]
        /// </summary>
        /// <param name="parent">
        /// Parent object to 'start' parsing from. Typically this will be the Page.
        /// </param>
        /// <param name="property">
        /// The property to retrieve. Example: 'Customer.Entity.Company'
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object GetPropertyEx(object parent, string property)
        {
            var type = parent.GetType();

            var at = property.IndexOf(".", StringComparison.Ordinal);
            if (at < 0)
            {
                // Complex parse of the property    
                return GetPropertyInternal(parent, property);
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            var main = property.Substring(0, at);
            var subs = property.Substring(at + 1);

            // Retrieve the next . section of the property
            var sub = GetPropertyInternal(parent, main);

            // Now go parse the left over sections
            return GetPropertyEx(sub, subs);
        }

        /// <summary>
        /// Gets the property information ex.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The <see cref="PropertyInfo"/>.
        /// </returns>
        public static PropertyInfo GetPropertyInfoEx(object parent, string property)
        {
            var type = parent.GetType();

            var at = property.IndexOf(".", StringComparison.Ordinal);
            if (at < 0)
            {
                // Complex parse of the property    
                return GetPropertyInfoInternal(parent, property);
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            var main = property.Substring(0, at);
            var subs = property.Substring(at + 1);

            // Retrieve the next . section of the property
            var sub = GetPropertyInternal(parent, main);

            // Now go parse the left over sections
            return GetPropertyInfoEx(sub, subs);
        }

        /// <summary>
        /// Returns a PropertyInfo structure from an extended Property reference
        /// </summary>
        /// <param name="parent">
        /// </param>
        /// <param name="property">
        /// </param>
        /// <returns>
        /// The <see cref="PropertyInfo"/>.
        /// </returns>
        public static PropertyInfo GetPropertyInfoInternal(object parent, string property)
        {
            if (property == "this" || property == "me")
            {
                return null;
            }

            var propertyName = property;

            // Deal with Array Property - strip off array indexer
            if (property.IndexOf("[", StringComparison.Ordinal) > -1)
            {
                propertyName = property.Substring(0, property.IndexOf("[", StringComparison.Ordinal));
            }

            // Get the member
            return parent.GetType().GetProperty(propertyName, MemberAccess);
        }

        /// <summary>
        /// Sets the property on an object. This is a simple method that uses straight Reflection
        ///     and doesn't support indexers.
        /// </summary>
        /// <param name="obj">
        /// Object to set property on
        /// </param>
        /// <param name="property">
        /// Name of the property to set
        /// </param>
        /// <param name="value">
        /// value to set it to
        /// </param>
        public static void SetProperty(object obj, string property, object value)
        {
            obj.GetType().GetProperty(property, MemberAccess).SetValue(obj, value, null);
        }

        /// <summary>
        /// Sets the field on an object. This is a simple method that uses straight Reflection
        ///     and doesn't support indexers.
        /// </summary>
        /// <param name="obj">
        /// Object to set property on
        /// </param>
        /// <param name="property">
        /// Name of the field to set
        /// </param>
        /// <param name="value">
        /// value to set it to
        /// </param>
        public static void SetField(object obj, string property, object value)
        {
            var fieldInfo = obj.GetType().GetField(property, MemberAccess);
            fieldInfo?.SetValue(obj, value);
        }

        /// <summary>
        /// Sets a value on an object. Supports . syntax for named properties
        ///     (ie. Customer.Entity.Company) as well as indexers.
        /// </summary>
        /// <param name="parent">
        /// Object to set the property on.
        /// </param>
        /// <param name="property">
        /// Property to set. Can be an object hierarchy with . syntax and can
        ///     include indexers. Examples: Customer.Entity.Company,
        ///     Customer.DataSet.Tables["Customers"].Rows[0]
        /// </param>
        /// <param name="value">
        /// Value to set the property to
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object SetPropertyEx(object parent, string property, object value)
        {
            var type = parent.GetType();

            // no more .s - we got our final object
            var lnAt = property.IndexOf(".", StringComparison.Ordinal);
            if (lnAt < 0)
            {
                SetPropertyInternal(parent, property, value);
                return null;
            }

            // Walk the . syntax
            var main = property.Substring(0, lnAt);
            var subs = property.Substring(lnAt + 1);

            var sub = GetPropertyInternal(parent, main);

            SetPropertyEx(sub, subs, value);

            return null;
        }

        /// <summary>
        /// Calls a method on an object dynamically. This version requires explicit
        ///     specification of the parameter type signatures.
        /// </summary>
        /// <param name="instance">
        /// Instance of object to call method on
        /// </param>
        /// <param name="method">
        /// The method to call as a stringToTypedValue
        /// </param>
        /// <param name="parameterTypes">
        /// Specify each of the types for each parameter passed.
        ///     You can also pass null, but you may get errors for ambiguous methods signatures
        ///     when null parameters are passed
        /// </param>
        /// <param name="parms">
        /// any variable number of parameters.
        /// </param>
        /// <returns>
        /// object
        /// </returns>
        public static object CallMethod(object instance, string method, Type[] parameterTypes, params object[] parms)
        {
            if (parameterTypes == null && parms.Length > 0)
            {
                // Call without explicit parameter types - might cause problems with overloads    
                // occurs when null parameters were passed and we couldn't figure out the parm type
                return instance.GetType()
                    .GetMethod(method, MemberAccess | BindingFlags.InvokeMethod)
                    .Invoke(instance, parms);
            }

            return
                instance.GetType()
                    .GetMethod(method, MemberAccess | BindingFlags.InvokeMethod, null, parameterTypes, null)
                    .Invoke(instance, parms);
        }

        /// <summary>
        /// Calls a method on an object dynamically.
        ///     This version doesn't require specific parameter signatures to be passed.
        ///     Instead parameter types are inferred based on types passed. Note that if
        ///     you pass a null parameter, type inferrance cannot occur and if overloads
        ///     exist the call may fail. if so use the more detailed overload of this method.
        /// </summary>
        /// <param name="instance">
        /// Instance of object to call method on
        /// </param>
        /// <param name="method">
        /// The method to call as a stringToTypedValue
        /// </param>
        /// <param name="parms">
        /// any variable number of parameters.
        /// </param>
        /// <returns>
        /// object
        /// </returns>
        public static object CallMethod(object instance, string method, params object[] parms)
        {
            // Pick up parameter types so we can match the method properly
            Type[] parameterTypes = null;
            if (parms != null)
            {
                parameterTypes = new Type[parms.Length];
                for (var x = 0; x < parms.Length; x++)
                {
                    // if we have null parameters we can't determine parameter types - exit
                    if (parms[x] == null)
                    {
                        parameterTypes = null; // clear out - don't use types        
                        break;
                    }

                    parameterTypes[x] = parms[x].GetType();
                }
            }

            return CallMethod(instance, method, parameterTypes, parms);
        }

        /// <summary>
        /// Calls a method on an object with extended . syntax (object: this Method: Entity.CalculateOrderTotal)
        /// </summary>
        /// <param name="parent">
        /// </param>
        /// <param name="method">
        /// </param>
        /// <param name="parms">
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object CallMethodEx(object parent, string method, params object[] parms)
        {
            var type = parent.GetType();

            // no more .s - we got our final object
            var lnAt = method.IndexOf(".", StringComparison.Ordinal);
            if (lnAt < 0)
            {
                return CallMethod(parent, method, parms);
            }

            // Walk the . syntax
            var main = method.Substring(0, lnAt);
            var subs = method.Substring(lnAt + 1);

            var sub = GetPropertyInternal(parent, main);

            // Recurse until we get the lowest ref
            return CallMethodEx(sub, subs, parms);
        }

        /// <summary>
        /// Creates an instance from a type by calling the parameterless constructor.
        ///     Note this will not work with COM objects - continue to use the Activator.CreateInstance
        ///     for COM objects.
        ///     <seealso>Class wwUtils</seealso>
        /// </summary>
        /// <param name="typeToCreate">
        /// The type from which to create an instance.
        /// </param>
        /// <param name="args">
        /// </param>
        /// <returns>
        /// object
        /// </returns>
        public static object CreateInstanceFromType(Type typeToCreate, params object[] args)
        {
            if (args == null)
            {
                var parms = Type.EmptyTypes;
                var constructorInfo = typeToCreate.GetConstructor(parms);
                if (constructorInfo != null)
                {
                    return constructorInfo.Invoke(null);
                }
            }

            return Activator.CreateInstance(typeToCreate, args);
        }

        /// <summary>
        /// Creates an instance of a type based on a string. Assumes that the type's
        /// </summary>
        /// <param name="typeName">
        /// </param>
        /// <param name="args">
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object CreateInstanceFromString(string typeName, params object[] args)
        {
            object instance;

            try
            {
                var type = GetTypeFromName(typeName);
                if (type == null)
                {
                    return null;
                }

                instance = Activator.CreateInstance(type, args);
            }
            catch
            {
                return null;
            }

            return instance;
        }

        /// <summary>
        /// Helper routine that looks up a type name and tries to retrieve the
        ///     full type reference in the actively executing assemblies.
        /// </summary>
        /// <param name="typeName">
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public static Type GetTypeFromName(string typeName)
        {
            var type = Type.GetType(typeName, false);
            if (type != null)
            {
                return type;
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // try to find manually
            foreach (var asm in assemblies)
            {
                type = asm.GetType(typeName, false);

                if (type != null)
                {
                    break;
                }
            }

            return type;
        }

        /// <summary>
        /// Creates a COM instance from a ProgID. Loads either
        ///     Exe or DLL servers.
        /// </summary>
        /// <param name="progId">
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object CreateComInstance(string progId)
        {
            var type = Type.GetTypeFromProgID(progId);
            if (type == null)
            {
                return null;
            }

            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Converts a type to string if possible. This method supports an optional culture generically on any value.
        ///     It calls the ToString() method on common types and uses a type converter on all other objects
        ///     if available
        /// </summary>
        /// <param name="rawValue">
        /// The Value or Object to convert to a string
        /// </param>
        /// <param name="culture">
        /// Culture for numeric and DateTime values
        /// </param>
        /// <param name="unsupportedReturn">
        /// Return string for unsupported types
        /// </param>
        /// <returns>
        /// string
        /// </returns>
        public static string TypedValueToString(
            object rawValue, 
            CultureInfo culture = null, 
            string unsupportedReturn = null)
        {
            if (rawValue == null)
            {
                return string.Empty;
            }

            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }

            var valueType = rawValue.GetType();
            string returnValue;

            if (valueType == typeof(string))
            {
                returnValue = rawValue as string;
            }
            else if (valueType == typeof(int) || valueType == typeof(decimal) || valueType == typeof(double)
                     || valueType == typeof(float) || valueType == typeof(float))
            {
                returnValue = string.Format(culture.NumberFormat, "{0}", rawValue);
            }
            else if (valueType == typeof(DateTime))
            {
                returnValue = string.Format(culture.DateTimeFormat, "{0}", rawValue);
            }
            else if (valueType == typeof(bool) || valueType == typeof(byte) || valueType.IsEnum)
            {
                returnValue = rawValue.ToString();
            }
            else if (valueType == typeof(Guid?))
            {
                return rawValue.ToString();
            }
            else
            {
                // Any type that supports a type converter
                var converter = TypeDescriptor.GetConverter(valueType);
                if (converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    returnValue = converter.ConvertToString(null, culture, rawValue);
                }
                else
                {
                    // Last resort - just call ToString() on unknown type
                    returnValue = !string.IsNullOrEmpty(unsupportedReturn) ? unsupportedReturn : rawValue.ToString();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Turns a string into a typed value generically.
        ///     Explicitly assigns common types and falls back
        ///     on using type converters for unhandled types.
        ///     Common uses:
        ///     * UI -&gt; to data conversions
        ///     * Parsers
        ///     <seealso>Class ReflectionUtils</seealso>
        /// </summary>
        /// <param name="sourceString">
        /// The string to convert from
        /// </param>
        /// <param name="targetType">
        /// The type to convert to
        /// </param>
        /// <param name="culture">
        /// Culture used for numeric and datetime values.
        /// </param>
        /// <returns>
        /// object. Throws exception if it cannot be converted.
        /// </returns>
        public static object StringToTypedValue(string sourceString, Type targetType, CultureInfo culture = null)
        {
            object result = null;

            var isEmpty = string.IsNullOrEmpty(sourceString);

            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }

            if (targetType == typeof(string))
            {
                result = sourceString;
            }
            else if (targetType == typeof(int) || targetType == typeof(int))
            {
                result = isEmpty ? 0 : int.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(long))
            {
                if (isEmpty)
                {
                    result = (long)0;
                }
                else
                {
                    result = long.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
                }
            }
            else if (targetType == typeof(short))
            {
                if (isEmpty)
                {
                    result = (short)0;
                }
                else
                {
                    result = short.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
                }
            }
            else if (targetType == typeof(decimal))
            {
                result = isEmpty ? 0M : decimal.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(DateTime))
            {
                result = isEmpty ? DateTime.MinValue : Convert.ToDateTime(sourceString, culture.DateTimeFormat);
            }
            else if (targetType == typeof(byte))
            {
                result = isEmpty ? 0 : Convert.ToByte(sourceString);
            }
            else if (targetType == typeof(double))
            {
                result = isEmpty ? 0F : double.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(float))
            {
                result = isEmpty ? 0F : float.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(bool))
            {
                if (sourceString != null
                    && (!isEmpty && sourceString.ToLower() == "true" || sourceString.ToLower() == "on"
                        || sourceString == "1"))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else if (targetType == typeof(Guid))
            {
                result = isEmpty ? Guid.Empty : new Guid(sourceString);
            }
            else if (targetType.IsEnum)
            {
                if (sourceString != null)
                {
                    result = Enum.Parse(targetType, sourceString);
                }
            }
            else if (targetType == typeof(byte[]))
            {
                // TODO: Convert HexBinary string to byte array
            }

            // Handle nullables explicitly since type converter won't handle conversions
            // properly for things like decimal separators currency formats etc.
            // Grab underlying type and pass value to that
            else if (targetType.Name.StartsWith("Nullable`"))
            {
                if (sourceString != null && (sourceString.ToLower() == "null" || sourceString == string.Empty))
                {
                }
                else
                {
                    targetType = Nullable.GetUnderlyingType(targetType);
                    result = StringToTypedValue(sourceString, targetType);
                }
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(targetType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    result = converter.ConvertFromString(null, culture, sourceString);
                }
                else
                {
                    Debug.Assert(
                        false, 
                        $"Type Conversion not handled in StringToTypedValue for {targetType.Name} {sourceString}");

                    // throw (new InvalidCastException("StringToTypedValueValueTypeConversionFailed" + targetType.Name));
                }
            }

            return result;
        }

        /// <summary>
        /// Generic version allow for automatic type conversion without the explicit type
        ///     parameter
        /// </summary>
        /// <typeparam name="T">
        /// Type to be converted to
        /// </typeparam>
        /// <param name="sourceString">
        /// input string value to be converted
        /// </param>
        /// <param name="culture">
        /// Culture applied to conversion
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T StringToTypedValue<T>(string sourceString, CultureInfo culture = null)
        {
            return (T)StringToTypedValue(sourceString, typeof(T), culture);
        }

        /// <summary>
        /// Returns a List of KeyValuePair object
        /// </summary>
        /// <param name="enumType">
        /// Type of the enum.
        /// </param>
        /// <param name="valueAsFieldValueNumber">
        /// if set to <c>true</c> [value as field value number].
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<KeyValuePair<string, string>> GetEnumList(
            Type enumType, 
            bool valueAsFieldValueNumber = false)
        {
            // string[] enumStrings = Enum.GetNames(enumType);
            var enumValues = Enum.GetValues(enumType);

            return (from object enumValue in enumValues
                    let strValue = enumValue.ToString()
                    select
                        !valueAsFieldValueNumber
                            ? new KeyValuePair<string, string>(enumValue.ToString(), strValue)
                            : new KeyValuePair<string, string>(((int)enumValue).ToString(), strValue)).ToList();
        }

        /// <summary>
        /// Retrieves a value from  a static property by specifying a type full name and property
        /// </summary>
        /// <param name="typeName">
        /// Full type name (namespace.class)
        /// </param>
        /// <param name="property">
        /// Property to get value from
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object GetStaticProperty(string typeName, string property)
        {
            var type = GetTypeFromName(typeName);
            if (type == null)
            {
                return null;
            }

            return GetStaticProperty(type, property);
        }

        /// <summary>
        /// Returns a static property from a given type
        /// </summary>
        /// <param name="type">
        /// Type instance for the static property
        /// </param>
        /// <param name="property">
        /// Property name as a string
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object GetStaticProperty(Type type, string property)
        {
            object result;
            try
            {
                result = type.InvokeMember(
                    property, 
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty, 
                    null, 
                    type, 
                    null);
            }
            catch
            {
                return null;
            }

            return result;
        }

        #region COM Reflection Routines

        /// <summary>
        /// Retrieve a dynamic 'non-typelib' property
        /// </summary>
        /// <param name="instance">
        /// Object to make the call on
        /// </param>
        /// <param name="property">
        /// Property to retrieve
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object GetPropertyCom(object instance, string property)
        {
            return instance.GetType()
                .InvokeMember(
                    property, 
                    MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, 
                    null, 
                    instance, 
                    null);
        }

        /// <summary>
        /// Returns a property or field value using a base object and sub members including . syntax.
        ///     For example, you can access: oCustomer.oData.Company with (this,"oCustomer.oData.Company")
        /// </summary>
        /// <param name="parent">
        /// Parent object to 'start' parsing from.
        /// </param>
        /// <param name="property">
        /// The property to retrieve. Example: 'oBus.oData.Company'
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object GetPropertyExCom(object parent, string property)
        {
            var type = parent.GetType();

            var lnAt = property.IndexOf(".", StringComparison.Ordinal);
            if (lnAt < 0)
            {
                if (property == "this" || property == "me")
                {
                    return parent;
                }

                // Get the member
                return parent.GetType()
                    .InvokeMember(
                        property, 
                        MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, 
                        null, 
                        parent, 
                        null);
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            var main = property.Substring(0, lnAt);
            var subs = property.Substring(lnAt + 1);

            var sub = parent.GetType()
                .InvokeMember(main, MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null, parent, null);

            // Recurse further into the sub-properties (Subs)
            return GetPropertyExCom(sub, subs);
        }

        /// <summary>
        /// Sets the property on an object.
        /// </summary>
        /// <param name="Object">
        /// Object to set property on
        /// </param>
        /// <param name="property">
        /// Name of the property to set
        /// </param>
        /// <param name="value">
        /// value to set it to
        /// </param>
        /// <exception cref="AmbiguousMatchException">More than one method matches the binding criteria. </exception>
        /// <exception cref="TargetException">The specified member cannot be invoked on <paramref name="target" />. </exception>
        /// <exception cref="MissingMethodException">No method can be found that matches the arguments in <paramref name="args" />.-or- The current <see cref="T:System.Type" /> object represents a type that contains open type parameters, that is, <see cref="P:System.Type.ContainsGenericParameters" /> returns true. </exception>
        /// <exception cref="MissingFieldException">The field or property cannot be found. </exception>
        /// <exception cref="MethodAccessException">The specified member is a class initializer. </exception>
        public static void SetPropertyCom(object obj, string property, object value)
        {
            obj.GetType()
                .InvokeMember(
                    property, 
                    MemberAccess | BindingFlags.SetProperty | BindingFlags.SetField, 
                    null, 
                    obj, 
                    new[] { value });
        }

        /// <summary>
        /// Sets the value of a field or property via Reflection. This method alws
        ///     for using '.' syntax to specify objects multiple levels down.
        ///     ReflectionUtils.SetPropertyEx(this,"Invoice.LineItemsCount",10)
        ///     which would be equivalent of:
        ///     Invoice.LineItemsCount = 10;
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object SetPropertyExCom(object parent, string property, object value)
        {
            var type = parent.GetType();

            var lnAt = property.IndexOf(".", StringComparison.Ordinal);
            if (lnAt < 0)
            {
                // Set the member
                parent.GetType()
                    .InvokeMember(
                        property, 
                        MemberAccess | BindingFlags.SetProperty | BindingFlags.SetField, 
                        null, 
                        parent, 
                        new[] { value });

                return null;
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            var main = property.Substring(0, lnAt);
            var subs = property.Substring(lnAt + 1);

            var sub = parent.GetType()
                .InvokeMember(main, MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null, parent, null);

            return SetPropertyExCom(sub, subs, value);
        }

        /// <summary>
        /// Wrapper method to call a 'dynamic' (non-typelib) method
        ///     on a COM object
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="parms">
        /// The parms.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// 1st - Method name, 2nd - 1st parameter, 3rd - 2nd parm etc.
        public static object CallMethodCom(object instance, string method, params object[] parms)
        {
            return instance.GetType()
                .InvokeMember(method, MemberAccess | BindingFlags.InvokeMethod, null, instance, parms);
        }

        /// <summary>
        /// Calls a method on a COM object with '.' syntax (Customer instance and Address.DoSomeThing method)
        /// </summary>
        /// <param name="parent">
        /// the object instance on which to call method
        /// </param>
        /// <param name="method">
        /// The method or . syntax path to the method (Address.Parse)
        /// </param>
        /// <param name="parms">
        /// Any number of parameters
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object CallMethodExCom(object parent, string method, params object[] parms)
        {
            var type = parent.GetType();

            // no more .s - we got our final object
            var at = method.IndexOf(".", StringComparison.Ordinal);
            if (at < 0)
            {
                return CallMethod(parent, method, parms);
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            var main = method.Substring(0, at);
            var subs = method.Substring(at + 1);

            var sub = parent.GetType()
                .InvokeMember(main, MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null, parent, null);

            // Recurse until we get the lowest ref
            return CallMethodEx(sub, subs, parms);
        }

        #endregion
    }
}