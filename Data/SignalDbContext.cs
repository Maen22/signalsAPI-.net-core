using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Audit;
using Server.Authentication;
using Server.Entities;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Helpers
{
    public class SignalDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public DbSet<Signal> Signals { get; set; }

        public SignalDbContext(DbContextOptions<SignalDbContext> options, IServiceProvider provider) : base(options)
        {
            _serviceProvider = provider;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.FilterSoftDeletedEntries<Signal>();
        }

        public override int SaveChanges()
        {
            var userId= GetCurrentUserInfo().Result;
            this.EnsureAudit<string>(userId);
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var userId = GetCurrentUserInfo().Result;
            this.EnsureAudit<string>(userId);
            return base.SaveChangesAsync(cancellationToken);
        }

        private async Task<string> GetCurrentUserInfo()
        {
            IHttpContextAccessor _httpContextAccessor = (IHttpContextAccessor)_serviceProvider.GetService(typeof(IHttpContextAccessor));
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string userId = null;

            if (_httpContextAccessor.HttpContext?.User != null)
            {
                userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return userId;
        }
    }
}
