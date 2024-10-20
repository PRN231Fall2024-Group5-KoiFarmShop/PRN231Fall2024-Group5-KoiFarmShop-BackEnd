using Koi.Services.Services;

namespace Koi.Services.Interface
{
    public interface IPayOSService
    {
        Task<string> CreateLink(int depositMoney, Guid txnRef);
        Task<PayOSWebhookResponse> ReturnWebhook(PayOSWebhookRequest payOSWebhook);
    }
}
