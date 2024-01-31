using Rinha.Domain.Entities;

namespace Rinha.Domain.Repositories
{
    public interface IPeopleRepository
    {
        Task<IEnumerable<PeopleEntity>> GetPeoples(string t);
        Task<PeopleEntity> GetPeople(Guid id);
        Task<PeopleEntity> CreatePeople(PeopleEntity pessoa);
        Task<int> CountPeoples();

    }
}