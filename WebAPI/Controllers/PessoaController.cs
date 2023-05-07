using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.Interfaces;
using WebAPI.Application.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;

        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PessoaViewModel>> Get(int page = 1, int quantity = 25, string orderBy = "Id", string order = "asc")
        {
            var pessoas = _pessoaService.GetPessoas(page, quantity, orderBy, order);
            return Ok(pessoas);
        }

        [HttpGet("Count")]
        public ActionResult<int> Count()
        {
            var qtd = _pessoaService.Count();
            return Ok(qtd);
        }

        [HttpGet("{id}", Name = "GetPessoa")]
        public ActionResult<PessoaViewModel> GetById(int id)
        {
            var pessoa = _pessoaService.GetPessoaById(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return Ok(pessoa);
        }

        [HttpPost]
        public ActionResult Create(PessoaViewModel pessoa)
        {
            _pessoaService.Add(pessoa);

            return CreatedAtRoute("GetPessoa", new { id = pessoa.Id }, pessoa);
        }

        [HttpPut("{id}")]
        public ActionResult Update(PessoaViewModel pessoa)
        {

            _pessoaService.Update(pessoa);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _pessoaService.Delete(id);

            return NoContent();
        }
    }
}
