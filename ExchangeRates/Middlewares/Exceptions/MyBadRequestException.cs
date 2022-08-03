using System;

namespace ExchangeRates.Middlewares.Exceptions
{
    public class MyBadRequestException : Exception
    {
        public MyBadRequestException(string message) : base(message)
        { }
    }
}