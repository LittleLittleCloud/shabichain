using System.Linq;
using System.Runtime.Serialization;

namespace ShabiChain.Core
{
    [DataContract]
    public class Transaction
    {
        public Transaction(UTXO[] txIns, string payToAddress, int value, string unlockSignature)
        {
            this.TxIns = txIns;
            this.PayToAddress = payToAddress;
            this.Value = value;
            this.UnlockSignature = unlockSignature;
        }

        [DataMember(IsRequired =true)]
        public UTXO[] TxIns { get; }

        [DataMember(IsRequired = true)]
        public string PayToAddress { get; }

        [DataMember(IsRequired = true)]
        public int Value { get; }

        [DataMember(IsRequired = true)]
        public string UnlockSignature { get; }

        public bool IsCoinbase { get => this.TxIns.Length == 0; }

        public string ID { get => Utils.ComputeHash($"{this.PayToAddress}{this.Value}{this.UnlockSignature}{string.Join("", this.TxIns.Select(x => x.ID))}"); }
    }
}