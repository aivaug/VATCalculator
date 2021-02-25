using Entities.Models.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.MembersRepositories
{
    public interface IMemberRepository
    {
        Task<Member> GetMember(int memberID, Expression<Func<Member, bool>> predicate = null);
    }
}
