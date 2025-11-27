using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.Entities
{
    public class History
    {
        public int IdHistory { get; set; }
        public string AffectedTable { get; set; }
        public string ActionType { get; set; }
        public int RecordId { get; set; }
        public string Description { get; set; }
        public int IdUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
