using AgriTrust.API.Services.Interfaces;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts;

namespace AgriTrust.API.Services.Implementations;

public class BlockchainService : IBlockchainService
{
    private readonly string rpcUrl;
    private readonly string contractAddress;
    private readonly string privateKey;

    private readonly string abi = @"[
        {
            ""inputs"": [
                {
                    ""internalType"": ""string"",
                    ""name"": ""_certificateNumber"",
                    ""type"": ""string""
                },
                {
                    ""internalType"": ""string"",
                    ""name"": ""_certificateHash"",
                    ""type"": ""string""
                }
            ],
            ""name"": ""storeCertificate"",
            ""outputs"": [],
            ""stateMutability"": ""nonpayable"",
            ""type"": ""function""
        }
    ]";

    public BlockchainService(IConfiguration configuration)
    {
        rpcUrl = configuration["Blockchain:RpcUrl"];
        contractAddress = configuration["Blockchain:ContractAddress"];
        privateKey = configuration["Blockchain:PrivateKey"];
    }
    
    public async Task StoreCertificateHashAsync(string certificateNumber, string hash)
    {
        var account = new Account(privateKey);
        var web3 = new Web3(account, rpcUrl);

        var contract = web3.Eth.GetContract(abi, contractAddress);
        var function = contract.GetFunction("storeCertificate");

        await function.SendTransactionAsync(
            account.Address,
            new HexBigInteger(300000),
            null,
            certificateNumber,
            hash
        );
    }
}