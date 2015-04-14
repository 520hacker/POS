using System.Xml;

namespace Lib.JSON.Converters
{
    internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement
    {
        private readonly XmlElement _element;

        public XmlElementWrapper(XmlElement element) : base(element)
        {
            this._element = element;
        }

        public void SetAttributeNode(IXmlNode attribute)
        {
            XmlNodeWrapper xmlAttributeWrapper = (XmlNodeWrapper)attribute;

            this._element.SetAttributeNode((XmlAttribute)xmlAttributeWrapper.WrappedNode);
        }

        public string GetPrefixOfNamespace(string namespaceUri)
        {
            return this._element.GetPrefixOfNamespace(namespaceUri);
        }
    }
}