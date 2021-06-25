using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace ShabiChain.Core
{
    [DataContract]
    public class Block : ChainNode
    {
        /// <summary>
        /// A version integer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Version { get; set; }

        [DataMember(IsRequired = true)]
        public override string? PreviousChainNodeID { get; set; }

        /// <summary>
        /// A hash of the Merkle tree containing all txns.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string? MerkleHash { get; set; }

        /// <summary>
        /// A UNIX timestamp of when this block was created.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Timestamp { get; set; }

        /// <summary>
        /// The difficulty target; i.e. the hash of this block header must be under
        /// (2 ** 256 >> bits) to consider work proved.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Bits { get; set; }

        /// <summary>
        /// # The value that's incremented in an attempt to get the block header to
        /// hash to a value below `bits`.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nonce { get; set; }

        [DataMember(IsRequired = true)]
        public List<Transaction>? Transactions { get; set; }

        public string Header
        {
            get => $"{this.Version}{this.PreviousChainNodeID}{this.MerkleHash}{this.Timestamp}{this.Bits}{this.Nonce}";
        }

        public override string ID { get => Utils.ComputeHash(this.Header); }
    }
}
