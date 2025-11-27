using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.Models.DTO;
using GestionPedidos.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static GestionPedidos.Common.Constants.Messages;

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

        public Product ReadOne(int id)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT
                            p.idProduct,
                            p.productName,
                            p.description,
                            p.stockQuantity,
                            p.salePrice,
                            p.isActive,
                            p.createdAt,
                            p.updatedAt,
                            p.deletedAt,
                            uCreated.userName AS CreatedBy,
                            uModified.userName AS ModifiedBy
                        FROM Products AS p
                        LEFT JOIN Users AS uCreated ON p.userCreation = uCreated.idUser
                        LEFT JOIN Users AS uModified ON p.userModification = uModified.idUser
                        WHERE p.idProduct = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Product
                                {
                                    IdProduct = Convert.ToInt32(reader["idProduct"]),
                                    ProductName = reader["productName"].ToString(),
                                    Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : string.Empty,
                                    SalePrice = Convert.ToDecimal(reader["salePrice"]),
                                    StockQuantity = Convert.ToInt32(reader["stockQuantity"]),
                                    UserCreation = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null,
                                    UserModification = reader["ModifiedBy"] != DBNull.Value ? reader["ModifiedBy"].ToString() : null,
                                    IsActive = Convert.ToBoolean(reader["isActive"]),
                                    CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                                    UpdatedAt = reader["updatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["updatedAt"]) : null,
                                    DeletedAt = reader["deletedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["deletedAt"]) : null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el producto: {ex.Message}", ex);
            }

            return null;
        }

        // Modificamos para recibir userId
        public bool Create(Product product, int userId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    // Usamos @UserId para userCreation
                    string query = @"INSERT INTO Products (productName, description, stockQuantity, salePrice, isActive, userCreation, createdAt)
                                    VALUES (@ProductName, @Description, @StockQuantity, @SalePrice, 1, @UserId, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                        cmd.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        cmd.Parameters.AddWithValue("@SalePrice", product.SalePrice);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear producto: {ex.Message}", ex);
            }
        }

        // Modificamos para recibir userId
        public bool Update(Product product, int userId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    // Usamos @UserId para userModification
                    string query = @"UPDATE Products
                                     SET productName = @ProductName,
                                         description = @Description,
                                         stockQuantity = @StockQuantity,
                                         salePrice = @SalePrice,
                                         isActive = @IsActive,
                                         userModification = @UserId, 
                                         updatedAt = GETDATE()
                                     WHERE idProduct = @IdProduct";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdProduct", product.IdProduct);
                        cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                        cmd.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        cmd.Parameters.AddWithValue("@SalePrice", product.SalePrice);
                        cmd.Parameters.AddWithValue("@IsActive", product.IsActive);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar producto: {ex.Message}", ex);
            }
        }

        public bool Delete(int id, int userId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Products 
                                     SET isActive = 0, 
                                         userModification = @UserId,
                                         updatedAt = GETDATE(),
                                         deletedAt = GETDATE() 
                                     WHERE idProduct = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar producto: {ex.Message}", ex);
            }
        }

        public IEnumerable<ProductListDto> SearchProducts(string productName)
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
                        LEFT JOIN Users AS uModified ON p.userModification = uModified.idUser
                        WHERE p.productName LIKE @ProductName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", $"%{productName}%");

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
                throw new Exception($"Error al buscar producto por nombre: {ex.Message}", ex);
            }
            return products;
        }

        public IEnumerable<ProductSelectDto> ReadProductsForCombo()
        {
            var products = new List<ProductSelectDto>();
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
                            p.stockQuantity
                        FROM Products AS p
                        WHERE p.isActive = 1 AND p.stockQuantity > 0
                        ORDER BY p.productName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductSelectDto
                            {
                                IdProduct = Convert.ToInt32(reader["idProduct"]),
                                ProductName = reader["productName"].ToString(),
                                SalePrice = Convert.ToDecimal(reader["salePrice"]),
                                StockQuantity = Convert.ToInt32(reader["stockQuantity"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener productos para el combo: {ex.Message}", ex);
            }

            return products;
        }

        /// <summary>
        /// Reduce el stock de un producto
        /// </summary>
        public bool ReduceStock(int productId, int quantity)
        {
            string query = @"
                UPDATE Products
                SET stockQuantity = stockQuantity - @Quantity
                WHERE idProduct = @ProductId AND stockQuantity >= @Quantity";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al reducir stock del producto {productId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Incrementa el stock de un producto
        /// </summary>
        public bool IncreaseStock(int productId, int quantity)
        {
            string query = @"
                UPDATE Products
                SET stockQuantity = stockQuantity + @Quantity
                WHERE idProduct = @ProductId";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al incrementar stock del producto {productId}: {ex.Message}", ex);
            }
        }
    }
}
