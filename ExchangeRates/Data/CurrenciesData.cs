using ExchangeRates.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeRates.Data
{
    //Класс получения списка валют из JSON-файла
    public class CurrenciesData
    {
        public async Task<IEnumerable<Valute>> GetCurrencies()
        {
            var url = "https://www.cbr-xml-daily.ru/daily_json.js";

            try
            {
                //Создаем HTTP запрос с хэдером Accept
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Accept = "application/json";
                //Получаем ответ
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonString = await streamReader.ReadToEndAsync();
                    var valuteString = JObject.Parse(jsonString)["Valute"].ToString();
                    var listOfCurrencies = JsonConvert.DeserializeObject<Dictionary<string, Valute>>(valuteString).Select(x => x.Value);

                    return listOfCurrencies;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}