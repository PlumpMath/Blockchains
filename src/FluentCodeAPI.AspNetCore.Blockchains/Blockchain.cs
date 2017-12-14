using FluentCodeAPI.AspNetCore.Blockchains.Internal;
using FluentCodeAPI.AspNetCore.Blockchains.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentCodeAPI.AspNetCore.Blockchains
{
    /// <summary>
    /// Represents the <see cref="Blockchain"/>.
    /// </summary>
    public class Blockchain
    {
        /// <summary>
        /// The current transaction list.
        /// </summary>
        private IList<Transaction> _currentTransactions;

        /// <summary>
        /// Initializes a <see cref="Blockchain"/>.
        /// </summary>
        public Blockchain()
        {
            _currentTransactions = new List<Transaction>();

            Chain = new List<Block>();
            Nodes = new List<Uri>();

            // create the genesis block "Hello World!"
            AddNewBlock(100, "7F83B1657FF1FC53B92DC18148A1D65DFC2D4B1FA3D677284ADDD200126D9069");
        }

        /// <summary>
        /// Gets the <see cref="Chain"/> as a <see cref="IList{Models.Block}"/>.
        /// </summary>
        public IList<Block> Chain { get; private set; }

        /// <summary>
        /// Gets the <see cref="Nodes"/> as a <see cref="IList{System.Uri}"/>.
        /// </summary>
        public IList<Uri> Nodes { get; }

        /// <summary>
        /// Gets the <see cref="LastBlock"/> as a <see cref="Block"/>.
        /// </summary>
        public Block LastBlock => Chain.Last();

        /// <summary>
        /// Gets the <see cref="Length"/> as a <see cref="Int32"/>.
        /// </summary>
        public int Length => Chain.Count;

        /// <summary>
        /// Creates a new Block in the Blockchain
        /// </summary>
        /// <param name="proof">The proof as a <see cref="Int32"/></param>
        /// <param name="previousHash">The optional previous hash as a <see cref="String"/></param>
        /// <returns>The new <see cref="Block"/></returns>
        public Block AddNewBlock(int proof, string previousHash = null)
        {
            var block = new Block()
            {
                Index = Length + 1,
                Transactions = _currentTransactions,
                Proof = proof,
                PreviousHash = previousHash ?? HashBlock(LastBlock)
            };

            // reset the current list of transactions
            _currentTransactions = new List<Transaction>();

            Chain.Add(block);

            return block;
        }

        /// <summary>
        /// Creates a new transaction to go into the next mined Block
        /// </summary>
        /// <param name="sender">Uri of the sender as a <see cref="String"/></param>
        /// <param name="recipient">Uri of the recipient as a <see cref="String"/></param>
        /// <param name="amount">The amount as a <see cref="Int32"/></param>
        /// <returns>The index of the Block that will hold this transaction as a <see cref="Int32"/></returns>
        public int AddNewTransaction(string sender, string recipient, int amount)
        {
            var transaction = new Transaction()
            {
                Sender = sender,
                Recipient = recipient,
                Amount = amount
            };

            _currentTransactions.Add(transaction);

            return LastBlock.Index + 1;
        }

        /// <summary>
        /// Creates a SHA-256 hash of a Block
        /// </summary>
        /// <param name="block">The block to hash as a <see cref="Block"/></param>
        /// <returns>The hash value for the specified block</returns>
        public string HashBlock(Block block)
        {
            var serializedBlock = JsonConvert.SerializeObject(block, Formatting.Indented); // indented and ordered to avoid inconsistencies

            return Hasher.ComputeSHA256(serializedBlock);
        }

        /// <summary>
        /// Simple Proof of Work Algorithm
        /// </summary>
        /// <returns>The computed proof of work as a <see cref="Int32"/></returns>
        public int ComputeProofOfWork()
        {
            var proof = 0;

            while (!IsValidProof(LastBlock.Proof, proof))
            {
                proof++;
            }

            return proof;
        }

        /// <summary>
        /// Validates the Proof
        /// </summary>
        /// <param name="lastProof">The previous proof as a <see cref="Int32"/></param>
        /// <param name="proof">The current proof as a <see cref="Int32"/></param>
        /// <returns>True if correct, False if not</returns>
        public bool IsValidProof(int lastProof, int proof)
        {
            var guess = $"{lastProof}{proof}";
            var hashedGuess = Hasher.ComputeSHA256(guess);

            return hashedGuess.Substring(hashedGuess.Length - 4, 4) == "0000";
        }

        /// <summary>
        /// Add a new node to the nodes list
        /// </summary>
        /// <param name="address">The Uri of the node as a <see cref="String"/></param>
        /// <returns>True if register, False if not</returns>
        public bool RegisterNode(string address)
        {
            if (Uri.TryCreate(address, UriKind.Absolute, out Uri uri))
            {
                if (!Nodes.Any(u => u.Host == uri.Host))
                {
                    Nodes.Add(uri);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determine if a given blockchain is valid
        /// </summary>
        /// <param name="chain">The blockchain as a <see cref="IList{Models.Block}"/></param>
        /// <returns>True if valid, False if not</returns>
        public bool IsValidChain(IList<Block> chain)
        {
            var lastBlock = chain.Last();
            var currentIndex = 1;

            while (currentIndex < chain.Count)
            {
                var block = chain[currentIndex];

                // Check our previous hashed block against the current hashed block
                if (HashBlock(LastBlock) != block.PreviousHash)
                {
                    return false;
                }

                // Check the Proof of Work
                if (!IsValidProof(LastBlock.Proof, block.Proof))
                {
                    return false;
                }

                lastBlock = block;
                currentIndex++;
            }

            return true;
        }

        /// <summary>
        /// Resolve the conflicts from our blockchain against the blockchains registered to our node lists.
        /// This is our Consensus Algorithm, it resolves conflicts by replacing our chain with the longest one in the network.
        /// </summary>
        /// <returns>True if our chain was replaced, False if not</returns>
        public async Task<bool> ResolveConflictsAsync()
        {
            var nodes = Nodes; // lock the nodes
            var maxLength = Length;
            var newChain = new List<Block>();

            // Check the chains from our network
            foreach (var node in nodes)
            {
                var response = await Web.GetAsync(node.ToString()); // Get /chain

                if (!string.IsNullOrEmpty(response))
                {
                    var chain = JsonConvert.DeserializeObject<List<Block>>(response);

                    var length = chain.Count;

                    if (length > maxLength && !IsValidChain(chain))
                    {
                        maxLength = length;
                        newChain = chain;
                    }
                }
            }

            // Replace our chain if we discovered a new and valid chain longer than ours
            if (newChain.Any())
            {
                Chain = newChain;
                return true;
            }

            return false;
        }
    }
}