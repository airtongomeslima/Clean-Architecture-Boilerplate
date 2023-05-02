using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Interface;
using WebAPI.Domain.Model;
using WebAPI.Infra.Data.Context;

namespace WebAPI.Infra.Data.Repository
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly AppDbContext _context;

        public PessoaRepository()
        {
            _context = new AppDbContext();
        }

        public IEnumerable<Pessoa> Get()
        {
            return _context.Pessoas.ToList();
        }

        public Pessoa Find(int id)
        {
            return _context.Pessoas.Find(id);
        }

        public void Add(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            _context.SaveChanges();
        }

        public void Update(Pessoa pessoa)
        {
            _context.Pessoas.Update(pessoa);
            _context.SaveChanges();
        }

        public void Delete(Pessoa pessoa)
        {
            _context.Pessoas.Remove(pessoa);
            _context.SaveChanges();
        }
    }
}
