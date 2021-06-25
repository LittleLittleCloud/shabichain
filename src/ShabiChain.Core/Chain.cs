using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShabiChain.Core
{
    public class Chain<T>
        where T: ChainNode
    {
        private readonly T root;
        private readonly object chainLock = new object();
        private T activeChainNode;
        private Dictionary<string, T> chainNodes = new Dictionary<string, T>();
        private Dictionary<string, T> leaveNodes = new Dictionary<string, T>();

        public Chain(T root)
        {
            this.root = root;
            this.activeChainNode = root;
            this.chainNodes[root.ID] = root;
            this.leaveNodes[root.ID] = root;
        }

        public IEnumerable<T> ReversedActiveChain
        {
            get
            {
                lock (this.chainLock)
                {
                    var current = this.activeChainNode;
                    while(current != null)
                    {
                        Debug.Assert(this.chainNodes.ContainsKey(current.ID));
                        yield return current;
                        if (current.IsRootNode)
                        {
                            break;
                        }
                        else
                        {
                            current = this.chainNodes[current.PreviousChainNodeID!];
                        }
                    }
                }
            }
        }

        public void ConnectChainNode(T node)
        {
            Debug.Assert(!node.IsRootNode);
            this.ValidateBlock(node);

            lock (this.chainLock)
            {
                if (this.chainNodes.ContainsKey(node.ID))
                {
                    throw new DuplicateChainNodeException(node);
                }

                if (this.chainNodes.TryGetValue(node.PreviousChainNodeID!, out var parentNode))
                {
                    // update height
                    node.Height = parentNode.Height + 1;
                    this.chainNodes[node.ID] = node;
                    this.leaveNodes[node.ID] = node;

                    if (this.leaveNodes.ContainsKey(parentNode.ID))
                    {
                        this.leaveNodes.Remove(parentNode.ID);
                    }

                    if (node.Height > this.activeChainNode.Height)
                    {
                        this.activeChainNode = node;
                    }
                }
                else
                {
                    throw new DanglingChainNodeException(node);
                }
            }
        }

        public void DisconnectChainNode(T node)
        {
            Debug.Assert(this.leaveNodes.ContainsKey(node.ID));
            Debug.Assert(this.leaveNodes.ContainsKey(node.ID));
            Debug.Assert(node != root);

            lock (this.chainLock)
            {
                this.leaveNodes.Remove(node.ID);

                if(node == this.activeChainNode)
                {
                    this.activeChainNode = this.leaveNodes.OrderByDescending(kv => kv.Value.Height).First().Value;
                }
            }
        }

        private void ValidateBlock(T node)
        {
            return;
        }
    }
}
