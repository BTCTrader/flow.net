﻿using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Client;
using Flow.Net.Sdk.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class CreateTransactionExample
    {
        private static async Task Demo()
        {
            // reading script from folder
            var script = Utilities.ReadCadenceScript("greeting");

            var proposerAddress = new FlowAddress("9a0766d93b6608b7");
            uint proposerKeyIndex = 3;

            var payerAddress = new FlowAddress("631e88ae7f1d7c20");
            var authorizerAddress = new FlowAddress("7aad92e5a0715d21");

            // Establish a connection with an access node
            var accessAPIHost = "";
            var FlowClient = new FlowClientAsync(accessAPIHost);

            // Get the latest sealed block to use as a reference block
            var latestBlock = await FlowClient.GetLatestBlockHeaderAsync();

            // Get the latest account info for this address
            var proposerAccount = await FlowClient.GetAccountAtLatestBlockAsync(proposerAddress);

            // Get the latest sequence number for this key
            var proposerKey = proposerAccount.Keys.Where(w => w.Index == proposerKeyIndex).FirstOrDefault();
            var sequenceNumber = proposerKey.SequenceNumber;

            var tx = new FlowTransaction
            {
                Script = script,
                GasLimit = 100,
                ProposalKey = new FlowProposalKey
                {
                    Address = proposerAddress,
                    KeyId = proposerKeyIndex,
                    SequenceNumber = sequenceNumber
                },
                Payer = payerAddress
            };

            // Add authorizer
            tx.Authorizers.Add(authorizerAddress);

            // Add argument            
            tx.Arguments.Add(new CadenceString("Hello"));
        }

        private static void Demo2()
        {
            var proposerAccount = new FlowAccount();
            var proposerKey = proposerAccount.Keys.Where(w => w.Index == 1).FirstOrDefault();

            var tx = new FlowTransaction
            {
                Script = "transaction { execute { log(\"Hello, World!\") } }",
                GasLimit = 100,
                ProposalKey = new FlowProposalKey
                {
                    Address = proposerAccount.Address,
                    KeyId = proposerKey.Index,
                    SequenceNumber = proposerKey.SequenceNumber
                },
                Payer = proposerAccount.Address
            };

            // construct a signer from your private key and configured signature/hash algorithms
            var signer = Sdk.Crypto.Ecdsa.Utilities.CreateSigner("privateKey", SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);

            tx = FlowTransaction.AddEnvelopeSignature(tx, proposerAccount.Address, proposerKey.Index, signer);
        }
    }
}
