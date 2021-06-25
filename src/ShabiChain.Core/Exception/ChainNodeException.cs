using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShabiChain.Core
{
    public class ChainNodeException : Exception
    {
        public ChainNode Node { get; protected set; }
    }

    public class DanglingChainNodeException : ChainNodeException
    {
        public DanglingChainNodeException(ChainNode node)
        {
            this.Node = node;
        }
    }

    public class DuplicateChainNodeException : ChainNodeException
    {
        public DuplicateChainNodeException(ChainNode node)
        {
            this.Node = node;
        }
    }
}
