namespace Api.Models
{
    public class PessoaEntity
    {

        public Guid Id { get; set; }
        public required string Apelido { get; set; }
        public required string Nome { get; set; }
        public DateTime? Nascimento { get; set; }
        public required string[] Stack { get; set; }

        
    }
}
