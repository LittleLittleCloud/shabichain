using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShabiChain.Core
{
    public class ChainNodeException : Exception
    {
        public ChainNodeException(ChainNode node)
        {
            this.Node = node;
        }

        public ChainNodeException(string msg, ChainNode node)
            : base(msg)
        {
            this.Node = node;
        }

        public ChainNode Node { get; protected set; }
    }

    public class DanglingChainNodeException : ChainNodeException
    {
        public DanglingChainNodeException(ChainNode node)
            : base(node)
        {
            this.Node = node;
        }
    }

    public class DuplicateChainNodeException : ChainNodeException
    {
        public DuplicateChainNodeException(ChainNode node)
            : base(node)
        {
            this.Node = node;
        }
    }
}
