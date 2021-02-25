using Entities.Models.Entities;
using Repositories.MembersRepositories;
using System.Threading.Tasks;
using Xunit;

namespace Tests.RepositoryTests
{
    public class MemberRepositoryTests : DatabaseSeed
    {
        private readonly IMemberRepository memberRepository;
        public MemberRepositoryTests()
        {
            memberRepository = new MemberRepository(Context);
            AddSampleData();
        }

        [Fact]
        public async Task GetMember_ExistingClient_ReturnsmemberObjectWithId1()
        {
            //Arrange
            int memberId = 1;

            //Act
            var memberObject = await memberRepository.GetMember(memberId);

            //Assert
            Assert.True(memberObject is Member);
            Assert.Equal(1, memberObject.Id);
        }

        [Fact]
        public async Task GetMember_ExistingProvider_ReturnsMemberObjectWithId3()
        {
            //Arrange
            int memberId = 3;

            //Act
            var memberObject = await memberRepository.GetMember(memberId, x => x.IsCompany);

            //Assert
            Assert.Equal(3, memberObject.Id);
        }

        [Fact]
        public async Task GetMember_NotExistinhProvider_ReturnsNull()
        {
            //Arrange
            int memberId = 1;

            //Act
            var memberObject = await memberRepository.GetMember(memberId, x => x.IsCompany);

            //Assert
            Assert.Null(memberObject);
        }

        [Fact]
        public async Task GetMember_DeletedMember_ReturnsNull()
        {
            //Arrange
            int memberId = 2;

            //Act
            var memberObject = await memberRepository.GetMember(memberId);

            //Assert
            Assert.Null(memberObject);
        }
    }
}
