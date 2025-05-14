    /**
     * Proyecto MiniTienda - Capa de Acceso a Datos
     * 
     * Implementación del patrón DAO (Data Access Object) para la gestión de proveedores.
     * Esta clase proporciona métodos CRUD (Create, Read, Update, Delete) para manipular
     * los datos de proveedores en la base de datos MySQL.
     * 
     * Autor: Brayan Esmid Cruz Chate
     * Fecha: Mayo 2024
     */

    using MySql.Data.MySqlClient;
    using System;
    using System.Data;

    namespace MiniTienda.Data
    {
        /// <summary>
        /// Clase para gestionar operaciones de datos de proveedores en la base de datos.
        /// Implementa el patrón DAO para abstraer la lógica de acceso a datos de la lógica de negocio.
        /// Utiliza procedimientos almacenados para todas las operaciones.
        /// </summary>
        public class ProvidersData
        {
            /// <summary>
            /// Instancia de la clase Persistence que proporciona acceso a la base de datos
            /// </summary>
            private readonly Persistence objPer = new Persistence();

            /// <summary>
            /// Obtiene todos los proveedores almacenados en la base de datos.
            /// Ejecuta el procedimiento almacenado 'spSelectProviders'.
            /// </summary>
            /// <returns>DataSet con los proveedores (columnas: prov_id, prov_nit, prov_nombre)</returns>
            public DataSet ShowProviders()
            {
                DataSet objData = new DataSet();
                MySqlDataAdapter objAdapter = new MySqlDataAdapter();
                MySqlCommand objSelectCmd = new MySqlCommand();

                objSelectCmd.Connection = objPer.openConnection();
                objSelectCmd.CommandText = "spSelectProviders";
                objSelectCmd.CommandType = CommandType.StoredProcedure;

                objAdapter.SelectCommand = objSelectCmd;
                objAdapter.Fill(objData);

                objPer.closeConnection();
                return objData;
            }

            /// <summary>
            /// Obtiene el ID y nombre de todos los proveedores para mostrar en controles DropDownList.
            /// Ejecuta el procedimiento almacenado 'spSelectProvidersDDL'.
            /// </summary>
            /// <returns>DataSet con los proveedores (columnas: prov_id, prov_nombre)</returns>
            public DataSet ShowProvidersDDL()
            {
                DataSet objData = new DataSet();
                MySqlDataAdapter objAdapter = new MySqlDataAdapter();
                MySqlCommand objSelectCmd = new MySqlCommand();

                objSelectCmd.Connection = objPer.openConnection();
                objSelectCmd.CommandText = "spSelectProvidersDDL";
                objSelectCmd.CommandType = CommandType.StoredProcedure;

                objAdapter.SelectCommand = objSelectCmd;
                objAdapter.Fill(objData);

                objPer.closeConnection();
                return objData;
            }

            /// <summary>
            /// Guarda un nuevo proveedor en la base de datos.
            /// Ejecuta el procedimiento almacenado 'spInsertProvider'.
            /// </summary>
            /// <param name="_nit">NIT del proveedor</param>
            /// <param name="_name">Nombre del proveedor</param>
            /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
            public bool SaveProvider(string _nit, string _name)
            {
                bool executed = false;
                int row;

                MySqlCommand objInsertCmd = new MySqlCommand();
                objInsertCmd.Connection = objPer.openConnection();
                objInsertCmd.CommandText = "spInsertProvider";
                objInsertCmd.CommandType = CommandType.StoredProcedure;
                objInsertCmd.Parameters.Add("p_nit", MySqlDbType.VarChar).Value = _nit;
                objInsertCmd.Parameters.Add("p_name", MySqlDbType.VarChar).Value = _name;

                try
                {
                    row = objInsertCmd.ExecuteNonQuery();
                    if (row == 1) executed = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.ToString());
                }

                objPer.closeConnection();
                return executed;
            }

            /// <summary>
            /// Actualiza un proveedor existente en la base de datos.
            /// Ejecuta el procedimiento almacenado 'spUpdateProvider'.
            /// </summary>
            /// <param name="id">ID del proveedor a actualizar</param>
            /// <param name="_nit">Nuevo NIT del proveedor</param>
            /// <param name="_name">Nuevo nombre del proveedor</param>
            /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
            public bool UpdateProvider(int id, string _nit, string _name)
            {
                bool executed = false;
                int row;

                MySqlCommand objUpdateCmd = new MySqlCommand();
                objUpdateCmd.Connection = objPer.openConnection();
                objUpdateCmd.CommandText = "spUpdateProvider";
                objUpdateCmd.CommandType = CommandType.StoredProcedure;
                objUpdateCmd.Parameters.Add("p_id", MySqlDbType.Int32).Value = id;
                objUpdateCmd.Parameters.Add("p_nit", MySqlDbType.VarChar).Value = _nit;
                objUpdateCmd.Parameters.Add("p_name", MySqlDbType.VarChar).Value = _name;

                try
                {
                    row = objUpdateCmd.ExecuteNonQuery();
                    if (row == 1) executed = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.ToString());
                }

                objPer.closeConnection();
                return executed;
            }

            /// <summary>
            /// Elimina un proveedor por su ID de la base de datos.
            /// Ejecuta el procedimiento almacenado 'spDeleteProvider'.
            /// </summary>
            /// <param name="id">ID del proveedor a eliminar</param>
            /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
            public bool DeleteProvider(int id)
            {
                bool executed = false;
                int row;

                MySqlCommand objDeleteCmd = new MySqlCommand();
                objDeleteCmd.Connection = objPer.openConnection();
                objDeleteCmd.CommandText = "spDeleteProvider";
                objDeleteCmd.CommandType = CommandType.StoredProcedure;
                objDeleteCmd.Parameters.Add("p_id", MySqlDbType.Int32).Value = id;

                try
                {
                    row = objDeleteCmd.ExecuteNonQuery();
                    if (row == 1) executed = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.ToString());
                }

                objPer.closeConnection();
                return executed;
            }
        }
    }
