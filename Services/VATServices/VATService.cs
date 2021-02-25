using Entities.Models.Entities;
using Repositories.MembersRepositories;
using System;
using System.Threading.Tasks;

namespace Services.VATServices
{
    public class VATService : IVATService
    {
        private readonly IMemberRepository _memberRepository;

        public VATService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<decimal> GetVATFullPrice(int providerId, int clientId, decimal price)
        {
            Member provider = await _memberRepository.GetMember(providerId, x => x.IsCompany);
            if (provider == null) throw new ArgumentException($"Existing provider with id {providerId} not found");

            Member client = await _memberRepository.GetMember(clientId);
            if (client == null) throw new ArgumentException($"Existing client with id {clientId} not found");

            //If provider is not VAT payer - return original price
            if (!provider.IsVATPayer) return price;

            //Know that provider is VAT payer. Have to pay VAT if client and provider lives in the same country
            if (client.CountryId == provider.CountryId) return price * provider.Country.VATPercent;

            //Have to calculate VAT if client is not VAT payer and lives in EU
            if (!client.IsVATPayer && client.Country.IsEU) return price * client.Country.VATPercent;

            return price;
        }
    }
}
