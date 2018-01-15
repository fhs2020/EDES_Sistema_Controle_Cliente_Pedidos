using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_Core_1_0.Models
{
    public class Pedidos
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public String ClienteNome { get; set; }
        public DateTime? DataPedido { get; set; }
        public List<ItemPedido> ListaItems { get; set; }

    }
}
