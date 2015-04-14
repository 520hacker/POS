using System;
using System.Linq;
using Lib.JSON.Linq;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// Represents a transaction input. If the PreviousOutput object is null,
    /// this is a coinbase input.
    /// </summary>
    public class Input
    {
        public Input(JObject i)
        {
            JObject prevOut = i["prev_out"] as JObject;
            if (prevOut != null)
            {
                this.PreviousOutput = new Output(prevOut, true);
            }

            this.Sequence = (long)i["sequence"];
            this.ScriptSignature = (string)i["script"];
        }

        /// <summary>
        /// Previous output. If null, this is a coinbase input.
        /// </summary>
        public Output PreviousOutput { get; private set; }

        /// <summary>
        /// Sequence number of the input
        /// </summary>
        public long Sequence { get; private set; }

        /// <summary>
        /// Script signature
        /// </summary>
        public string ScriptSignature { get; private set; }
    }
}