using ExchangeRates.Middlewares.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRates.Middlewares
{
    //Класс обработки кастомных исключений.
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //Код по умолчанию - 500
            var code = HttpStatusCode.InternalServerError;

            if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is MyBadRequestException) code = HttpStatusCode.BadRequest;

            //Генерация JSON-файла с кодом и текстом ошибки
            var result = JsonSerializer.Serialize(new { Код = code, Ошибка = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}