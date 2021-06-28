using System;
namespace ShabiChain.Core
{
    public class ActiveChainChangeEventArgs: EventArgs
    {
        public ActiveChainChangeEventArgs(ChainNode oldHead, ChainNode newHead)
        {
            this.Old = oldHead;
            this.New = newHead;
        }

        public ChainNode Old { get; }

        public ChainNode New { get; }
    }
}
