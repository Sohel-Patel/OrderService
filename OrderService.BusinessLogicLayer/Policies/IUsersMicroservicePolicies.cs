using Polly;

namespace OrderService.BusinessLogicLayer.Policies
{
    public interface IUsersMicroservicePolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
    }
}