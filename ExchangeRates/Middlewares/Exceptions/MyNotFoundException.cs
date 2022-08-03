using System;

namespace ExchangeRates.Middlewares.Exceptions
{
    public class MyNotFoundException : Exception
    {
        public MyNotFoundException(string message) : base(message)
        { }
    }
}