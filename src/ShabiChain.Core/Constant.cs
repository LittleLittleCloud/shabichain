using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShabiChain.Core
{
    public static class Constant
    {
        public readonly static Block GENESIS_BLOCK = new()
        {
            Version = 0,
            PreviousChainNodeID = Utils.ComputeHash("I'm shabi coin"),
            MerkleHash = Utils.ComputeHash("I'm shabi coin"),
            Timestamp = 1622012400, // 2021-05-26 00:00:00
            Bits = 24,
            Nonce = 10126761,
        };
    }
}
