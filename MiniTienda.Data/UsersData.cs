/**
 * Proyecto MiniTienda - Capa de Acceso a Datos
 * 
 * Implementación del patrón DAO (Data Access Object) para la gestión de usuarios.
 * Esta clase proporciona métodos CRUD (Create, Read, Update) para manipular
 * los datos de usuarios en la base de datos MySQL.
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
    /// Clase para gestionar operaciones de datos de usuarios en la base de datos.
    /// Implementa el patrón DAO para abstraer la lógica de acceso a datos de la lógica de negocio.
    /// Utiliza procedimientos almacenados para todas las operaciones.
    /// </summary>
    public class UsersData
    {
        /// <summary>
        /// Instancia de la clase Persistence que proporciona acceso a la base de datos
        /// </summary>
        private readonly Persistence objPer = new Persistence();

        /// <summary>
        /// Obtiene todos los usuarios almacenados en la base de datos.
        /// Ejecuta el procedimiento almacenado 'spSelectProvidersDDL'.
        /// </summary>
        /// <returns>DataSet con los usuarios</returns>
        public DataSet showUsers()
        {
            MySqlDataAdapter objAdapter = new MySqlDataAdapter();
            DataSet objData = new DataSet();

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
        /// Guarda un nuevo usuario en la base de datos.
        /// Ejecuta el procedimiento almacenado 'spInsertUsers'.
        /// </summary>
        /// <param name="_mail">Correo electrónico del usuario</param>
        /// <param name="_password">Contraseña del usuario</param>
        /// <param name="_salt">Salt para la contraseña</param>
        /// <param name="_state">Estado del usuario (activo/inactivo)</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        public bool saveUsers(string _mail, string _password, string _salt, string _state)
        {
            bool executed = false;  // Variable que indica si la operación fue exitosa
            int row;  // Almacena el número de filas afectadas por la ejecución del comando

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();

            // Se indica el nombre del procedimiento almacenado a ejecutar
            objSelectCmd.CommandText = "spInsertUsers"; // <-- Aquí debe ir el nombre exacto del procedimiento en MySQL

            // Se indica que el comando es un procedimiento almacenado
            objSelectCmd.CommandType = CommandType.StoredProcedure;

            // Se agregan los parámetros requeridos por el procedimiento almacenado, junto con sus valores
            objSelectCmd.Parameters.Add("p_mail", MySqlDbType.VarString).Value = _mail;
            objSelectCmd.Parameters.Add("p_password", MySqlDbType.VarString).Value = _password;
            objSelectCmd.Parameters.Add("p_salt", MySqlDbType.VarString).Value = _salt;
            objSelectCmd.Parameters.Add("p_state", MySqlDbType.VarString).Value = _state;

            try
            {
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
        /// Actualiza los datos de un usuario existente en la base de datos.
        /// Ejecuta el procedimiento almacenado 'spUpdateUsers'.
        /// </summary>
        /// <param name="_id">ID del usuario a actualizar</param>
        /// <param name="_mail">Nuevo correo electrónico</param>
        /// <param name="_password">Nueva contraseña</param>
        /// <param name="_salt">Nuevo salt para la contraseña</param>
        /// <param name="_state">Nuevo estado del usuario</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        public bool updateUsers(int _id, string _mail, string _password, string _salt, string _state)
        {
            bool executed = false; 
            int row; 

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();

            objSelectCmd.CommandText = "spUpdateUsers"; // <-- Este debe coincidir con el nombre del procedimiento en MySQL

            // Indicar que se ejecutará un procedimiento almacenado
            objSelectCmd.CommandType = CommandType.StoredProcedure;

            // Agregar parámetros al comando, que serán enviados al procedimiento almacenado
            objSelectCmd.Parameters.Add("p_id", MySqlDbType.Int32).Value = _id;              
            objSelectCmd.Parameters.Add("p_mail", MySqlDbType.VarString).Value = _mail;     
            objSelectCmd.Parameters.Add("p_password", MySqlDbType.VarString).Value = _password; 
            objSelectCmd.Parameters.Add("p_salt", MySqlDbType.VarString).Value = _salt;     
            objSelectCmd.Parameters.Add("p_state", MySqlDbType.VarString).Value = _state;    

            try
            {
                row = objSelectCmd.ExecuteNonQuery();

                // Si se afectó exactamente una fila, se considera una actualización exitosa
                if (row == 1)
                {
                    executed = true;
                }
            }
            catch (Exception e)
            {
                // En caso de error, mostrar el mensaje en la consola
                Console.WriteLine("Error " + e.ToString());
            }


            objPer.closeConnection();
            return executed;
        }
    }
}
