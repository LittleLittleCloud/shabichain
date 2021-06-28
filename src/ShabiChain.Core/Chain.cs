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
                return this.GetReversedChainFromNode(this.activeChainNode);
            }
        }

        public event EventHandler<ActiveChainChangeEventArgs>? ActiveChainChangeEvent;

        public virtual void ConnectChainNode(T node)
        {
            Debug.Assert(!node.IsRootNode, $"node {node.ID} is root node");
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
                        if (node.PreviousChainNodeID != this.activeChainNode.PreviousChainNodeID)
                        {
                            this.ActiveChainChangeEvent?.Invoke(this, new ActiveChainChangeEventArgs(this.activeChainNode, node));
                        }
                        this.activeChainNode = node;
                    }
                }
                else
                {
                    throw new DanglingChainNodeException(node);
                }
            }
        }

        public virtual void DisconnectChainNode(T node)
        {
            Debug.Assert(this.leaveNodes.ContainsKey(node.ID), $"node {node.ID} is not leaf node");
            Debug.Assert(node != root);

            lock (this.chainLock)
            {
                this.leaveNodes.Remove(node.ID);
                var previousNode = this.chainNodes[node.PreviousChainNodeID!];
                this.leaveNodes[previousNode.ID] = previousNode;
                if(node == this.activeChainNode)
                {
                    var newActiveChainNode = this.leaveNodes.OrderByDescending(kv => kv.Value.Height).First().Value;
                    if(newActiveChainNode.ID != node.PreviousChainNodeID)
                    {
                        this.ActiveChainChangeEvent?.Invoke(this, new ActiveChainChangeEventArgs(this.activeChainNode, newActiveChainNode));
                    }
                    this.activeChainNode = newActiveChainNode;
                }
            }
        }

        public IEnumerable<T> GetReversedChainFromNode(T node)
        {
            if (!this.chainNodes.ContainsKey(node.ID))
            {
                throw new ChainNodeException($"{node.ID} not exist", node);
            }

            var current = node;
            while (current != null)
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

        private void ValidateBlock(T node)
        {
            return;
        }
    }
}
