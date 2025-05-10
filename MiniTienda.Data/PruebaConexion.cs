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
        /// 3. Verifica la conexión a la base de datos 'minitienda'
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
                    
                    // Nota: Se ha eliminado la ejecución de procedimientos almacenados para evitar errores de sintaxis
                    // con los comandos DELIMITER. Se recomienda ejecutar estos procedimientos directamente desde
                    // MySQL Workbench u otra herramienta de administración de bases de datos.
                }
                
                // Verificamos la cadena de conexión
                Console.WriteLine("Cadena de conexión: " + connectionString);
                
                // Ahora verificamos la conexión a la base de datos minitienda
                Persistence persistence = new Persistence();
                using (var connection = persistence.GetConnection())
                {
                    connection.Open();
                    Console.WriteLine("Conexión configurada correctamente.");
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