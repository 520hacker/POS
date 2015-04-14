using System;
using System.Linq;
using Lib.JSON.Linq;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// Represents an unspent transaction output.
    /// </summary>
    public class UnspentOutput
    {
        public UnspentOutput(JObject o)
        {
            this.N = (int)o["tx_output_n"];
            this.TransactionHash = (string)o["tx_hash"];
            this.TransactionIndex = (long)o["tx_index"];
            this.Script = (string)o["script"];
            this.Value = (long)o["value"];
            this.Confirmations = (long)o["confirmations"];
        }

        /// <summary>
        /// Index of the output in a transaction
        /// </summary>
        public int N { get; private set; }

        /// <summary>
        /// Transaction hash
        /// </summary>
        public string TransactionHash { get; private set; }

        /// <summary>
        /// Transaction index
        /// </summary>
        public long TransactionIndex { get; private set; }

        /// <summary>
        /// Output script
        /// </summary>
        public string Script { get; private set; }

        /// <summary>
        /// Value of the output (in satoshi)
        /// </summary>
        public long Value { get; private set; }

        /// <summary>
        /// Number of confirmations
        /// </summary>
        public long Confirmations { get; private set; }
    }
}