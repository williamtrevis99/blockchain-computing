using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Transaction
    {
        public String Hash;
        public String Signature;
        public String SenderAddress;
        public String RecipientAddress;
        public DateTime Timestamp;
        public Double Amount;
        public Double Fee;

        public Transaction(String SenderAddress, String RecipientAddress, Double Amount, Double Fee, String senderPrivKey )
        {
            this.SenderAddress = SenderAddress;
            this.RecipientAddress = RecipientAddress;
            this.Amount = Amount;
            this.Fee = Fee;
            this.Timestamp = DateTime.Now;
            this.Hash = generateTransactionHash();
            this.Signature = Wallet.Wallet.CreateSignature(this.SenderAddress, senderPrivKey, this.Hash);
        }

        private String generateTransactionHash()
        {
            SHA256 hasher;
            hasher = SHA256Managed.Create();
            String input = this.SenderAddress + this.Timestamp.ToString() + this.RecipientAddress;
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes((input)));

            String hash = string.Empty;

            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }
            return hash;
        }

        public String getTransactionInfo()
        {
            String info ="\r" +
                "Transaction Hash: " + this.Hash + "\r" +
                          "Digital Signature: " + this.Signature + "\r" +
                          "Timestamp: " + this.Timestamp.ToString() + "\r" +
                          "Transferred: " + this.Amount + " HerbCoin" + "\r" +
                          "Fees: " + this.Fee + "\r" +
                          "Sender Address: " + this.SenderAddress + "\r" +
                          "Reciever Address: " + this.RecipientAddress;

            return info;
        }
    }
}
