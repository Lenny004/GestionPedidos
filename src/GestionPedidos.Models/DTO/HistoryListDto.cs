using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.DTO
{
    public class HistoryListDto
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string User { get; set; }
        public DateTime Date { get; set; } // CreatedAt
           
        public string Action { get; set; } // ActionType (INSERT, UPDATE...)
        public string Table { get; set; }  // AffectedTable
        
        public int RecordId { get; set; } // ID del registro afectado
    }
}
