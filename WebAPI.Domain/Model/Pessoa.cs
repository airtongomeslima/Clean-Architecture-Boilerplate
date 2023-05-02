namespace WebAPI.Domain.Model
{
    public class Pessoa
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public Guid? IdPessoaResponsavel { get; set; }
        public string SobreNome { get; set; }
        public string Sexo { get; set; }
        public int Idade { get; set; }
        public Endereco Endereco { get; set; }
        public List<Telefone> Telefones { get; set; }
    }
}