using Convey;
using Convey.WebApi.Exceptions;
using Spirebyte.Services.Sprints.Application.Exceptions.Base;
using Spirebyte.Services.Sprints.Core.Exceptions.Base;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace Spirebyte.Services.Sprints.Infrastructure.Exceptions
{
    internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        private static readonly ConcurrentDictionary<Type, string> Codes = new ConcurrentDictionary<Type, string>();

        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DomainException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                    HttpStatusCode.BadRequest),
                AppException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                    HttpStatusCode.BadRequest),
                _ => new ExceptionResponse(new { code = "error", reason = "There was an error." },
                    HttpStatusCode.BadRequest)
            };

        private static string GetCode(Exception exception)
        {
            var type = exception.GetType();
            if (Codes.TryGetValue(type, out var code))
            {
                return code;
            }

            var exceptionCode = exception switch
            {
                DomainException domainException when !string.IsNullOrWhiteSpace(domainException.Code) => domainException.Code,
                AppException appException when !string.IsNullOrWhiteSpace(appException.Code) => appException.Code,
                _ => exception.GetType().Name.Underscore().Replace("_exception", string.Empty)
            };

            Codes.TryAdd(type, exceptionCode);

            return exceptionCode;
        }
    }
}
