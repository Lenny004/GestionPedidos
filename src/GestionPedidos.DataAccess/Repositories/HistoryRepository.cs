using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.Models.DTO;


namespace GestionPedidos.DataAccess.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        public IEnumerable<HistoryListDto> ReadAllHistory()
        {
            var historyLog = new List<HistoryListDto>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    // Ordenamos por fecha descendente (lo más nuevo primero)
                    string query = @"
                        SELECT 
                            h.idHistory, 
                            h.createdAt, 
                            u.userName AS UserName, 
                            h.actionType, 
                            h.affectedTable, 
                            h.description,
                            h.recordId
                        FROM History h
                        INNER JOIN Users u ON h.idUser = u.idUser
                        ORDER BY h.createdAt DESC";

                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            historyLog.Add(new HistoryListDto
                            {
                                Id = Convert.ToInt32(reader["idHistory"]),
                                Date = Convert.ToDateTime(reader["createdAt"]),
                                User = reader["UserName"].ToString(),
                                Action = reader["actionType"].ToString(),
                                Table = reader["affectedTable"].ToString(),
                                Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : "",
                                RecordId = Convert.ToInt32(reader["recordId"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex) { throw new Exception("Error al leer el historial", ex); }
            return historyLog;
        }

        public HistoryListDto ReadOne(int idHistory)
        {
            HistoryListDto historyItem = null;
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    h.idHistory, h.createdAt, u.userName AS UserName, 
                    h.actionType, h.affectedTable, h.description, h.recordId
                FROM History h
                INNER JOIN Users u ON h.idUser = u.idUser
                WHERE h.idHistory = @Id";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", idHistory);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                historyItem = new HistoryListDto
                                {
                                    Id = Convert.ToInt32(reader["idHistory"]),
                                    Date = Convert.ToDateTime(reader["createdAt"]),
                                    User = reader["UserName"].ToString(),
                                    Action = reader["actionType"].ToString(),
                                    Table = reader["affectedTable"].ToString(),
                                    Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : "",
                                    RecordId = Convert.ToInt32(reader["recordId"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { throw new Exception("Error al leer detalle del historial", ex); }
            return historyItem;
        }
    }

}