using System;

namespace WebAPI.Domain.Model
{
    public class Pessoa : Entity
    {
        public int IdEndereco { get; set; }
        public int IdPessoaResponsavel { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string Sexo { get; set; }
        public int Idade { get; set; }
        public Pessoa? PessoaResponsavel { get; set; }
        public Endereco Endereco { get; set; }
        public List<Telefone> Telefones { get; set; }
    }
}
