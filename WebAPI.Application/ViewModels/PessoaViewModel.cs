using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Model;

namespace WebAPI.Application.ViewModels
{
    public class PessoaViewModel
    {
        public int Id { get; set; }
        public int IdEndereco { get; set; }
        public int? IdPessoaResponsavel { get; set; } //Id de outra Pessoa
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string Sexo { get; set; }
        public int Idade { get; set; }
        public PessoaViewModel? PessoaResponsavel { get; set; }
        public EnderecoViewModel Endereco { get; set; }
        public List<TelefoneViewModel> Telefones { get; set; }
    }
}
