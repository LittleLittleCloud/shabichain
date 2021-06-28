using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ShabiChain.Core.Tests
{
    public class ChainTest:TestBase
    {
        public ChainTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public void Chain_get_active_chain_test()
        {
            var chain = this.CreateSingleChain();
            var activeChain = chain.ReversedActiveChain.Reverse().ToArray();
            activeChain.Length.Should().Be(10);
            activeChain[0].IsRootNode.Should().BeTrue();
            activeChain[9].Height.Should().Be(9);
            activeChain[9].ID.Should().Be("9");
        }

        [Fact]
        public void Chain_should_reorg_if_active_chain_change()
        {
            var root = new SimpleBlock("0");
            var chain = new Chain<SimpleBlock>(root);
            var block1 = new SimpleBlock("1");
            block1.PreviousChainNodeID = root.ID;
            chain.ConnectChainNode(block1);
            chain.ReversedActiveChain.Count().Should().Be(2);
            chain.ReversedActiveChain.First().Should().Be(block1);
            chain.ReversedActiveChain.First().Height.Should().Be(1);

            var block2 = new SimpleBlock("2");
            block2.PreviousChainNodeID = root.ID;
            var block3 = new SimpleBlock("3");
            block3.PreviousChainNodeID = block2.ID;
            chain.ActiveChainChangeEvent += (object sender, ActiveChainChangeEventArgs e) =>
            {
                e.Old.ID.Should().Be("1");
                e.New.ID.Should().Be("3");
            };

            chain.ConnectChainNode(block2);
            chain.ConnectChainNode(block3);

            chain.ReversedActiveChain.Count().Should().Be(3);
            chain.ReversedActiveChain.First().Should().Be(block3);
            chain.ReversedActiveChain.First().Height.Should().Be(2);
        }

        [Fact]
        public void Chain_should_reorg_if_disconnect()
        {
            var root = new SimpleBlock("0");
            var chain = new Chain<SimpleBlock>(root);
            var block1 = new SimpleBlock("1");
            block1.PreviousChainNodeID = root.ID;
            chain.ConnectChainNode(block1);
            var block2 = new SimpleBlock("2");
            block2.PreviousChainNodeID = root.ID;
            var block3 = new SimpleBlock("3");
            block3.PreviousChainNodeID = block2.ID;

            chain.ConnectChainNode(block2);
            chain.ConnectChainNode(block3);

            chain.ReversedActiveChain.Count().Should().Be(3);
            chain.ReversedActiveChain.First().Should().Be(block3);
            chain.ReversedActiveChain.First().Height.Should().Be(2);

            chain.ActiveChainChangeEvent += (object sender, ActiveChainChangeEventArgs e) =>
            {
                e.Old.ID.Should().Be("3");
                e.New.ID.Should().Be("1");
            };

            chain.DisconnectChainNode(block3);
            chain.DisconnectChainNode(block2);
            chain.ReversedActiveChain.Count().Should().Be(2);
            chain.ReversedActiveChain.First().Should().Be(block1);
            chain.ReversedActiveChain.First().Height.Should().Be(1);
        }

        [Fact]
        public void Chain_should_not_connect_dangling_block()
        {
            var chain = this.CreateSingleChain();
            var danglingBlock = new SimpleBlock("_0");
            danglingBlock.PreviousChainNodeID = "_1";
            Action action = () => chain.ConnectChainNode(danglingBlock);
            action.Should().ThrowExactly<DanglingChainNodeException>()
                           .Where(e => e.Node.ID == "_0");
        }

        [Fact]
        public void Chain_should_not_connect_root_block()
        {
            var chain = this.CreateSingleChain();
            var root = new SimpleBlock("0");
            Action action = () => chain.ConnectChainNode(root);
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Chain_should_not_connect_dup_block()
        {
            var chain = this.CreateSingleChain();
            var block = new SimpleBlock("8");
            block.PreviousChainNodeID = "7";
            Action action = () => chain.ConnectChainNode(block);
            action.Should().Throw<DuplicateChainNodeException>()
                           .Where(e => e.Node == block);
        }

        private Chain<SimpleBlock> CreateSingleChain()
        {
            var block = new SimpleBlock("0");
            block.Height = 0;
            var chain = new Chain<SimpleBlock>(block);
            for(int i = 1; i!= 10; ++i)
            {
                block = new SimpleBlock(i.ToString());
                block.PreviousChainNodeID = (i - 1).ToString();
                chain.ConnectChainNode(block);
            }

            return chain;
        }

        private class SimpleBlock : ChainNode
        {
            public SimpleBlock(string id)
            {
                this.ID = id;
            }

            public override string ID { get; }
        }
    }
}
