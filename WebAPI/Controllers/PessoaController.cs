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
        public ActionResult<IEnumerable<PessoaViewModel>> Get()
        {
            var pessoas = _pessoaService.GetPessoas();
            return Ok(pessoas);
        }

        [HttpGet("{id}", Name = "GetPessoa")]
        public ActionResult<PessoaViewModel> GetById(Guid id)
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
        public ActionResult Delete(Guid id)
        {
            _pessoaService.Delete(id);

            return NoContent();
        }
    }
}
