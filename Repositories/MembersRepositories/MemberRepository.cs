using Entities;
using Entities.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.MembersRepositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly VATDBContext _context;
        public MemberRepository(VATDBContext context)
        {
            _context = context;
        }

        public async Task<Member> GetMember(int memberID, Expression<Func<Member, bool>> predicate = null)
        {
            IQueryable<Member> query = _context.Members;

            if(predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.Include(item => item.Country)
                              .WhereExists()
                              .FirstOrDefaultAsync(item => item.Id == memberID) as Member;
        }
    }
}
