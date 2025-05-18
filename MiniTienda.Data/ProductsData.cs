/**
 * Proyecto MiniTienda - Capa de Acceso a Datos
 * 
 * Implementación del patrón DAO (Data Access Object) para la gestión de productos.
 * Esta clase proporciona métodos CRUD (Create, Read, Update) para manipular
 * los datos de productos en la base de datos MySQL.
 * 
 * Autor: Elkin
 * Fecha: 02/05/2025
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using MySql.Data.MySqlClient;

namespace MiniTienda.Data
{
    /// <summary>
    /// Clase para gestionar operaciones de datos de productos en la base de datos.
    /// Implementa el patrón DAO para abstraer la lógica de acceso a datos de la lógica de negocio.
    /// Utiliza procedimientos almacenados para todas las operaciones.
    /// </summary>
    public class ProductsData
    {
        /// <summary>
        /// Instancia de la clase Persistence que proporciona acceso a la base de datos
        /// </summary>
        private readonly Persistence objPer = new Persistence();

        /// <summary>
        /// Obtiene todos los productos almacenados en la base de datos.
        /// Ejecuta el procedimiento almacenado que recupera la lista de productos.
        /// </summary>
        /// <returns>DataSet con los productos</returns>
        public DataSet showProducts()
        {
            MySqlDataAdapter objAdapter = new MySqlDataAdapter();
            DataSet objData = new DataSet();

            try
            {
                MySqlCommand objSelectCmd = new MySqlCommand();
                objSelectCmd.Connection = objPer.openConnection();
                objSelectCmd.CommandText = "spSelectProducts"; // Nombre del procedimiento almacenado
                objSelectCmd.CommandType = CommandType.StoredProcedure;
                
                // Configurar un timeout más largo para diagnosticar problemas de rendimiento
                objSelectCmd.CommandTimeout = 60; // 60 segundos
                
                Console.WriteLine("Ejecutando procedimiento almacenado: spSelectProducts");
                
                // Como alternativa, si el procedimiento almacenado no funciona, podemos usar una consulta SQL directa
                try
                {
                    objAdapter.SelectCommand = objSelectCmd;
                    objAdapter.Fill(objData);
                    
                    // Imprimir información sobre el resultado
                    if (objData != null && objData.Tables.Count > 0)
                    {
                        Console.WriteLine($"Datos recibidos: {objData.Tables[0].Rows.Count} filas");
                        
                        // Usar una lista para las columnas en lugar de Cast<T>()
                        List<string> columnNames = new List<string>();
                        foreach (DataColumn col in objData.Tables[0].Columns)
                        {
                            columnNames.Add(col.ColumnName);
                        }
                        Console.WriteLine($"Columnas disponibles: {string.Join(", ", columnNames)}");
                        
                        // Imprimir algunos datos de muestra
                        if (objData.Tables[0].Rows.Count > 0)
                        {
                            Console.WriteLine("Primera fila de datos:");
                            foreach (DataColumn col in objData.Tables[0].Columns)
                            {
                                Console.WriteLine($"  {col.ColumnName}: {objData.Tables[0].Rows[0][col]}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se recibieron datos del procedimiento almacenado");
                        
                        // Si el procedimiento no devuelve datos, intentar con una consulta directa SQL
                        Console.WriteLine("Intentando con consulta SQL directa...");
                        
                        objSelectCmd = new MySqlCommand();
                        objSelectCmd.Connection = objPer.openConnection();
                        objSelectCmd.CommandText = @"
                            SELECT 
                                p.pro_id, p.pro_codigo, p.pro_descripcion, p.pro_cantidad, p.pro_precio,
                                p.tbl_categorias_cat_id, c.cat_descripcion, 
                                p.tbl_proveedores_prov_id, pv.prov_nombre
                            FROM tbl_productos p
                            LEFT JOIN tbl_categorias c ON p.tbl_categorias_cat_id = c.cat_id
                            LEFT JOIN tbl_proveedores pv ON p.tbl_proveedores_prov_id = pv.prov_id
                            ORDER BY p.pro_id";
                        objSelectCmd.CommandType = CommandType.Text;
                        
                        objAdapter = new MySqlDataAdapter();
                        objAdapter.SelectCommand = objSelectCmd;
                        objData = new DataSet();
                        objAdapter.Fill(objData);
                        
                        if (objData != null && objData.Tables.Count > 0)
                        {
                            Console.WriteLine($"Datos recibidos con consulta directa: {objData.Tables[0].Rows.Count} filas");
                            
                            // Usar una lista para las columnas en lugar de Cast<T>()
                            List<string> columnNames = new List<string>();
                            foreach (DataColumn col in objData.Tables[0].Columns)
                            {
                                columnNames.Add(col.ColumnName);
                            }
                            Console.WriteLine($"Columnas disponibles: {string.Join(", ", columnNames)}");
                        }
                        else
                        {
                            Console.WriteLine("No se recibieron datos tampoco con la consulta SQL directa");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al ejecutar procedimiento almacenado: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general en showProducts: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                throw;
            }
            finally
            {
                objPer.closeConnection();
            }
            
            return objData;
        }

        /// <summary>
        /// Guarda un nuevo producto en la base de datos.
        /// Ejecuta el procedimiento almacenado 'spInsertProducts'.
        /// </summary>
        /// <param name="_code">Código del producto</param>
        /// <param name="_description">Descripción del producto</param>
        /// <param name="_quantity">Cantidad en inventario</param>
        /// <param name="_price">Precio del producto</param>
        /// <param name="_fkCategory">ID de la categoría asociada</param>
        /// <param name="_fkProvider">ID del proveedor asociado</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        public bool saveProducts(string _code, string _description, int _quantity, double _price, int _fkCategory, int _fkProvider)
        {
            bool executed = false;
            int row = 0;

            Console.WriteLine($"Intentando guardar producto: Código={_code}, Descripción={_description}, Cantidad={_quantity}, Precio={_price}, CategoríaID={_fkCategory}, ProveedorID={_fkProvider}");

            try
            {
                // Primer intento: usar el procedimiento almacenado
                try
                {
                    MySqlCommand objSelectCmd = new MySqlCommand();
                    objSelectCmd.Connection = objPer.openConnection();

                    // Asignar el nombre del procedimiento almacenado que inserta productos
                    objSelectCmd.CommandText = "spInsertProducts";
                    objSelectCmd.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros requeridos por el procedimiento con sus respectivos valores
                    objSelectCmd.Parameters.Add("p_code", MySqlDbType.VarString).Value = _code;               
                    objSelectCmd.Parameters.Add("p_description", MySqlDbType.VarString).Value = _description; 
                    objSelectCmd.Parameters.Add("p_quantity", MySqlDbType.Int32).Value = _quantity;           
                    objSelectCmd.Parameters.Add("p_price", MySqlDbType.Double).Value = _price;                
                    objSelectCmd.Parameters.Add("p_fkcategory", MySqlDbType.Int32).Value = _fkCategory;       
                    objSelectCmd.Parameters.Add("p_fkprovider", MySqlDbType.Int32).Value = _fkProvider;      

                    // Ejecutar el comando y obtener cuántas filas fueron afectadas
                    row = objSelectCmd.ExecuteNonQuery();
                    Console.WriteLine($"Resultado del procedimiento almacenado: {row} filas afectadas");

                    if (row == 1)
                    {
                        executed = true;
                        Console.WriteLine("Producto guardado exitosamente mediante procedimiento almacenado");
                        objPer.closeConnection();
                        return executed;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error al guardar producto con procedimiento almacenado: {e.Message}");
                    Console.WriteLine("Intentando guardar con SQL directo...");
                }

                // Segundo intento: usar SQL directo
                try
                {
                    MySqlCommand objDirectInsertCmd = new MySqlCommand();
                    objDirectInsertCmd.Connection = objPer.openConnection();

                    // Consulta SQL para insertar un producto
                    string insertQuery = @"
                        INSERT INTO tbl_productos (
                            pro_codigo, 
                            pro_descripcion, 
                            pro_cantidad, 
                            pro_precio, 
                            tbl_categorias_cat_id, 
                            tbl_proveedores_prov_id
                        ) VALUES (
                            @codigo, 
                            @descripcion, 
                            @cantidad, 
                            @precio, 
                            @categoria_id, 
                            @proveedor_id
                        )";

                    objDirectInsertCmd.CommandText = insertQuery;
                    objDirectInsertCmd.CommandType = CommandType.Text;

                    // Agregar parámetros para SQL directo
                    objDirectInsertCmd.Parameters.Add("@codigo", MySqlDbType.VarString).Value = _code;
                    objDirectInsertCmd.Parameters.Add("@descripcion", MySqlDbType.VarString).Value = _description;
                    objDirectInsertCmd.Parameters.Add("@cantidad", MySqlDbType.Int32).Value = _quantity;
                    objDirectInsertCmd.Parameters.Add("@precio", MySqlDbType.Double).Value = _price;
                    objDirectInsertCmd.Parameters.Add("@categoria_id", MySqlDbType.Int32).Value = _fkCategory;
                    objDirectInsertCmd.Parameters.Add("@proveedor_id", MySqlDbType.Int32).Value = _fkProvider;

                    // Ejecutar la consulta directa
                    row = objDirectInsertCmd.ExecuteNonQuery();
                    Console.WriteLine($"Resultado de la consulta SQL directa: {row} filas afectadas");

                    if (row == 1)
                    {
                        executed = true;
                        Console.WriteLine("Producto guardado exitosamente mediante SQL directo");
                    }
                    else
                    {
                        Console.WriteLine($"No se pudo guardar el producto. Filas afectadas: {row}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error al guardar producto con SQL directo: {e.Message}");
                    if (e.InnerException != null)
                    {
                        Console.WriteLine($"Error interno: {e.InnerException.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error general al guardar producto: {e.ToString()}");
            }
            finally
            {
                objPer.closeConnection();
            }

            return executed;
        }

        /// <summary>
        /// Actualiza los datos de un producto existente en la base de datos.
        /// Ejecuta el procedimiento almacenado 'spUpdateProduct'.
        /// </summary>
        /// <param name="_id">ID del producto a actualizar</param>
        /// <param name="_code">Nuevo código del producto</param>
        /// <param name="_description">Nueva descripción</param>
        /// <param name="_quantity">Nueva cantidad en inventario</param>
        /// <param name="_price">Nuevo precio</param>
        /// <param name="_fkCategory">Nueva categoría asociada</param>
        /// <param name="_fkProvider">Nuevo proveedor asociado</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        public bool updateProducts(int _id, string _code, string _description, int _quantity, double _price, int _fkCategory, int _fkProvider)
        {
            bool executed = false; 
            int row;  

            try
            {
                // Primer intento: Usar el procedimiento almacenado
                MySqlCommand objUpdateCmdSp = new MySqlCommand();
                objUpdateCmdSp.Connection = objPer.openConnection();
                objUpdateCmdSp.CommandText = "spUpdateProduct"; 
                objUpdateCmdSp.CommandType = CommandType.StoredProcedure;

                objUpdateCmdSp.Parameters.Add("p_id", MySqlDbType.Int32).Value = _id;                      
                objUpdateCmdSp.Parameters.Add("p_code", MySqlDbType.VarString).Value = _code;              
                objUpdateCmdSp.Parameters.Add("p_description", MySqlDbType.VarString).Value = _description;
                objUpdateCmdSp.Parameters.Add("p_quantity", MySqlDbType.Int32).Value = _quantity;          
                objUpdateCmdSp.Parameters.Add("p_price", MySqlDbType.Double).Value = _price;               
                objUpdateCmdSp.Parameters.Add("p_fkcategory", MySqlDbType.Int32).Value = _fkCategory;      
                objUpdateCmdSp.Parameters.Add("p_fkprovider", MySqlDbType.Int32).Value = _fkProvider;      

                row = objUpdateCmdSp.ExecuteNonQuery();

                if (row == 1)
                {
                    executed = true;
                    Console.WriteLine($"Producto con ID {_id} actualizado correctamente mediante procedimiento almacenado.");
                    return executed; // Salir si el SP tuvo éxito
                }
                else
                {
                    Console.WriteLine($"Procedimiento almacenado spUpdateProduct no afectó filas para el producto ID {_id}. Intentando con SQL directo.");
                }
            }
            catch (Exception eSp)
            {
                Console.WriteLine($"Error al actualizar producto con procedimiento almacenado: {eSp.Message}. Intentando con SQL directo.");
            }
            finally
            {
                // Cerrar la conexión solo si no se va a reintentar o si ya se terminó
                if (executed) objPer.closeConnection();
            }

            // Segundo intento: Usar SQL directo si el SP falló o no afectó filas
            if (!executed)
            {
                try
                {
                    MySqlCommand objUpdateCmdSql = new MySqlCommand();
                    objUpdateCmdSql.Connection = objPer.openConnection(); // Reabrir o asegurar que esté abierta
                    
                    string updateQuery = @"
                        UPDATE tbl_productos SET
                            pro_codigo = @code,
                            pro_descripcion = @description,
                            pro_cantidad = @quantity,
                            pro_precio = @price,
                            tbl_categorias_cat_id = @fkCategory,
                            tbl_proveedores_prov_id = @fkProvider
                        WHERE pro_id = @id";
                    
                    objUpdateCmdSql.CommandText = updateQuery;
                    objUpdateCmdSql.CommandType = CommandType.Text;

                    objUpdateCmdSql.Parameters.Add("@id", MySqlDbType.Int32).Value = _id;
                    objUpdateCmdSql.Parameters.Add("@code", MySqlDbType.VarString).Value = _code;
                    objUpdateCmdSql.Parameters.Add("@description", MySqlDbType.VarString).Value = _description;
                    objUpdateCmdSql.Parameters.Add("@quantity", MySqlDbType.Int32).Value = _quantity;
                    objUpdateCmdSql.Parameters.Add("@price", MySqlDbType.Double).Value = _price;
                    objUpdateCmdSql.Parameters.Add("@fkCategory", MySqlDbType.Int32).Value = _fkCategory;
                    objUpdateCmdSql.Parameters.Add("@fkProvider", MySqlDbType.Int32).Value = _fkProvider;

                    row = objUpdateCmdSql.ExecuteNonQuery();

                    if (row == 1)
                    {
                        executed = true;
                        Console.WriteLine($"Producto con ID {_id} actualizado correctamente mediante SQL directo.");
                    }
                    else
                    {
                        Console.WriteLine($"SQL directo no afectó filas para el producto ID {_id}.");
                    }
                }
                catch (Exception eSql)
                {
                    Console.WriteLine($"Error al actualizar producto con SQL directo: {eSql.ToString()}");
                }
                finally
                {
                    objPer.closeConnection();
                }
            }
            return executed;
        }

        /// <summary>
        /// Elimina un producto existente de la base de datos.
        /// </summary>
        /// <param name="_id">ID del producto a eliminar</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        public bool deleteProduct(int _id)
        {
            bool executed = false;
            int row;

            try
            {
                // Primera opción: intentar usar el procedimiento almacenado
                try
                {
                    MySqlCommand objDeleteCmd = new MySqlCommand();
                    objDeleteCmd.Connection = objPer.openConnection();

                    // Nombre del procedimiento almacenado para eliminar productos
                    objDeleteCmd.CommandText = "spDeleteProduct";
                    objDeleteCmd.CommandType = CommandType.StoredProcedure;

                    // Parámetro para el ID del producto a eliminar
                    objDeleteCmd.Parameters.Add("p_id", MySqlDbType.Int32).Value = _id;

                    // Ejecutar el comando
                    row = objDeleteCmd.ExecuteNonQuery();

                    if (row == 1)
                    {
                        executed = true;
                        Console.WriteLine($"Producto con ID {_id} eliminado correctamente mediante procedimiento almacenado");
                        return executed;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar eliminar producto con procedimiento almacenado: {ex.Message}");
                    Console.WriteLine("Intentando con consulta SQL directa...");
                }

                // Segunda opción: si el procedimiento almacenado falla, usar consulta SQL directa
                MySqlCommand objDeleteDirectCmd = new MySqlCommand();
                objDeleteDirectCmd.Connection = objPer.openConnection();

                // Consulta SQL para eliminar el producto por ID
                objDeleteDirectCmd.CommandText = "DELETE FROM tbl_productos WHERE pro_id = @id";
                objDeleteDirectCmd.CommandType = CommandType.Text;
                objDeleteDirectCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = _id;

                // Ejecutar la consulta directa
                row = objDeleteDirectCmd.ExecuteNonQuery();

                if (row == 1)
                {
                    executed = true;
                    Console.WriteLine($"Producto con ID {_id} eliminado correctamente mediante consulta SQL directa");
                }
                else
                {
                    Console.WriteLine($"No se pudo eliminar el producto con ID {_id}. Filas afectadas: {row}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar producto: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
            }
            finally
            {
                objPer.closeConnection();
            }

            return executed;
        }
    }
}
