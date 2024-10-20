using Koi.Services.Services;
using Net.payOS.Types;

namespace Koi.Services.Interface
{
    public interface IPayOSService
    {
        Task<string> CreateLink(int depositMoney, int txnRef);
        Task<PayOSWebhookResponse> ReturnWebhook(WebhookType webhookType);
    }
}
