using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Nethereum.Web3;
using System;
using System.Numerics;
using System.Threading.Tasks;
using YazilimNedirBSCToken.Models.Configuration;

namespace YazilimNedirBSCToken.TokenService
{
    public class BSCTokenService
    {
        private const string DECIMALS_KEY = "contract.decimals";
        private const string SYMBOL_KEY = "contract.symbol";

        private string contractABI;
        private string rpcEndpoint;
        private string contractAddress;

        private readonly IMemoryCache _cache;
        
        public BSCTokenService(IConfiguration configuration, 
            ConfigContractABI configContractABI,
            IMemoryCache cache)
        {
            _cache = cache;
            rpcEndpoint = configuration.GetValue<string>("RPCEndpoint");
            contractABI = configContractABI.JsonStr;
            contractAddress = configuration.GetValue<string>("ContractAddress");
        }

        public async Task<string> GetSymbol()
        {
            string symbol;
            if(_cache.TryGetValue(SYMBOL_KEY, out symbol))
                return symbol;

            Web3 web3 = new Web3(rpcEndpoint);
            var contract = web3.Eth.GetContract(contractABI, contractAddress);
            var symbolFunction = contract.GetFunction("symbol");
            symbol = await symbolFunction.CallAsync<string>();
            _cache.Set(SYMBOL_KEY, symbol, TimeSpan.FromDays(365));
            return symbol;
        }

        public async Task<int> GetDecimal()
        {
            int decimals;
            if (_cache.TryGetValue(DECIMALS_KEY, out decimals))
                return decimals;

            Web3 web3 = new Web3(rpcEndpoint);
            var contract = web3.Eth.GetContract(contractABI, contractAddress);
            var decimalsFunction = contract.GetFunction("decimals");
            decimals = await decimalsFunction.CallAsync<int>();
            _cache.Set(DECIMALS_KEY, decimals, TimeSpan.FromDays(365));
            return decimals;
        }

        public async Task<BigInteger> GetBalanceOf(string address)
        {
            Web3 web3 = new Web3(rpcEndpoint);
            var contract = web3.Eth.GetContract(contractABI, contractAddress);
            var balanceFunction = contract.GetFunction("balanceOf");
            var balance = await balanceFunction.CallAsync<BigInteger>(address);
            return balance;
        }

    }
}
