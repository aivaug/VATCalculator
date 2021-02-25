using Entities.Models.Entities;
using NSubstitute;
using Repositories.MembersRepositories;
using Services.VATServices;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Tests.ServiceTests
{
    public class VATServiceTests
    {
        private readonly Country _euCountry;
        private readonly Country _euCountryDifferentVAT;
        private readonly Country _notEuCountry;
        private readonly VATService _vatService;
        private readonly IMemberRepository _memberRepository;

        public VATServiceTests()
        {
            _euCountry = new Country { IsEU = true, VAT = 20 };
            _euCountryDifferentVAT = new Country { IsEU = true, VAT = 30 };
            _notEuCountry = new Country { IsEU = false, VAT = 40 };

            _memberRepository = Substitute.For<IMemberRepository>();
            _vatService = new VATService(_memberRepository);
        }

        [Fact]
        public async Task GetVATFullPrice_TheSameCountryProviderIsVATPayer_ReturnsFinalPriceWithVAT()
        {
            //Arrange
            Member provider = ContructMember(_euCountry, 1, true, true);
            Member client = ContructMember(_euCountry, 1, false);
            _memberRepository.GetMember(1, Arg.Any<Expression<Func<Member, bool>>>()).Returns(provider);
            _memberRepository.GetMember(2).Returns(client);
            int providerId = 1;
            int clientId = 2;

            //Act
            decimal finalPrice = await _vatService.GetVATFullPrice(providerId, clientId, 10);

            //Assert
            Assert.Equal(new decimal(12), finalPrice);
        }

        [Fact]
        public async Task GetVATFullPrice_TheSameCountryProviderIsNotVATPayer_ReturnsOriginalPrice()
        {
            //Arrange
            Member provider = ContructMember(_euCountry, 1, false, true);
            Member client = ContructMember(_euCountry, 1, false);
            _memberRepository.GetMember(1, Arg.Any<Expression<Func<Member, bool>>>()).Returns(provider);
            _memberRepository.GetMember(2).Returns(client);
            int providerId = 1;
            int clientId = 2;

            //Act
            decimal finalPrice = await _vatService.GetVATFullPrice(providerId, clientId, 10);

            //Assert
            Assert.Equal(new decimal(10), finalPrice);
        }

        [Fact]
        public async Task GetVATFullPrice_ClientNotEuCountry_ReturnsOriginalPrice()
        {
            //Arrange
            Member provider = ContructMember(_euCountry, 1, true, true);
            Member client = ContructMember(_notEuCountry, 2, false);
            _memberRepository.GetMember(1, Arg.Any<Expression<Func<Member, bool>>>()).Returns(provider);
            _memberRepository.GetMember(2).Returns(client);
            int providerId = 1;
            int clientId = 2;

            //Act
            decimal finalPrice = await _vatService.GetVATFullPrice(providerId, clientId, 10);

            //Assert
            Assert.Equal(new decimal(10), finalPrice);
        }

        [Fact]
        public async Task GetVATFullPrice_CalculateClientsCountryVAT_ReturnsClientsCountriesPriceWithVAT()
        {
            //Arrange
            Member provider = ContructMember(_euCountry, 1, true, true);
            Member client = ContructMember(_euCountryDifferentVAT, 2, false);
            _memberRepository.GetMember(1, Arg.Any<Expression<Func<Member, bool>>>()).Returns(provider);
            _memberRepository.GetMember(2).Returns(client);
            int providerId = 1;
            int clientId = 2;

            //Act
            decimal finalPrice = await _vatService.GetVATFullPrice(providerId, clientId, 10);

            //Assert
            Assert.Equal(new decimal(13), finalPrice);
        }

        [Fact]
        public async Task GetVATFullPrice_ClientIsVATPayer_ReturnsOriginalPrice()
        {
            //Arrange
            Member provider = ContructMember(_euCountry, 1, true, true);
            Member client = ContructMember(_euCountryDifferentVAT, 2, true);
            _memberRepository.GetMember(1, Arg.Any<Expression<Func<Member, bool>>>()).Returns(provider);
            _memberRepository.GetMember(2).Returns(client);
            int providerId = 1;
            int clientId = 2;

            //Act
            decimal finalPrice = await _vatService.GetVATFullPrice(providerId, clientId, 10);

            //Assert
            Assert.Equal(new decimal(10), finalPrice);
        }

        [Fact]
        public async Task GetVATFullPrice_TheSameCountry_ReturnsPriceWithVAT()
        {
            //Arrange
            Member provider = ContructMember(_euCountry, 1, true, true);
            Member client = ContructMember(_euCountryDifferentVAT, 1, true);
            _memberRepository.GetMember(1, Arg.Any<Expression<Func<Member, bool>>>()).Returns(provider);
            _memberRepository.GetMember(2).Returns(client);
            int providerId = 1;
            int clientId = 2;

            //Act
            decimal finalPrice = await _vatService.GetVATFullPrice(providerId, clientId, 10);

            //Assert
            Assert.Equal(new decimal(12), finalPrice);
        }

        [Fact]
        public async Task GetVATFullPrice_ProviderNotFound_ReturnsArgumentExceptionThrown()
        {
            //Arrange
            Member provider = ContructMember(_euCountry, 1, true, true);
            Member client = ContructMember(_euCountryDifferentVAT, 1, true);
            _memberRepository.GetMember(1, Arg.Any<Expression<Func<Member, bool>>>()).Returns((Member)null);
            _memberRepository.GetMember(2).Returns(client);
            int providerId = 1;
            int clientId = 2;

            //Act
            Func<Task> action = async () => await _vatService.GetVATFullPrice(providerId, clientId, 10);

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact]
        public async Task GetVATFullPrice_NotExistingClient_ReturnsArgumentExceptionThrown()
        {
            //Arrange
            Member provider = ContructMember(_euCountry, 1, true, true);
            _memberRepository.GetMember(1, Arg.Any<Expression<Func<Member, bool>>>()).Returns(provider);
            _memberRepository.GetMember(2).Returns((Member)null);
            int providerId = 1;
            int clientId = 2;

            //Act
            Func<Task> action = async () => await _vatService.GetVATFullPrice(providerId, clientId, 10);

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        private Member ContructMember(Country country, int countryId, bool isVatPayer, bool isCompany = false)
        {
            return new Member
            {
                Country = country,
                CountryId = countryId,
                IsVATPayer = isVatPayer,
                IsCompany = isCompany
            };
        }
    }
}
