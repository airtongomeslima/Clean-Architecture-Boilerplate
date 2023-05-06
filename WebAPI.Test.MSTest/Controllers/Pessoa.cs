using Moq;
using WebAPI.Application.Interfaces;
using WebAPI.Application.ViewModels;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebAPI.Test.MSTest.Controllers
{
    [TestClass]
    public class PessoaControllerTests
    {
        private Mock<IPessoaService> _pessoaServiceMock;
        private PessoaController _pessoaController;

        [TestInitialize]
        public void Setup()
        {
            _pessoaServiceMock = new Mock<IPessoaService>();
            _pessoaController = new PessoaController(_pessoaServiceMock.Object);
        }

        [TestMethod]
        public void Get_ReturnsOkWithPessoas()
        {
            // Arrange
            // Arrange
            var pessoas = new List<PessoaViewModel>
                {
                    new PessoaViewModel { Id = 1, Nome = "Fulano" },
                    new PessoaViewModel { Id = 2, Nome = "Ciclano" },
                    new PessoaViewModel { Id = 3, Nome = "Beltrano" },
                }.ToArray();
            _pessoaServiceMock.Setup(s => s.GetPessoas()).Returns(pessoas);

            // Act
            var result = _pessoaController.Get();
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.IsTrue(okResult.Value is PessoaViewModel[]);
            var pessoasRetornadas = okResult.Value as PessoaViewModel[];
            CollectionAssert.AreEqual(pessoas, pessoasRetornadas);
        }

        public void GetById_ReturnsOkWithPessoaById()
        {
            // Arrange
            var pessoa = new PessoaViewModel { Id = 1, Nome = "Fulano" };
            _pessoaServiceMock.Setup(s => s.GetPessoaById(1)).Returns(pessoa);

            // Act
            var result = _pessoaController.GetById(1);
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.IsTrue(okResult.Value is PessoaViewModel);
            var pessoaRetornada = okResult.Value as PessoaViewModel;
            Assert.AreEqual(pessoa, pessoaRetornada);

        }

        [TestMethod]
        public void GetById_ReturnsNotFound()
        {
            // Arrange
            _pessoaServiceMock.Setup(s => s.GetPessoaById(1)).Returns((PessoaViewModel)null);

            // Act
            var result = _pessoaController.GetById(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Create_ReturnsCreatedAtRouteWithPessoa()
        {
            // Arrange
            var pessoa = new PessoaViewModel { Id = 1, Nome = "Fulano" };

            // Act
            var result = _pessoaController.Create(pessoa);
            var createdAtRouteResult = result as CreatedAtRouteResult;

            // Assert
            Assert.IsNotNull(createdAtRouteResult);
            Assert.AreEqual("GetPessoa", createdAtRouteResult.RouteName);
            Assert.AreEqual(pessoa.Id, createdAtRouteResult.RouteValues["id"]);
            Assert.AreEqual(pessoa, createdAtRouteResult.Value);
            _pessoaServiceMock.Verify(s => s.Add(pessoa), Times.Once);
        }

        [TestMethod]
        public void Update_ReturnsNoContentAndUpdatesPessoa()
        {
            // Arrange
            var pessoa = new PessoaViewModel { Id = 1, Nome = "Fulano" };

            // Act
            var result = _pessoaController.Update(pessoa);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _pessoaServiceMock.Verify(s => s.Update(pessoa), Times.Once);
        }

        [TestMethod]
        public void Delete_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            _pessoaServiceMock.Setup(s => s.GetPessoaById(1)).Returns((PessoaViewModel)null);

            // Act
            var result = _pessoaController.Delete(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

    }

}
