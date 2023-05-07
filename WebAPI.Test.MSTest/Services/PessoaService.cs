using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.Mapper.Profiles;
using WebAPI.Application.Services;
using WebAPI.Application.ViewModels;
using WebAPI.Domain.Interface;
using WebAPI.Domain.Model;
using WebAPI.Test.MSTest.Extensions;

namespace WebAPI.Test.MSTest.Services
{
    [TestClass]
    public class PessoaServiceTests
    {
        private Mock<IPessoaRepository> _pessoaRepositoryMock;
        private IMapper _mapper;
        private PessoaService _pessoaService;

        [TestInitialize]
        public void Setup()
        {
            _pessoaRepositoryMock = new Mock<IPessoaRepository>();
            _mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile<PessoaProfile>();
                mc.AddProfile<TelefoneProfile>();
                mc.AddProfile<EnderecoProfile>();
            }).CreateMapper();
            _pessoaService = new PessoaService(_pessoaRepositoryMock.Object, _mapper);
        }

        [TestMethod]
        public void GetPessoas_ReturnsArrayOfPessoaViewModel()
        {
            // Arrange
            var pessoas = new List<Pessoa>
                {
                    new Pessoa { Id = 1, Nome = "Fulano" },
                    new Pessoa { Id = 2, Nome = "Ciclano" },
                    new Pessoa { Id = 3, Nome = "Beltrano" },
                }.ToArray();
            _pessoaRepositoryMock.Setup(r => r.FindAll(1, 3, "Id", "asc")).Returns(pessoas);

            // Act
            var result = _pessoaService.GetPessoas(1, 3, "Id", "asc");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is PessoaViewModel[]);
            var pessoasRetornadas = result as PessoaViewModel[];
            AssertExtensions.AreEqualByValue(_mapper.Map<Pessoa[], PessoaViewModel[]>(pessoas), pessoasRetornadas);
        }

        [TestMethod]
        public void GetPessoaById_ReturnsPessoaViewModel()
        {
            // Arrange
            var pessoa = new Pessoa { Id = 1, Nome = "Fulano" };
            var pessoaVM = _mapper.Map<Pessoa, PessoaViewModel>(pessoa);
            _pessoaRepositoryMock.Setup(r => r.FindById(1)).Returns(pessoa);

            // Act
            var result = _pessoaService.GetPessoaById(1);

            // Assert
            Assert.IsNotNull(result);
            AssertExtensions.AreEqualByValue(pessoaVM, result);
        }

        [TestMethod]
        public void Add_CreatesPessoa()
        {
            // Arrange
            var pessoa = new PessoaViewModel { Id = 1, Nome = "Fulano" };

            // Act
            _pessoaService.Add(pessoa);

            // Assert
            _pessoaRepositoryMock.Verify(r => r.Create(It.Is<Pessoa>(p => p.Id == pessoa.Id && p.Nome == pessoa.Nome)), Times.Once);
        }

        [TestMethod]
        public void Update_UpdatesPessoa()
        {
            // Arrange
            var pessoaViewModel = new PessoaViewModel { Id = 1, Nome = "Fulano" };
            var pessoa = new Pessoa { Id = 1, Nome = "Fulano" };
            _pessoaRepositoryMock.Setup(r => r.FindById(1)).Returns(pessoa);
            _pessoaRepositoryMock.Setup(r => r.Update(It.IsAny<Pessoa>())).Callback<Pessoa>(p => pessoa = p);

            // Act
            _pessoaService.Update(pessoaViewModel);

            // Assert
            _pessoaRepositoryMock.Verify(r => r.Update(It.Is<Pessoa>(p => p == pessoa)), Times.Once);
        }

        [TestMethod]
        public void Update_ThrowsExceptionIfPessoaIsNull()
        {
            // Arrange
            PessoaViewModel pessoaViewModel = null;

            // Act / Assert
            Assert.ThrowsException<Exception>(() => _pessoaService.Update(pessoaViewModel));
            _pessoaRepositoryMock.Verify(r => r.Update(It.IsAny<Pessoa>()), Times.Never);
        }

        [TestMethod]
        public void Update_ThrowsExceptionIfPessoaNotFound()
        {
            // Arrange
            var pessoaViewModel = new PessoaViewModel { Id = 1, Nome = "Fulano" };
            _pessoaRepositoryMock.Setup(r => r.FindById(1)).Returns((Pessoa)null);

            // Act / Assert
            Assert.ThrowsException<Exception>(() => _pessoaService.Update(pessoaViewModel));
            _pessoaRepositoryMock.Verify(r => r.Update(It.IsAny<Pessoa>()), Times.Never);
        }

        [TestMethod]
        public void Delete_DeletesPessoa()
        {
            // Arrange
            var id = 1;
            var pessoa = new Pessoa { Id = id, Nome = "Fulano" };
            _pessoaRepositoryMock.Setup(r => r.FindById(id)).Returns(pessoa);

            // Act
            _pessoaService.Delete(id);

            // Assert
            _pessoaRepositoryMock.Verify(r => r.Delete(It.Is<Pessoa>(p => p == pessoa)), Times.Once);
        }

        [TestMethod]
        public void Delete_ThrowsExceptionIfPessoaNotFound()
        {
            // Arrange
            var id = 1;
            _pessoaRepositoryMock.Setup(r => r.FindById(id)).Returns((Pessoa)null);

            // Act / Assert
            Assert.ThrowsException<Exception>(() => _pessoaService.Delete(id));
            _pessoaRepositoryMock.Verify(r => r.Delete(It.IsAny<Pessoa>()), Times.Never);
        }
    }



}
