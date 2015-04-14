using System;
using System.Collections.ObjectModel;
using System.Linq;
using Lib.JSON.Linq;

namespace Info.Blockchain.API.BlockExplorer
{
    /// <summary>
    /// Represents an address.
    /// </summary>
    public class Address
    {
        public Address(JObject a)
        {
            this.Hash160 = (string)a["hash160"];
            this.AddressStr = (string)a["address"];
            this.TotalReceived = (long)a["total_received"];
            this.TotalSent = (long)a["total_sent"];
            this.FinalBalance = (long)a["final_balance"];

            var txs = a["txs"].Select(x => new Transaction((JObject)x, -1, false)).ToList();
            this.Transactions = new ReadOnlyCollection<Transaction>(txs);
        }

        /// <summary>
        /// Hash160 representation of the address
        /// </summary>
        public string Hash160 { get; private set; }

        /// <summary>
        /// Base58Check representation of the address
        /// </summary>
        public string AddressStr { get; private set; }

        /// <summary>
        /// Total amount received (in satoshi)
        /// </summary>
        public long TotalReceived { get; private set; }

        /// <summary>
        /// Total amount sent (in satoshi)
        /// </summary>
        public long TotalSent { get; private set; }

        /// <summary>
        /// Final balance of the address (in satoshi)
        /// </summary>
        public long FinalBalance { get; private set; }

        /// <summary>
        /// List of transactions associated with this address
        /// </summary>
        public ReadOnlyCollection<Transaction> Transactions { get; private set; }
    }
}