using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Block
    {
        public int index;
        public String hash;
        public String prevHash;
        DateTime timestamp;
        public List<Transaction> transactionList = new List<Transaction>();
        public int nonce;
        public float difficultyThreshold = 3;
        public float reward = 20;
        public double accumFees = 0;
        public String minersPublicKey;
        public String merkleRoot = "";

        public Block(String prevHash, int index, List<Transaction> transactionList, String minersPublicKey)
        {
            foreach (Transaction t in transactionList)
            {
                this.transactionList.Add(t);
            }
            this.nonce = 0;
            this.index = index += 1;          
            this.hash = Mine();
            createMineReward();
            this.minersPublicKey = minersPublicKey;
            this.prevHash = prevHash;
            this.timestamp = DateTime.Now;
        }

        public Block()
        {
            this.index = 0;
            this.hash = CreateHash();
            this.timestamp = DateTime.Now;
            this.prevHash = "";
        }


        public void calculateMerkle()
        {

            if(transactionList.Count == 5)
            {
               
                // calculate h1,2 through t1 and t2

                String hash1 = transactionList[0].Hash;
                String hash2 = transactionList[1].Hash;

                String hash1_2 = HashCode.HashTools.CombineHash(hash1, hash2);

                // calculate h3,4 through t3 and t4

                String hash3 = transactionList[2].Hash;
                String hash4 = transactionList[3].Hash;

                String hash2_3 = HashCode.HashTools.CombineHash(hash1, hash2);

                // calculate h1,2,3,4 though h1,2 and h,3,4

                String hash1_2_3_4 = HashCode.HashTools.CombineHash(hash1_2, hash2_3);

                // calculate h1,2,3,4,5 through h1,2,3,4 and h5

                String hash5 = transactionList[4].Hash;
                String hash1_2_3_4_5 = HashCode.HashTools.CombineHash(hash1_2_3_4, hash5);

                this.merkleRoot = hash1_2_3_4_5;
                Console.WriteLine("Calculated Merkle root: " + this.merkleRoot);
            }
      
            
        }

        public void createMineReward()
        {
            
            foreach(Transaction t in this.transactionList)
            {
                accumFees = accumFees + t.Fee;
            }

            Transaction transaction = new Transaction("Mine Rewards", this.minersPublicKey, (reward + accumFees), 0, "");
            this.transactionList.Add(transaction);


        }


        public String CreateHash()
        {
            SHA256 hasher;
            hasher = SHA256Managed.Create();
            String input = this.nonce + this.index.ToString() + this.timestamp.ToString() + this.prevHash + this.difficultyThreshold + this.reward;
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes((input)));

            String hash = string.Empty;

            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }
            return hash;

        }

        public String Mine()
        {
            while (1 == 1)
            {
                String hash = CreateHash();
                int numberOfZeros = 0;
                foreach (char c in hash)
                {
                    if(c == '0')
                    {
                        numberOfZeros += 1;
                    }
                    else
                    {
                        break;
                    }
                }


                if (numberOfZeros >= difficultyThreshold)
                {
                    return hash ;
                }
                else
                {
                    this.nonce += 1;
                }
            }
        }

        public String getBlockInfo()
        {
            double totalReward = this.reward + this.accumFees;

            String info = "Block Index: " + this.index.ToString() + "   " + "Timestamp: " +
                           this.timestamp.ToString() + "\r" + "Hash: " + this.hash + "\r" +
                           "Previous Hash: " + this.prevHash + "\r" +
                           "Nonce: " + this.nonce + "\r" +
                           "Dificulty: " + this.difficultyThreshold + "\r" +
                           "Transactions: " + this.transactionList.Count + "\r" +
                           "Reward: " + this.reward + "\r" +
                           "Fees: " + this.accumFees + "\r" +
                           "Total Reward: " + totalReward + "\r" +
                           "Merkle Root: " + this.merkleRoot;


            
            foreach (Transaction t in this.transactionList)
            {
                info += "\r" + t.getTransactionInfo() + "\r";
            }
            

            return info;
        }



    }
}
