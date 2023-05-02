using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Model;

namespace WebAPI.Domain.Interface
{
    public interface IPessoaRepository
    {
        IEnumerable<Pessoa> Get();
        Pessoa Find(Guid id);
        void Add(Pessoa pessoa);
        void Update(Pessoa pessoa);
        void Delete(Pessoa pessoa);
    }
}
