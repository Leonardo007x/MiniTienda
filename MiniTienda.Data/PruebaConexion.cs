/**
 * Proyecto MiniTienda - Capa de Acceso a Datos
 * 
 * Clase utilitaria para verificar la conexión a la base de datos MySQL.
 * Proporciona funcionalidad para probar la conexión, crear la base de datos
 * si no existe y ejecutar scripts de inicialización.
 * 
 * Autor: Leonardo
 * Fecha: 02/05/2025
 */

using System;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;

namespace MiniTienda.Data
{
    /// <summary>
    /// Clase que proporciona funcionalidad para verificar la conexión
    /// a la base de datos MySQL y configurar el entorno inicial.
    /// </summary>
    public class PruebaConexion
    {
        /// <summary>
        /// Verifica la conexión a la base de datos MySQL y configura el entorno inicial.
        /// 
        /// Este método realiza las siguientes acciones:
        /// 1. Intenta conectarse al servidor MySQL sin especificar base de datos
        /// 2. Crea la base de datos 'minitienda' si no existe
        /// 3. Ejecuta el script de creación de tablas
        /// 4. Verifica la conexión a la base de datos 'minitienda'
        /// 
        /// </summary>
        /// <returns>True si la conexión es exitosa, False en caso contrario</returns>
        public static bool VerificarConexion()
        {
            try
            {
                // Primero intentamos conectar sin especificar base de datos para crear minitienda si no existe
                string connectionString = ConfigurationManager.ConnectionStrings["MiniTiendaDB"].ConnectionString;
                string connectionSinDB = connectionString.Replace("Database=minitienda;", "");
                
                using (var connection = new MySqlConnection(connectionSinDB))
                {
                    connection.Open();
                    Console.WriteLine("Conexión exitosa al servidor MySQL!");
                    
                    // Intentamos crear la base de datos si no existe
                    using (var command = new MySqlCommand("CREATE DATABASE IF NOT EXISTS minitienda;", connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Base de datos 'minitienda' creada o verificada exitosamente.");
                    }
                    
                    // Ahora ejecutamos el script de creación de tablas
                    string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "script_creacion_bd.sql");
                    if (File.Exists(scriptPath))
                    {
                        string script = File.ReadAllText(scriptPath);
                        
                        // Cambiamos a la base de datos minitienda
                        using (var command = new MySqlCommand("USE minitienda;", connection))
                        {
                            command.ExecuteNonQuery();
                        }
                        
                        // Dividimos el script en instrucciones individuales
                        string[] commands = script.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        
                        foreach (string commandText in commands)
                        {
                            if (!string.IsNullOrWhiteSpace(commandText))
                            {
                                using (var command = new MySqlCommand(commandText + ";", connection))
                                {
                                    try
                                    {
                                        command.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error al ejecutar comando: {ex.Message}");
                                        Console.WriteLine($"Comando: {commandText}");
                                    }
                                }
                            }
                        }
                        
                        Console.WriteLine("Script SQL ejecutado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine($"No se encontró el script SQL en: {scriptPath}");
                    }
                }
                
                // Ahora verificamos la conexión a la base de datos minitienda
                Persistence persistence = new Persistence();
                using (var connection = persistence.GetConnection())
                {
                    connection.Open();
                    Console.WriteLine("Conexión exitosa a la base de datos minitienda!");
                    Console.WriteLine($"Versión del servidor: {connection.ServerVersion}");
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                return false;
            }
        }
    }
} 