using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_Core_1_0.Models
{
    public class Cliente
    {
        public int ID { get; set; }
        public string Empresa { get; set; }
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Endereco { get; set; }
        public string VendedorId { get; set; }
    }
}
