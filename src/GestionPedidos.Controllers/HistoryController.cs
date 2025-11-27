using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.DataAccess.Repositories;
using GestionPedidos.Models.DTO;
using NLog;


namespace GestionPedidos.Controllers
{
    public class HistoryController
    {
        private readonly IHistoryRepository _historyRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public HistoryController()
        {
            _historyRepository = new HistoryRepository();
        }

        public (bool Success, string Message, IEnumerable<HistoryListDto> History) ReadAll()
        {
            try
            {
                var history = _historyRepository.ReadAllHistory();

                if (history == null)
                    return (false, "Error al conectar con el historial.", null);

                return (true, "Historial cargado.", history);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error leyendo historial");
                return (false, ex.Message, null);
            }
        }
        public (bool Success, string Message, HistoryListDto History) ReadOne(int id)
        {
            try
            {
                var history = _historyRepository.ReadOne(id);
                if (history == null) return (false, "Registro no encontrado.", null);
                return (true, "Encontrado.", history);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error leyendo historial ID {id}");
                return (false, ex.Message, null);
            }
        }
    }
}