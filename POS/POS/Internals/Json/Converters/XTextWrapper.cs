using System.Xml.Linq;

namespace Lib.JSON.Converters
{
    internal class XTextWrapper : XObjectWrapper
    {
        private XText Text
        {
            get
            {
                return (XText)this.WrappedNode;
            }
        }

        public XTextWrapper(XText text) : base(text)
        {
        }

        public override string Value
        {
            get
            {
                return this.Text.Value;
            }
            set
            {
                this.Text.Value = value;
            }
        }

        public override IXmlNode ParentNode
        {
            get
            {
                if (this.Text.Parent == null)
                {
                    return null;
                }
          
                return XContainerWrapper.WrapNode(this.Text.Parent);
            }
        }
    }
}