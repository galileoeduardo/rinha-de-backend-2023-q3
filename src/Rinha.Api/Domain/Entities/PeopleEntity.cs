namespace Rinha.Domain.Entities
{
    public class PeopleEntity
    {

        public Guid Id { get; set; }
        public required string Apelido { get; set; }
        public required string Nome { get; set; }
        public DateTime? Nascimento { get; set; }
        public required string[] Stack { get; set; }
    }
}
