using System.Threading.Tasks;

namespace Services.VATServices
{
    public interface IVATService
    {
        Task<decimal> GetVATFullPrice(int providerId, int clientId, decimal price);
    }
}
