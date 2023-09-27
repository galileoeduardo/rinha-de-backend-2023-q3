using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        { }

        public DbSet<PessoaEntity> Pessoas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<PessoaEntity>().HasKey(c => c.Id);
            mb.Entity<PessoaEntity>().Property(c => c.Apelido).HasMaxLength(32).IsRequired();
            mb.Entity<PessoaEntity>().Property(c => c.Nome).HasMaxLength(100);
            mb.Entity<PessoaEntity>().Property(c => c.Nascimento); //Todo:AAAA-MM-DD
            mb.Entity<PessoaEntity>().Property(c => c.Stack);
        }
    }
}
