using WebAPI.Domain.Model;
using System;

namespace WebAPI.Domain.Interface
{
    public interface IPessoaRepository : IRepository<Pessoa>, IDisposable
    {

    }
}
