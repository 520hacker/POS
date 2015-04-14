using System;
using System.Collections.ObjectModel;
using System.Linq;
using Lib.JSON.Linq;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// Represents a transaction.
    /// </summary>
    public class Transaction
    {
        public Transaction(JObject t, long? blockHeight = null, bool? doubleSpend = null)
        {
            this.DoubleSpend = doubleSpend != null ? doubleSpend.Value : (bool)t["double_spend"];
            this.BlockHeight = blockHeight != null ? blockHeight.Value : -1;
            
            // this mitigates the bug where unconfirmed txs lack the block height field
            if (this.BlockHeight == -1)
            {
                JToken height = t["block_height"];
                if (height != null)
                {
                    this.BlockHeight = (long)t["block_height"];
                }
            }

            this.Time = (long)t["time"];
            this.RelayedBy = (string)t["relayed_by"];
            this.Hash = (string)t["hash"];
            this.Index = (long)t["tx_index"];
            this.Version = (int)t["ver"];
            this.Size = (long)t["size"];

            var ins = t["inputs"].AsJEnumerable().Select(x => new Input((JObject)x)).ToList();
            this.Inputs = new ReadOnlyCollection<Input>(ins);

            var outs = t["out"].AsJEnumerable().Select(x => new Output((JObject)x)).ToList();
            this.Outputs = new ReadOnlyCollection<Output>(outs);
        }

        /// <summary>
        /// Whether the transaction is a double spend
        /// </summary>
        public bool DoubleSpend { get; private set; }

        /// <summary>
        /// Block height of the parent block. -1 for unconfirmed transactions.
        /// </summary>
        public long BlockHeight { get; private set; }

        /// <summary>
        /// Timestamp of the transaction (unix time in seconds)
        /// </summary>
        public long Time { get; private set; }

        /// <summary>
        /// IP address that relayed the transaction
        /// </summary>
        public string RelayedBy { get; private set; }

        /// <summary>
        /// Transaction hash
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        /// Transaction index
        /// </summary>
        public long Index { get; private set; }

        /// <summary>
        /// Transaction format version
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Serialized size of the transaction
        /// </summary>
        public long Size { get; private set; }

        /// <summary>
        /// List of inputs
        /// </summary>
        public ReadOnlyCollection<Input> Inputs { get; private set; }

        /// <summary>
        /// List of outputs
        /// </summary>
        public ReadOnlyCollection<Output> Outputs { get; private set; }
    }
}