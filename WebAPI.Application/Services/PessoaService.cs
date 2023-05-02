using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.Interfaces;
using WebAPI.Application.ViewModels;
using WebAPI.Domain.Interface;
using WebAPI.Domain.Model;

namespace WebAPI.Application.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IMapper _mapper;

        public PessoaService(IPessoaRepository pessoaRepository, IMapper mapper)
        {
            _mapper = mapper;
            _pessoaRepository = pessoaRepository;
        }

        public PessoaViewModel GetPessoas()
        {
            var pessoas = _pessoaRepository.Get();
            return _mapper.Map<PessoaViewModel>(pessoas);
        }

        public PessoaViewModel GetPessoaById(Guid id)
        {
            var pessoa = _pessoaRepository.Find(id);
            return _mapper.Map<PessoaViewModel>(pessoa);
        }

        public void Add(PessoaViewModel pessoaViewModel)
        {
            var pessoa = _mapper.Map<Pessoa>(pessoaViewModel);
            _pessoaRepository.Add(pessoa);
        }

        public void Update(PessoaViewModel pessoaViewModel)
        {
            if (pessoaViewModel == null)
            {
                throw new Exception("Pessoa não encontrada");
            }

            var pessoaExistente = GetPessoaById(pessoaViewModel.Id);

            if (pessoaExistente == null)
            {
                throw new Exception("Pessoa não encontrada");
            }

            pessoaExistente.Nome = pessoaViewModel.Nome;
            pessoaExistente.IdPessoaResponsavel = pessoaViewModel.IdPessoaResponsavel;
            pessoaExistente.SobreNome = pessoaViewModel.SobreNome;
            pessoaExistente.Sexo = pessoaViewModel.Sexo;
            pessoaExistente.Idade = pessoaViewModel.Idade;
            pessoaExistente.Endereco = pessoaViewModel.Endereco;
            pessoaExistente.Telefones = pessoaViewModel.Telefones;

            var pessoa = _mapper.Map<Pessoa>(pessoaViewModel);
            _pessoaRepository.Update(pessoa);
        }

        public void Delete(Guid id)
        {
            var pessoa = _pessoaRepository.Find(id);
            _pessoaRepository.Delete(pessoa);
        }
    }
}
