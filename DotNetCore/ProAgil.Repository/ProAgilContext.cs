using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Domain.Identity;

namespace ProAgil.Repository
{
    public class ProAgilContext: IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole,
                                 IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ProAgilContext(DbContextOptions<ProAgilContext> options) : base(options)
        {

        }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<RedeSocial> RedeSocials { get; set; }
        public DbSet<PalestranteEvento> PalestranteEventos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(UserRole => 
                {
                    UserRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                    UserRole.HasOne(ur => ur.Role)
                            .WithMany(ur => ur.UserRoles)
                            .HasForeignKey(ur => ur.RoleId)
                            .IsRequired();
                    
                    UserRole.HasOne(ur => ur.User)
                            .WithMany(ur => ur.UserRoles)
                            .HasForeignKey(ur => ur.UserId)
                            .IsRequired();
                } 
            );

            modelBuilder.Entity<PalestranteEvento>()
                .HasKey(PE => new { PE.EventoId, PE.PalestranteId });
        }

    }
}