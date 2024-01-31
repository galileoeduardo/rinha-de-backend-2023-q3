using Microsoft.EntityFrameworkCore;
using Rinha.Domain.Entities;
using Rinha.InfraStructure.Contexts;

namespace Rinha.Domain.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly MyContext _context;
        public PeopleRepository(MyContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PeopleEntity>> GetPeoples(string t)
        {
            return await _context.Peoples.Where(
                    e => e.Apelido.Contains(t) ||
                    e.Nome.Contains(t)
                    //TODO: Where in array field || e.Stack.Contains(t, StringComparer.OrdinalIgnoreCase)
                ).ToListAsync();
        }

        public async Task<PeopleEntity> GetPeople(Guid id)
        {
            return await _context.Peoples.FindAsync(id);
        }

        public async Task<PeopleEntity> CreatePeople(PeopleEntity pessoa)
        {
            await _context.Peoples.AddAsync(pessoa);
            await _context.SaveChangesAsync();
            return pessoa;
        }

        public async Task<int> CountPeoples()
        {
            return await _context.Peoples.CountAsync();
        }
    }
}
