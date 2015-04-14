namespace Lib.JSON.Converters
{
    internal interface IXmlDeclaration : IXmlNode
    {
        string Version { get; }

        string Encoding { get; set; }

        string Standalone { get; set; }
    }
}