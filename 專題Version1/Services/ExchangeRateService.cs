using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace 專題Version1.Services
{
    public class ExchangeRateService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiUrl = "http://api.currencylayer.com/live"; // 替換為你使用的API URL
        private const string apiKey = "62eff89ef39e740c00ebf3cbcff33757"; // 替換為你的API Key

        public async Task<Dictionary<string, decimal>> GetExchangeRates(string fromCurrency, List<string> toCurrencies)
        {
            var toCurrenciesString = string.Join(",", toCurrencies);
            var response = await client.GetStringAsync($"{apiUrl}?access_key={apiKey}&currencies={toCurrenciesString}&source={fromCurrency}");
            var data = JObject.Parse(response);
            var rates = new Dictionary<string, decimal>();

            foreach (var currency in toCurrencies)
            {
                var rateKey = $"{fromCurrency}{currency}";
                rates[currency] = data["quotes"][rateKey].Value<decimal>();
            }

            return rates;
        }
    }
}