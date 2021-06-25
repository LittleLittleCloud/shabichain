using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShabiChain.Core
{
    public abstract class ChainNode : IEquatable<ChainNode>
    {
        public virtual string? PreviousChainNodeID { get; set; }

        public abstract string ID { get; }

        public long Height { get; set; }

        public bool IsRootNode { get => this.PreviousChainNodeID is null; }

        public bool Equals(ChainNode? other)
        {
            if(other is null)
            {
                return false;
            }

            return this.ID == other!.ID;
        }
    }
}
