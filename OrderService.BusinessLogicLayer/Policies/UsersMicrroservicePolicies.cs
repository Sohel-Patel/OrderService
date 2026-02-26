using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace OrderService.BusinessLogicLayer.Policies
{
    public class UsersMicrroservicePolicies : IUsersMicroservicePolicies
    {
        private readonly ILogger<UsersMicrroservicePolicies> _logger;
        public UsersMicrroservicePolicies(ILogger<UsersMicrroservicePolicies> logger)
        {
            _logger = logger;
        }

        IAsyncPolicy<HttpResponseMessage> IUsersMicroservicePolicies.GetRetryPolicy()
        {
            return Policy.HandleResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode).WaitAndRetryAsync(retryCount:5,sleepDurationProvider:x => TimeSpan.FromSeconds(2),onRetry:(outcome,timespan,retryAttempt,context) =>
            {
                _logger.LogInformation($"Retry {retryAttempt} after {timespan.TotalSeconds} Seconds");
            });
        }
    }
}