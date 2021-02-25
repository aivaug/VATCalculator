using Entities.Models;
using System.Linq;

namespace Repositories.Helpers
{
    public static class ExtensionMethods
    {
        public static IQueryable<BaseEntity> WhereExists(this IQueryable<BaseEntity> items)
        {
            return items.Where(i => !i.IsDeleted);
        }
    }
}
