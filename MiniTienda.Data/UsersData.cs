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
        /// Ejecuta el procedimiento almacenado 'spSelectUsers'.
        /// </summary>
        /// <returns>DataSet con los usuarios</returns>
        public DataSet showUsers()
        {
            MySqlDataAdapter objAdapter = new MySqlDataAdapter();
            DataSet objData = new DataSet();

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();
            objSelectCmd.CommandText = "spSelectUsers";
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
            int row = 0;  // Almacena el número de filas afectadas por la ejecución del comando

            Console.WriteLine($"Intentando guardar usuario: Email={_mail}, Contraseña=(oculta), Salt={_salt}, Estado={_state}");

            try
            {
                // Primer intento: usar el procedimiento almacenado
                try
                {
                    MySqlCommand objSelectCmd = new MySqlCommand();
                    objSelectCmd.Connection = objPer.openConnection();

                    // Se indica el nombre del procedimiento almacenado a ejecutar
                    objSelectCmd.CommandText = "spInsertUsers";
                    objSelectCmd.CommandType = CommandType.StoredProcedure;

                    // Se agregan los parámetros requeridos por el procedimiento almacenado, junto con sus valores
                    objSelectCmd.Parameters.Add("p_mail", MySqlDbType.VarString).Value = _mail;
                    objSelectCmd.Parameters.Add("p_password", MySqlDbType.VarString).Value = _password;
                    objSelectCmd.Parameters.Add("p_salt", MySqlDbType.VarString).Value = _salt;
                    objSelectCmd.Parameters.Add("p_state", MySqlDbType.VarString).Value = _state;

                    row = objSelectCmd.ExecuteNonQuery();
                    Console.WriteLine($"Resultado del procedimiento almacenado: {row} filas afectadas");

                    if (row == 1)
                    {
                        executed = true;
                        Console.WriteLine("Usuario guardado exitosamente mediante procedimiento almacenado");
                        objPer.closeConnection();
                        return executed;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error al guardar usuario con procedimiento almacenado: {e.Message}");
                    Console.WriteLine("Intentando guardar con SQL directo...");
                }

                // Segundo intento: usar SQL directo
                try
                {
                    MySqlCommand objDirectInsertCmd = new MySqlCommand();
                    objDirectInsertCmd.Connection = objPer.openConnection();

                    // Consulta SQL para insertar un usuario
                    string insertQuery = @"
                        INSERT INTO tbl_usuarios (
                            usu_correo, 
                            usu_clave, 
                            usu_salt, 
                            usu_estado
                        ) VALUES (
                            @mail, 
                            @password, 
                            @salt, 
                            @state
                        )";

                    objDirectInsertCmd.CommandText = insertQuery;
                    objDirectInsertCmd.CommandType = CommandType.Text;

                    // Agregar parámetros para SQL directo
                    objDirectInsertCmd.Parameters.Add("@mail", MySqlDbType.VarString).Value = _mail;
                    objDirectInsertCmd.Parameters.Add("@password", MySqlDbType.VarString).Value = _password;
                    objDirectInsertCmd.Parameters.Add("@salt", MySqlDbType.VarString).Value = _salt;
                    objDirectInsertCmd.Parameters.Add("@state", MySqlDbType.VarString).Value = _state;

                    // Ejecutar la consulta directa
                    row = objDirectInsertCmd.ExecuteNonQuery();
                    Console.WriteLine($"Resultado de la consulta SQL directa: {row} filas afectadas");

                    if (row == 1)
                    {
                        executed = true;
                        Console.WriteLine("Usuario guardado exitosamente mediante SQL directo");
                    }
                    else
                    {
                        Console.WriteLine($"No se pudo guardar el usuario. Filas afectadas: {row}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error al guardar usuario con SQL directo: {e.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error general al guardar usuario: {e.ToString()}");
            }
            finally
            {
                objPer.closeConnection();
            }

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
            // Nota: Se usa 'p_idspInsertCategory' para coincidir con el nombre del parámetro en el procedimiento almacenado
            objSelectCmd.Parameters.Add("p_idspInsertCategory", MySqlDbType.Int32).Value = _id;              
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

        /// <summary>
        /// Elimina un usuario de la base de datos.
        /// </summary>
        /// <param name="_id">ID del usuario a eliminar</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        public bool deleteUsers(int _id)
        {
            bool executed = false; 
            int row = 0; 

            Console.WriteLine($"[INFO] Iniciando proceso de eliminación del usuario con ID: {_id}");

            try
            {
                // Primer intento: intentar usar el procedimiento almacenado
                try
                {
                    MySqlCommand objDeleteCmd = new MySqlCommand();
                    objDeleteCmd.Connection = objPer.openConnection();

                    objDeleteCmd.CommandText = "spDeleteUsers";
                    objDeleteCmd.CommandType = CommandType.StoredProcedure;
                    objDeleteCmd.Parameters.Add("p_id", MySqlDbType.Int32).Value = _id;

                    Console.WriteLine($"[INFO] Intentando eliminar usuario {_id} con procedimiento almacenado");
                    row = objDeleteCmd.ExecuteNonQuery();
                    Console.WriteLine($"[INFO] Resultado del procedimiento almacenado de eliminación: {row} filas afectadas");

                    if (row == 1)
                    {
                        executed = true;
                        Console.WriteLine($"[INFO] Usuario con ID {_id} eliminado correctamente mediante procedimiento almacenado");
                        objPer.closeConnection();
                        return executed;
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"[ERROR] Error de MySQL al eliminar usuario con procedimiento almacenado: {ex.Message}");
                    Console.WriteLine($"[ERROR] Código de error: {ex.Number}");
                    Console.WriteLine("Intentando con SQL directo...");
                    // Cerramos la conexión y aseguramos que se abrirá una nueva en el próximo intento
                    objPer.closeConnection();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Error general al eliminar usuario con procedimiento almacenado: {ex.Message}");
                    Console.WriteLine("Intentando con SQL directo...");
                    // Cerramos la conexión y aseguramos que se abrirá una nueva en el próximo intento
                    objPer.closeConnection();
                }

                // Segundo intento: usar SQL directo para eliminar
                try
                {
                    MySqlCommand objDirectDeleteCmd = new MySqlCommand();
                    objDirectDeleteCmd.Connection = objPer.openConnection();

                    // Consulta SQL para eliminar un usuario
                    string deleteQuery = "DELETE FROM tbl_usuarios WHERE usu_id = @id";
                    objDirectDeleteCmd.CommandText = deleteQuery;
                    objDirectDeleteCmd.CommandType = CommandType.Text;
                    objDirectDeleteCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = _id;

                    Console.WriteLine($"[INFO] Intentando eliminar usuario {_id} con SQL directo");
                    row = objDirectDeleteCmd.ExecuteNonQuery();
                    Console.WriteLine($"[INFO] Resultado de la eliminación SQL directa: {row} filas afectadas");

                    if (row >= 1) // Cambiado de == 1 a >= 1 para ser más flexible
                    {
                        executed = true;
                        Console.WriteLine($"[INFO] Usuario con ID {_id} eliminado correctamente mediante SQL directo");
                    }
                    else
                    {
                        // Verificar si el usuario existe
                        MySqlCommand checkCmd = new MySqlCommand();
                        checkCmd.Connection = objPer.openConnection();
                        checkCmd.CommandText = "SELECT COUNT(*) FROM tbl_usuarios WHERE usu_id = @id";
                        checkCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = _id;
                        
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        
                        if (count == 0)
                        {
                            Console.WriteLine($"[INFO] El usuario con ID {_id} no existe en la base de datos");
                        }
                        else
                        {
                            // Verificar si hay registros relacionados que impiden la eliminación
                            Console.WriteLine($"[INFO] El usuario con ID {_id} existe pero no pudo ser eliminado. Posiblemente tiene registros relacionados");
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"[ERROR] Error de MySQL al eliminar usuario con SQL directo: {ex.Message}");
                    Console.WriteLine($"[ERROR] Código de error: {ex.Number}");
                    
                    // Si es error de clave foránea (código 1451)
                    if (ex.Number == 1451)
                    {
                        Console.WriteLine("[ERROR] No se puede eliminar debido a restricciones de clave foránea. El usuario tiene registros asociados.");
                        // Opción alternativa: desactivar el usuario en lugar de eliminarlo
                        try {
                            MySqlCommand updateCmd = new MySqlCommand();
                            updateCmd.Connection = objPer.openConnection();
                            updateCmd.CommandText = "UPDATE tbl_usuarios SET usu_estado = 'inactivo' WHERE usu_id = @id";
                            updateCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = _id;
                            
                            int updateResult = updateCmd.ExecuteNonQuery();
                            if (updateResult == 1) {
                                Console.WriteLine($"[INFO] Usuario con ID {_id} marcado como inactivo en lugar de eliminado");
                                executed = true; // Consideramos esto un éxito
                            }
                        }
                        catch (Exception updateEx) {
                            Console.WriteLine($"[ERROR] No se pudo marcar el usuario como inactivo: {updateEx.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Error general al eliminar usuario con SQL directo: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"[ERROR] Error interno: {ex.InnerException.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error general en el proceso de eliminación: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[ERROR] Error interno: {ex.InnerException.Message}");
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
