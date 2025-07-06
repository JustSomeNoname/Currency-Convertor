using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CurrencyConvertor
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.frankfurter.app";

        public CurrencyService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<Dictionary<string, decimal>> GetAvailableCurrenciesAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{BaseUrl}/latest");
                var result = JsonConvert.DeserializeObject<ExchangeRateResponse>(response);

                var currencies = new Dictionary<string, decimal> { { result.Base, 1.0m } };
                foreach (var rate in result.Rates)
                {
                    currencies[rate.Key] = rate.Value;
                }

                return currencies;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch currencies: {ex.Message}");
            }
        }

        public async Task<decimal> ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency)
                return amount;

            try
            {
                var url = $"{BaseUrl}/latest?amount={amount}&from={fromCurrency}&to={toCurrency}";
                var response = await _httpClient.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<ConversionResponse>(response);

                if (result?.Rates != null && result.Rates.ContainsKey(toCurrency))
                {
                    return result.Rates[toCurrency];
                }

                throw new Exception("Invalid response from currency API");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                throw new Exception("Request timeout - please check your internet connection");
            }
            catch (Exception ex)
            {
                throw new Exception($"Conversion failed: {ex.Message}");
            }
        }
    }

    public class ExchangeRateResponse
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public class ConversionResponse
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}