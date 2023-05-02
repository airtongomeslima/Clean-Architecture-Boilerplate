using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Domain.Model
{
    public class Telefone
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public string DDD { get; set; }
        public string Numero { get; set; }
    }
}
