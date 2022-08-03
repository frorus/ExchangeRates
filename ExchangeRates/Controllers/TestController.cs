using ExchangeRates.Data;
using ExchangeRates.Filters;
using ExchangeRates.Middlewares.Exceptions;
using ExchangeRates.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRates.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class TestController : ControllerBase
    {
        private readonly CurrenciesData _currencies;

        public TestController(CurrenciesData currencies)
        {
            _currencies = currencies;
        }

        [HttpGet]
        [ActionName("currencies")]
        public async Task<IEnumerable<Valute>> GetCurrencies(int? pageNumber)
        {
            int pageSize = 9;

            try
            {
                var currencies = await _currencies.GetCurrencies();
                var data = PaginationFilter<Valute>.Create(currencies.OrderBy(x => x.Name).AsQueryable(), pageNumber ?? 1, pageSize);
                if (pageNumber > data.TotalPages)
                {
                    throw new MyBadRequestException("Страницы с таким номером не существует.");
                }
                return data;
            }
            catch (Exception ex)
            {
                if (ex is MyBadRequestException)
                {
                    throw new MyBadRequestException(ex.Message);
                }
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("currency")]
        public async Task<Valute> GetCurrency(string id)
        {
            try
            {
                var currencies = await _currencies.GetCurrencies();
                var currency = currencies.FirstOrDefault(x => string.Equals(x.ID, id, StringComparison.OrdinalIgnoreCase));
                if (currency == null)
                {
                    throw new MyNotFoundException("Валюта с таким ID не найдена.");
                }
                return currency;
            }
            catch (Exception ex)
            {
                if (ex is MyNotFoundException)
                {
                    throw new MyNotFoundException(ex.Message);
                }
                throw new Exception(ex.Message);
            }
        }
    }
}