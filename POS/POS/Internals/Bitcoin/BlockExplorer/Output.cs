using System;
using System.Linq;
using Lib.JSON.Linq;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// Represents a transaction output.
    /// </summary>
    public class Output
    {
        public Output(JObject o, bool? spent = null)
        {
            this.N = (int)o["n"];
            this.Value = (long)o["value"];
            this.Address = (string)o["addr"];
            this.TxIndex = (long)o["tx_index"];
            this.Script = (string)o["script"];
            this.Spent = spent != null ? spent.Value : (bool)o["spent"];
        }

        /// <summary>
        /// Index of the output in a transaction
        /// </summary>
        public int N { get; private set; }

        /// <summary>
        /// Value of the output (in satoshi)
        /// </summary>
        public long Value { get; private set; }

        /// <summary>
        /// Address that the output belongs to
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Transaction index
        /// </summary>
        public long TxIndex { get; private set; }

        /// <summary>
        /// Output script
        /// </summary>
        public string Script { get; private set; }

        /// <summary>
        /// Whether the output is spent
        /// </summary>
        public bool Spent { get; private set; }
    }
}