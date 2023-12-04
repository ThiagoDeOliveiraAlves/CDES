using CompartilhamentoDeDicasSustentaveis.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CompartilhamentoDeDicasSustentaveis.Data
{
    public class ApplicationDbContext : IdentityDbContext<UsuarioIdentity>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Postagem> Postagem { get; set; }
        public DbSet<ImagemPath> ImagemPath { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsuarioIdentity>()
                .HasMany(u => u.Postagens)
                .WithOne(u => u.Remetente)
                .HasForeignKey(p => p.RemetenteId)
                .OnDelete(DeleteBehavior.Cascade);
            

            modelBuilder.Entity<Postagem>()
                .HasMany(i => i.ImagemPath)
                .WithOne(p => p.Postagem)
                .HasForeignKey(ip => ip.PostagemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
