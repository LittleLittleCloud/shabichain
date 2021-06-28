using System;
namespace ShabiChain.Core
{
    public class ShabiChain
    {
        private Chain<Block> chain;
        public ShabiChain()
        {
            this.chain = new Chain<Block>(Constant.GENESIS_BLOCK);
        }

        public void ConnectChainNode(Block node)
        {
            this.ValidateBasicInfo(node);
            this.ValidateTransaction(node);
            this.chain.ConnectChainNode(node);
            this.AddUTXOsToBank(node);
        }

        private void AddUTXOsToBank(Block node)
        {
            throw new NotImplementedException();
        }

        public void DisconnectChainNode(Block node)
        {
            this.chain.DisconnectChainNode(node);
            this.RemoveUTXOsFromBank(node);
        }

        private void RemoveUTXOsFromBank(Block node)
        {
            throw new NotImplementedException();
        }

        private void ValidateTransaction(Block node)
        {
            throw new NotImplementedException();
        }

        private void ValidateBasicInfo(Block node)
        {
            throw new NotImplementedException();
        }
    }
}
