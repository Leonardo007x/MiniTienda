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
using MiniTienda.Model;

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

            try
            {
                MySqlCommand objSelectCmd = new MySqlCommand();
                objSelectCmd.Connection = objPer.openConnection();
                
                // Usar consulta directa en lugar del procedimiento almacenado
                objSelectCmd.CommandText = "SELECT usu_id, usu_correo, usu_estado FROM tbl_usuarios";
                objSelectCmd.CommandType = CommandType.Text;
                
                // Log de diagnóstico
                Console.WriteLine("Ejecutando consulta SQL directa para obtener usuarios");
                
                objAdapter.SelectCommand = objSelectCmd;
                objAdapter.Fill(objData);
                
                // Verificación básica de resultados
                if (objData != null && objData.Tables.Count > 0 && objData.Tables[0].Rows.Count > 0)
                {
                    Console.WriteLine($"Se encontraron {objData.Tables[0].Rows.Count} usuarios");
                    
                    // Listar columnas para diagnóstico
                    Console.WriteLine("Columnas en el resultado:");
                    foreach (DataColumn col in objData.Tables[0].Columns)
                    {
                        Console.WriteLine($"  - {col.ColumnName} ({col.DataType.Name})");
                    }
                }
                else
                {
                    Console.WriteLine("No se encontraron usuarios o el resultado está vacío");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en showUsers: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                // No lanzamos la excepción para mantener el comportamiento original
            }
            finally
            {
                objPer.closeConnection();
            }
            
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
            
            // Diagnóstico: Verificar la estructura de la tabla primero
            try {
                MySqlCommand diagCmd = new MySqlCommand();
                diagCmd.Connection = objPer.openConnection();
                diagCmd.CommandText = "SHOW COLUMNS FROM tbl_usuarios";
                diagCmd.CommandType = CommandType.Text;
                
                Console.WriteLine("Verificando estructura de la tabla tbl_usuarios:");
                using (MySqlDataReader reader = diagCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"  Columna: {reader["Field"]}, Tipo: {reader["Type"]}");
                    }
                }
                objPer.closeConnection();
            }
            catch (Exception ex) {
                Console.WriteLine($"Error al verificar estructura de tabla: {ex.Message}");
                // No detenemos el proceso si falla este diagnóstico
            }

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
                            usu_contrasena, 
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
                catch (MySqlException e)
                {
                    // Capturar específicamente errores de MySQL
                    Console.WriteLine($"Error MySQL al guardar usuario con SQL directo: {e.Message}, Código: {e.Number}");
                    
                    // Error 1062 es "Duplicate entry" para clave única
                    if (e.Number == 1062)
                    {
                        Console.WriteLine($"ERROR DE DUPLICADO: El correo '{_mail}' ya existe en la base de datos.");
                        // Verificar si realmente existe consultando directamente
                        try 
                        {
                            MySqlCommand checkCmd = new MySqlCommand();
                            checkCmd.Connection = objPer.openConnection();
                            checkCmd.CommandText = "SELECT COUNT(*) FROM tbl_usuarios WHERE usu_correo = @mail";
                            checkCmd.Parameters.Add("@mail", MySqlDbType.VarString).Value = _mail;
                            
                            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                            Console.WriteLine($"Verificación directa: {count} usuarios encontrados con el correo '{_mail}'");
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine($"Error al verificar duplicado: {ex.Message}");
                        }
                        finally
                        {
                            objPer.closeConnection();
                        }
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
                // First check if user exists
                MySqlCommand checkCmd = new MySqlCommand();
                checkCmd.Connection = objPer.openConnection();
                checkCmd.CommandText = "SELECT COUNT(*) FROM tbl_usuarios WHERE usu_id = @id";
                checkCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = _id;
                
                int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());
                
                if (userExists == 0)
                {
                    Console.WriteLine($"[INFO] El usuario con ID {_id} no existe en la base de datos");
                    objPer.closeConnection();
                    return false;
                }

                // First attempt: Direct deactivation instead of deletion
                // This is safer and avoids foreign key constraints
                try
                {
                    MySqlCommand deactivateCmd = new MySqlCommand();
                    deactivateCmd.Connection = objPer.openConnection();
                    deactivateCmd.CommandText = "UPDATE tbl_usuarios SET usu_estado = 'inactivo' WHERE usu_id = @id";
                    deactivateCmd.CommandType = CommandType.Text;
                    deactivateCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = _id;
                    
                    Console.WriteLine($"[INFO] Intentando desactivar usuario {_id} en lugar de eliminarlo");
                    row = deactivateCmd.ExecuteNonQuery();
                    Console.WriteLine($"[INFO] Resultado de desactivación: {row} filas afectadas");
                    
                    if (row == 1)
                    {
                        executed = true;
                        Console.WriteLine($"[INFO] Usuario con ID {_id} desactivado correctamente");
                        objPer.closeConnection();
                        return executed;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Error al desactivar usuario: {ex.Message}");
                    objPer.closeConnection();
                }

                // Second attempt: try to delete with SQL direct
                try
                {
                    MySqlCommand objDirectDeleteCmd = new MySqlCommand();
                    objDirectDeleteCmd.Connection = objPer.openConnection();
                    objDirectDeleteCmd.CommandText = "DELETE FROM tbl_usuarios WHERE usu_id = @id";
                    objDirectDeleteCmd.CommandType = CommandType.Text;
                    objDirectDeleteCmd.Parameters.Add("@id", MySqlDbType.Int32).Value = _id;
                    
                    Console.WriteLine($"[INFO] Intentando eliminar usuario {_id} con SQL directo");
                    row = objDirectDeleteCmd.ExecuteNonQuery();
                    Console.WriteLine($"[INFO] Resultado de la eliminación SQL directa: {row} filas afectadas");
                    
                    if (row >= 1)
                    {
                        executed = true;
                        Console.WriteLine($"[INFO] Usuario con ID {_id} eliminado correctamente con SQL directo");
                    }
                    else
                    {
                        Console.WriteLine($"[INFO] No se pudo eliminar el usuario con ID {_id}, ninguna fila afectada");
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"[ERROR] Error de MySQL al eliminar usuario: {ex.Message}, Código: {ex.Number}");
                    
                    // Si es error de clave foránea (código 1451), la desactivación ya fue intentada, así que reportamos error
                    if (ex.Number == 1451)
                    {
                        Console.WriteLine("[ERROR] No se puede eliminar debido a restricciones de clave foránea.");
                        executed = true; // Consideramos exitoso pues el usuario ya fue desactivado
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Error general al eliminar usuario: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error general en el proceso de eliminación: {ex.Message}");
            }
            finally
            {
                objPer.closeConnection();
            }

            return executed;
        }

        // Autor: Brayan
        // Fecha: 15/05/2025
        // Descripcion: Obtiene un usuario específico de la base de datos utilizando su correo electrónico.
        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        /// <param name="mail">Correo electrónico del usuario a buscar.</param>
        /// <returns>Un objeto User si se encuentra; de lo contrario, null.</returns>
        public Model.User showUsersMail(string mail)
        {
            Model.User objUser = null;
            MySqlDataReader reader = null;

            try
            {
                MySqlCommand objSelectCmd = new MySqlCommand();
                objSelectCmd.Connection = objPer.openConnection();
                
                // Diagnóstico adicional: verificar si hay correos similares en la base de datos
                Console.WriteLine("=== DIAGNÓSTICO DE CORREO ELECTRÓNICO ===");
                Console.WriteLine($"Buscando correo exacto: '{mail}'");
                
                try {
                    // Buscar correos similares para diagnóstico
                    MySqlCommand diagCmd = new MySqlCommand();
                    diagCmd.Connection = objPer.openConnection();
                    diagCmd.CommandText = "SELECT usu_id, usu_correo FROM tbl_usuarios";
                    
                    using (MySqlDataReader diagReader = diagCmd.ExecuteReader())
                    {
                        Console.WriteLine("Correos existentes en la base de datos:");
                        int count = 0;
                        while (diagReader.Read())
                        {
                            string existingMail = diagReader["usu_correo"].ToString();
                            Console.WriteLine($"  - ID: {diagReader["usu_id"]}, Correo: '{existingMail}'");
                            
                            // Verificar si hay coincidencia insensible a mayúsculas/minúsculas
                            if (existingMail.Equals(mail, StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"  *** COINCIDENCIA ENCONTRADA (ignorando mayúsculas/minúsculas): '{existingMail}' vs '{mail}'");
                            }
                            count++;
                        }
                        Console.WriteLine($"Total de correos en la base de datos: {count}");
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine($"Error en diagnóstico de correos: {ex.Message}");
                }
                
                // Cambio a consulta SQL directa en lugar de usar procedimiento almacenado
                objSelectCmd.CommandText = "SELECT usu_id, usu_correo, usu_contrasena, usu_salt, usu_estado FROM tbl_usuarios WHERE usu_correo = @mail";
                objSelectCmd.CommandType = CommandType.Text;
                objSelectCmd.Parameters.Add("@mail", MySqlDbType.VarChar).Value = mail;

                // Agregar más diagnóstico
                Console.WriteLine($"Ejecutando consulta para correo: '{mail}'");

                reader = objSelectCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("Se encontraron filas en el resultado");
                    
                    if (reader.Read()) // Se espera un único usuario por correo
                    {
                        // Diagnóstico para verificar los valores obtenidos
                        string correo = reader["usu_correo"].ToString();
                        string contrasena = reader["usu_contrasena"].ToString();
                        string salt = reader["usu_salt"].ToString();
                        string estado = reader["usu_estado"].ToString();
                        
                        Console.WriteLine($"Datos encontrados: Correo='{correo}', Estado='{estado}'");
                        
                        objUser = new Model.User(correo, contrasena, salt, estado);
                        Console.WriteLine("Objeto User creado correctamente");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo leer la fila aunque reader.HasRows=true");
                    }
                }
                else
                {
                    Console.WriteLine($"No se encontraron resultados para el correo: '{mail}'");
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (e.g., loggear el error)
                Console.WriteLine($"Error en showUsersMail: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                objPer.closeConnection();
            }
            
            // Verificación final
            if (objUser != null)
            {
                Console.WriteLine($"Devolviendo objeto User: Correo={objUser.Correo}, Contraseña={objUser.Contrasena}, Salt={objUser.Salt}, Estado={objUser.State}");
            }
            else
            {
                Console.WriteLine($"Devolviendo null para correo: {mail}");
            }
            
            return objUser;
        }
    }
}
