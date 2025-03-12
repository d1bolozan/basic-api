using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Newtonsoft.Json;

namespace api.Service
{
    public class FMPService : IFMPService
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;

        public FMPService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                System.Console.WriteLine(
                    $"{_configuration["FMP:BaseUrl"]}/profile/{symbol}?apikey={_configuration["FMP:ApiKey"]}"
                );
                var result = await _httpClient.GetAsync(
                    $"{_configuration["FMP:BaseUrl"]}/profile/{symbol}?apikey={_configuration["FMP:ApiKey"]}"
                );
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock = tasks[0];
                    if (stock != null)
                    {
                        return stock.ToStockFromFMP();
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
                throw new Exception(e.Message);
            }
        }
    }
}
