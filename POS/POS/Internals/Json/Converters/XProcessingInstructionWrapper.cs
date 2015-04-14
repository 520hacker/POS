using System.Xml.Linq;

namespace Lib.JSON.Converters
{
    internal class XProcessingInstructionWrapper : XObjectWrapper
    {
        private XProcessingInstruction ProcessingInstruction
        {
            get
            {
                return (XProcessingInstruction)this.WrappedNode;
            }
        }

        public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction) : base(processingInstruction)
        {
        }

        public override string LocalName
        {
            get
            {
                return this.ProcessingInstruction.Target;
            }
        }

        public override string Value
        {
            get
            {
                return this.ProcessingInstruction.Data;
            }
            set
            {
                this.ProcessingInstruction.Data = value;
            }
        }
    }
}