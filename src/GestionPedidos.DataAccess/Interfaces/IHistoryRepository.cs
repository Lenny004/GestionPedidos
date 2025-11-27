using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionPedidos.Models.DTO;
using System.Collections.Generic;

namespace GestionPedidos.DataAccess.Interfaces
{
    // Hacer la interfaz pública para que sea accesible desde los controladores u otros assemblies.
    public interface IHistoryRepository
    {
        // Declaración del método que devuelve la lista de historial.
        IEnumerable<HistoryListDto> ReadAllHistory();
        HistoryListDto ReadOne(int idHistory);
    }
}
