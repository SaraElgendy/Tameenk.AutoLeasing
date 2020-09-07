
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;

namespace Tameenk.AutoLeasing.Identity
{
    public class AdminContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>> 
    {
        public AdminContext(DbContextOptions<AdminContext> options)
         : base(options)
        {

        }
        public AdminContext()
        { }
        public virtual DbSet<AdminRequestLog> AdminRequestLog { get; set; }
        public virtual DbSet<InsuranceCompany> InsuranceCompanies { get; set; }
        public virtual DbSet<AspNetUserCompany> AspNetUserCompanies { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

             optionsBuilder.AddConnectionToDbContext("DefaultConnection");            
           
        }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry(entity);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<AspNetUserCompany>(userCompany =>
            {
                userCompany.HasKey(ur => new { ur.UserId, ur.CompanyId });

                //userCompany.HasOne(ur => ur.Company)
                //    .WithMany(r => r.UserCompanies)
                //    .HasForeignKey(ur => ur.Company)
                //    .IsRequired();

                //userCompany.HasOne(ur => ur.User)
                //    .WithMany(r => r.UserCompanies)
                //    .HasForeignKey(ur => ur.User)
                //    .IsRequired();
            });

        }

       
        #region ConfigurSaveChanges

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


      
        #endregion

    }
}
