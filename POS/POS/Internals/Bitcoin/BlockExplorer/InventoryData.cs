using System;
using System.Linq;
using Lib.JSON.Linq;

namespace POS.Internals.Bitcoin.BlockExplorer
{
    /// <summary>
    /// This class contains data related to inventory messages 
    /// that Blockchain.info received for an object.
    /// </summary>
    public class InventoryData
    {
        public InventoryData(JObject i)
        {
            this.Hash = (string)i["hash"];
            this.Type = (string)i["type"];
            this.InitialTime = (long)i["initial_time"];
            this.LastTime = (long)i["last_time"];
            this.InitialIP = (string)i["initial_ip"];
            this.NConnected = (int)i["nconnected"];
            this.RelayedCount = (int)i["relayed_count"];
            this.RelayedPercent = (int)i["relayed_percent"];
        }

        /// <summary>
        /// Object hash
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        /// Object type
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// The time Blockchain.info first received an inventory message
        /// containing a hash for this transaction (unix time in ms).
        /// </summary>
        public long InitialTime { get; private set; }

        /// <summary>
        /// The last time Blockchain.info received an inventory message 
        /// containing a hash for this transaction (unix time in ms).
        /// </summary>
        public long LastTime { get; private set; }

        /// <summary>
        /// IP of the peer from which Blockchain.info first received an inventory 
        /// message containing a hash for this transaction.
        /// </summary>
        public string InitialIP { get; private set; }

        /// <summary>
        /// Number of nodes that Blockchain.info is currently connected to.
        /// </summary>
        public int NConnected { get; private set; }
        
        /// <summary>
        /// Number of nodes Blockchain.info received an inventory message containing 
        /// a hash for this transaction from.
        /// </summary>
        public int RelayedCount { get; private set; }

        /// <summary>
        /// Ratio of nodes that Blockchain.info received an inventory message
        /// containing a hash for this transaction from and the number of connected nodes.
        /// </summary>
        public int RelayedPercent { get; private set; }
    }
}