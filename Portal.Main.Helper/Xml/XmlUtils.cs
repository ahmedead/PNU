// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlUtils.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   String utility class that provides a host of string related operations
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Xml
{
    using System;
    using System.Xml;

    /// <summary>
    ///     String utility class that provides a host of string related operations
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// Retrieves a result string from an XPATH query. Null if not found.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="xPath">
        /// The x path.
        /// </param>
        /// <param name="ns">
        /// The ns.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetXmlString(XmlNode node, string xPath, XmlNamespaceManager ns)
        {
            var selNode = node.SelectSingleNode(xPath, ns);

            return selNode?.InnerText;
        }

        /// <summary>
        /// Retrieves a result int value from an XPATH query. 0 if not found.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="xPath">
        /// The x path.
        /// </param>
        /// <param name="ns">
        /// The ns.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetXmlInt(XmlNode node, string xPath, XmlNamespaceManager ns)
        {
            var val = GetXmlString(node, xPath, ns);
            if (val == null)
            {
                return 0;
            }

            int result;
            int.TryParse(val, out result);

            return result;
        }

        /// <summary>
        /// Retrieves a result bool from an XPATH query. false if not found.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="xPath">
        /// The x path.
        /// </param>
        /// <param name="ns">
        /// The ns.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GetXmlBool(XmlNode node, string xPath, XmlNamespaceManager ns)
        {
            var val = GetXmlString(node, xPath, ns);
            if (val == null)
            {
                return false;
            }

            if (val == "1" || val == "true" || val == "True")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieves a result DateTime from an XPATH query. 1/1/1900  if not found.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="xPath">
        /// The x path.
        /// </param>
        /// <param name="ns">
        /// The ns.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetXmlDateTime(XmlNode node, string xPath, XmlNamespaceManager ns)
        {
            var dtVal = new DateTime(1900, 1, 1, 0, 0, 0);

            var val = GetXmlString(node, xPath, ns);
            if (val == null)
            {
                return dtVal;
            }

            try
            {
                dtVal = XmlConvert.ToDateTime(val, XmlDateTimeSerializationMode.Utc);
            }
            catch
            {
                // ignored
            }

            return dtVal;
        }

        /// <summary>
        /// Gets an attribute by name
        /// </summary>
        /// <param name="node">
        /// </param>
        /// <param name="attributeName">
        /// </param>
        /// <returns>
        /// value or null if not available
        /// </returns>
        public static string GetXmlAttributeString(XmlNode node, string attributeName)
        {
            var att = node.Attributes?[attributeName];

            return att?.InnerText;
        }

        /// <summary>
        /// Returns an integer value from an attribute
        /// </summary>
        /// <param name="node">
        /// </param>
        /// <param name="attributeName">
        /// </param>
        /// <param name="defaultValue">
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetXmlAttributeInt(XmlNode node, string attributeName, int defaultValue)
        {
            var val = GetXmlAttributeString(node, attributeName);
            if (val == null)
            {
                return defaultValue;
            }

            return XmlConvert.ToInt32(val);
        }

        /// <summary>
        /// Converts a .NET type into an XML compatible type - roughly
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string MapTypeToXmlType(Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (type == typeof(string) || type == typeof(char))
            {
                return "string";
            }

            if (type == typeof(int) || type == typeof(int))
            {
                return "integer";
            }

            if (type == typeof(short) || type == typeof(byte))
            {
                return "short";
            }

            if (type == typeof(long) || type == typeof(long))
            {
                return "long";
            }

            if (type == typeof(bool))
            {
                return "boolean";
            }

            if (type == typeof(DateTime))
            {
                return "datetime";
            }

            if (type == typeof(float))
            {
                return "float";
            }

            if (type == typeof(decimal))
            {
                return "decimal";
            }

            if (type == typeof(double))
            {
                return "double";
            }

            if (type == typeof(float))
            {
                return "single";
            }

            if (type == typeof(byte))
            {
                return "byte";
            }

            if (type == typeof(byte[]))
            {
                return "base64Binary";
            }

            return null;

            // *** hope for the best
            // return type.ToString().ToLower();
        }

        /// <summary>
        /// The map xml type to type.
        /// </summary>
        /// <param name="xmlType">
        /// The xml type.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public static Type MapXmlTypeToType(string xmlType)
        {
            xmlType = xmlType.ToLower();

            if (xmlType == "string")
            {
                return typeof(string);
            }

            if (xmlType == "integer")
            {
                return typeof(int);
            }

            if (xmlType == "long")
            {
                return typeof(long);
            }

            if (xmlType == "boolean")
            {
                return typeof(bool);
            }

            if (xmlType == "datetime")
            {
                return typeof(DateTime);
            }

            if (xmlType == "float")
            {
                return typeof(float);
            }

            if (xmlType == "decimal")
            {
                return typeof(decimal);
            }

            if (xmlType == "double")
            {
                return typeof(double);
            }

            if (xmlType == "single")
            {
                return typeof(float);
            }

            if (xmlType == "byte")
            {
                return typeof(byte);
            }

            if (xmlType == "base64binary")
            {
                return typeof(byte[]);
            }

            // return null if no match is found
            // don't throw so the caller can decide more efficiently what to do 
            // with this error result
            return null;
        }

        /// <summary>
        /// Creates an Xml NamespaceManager for an XML document by looking
        ///     at all of the namespaces defined on the document root element.
        /// </summary>
        /// <param name="doc">
        /// The XmlDom instance to attach the namespacemanager to
        /// </param>
        /// <param name="defaultNamespace">
        /// The prefix to use for prefix-less nodes (which are not supported if any namespaces are
        ///     used in XmlDoc).
        /// </param>
        /// <returns>
        /// The <see cref="XmlNamespaceManager"/>.
        /// </returns>
        public static XmlNamespaceManager CreateXmlNamespaceManager(XmlDocument doc, string defaultNamespace)
        {
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            if (doc.DocumentElement != null)
            {
                foreach (XmlAttribute attr in doc.DocumentElement.Attributes)
                {
                    if (attr.Prefix == "xmlns")
                    {
                        nsmgr.AddNamespace(attr.LocalName, attr.Value);
                    }

                    if (attr.Name == "xmlns")
                    {
                        // default namespace MUST use a prefix
                        nsmgr.AddNamespace(defaultNamespace, attr.Value);
                    }
                }
            }

            return nsmgr;
        }
    }
}