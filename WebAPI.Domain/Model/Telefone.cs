using System;

namespace WebAPI.Domain.Model
{
    public class Telefone : Entity
    {
        public int IdPessoa { get; set; }
        public string DDD { get; set; }
        public string Numero { get; set; }
    }
}
