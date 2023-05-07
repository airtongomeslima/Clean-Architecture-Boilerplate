using AutoMapper;
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

        public PessoaViewModel[] GetPessoas(int page = 1, int quantity = 25, string orderBy = "Id", string order = "asc")
        {
            var pessoas = _pessoaRepository.FindAll(page, quantity, orderBy, order);
            return _mapper.Map<Pessoa[], PessoaViewModel[]>(pessoas);
        }

        public PessoaViewModel GetPessoaById(int id)
        {
            var pessoa = _pessoaRepository.FindById(id);
            return _mapper.Map<Pessoa, PessoaViewModel>(pessoa);
        }

        public void Add(PessoaViewModel pessoaViewModel)
        {
            var pessoa = _mapper.Map<PessoaViewModel, Pessoa>(pessoaViewModel);
            _pessoaRepository.Create(pessoa);
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

            var pessoa = _mapper.Map<PessoaViewModel, Pessoa>(pessoaViewModel);
            _pessoaRepository.Update(pessoa);
        }

        public void Delete(int id)
        {
            var pessoa = _pessoaRepository.FindById(id);
            if(pessoa == null)
            {
                throw new Exception("Pessoa não encontrada");
            }
            _pessoaRepository.Delete(pessoa);
        }
    }
}
