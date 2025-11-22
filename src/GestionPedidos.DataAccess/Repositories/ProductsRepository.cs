using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GestionPedidos.DataAccess.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        public IEnumerable<ProductListDto> ReadAllProducts()
        {
            var products = new List<ProductListDto>();

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT
                            p.idProduct,
                            p.productName,
                            p.salePrice,
                            p.stockQuantity,
                            p.isActive,
                            uCreated.userName AS CreatedBy,
                            uModified.userName AS ModifiedBy
                        FROM Products AS p
                        LEFT JOIN Users AS uCreated ON p.userCreation = uCreated.idUser
                        LEFT JOIN Users AS uModified ON p.userModification = uModified.idUser";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var product = new ProductListDto
                                {
                                    IdProduct = Convert.ToInt32(reader["idProduct"]),
                                    ProductName = reader["productName"].ToString(),
                                    SalePrice = Convert.ToDecimal(reader["salePrice"]),
                                    StockQuantity = Convert.ToInt32(reader["stockQuantity"]),
                                    IsActive = Convert.ToBoolean(reader["isActive"]),
                                    CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null,
                                    ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? reader["ModifiedBy"].ToString() : null
                                };
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al leer registros en productos: {ex.Message}", ex);
            }

            return products;
        }
    }
}
