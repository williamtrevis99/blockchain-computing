using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {
        Blockchain blockchain;
        Wallet.Wallet myNewWallet;
        public int index;


        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain();
            richTextBox1.Text = "New Blockchain Initialised";
            button5.Enabled = false;
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + "\r\r" + blockchain.printBlockFromBlocks(this.index);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.index = Int32.Parse(textBox1.Text);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            String privKey;
            myNewWallet = new Wallet.Wallet(out privKey);
            String publicKey = myNewWallet.publicID;
            myNewWallet.balance = 0;
            textBox2.Text = publicKey;
            textBox3.Text = privKey;
            button5.Enabled = true;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(Wallet.Wallet.ValidatePrivateKey(textBox3.Text, textBox2.Text))
            {
                richTextBox1.Text = "Keys are Valid";
            }
            else
            {
                richTextBox1.Text = "Keys are not valid";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if(blockchain.pendingTransactions.Count <= 3)
            {
                // check the user has enough money in there wallet
                if (myNewWallet.balance - Convert.ToDouble(textBox4.Text) >= 0)
                {
                    Transaction transaction = new Transaction(textBox2.Text, textBox6.Text, Convert.ToDouble(textBox4.Text),
                                                      Convert.ToDouble(textBox5.Text), textBox3.Text);
                    richTextBox1.Text = transaction.getTransactionInfo();
                    blockchain.pendingTransactions.Add(transaction);
                    myNewWallet.balance = myNewWallet.balance - Convert.ToDouble(textBox4.Text);
                }
                else
                {
                    richTextBox1.Text = richTextBox1.Text + "\r\r" + "You do not have enough balance in your wallet to complete this transaction" + "\r" + "Balance: " + myNewWallet.balance;
                }
               
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Block lastBlock = blockchain.getLastBlock();
            richTextBox1.Text = richTextBox1.Text + "\r\r" + "Mining New Block" + "\r";
            Block block = new Block(lastBlock.hash, lastBlock.index, blockchain.pendingTransactions, textBox2.Text);
            blockchain.Blocks.Add(block);
            richTextBox1.Text = richTextBox1.Text + "New Block Mined" + "\r";
            myNewWallet.balance += block.accumFees + block.reward;

            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String allInfo = "";
            for (int i = 0; i < blockchain.Blocks.Count; i++)
            {
                allInfo = allInfo + blockchain.Blocks[i].getBlockInfo();
                allInfo = allInfo + "\r\r";
            }

            richTextBox1.Text = allInfo;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (Transaction t in blockchain.pendingTransactions)
            {
                blockchain.Blocks[this.index].transactionList.Add(t);
            }
            blockchain.pendingTransactions = blockchain.pendingTransactions.Except(blockchain.pendingTransactions).ToList();

        }

        // perform fullverification of the blockchain
        private void button8_Click(object sender, EventArgs e)
        {
            foreach (Block b in blockchain.Blocks)
            {
                b.calculateMerkle();
            }
            
            String prevBlockHash = "";

            foreach(Block b in blockchain.Blocks)
            {

                if (b.index != 0)
                {
                    if (b.prevHash != prevBlockHash)
                    {
                        richTextBox1.Text = richTextBox1.Text + "\r\r" + "Blockchain Invalid" + "\r";
                        return;
                    }
                    else
                    {
                        prevBlockHash = b.hash;
                    }

                }
                else
                {
                    prevBlockHash = b.hash;
                }            
            }

            richTextBox1.Text = richTextBox1.Text + "\r\r" + "Blockchain Valid" + "\r";
            return;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + "\r\r" + "Balance: " + myNewWallet.balance;
        }
    }
}
