// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryExtensions.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   Extends dictionary classes with XML
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Extensions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Portal.Main.Helper.Reflection;
    using Portal.Main.Helper.Xml;

    /// <summary>
    ///     Extends dictionary classes with XML
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Serializes the dictionary to an XML string
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="root">
        /// The root.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToXml(this IDictionary items, string root = "root")
        {
            var rootNode = new XElement(root);

            foreach (DictionaryEntry item in items)
            {
                var xmlType = XmlUtils.MapTypeToXmlType(item.Value.GetType());

                // if it's a simple type use it
                if (!string.IsNullOrEmpty(xmlType))
                {
                    var typeAttr = new XAttribute("type", xmlType);
                    if (item.Key != null)
                    {
                        rootNode.Add(new XElement((string)item.Key, typeAttr, item.Value));
                    }
                }
                else
                {
                    // complex type use serialization
                    string xmlString;
                    if (SerializationUtils.SerializeObject(item.Value, out xmlString))
                    {
                        var el = XElement.Parse(xmlString);

                        if (item.Key != null)
                        {
                            rootNode.Add(
                                new XElement(
                                    (string)item.Key,
                                    new XAttribute("type", "___" + item.Value.GetType().FullName),
                                    el));
                        }
                    }
                }
            }

            return rootNode.ToString();
        }

        //public static void ToKeyValuePairList(this Dictionary<string, string> items)
        //{
        //    //List<KeyValuePair<string, string>>
        //    return items.Select(r => new KeyValuePair<string, string>(r.Key, r.Value.TitleAr)).ToList();
        //}
    

        /// <summary>
        /// Loads the dictionary from an Xml string
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="xml">
        /// The XML.
        /// </param>
        public static void FromXml(this IDictionary items, string xml)
        {
            items.Clear();

            var root = XElement.Parse(xml);

            foreach (var el in root.Elements())
            {
                string typeString = null;

                var typeAttr = el.Attribute("type");
                if (typeAttr != null)
                {
                    typeString = typeAttr.Value;
                }

                var val = el.Value;

                if (!string.IsNullOrEmpty(typeString) && typeString != "string" && !typeString.StartsWith("__"))
                {
                    // Simple type we know how to convert
                    var type = XmlUtils.MapXmlTypeToType(typeString);
                    items.Add(el.Name.LocalName, type != null ? ReflectionUtils.StringToTypedValue(val, type) : val);
                }
                else if (typeString != null && typeString.StartsWith("___"))
                {
                    var type = ReflectionUtils.GetTypeFromName(typeString.Substring(3));
                    var serializationUtilsDeSerializeObject =
                        SerializationUtils.DeSerializeObject(el.Elements().First().CreateReader(), type);
                    items.Add(el.Name.LocalName, serializationUtilsDeSerializeObject);
                }
                else
                {
                    // it's a string or unknown type
                    items.Add(el.Name.LocalName, val);
                }
            }
        }
    }
}