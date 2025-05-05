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
        /// <remarks>
        /// NOTA: Este método tiene un error en el nombre del procedimiento almacenado,
        /// debería usar un procedimiento para productos, no para proveedores.
        /// </remarks>
        public DataSet showProducts()
        {
            MySqlDataAdapter objAdapter = new MySqlDataAdapter();
            DataSet objData = new DataSet();

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();
            objSelectCmd.CommandType = CommandType.StoredProcedure;
            objAdapter.SelectCommand = objSelectCmd;
            objAdapter.Fill(objData);
            objPer.closeConnection();
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
            int row;

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();

            // Asignar el nombre del procedimiento almacenado que inserta productos
            objSelectCmd.CommandText = "spInsertProducts"; // <-- Asegúrate de que este procedimiento exista en MySQL
            objSelectCmd.CommandType = CommandType.StoredProcedure;

            // Agregar los parámetros requeridos por el procedimiento con sus respectivos valores
            objSelectCmd.Parameters.Add("p_code", MySqlDbType.VarString).Value = _code;               
            objSelectCmd.Parameters.Add("p_description", MySqlDbType.VarString).Value = _description; 
            objSelectCmd.Parameters.Add("p_quantity", MySqlDbType.Int32).Value = _quantity;           
            objSelectCmd.Parameters.Add("p_price", MySqlDbType.Double).Value = _price;                
            objSelectCmd.Parameters.Add("p_fkcategory", MySqlDbType.Int32).Value = _fkCategory;       
            objSelectCmd.Parameters.Add("p_fkprovider", MySqlDbType.Int32).Value = _fkProvider;      

            try
            {
                // Ejecutar el comando y obtener cuántas filas fueron afectadas
                row = objSelectCmd.ExecuteNonQuery();

                if (row == 1)
                {
                    executed = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e.ToString());
            }


            objPer.closeConnection();
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

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();

            // Se asigna el nombre del procedimiento almacenado encargado de actualizar productos
            objSelectCmd.CommandText = "spUpdateProduct"; // <-- Asegúrate de que este procedimiento exista en tu base de datos
            objSelectCmd.CommandType = CommandType.StoredProcedure;

            // Se agregan los parámetros requeridos por el procedimiento almacenado con sus valores correspondientes
            objSelectCmd.Parameters.Add("p_id", MySqlDbType.Int32).Value = _id;                      
            objSelectCmd.Parameters.Add("p_code", MySqlDbType.VarString).Value = _code;              
            objSelectCmd.Parameters.Add("p_description", MySqlDbType.VarString).Value = _description;
            objSelectCmd.Parameters.Add("p_quantity", MySqlDbType.Int32).Value = _quantity;          
            objSelectCmd.Parameters.Add("p_price", MySqlDbType.Double).Value = _price;               
            objSelectCmd.Parameters.Add("p_fkcategory", MySqlDbType.Int32).Value = _fkCategory;      
            objSelectCmd.Parameters.Add("p_fkprovider", MySqlDbType.Int32).Value = _fkProvider;      

            try
            {
                // Se ejecuta el procedimiento almacenado y se obtiene cuántas filas fueron afectadas
                row = objSelectCmd.ExecuteNonQuery();

                if (row == 1)
                {
                    executed = true;
                }
            }
            catch (Exception e)
            {
                // Si ocurre una excepción, se muestra el mensaje de error en consola
                Console.WriteLine("Error " + e.ToString());
            }

            objPer.closeConnection();
            return executed;
        }


    }
}
