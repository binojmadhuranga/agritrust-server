using AgriTrust.API.Services.Interfaces;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;

namespace AgriTrust.API.Services.Implementations;

public class BlockchainService : IBlockchainService
{
    private readonly string rpcUrl = "http://127.0.0.1:7545";
    private readonly string contractAddress = "0x1106e70CadD80e946CACdeB66E587136FB64b9fd";
    private readonly string privateKey = "0xc85874c044eacd78e487f557dfc31871c910495dca8ea447caf1d0a8cb0f2cc8";
    private readonly string accountAddress = "0xd096a37f0abad9C9E91EcB7C9F6EB87771fB5673";
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
	},
	{
		""inputs"": [
			{
				""internalType"": ""string"",
				""name"": """",
				""type"": ""string""
			}
		],
		""name"": ""certificates"",
		""outputs"": [
			{
				""internalType"": ""string"",
				""name"": ""certificateNumber"",
				""type"": ""string""
			},
			{
				""internalType"": ""string"",
				""name"": ""certificateHash"",
				""type"": ""string""
			},
			{
				""internalType"": ""uint256"",
				""name"": ""timestamp"",
				""type"": ""uint256""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""string"",
				""name"": ""_certificateNumber"",
				""type"": ""string""
			}
		],
		""name"": ""getCertificate"",
		""outputs"": [
			{
				""internalType"": ""string"",
				""name"": """",
				""type"": ""string""
			},
			{
				""internalType"": ""string"",
				""name"": """",
				""type"": ""string""
			},
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	}
]";

    public async Task StoreCertificateHashAsync(string certificateNumber, string hash)
    {
        var web3 = new Web3(rpcUrl);

        var contract = web3.Eth.GetContract(abi, contractAddress);

        var function = contract.GetFunction("storeCertificate");

        await function.SendTransactionAsync(
            accountAddress,
            new HexBigInteger(300000),
            null,
            certificateNumber,
            hash
        );
    }
}