using Microsoft.Extensions.DependencyInjection;
using Repositories.MembersRepositories;
using Services.VATServices;

namespace HomeworkProject.Helpers
{
    public static class InjectServices
    {
        public static IServiceCollection ConfigureInjections(this IServiceCollection services)
        {
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IVATService, VATService>();

            return services;
        }
    }
}
