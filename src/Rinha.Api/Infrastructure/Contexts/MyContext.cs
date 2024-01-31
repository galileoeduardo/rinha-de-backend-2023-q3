using Microsoft.EntityFrameworkCore;
using Rinha.Domain.Entities;

namespace Rinha.InfraStructure.Contexts
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        { }

        public DbSet<PeopleEntity> Peoples { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<PeopleEntity>().HasKey(c => c.Id);
            mb.Entity<PeopleEntity>().Property(c => c.Apelido).HasMaxLength(32).IsRequired();
            mb.Entity<PeopleEntity>().Property(c => c.Nome).HasMaxLength(100);
            mb.Entity<PeopleEntity>().Property(c => c.Nascimento); //Todo:AAAA-MM-DD
            mb.Entity<PeopleEntity>().Property(c => c.Stack);
        }
    }
}
