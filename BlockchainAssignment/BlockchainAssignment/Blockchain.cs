using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Blockchain
    {
        public List<Block> Blocks = new List<Block>();
        public List<Transaction> pendingTransactions = new List<Transaction>();

        public Blockchain()
        {
            Blocks.Add(new Block());

        }

        public String printBlockFromBlocks(int index)
        {
           return Blocks[index].getBlockInfo();
            
        }
        
        public Block getLastBlock()
        {
            return Blocks[Blocks.Count - 1];
        }
       
    }
}
