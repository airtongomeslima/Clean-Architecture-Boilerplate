namespace WebAPI.Domain.Model
{
    public class Pessoa
    {
        public int Id { get; set; }
        public int IdEndereco { get; set; }
        public int? IdPessoaResponsavel { get; set; } //Id de outra Pessoa
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string Sexo { get; set; }
        public int Idade { get; set; }
        public Endereco Endereco { get; set; }
        public List<Telefone> Telefones { get; set; }
    }
}