using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.ViewModels;

namespace WebAPI.Application.Interfaces
{
    public interface IPessoaService
    {
        PessoaViewModel GetPessoas();
        PessoaViewModel GetPessoaById(int id);
        void Add(PessoaViewModel pessoaViewModel);
        void Update(PessoaViewModel pessoaViewModel);
        void Delete(int id);

        
    }
}
