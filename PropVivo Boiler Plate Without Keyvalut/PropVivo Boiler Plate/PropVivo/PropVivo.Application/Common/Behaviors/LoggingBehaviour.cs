﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace PropVivo.Application.Common.Behaviors
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("PropVivo Architecture Request Handling: { name } {@request }", typeof(TRequest).Name, JsonSerializer.Serialize(request));
            var response = await next();
            //_logger.LogInformation("PropVivo Architecture Response Handling: { name } {@response }", typeof(TResponse).Name, JsonSerializer.Serialize(response));

            return response;
        }
    }
}