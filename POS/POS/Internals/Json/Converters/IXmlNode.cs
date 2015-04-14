using System.Collections.Generic;
using System.Xml;

namespace Lib.JSON.Converters
{
    internal interface IXmlNode
    {
        XmlNodeType NodeType { get; }

        string LocalName { get; }

        IList<IXmlNode> ChildNodes { get; }

        IList<IXmlNode> Attributes { get; }

        IXmlNode ParentNode { get; }

        string Value { get; set; }

        IXmlNode AppendChild(IXmlNode newChild);

        string NamespaceUri { get; }

        object WrappedNode { get; }
    }
}