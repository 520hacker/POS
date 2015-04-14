namespace Lib.JSON.Converters
{
    internal interface IXmlElement : IXmlNode
    {
        void SetAttributeNode(IXmlNode attribute);

        string GetPrefixOfNamespace(string namespaceUri);
    }
}