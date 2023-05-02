﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Model;

namespace WebAPI.Application.ViewModels
{
    public class PessoaViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public Guid? IdPessoaResponsavel { get; set; }
        public string SobreNome { get; set; }
        public string Sexo { get; set; }
        public int Idade { get; set; }
        public EnderecoViewModel Endereco { get; set; }
        public List<TelefoneViewModel> Telefones { get; set; }
    }
}
