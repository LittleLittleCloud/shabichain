using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShabiChain.Core
{
    /// <summary>
    /// Unspent Transaction Output
    /// </summary>
    [DataContract]
    public class UTXO
    {
        public UTXO(int value, string address, string blockHash, string txHash, int txOutIdx)
        {
            Debug.Assert(value > 0);
            Debug.Assert(txOutIdx > 0);

            this.Value = value;
            this.Address = address;
            this.BlockHash = blockHash;
            this.TransactionHash = txHash;
            this.TransactionOutIdx = txOutIdx;
        }

        [DataMember(IsRequired = true)]
        public int Value { get; }

        [DataMember(IsRequired = true)]
        public string Address { get; }

        [DataMember(IsRequired = true)]
        public string BlockHash { get; }

        [DataMember(IsRequired = true)]
        public string TransactionHash { get; }

        [DataMember(IsRequired = true)]
        public int TransactionOutIdx { get; }

        public string ID { get => Utils.ComputeHash($"{this.Value}{this.Address}{this.BlockHash}{this.TransactionHash}{this.TransactionOutIdx}"); }
    }
}
