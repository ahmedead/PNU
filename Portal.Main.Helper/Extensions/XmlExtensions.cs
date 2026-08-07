// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlExtensions.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   Xml Extensions
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Extensions
{
    using System.Linq;
    using System.Xml;

    /// <summary>
    ///     Xml Extensions
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Appends a child to a XML node
        /// </summary>
        /// <param name="source">
        /// The parent node
        /// </param>
        /// <param name="childNode">
        /// The name of the child node
        /// </param>
        /// <returns>
        /// The newly created XML node
        /// </returns>
        public static XmlNode CreateChildNode(this XmlNode source, string childNode)
        {
            var xmlDocument = source as XmlDocument;
            var document = xmlDocument ?? source.OwnerDocument;
            if (document != null)
            {
                XmlNode node = document.CreateElement(childNode);
                source.AppendChild(node);
                return node;
            }

            return null;
        }

        /// <summary>
        /// Appends a child to a XML node
        /// </summary>
        /// <param name="source">
        /// The parent node
        /// </param>
        /// <param name="childNode">
        /// The name of the child node
        /// </param>
        /// <param name="namespaceUri">
        /// The node namespace
        /// </param>
        /// <returns>
        /// The newly cerated XML node
        /// </returns>
        public static XmlNode CreateChildNode(this XmlNode source, string childNode, string namespaceUri)
        {
            var xmlDocument = source as XmlDocument;
            var document = xmlDocument ?? source.OwnerDocument;
            if (document != null)
            {
                XmlNode node = document.CreateElement(childNode, namespaceUri);
                source.AppendChild(node);
                return node;
            }

            return null;
        }

        /// <summary>
        /// Appends a CData section to a XML node
        /// </summary>
        /// <param name="source">
        /// The parent node
        /// </param>
        /// <returns>
        /// The created CData Section
        /// </returns>
        public static XmlCDataSection CreateCDataSection(this XmlNode source)
        {
            return source.CreateCDataSection(string.Empty);
        }

        /// <summary>
        /// Appends a CData section to a XML node and prefills the provided data
        /// </summary>
        /// <param name="source">
        /// The parent node
        /// </param>
        /// <param name="cData">
        /// The CData section value
        /// </param>
        /// <returns>
        /// The created CData Section
        /// </returns>
        public static XmlCDataSection CreateCDataSection(this XmlNode source, string cData)
        {
            var xmlDocument = source as XmlDocument;
            var document = xmlDocument ?? source.OwnerDocument;
            if (document != null)
            {
                var node = document.CreateCDataSection(cData);
                source.AppendChild(node);
                return node;
            }

            return null;
        }

        /// <summary>
        /// Appends a child to a XML node
        /// </summary>
        /// <param name="childNode">
        /// The name of the child node
        /// </param>
        /// <param name="sourceNode">
        /// The parent node
        /// </param>
        public static void AppendChildNodeTo(this string childNode, XmlNode sourceNode)
        {
            var xmlDocument = sourceNode as XmlDocument;
            var document = xmlDocument ?? sourceNode.OwnerDocument;
            if (document != null)
            {
                XmlNode node = document.CreateElement(childNode);
                sourceNode.AppendChild(node);
            }
        }

        /// <summary>
        /// Appends a child to a XML node
        /// </summary>
        /// <param name="childNode">
        /// The name of the child node
        /// </param>
        /// <param name="sourceNode">
        /// The parent node
        /// </param>
        /// <param name="namespaceUri">
        /// The node namespace
        /// </param>
        public static void AppendChildNodeTo(this string childNode, XmlNode sourceNode, string namespaceUri)
        {
            var xmlDocument = sourceNode as XmlDocument;
            var document = xmlDocument ?? sourceNode.OwnerDocument;
            if (document != null)
            {
                XmlNode node = document.CreateElement(childNode, namespaceUri);
                sourceNode.AppendChild(node);
            }
        }

        /// <summary>
        /// Appends a CData section to a XML node and prefills the provided data
        /// </summary>
        /// <param name="cData">
        /// The CData section value
        /// </param>
        /// <param name="sourceNode">
        /// The parent node
        /// </param>
        public static void AppendCDataSectionTo(this string cData, XmlNode sourceNode)
        {
            var xmlDocument = sourceNode as XmlDocument;
            var document = xmlDocument ?? sourceNode.OwnerDocument;
            if (document != null)
            {
                var node = document.CreateCDataSection(cData);
                sourceNode.AppendChild(node);
            }
        }

        /// <summary>
        /// Returns the value of a nested CData section
        /// </summary>
        /// <param name="source">
        /// The parent node
        /// </param>
        /// <returns>
        /// The CData section content
        /// </returns>
        public static string GetCDataSection(this XmlNode source)
        {
            return source.ChildNodes.OfType<XmlCDataSection>().Select(e => e.Value).FirstOrDefault();
        }

        /// <summary>
        /// Gets an attribute value
        ///     If the value is empty, uses the specified default value
        /// </summary>
        /// <param name="source">
        /// The node to retreive the value from
        /// </param>
        /// <param name="attributeName">
        /// The Name of the attribute
        /// </param>
        /// <param name="defaultValue">
        /// The default value to be returned if no matching attribute exists
        /// </param>
        /// <returns>
        /// The attribute value
        /// </returns>
        public static string GetAttribute(this XmlNode source, string attributeName, string defaultValue = null)
        {
            if (source.Attributes != null)
            {
                var attribute = source.Attributes[attributeName];
                return attribute?.InnerText ?? defaultValue;
            }

            return null;
        }

        /// <summary>
        /// Gets an attribute value converted to the specified data type
        ///     If the value is empty, uses the specified default value
        /// </summary>
        /// <typeparam name="T">
        /// The desired return data type
        /// </typeparam>
        /// <param name="source">
        /// The node to evaluate
        /// </param>
        /// <param name="attributeName">
        /// The Name of the attribute
        /// </param>
        /// <param name="defaultValue">
        /// The default value to be returned if no matching attribute exists
        /// </param>
        /// <returns>
        /// The attribute value
        /// </returns>
        public static T GetAttribute<T>(this XmlNode source, string attributeName, T defaultValue = default(T))
        {
            var value = GetAttribute(source, attributeName);

            return !string.IsNullOrEmpty(value) ? value.To(defaultValue) : defaultValue;
        }

        /// <summary>
        /// Creates or updates an attribute with the passed object
        /// </summary>
        /// <param name="source">
        /// The node to evaluate
        /// </param>
        /// <param name="name">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        public static void SetAttribute(this XmlNode source, string name, object value)
        {
            SetAttribute(source, name, value?.ToString());
        }

        /// <summary>
        /// Creates or updates an attribute with the passed value
        /// </summary>
        /// <param name="source">
        /// The node to evaluate
        /// </param>
        /// <param name="name">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        public static void SetAttribute(this XmlNode source, string name, string value)
        {
            if (source?.Attributes != null)
            {
                var attribute = source.Attributes[name, source.NamespaceURI];

                if (attribute == null)
                {
                    if (source.OwnerDocument != null)
                    {
                        attribute = source.OwnerDocument.CreateAttribute(name, source.OwnerDocument.NamespaceURI);
                    }

                    if (attribute != null)
                    {
                        source.Attributes.Append(attribute);
                    }
                }

                if (attribute != null)
                {
                    attribute.InnerText = value;
                }
            }
        }
    }
}