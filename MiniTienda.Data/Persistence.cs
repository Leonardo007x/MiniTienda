/**
 * Proyecto MiniTienda - Capa de Acceso a Datos
 * 
 * Implementación del patrón DAO (Data Access Object) para la gestión de
 * la persistencia y conexión a la base de datos MySQL.
 * 
 * Autor: Leonardo
 * Fecha: 02/05/2025
 */

using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace MiniTienda.Data
{
    
    /// <summary>
    /// Clase que maneja la persistencia de datos y conexión a la base de datos MySQL.
    /// Esta clase implementa el patrón Singleton para optimizar la gestión de conexiones.
    /// Proporciona un punto centralizado para obtener conexiones a la base de datos.
    /// </summary>
    public class Persistence
    {
        // Conexión para los métodos Open/Close
        private MySqlConnection _connection;
        
        // Cadena de conexión obtenida del archivo App.config
        private readonly string _connectionString;

        /// <summary>
        /// Constructor que inicializa la cadena de conexión desde App.config.
        /// Lee la configuración de conexión del archivo de configuración de la aplicación
        /// utilizando la clave "MiniTiendaDB" definida en la sección connectionStrings.
        /// </summary>
        public Persistence()
        {
            try
            {
                _connectionString = ConfigurationManager.ConnectionStrings["MiniTiendaDB"]?.ConnectionString;
                
                // Si no se pudo obtener la cadena de conexión desde el App.config, usar una predeterminada
                if (string.IsNullOrEmpty(_connectionString))
                {
                    Console.WriteLine("ADVERTENCIA: No se pudo leer la configuración. Usando cadena de conexión predeterminada.");
                    _connectionString = "Server=localhost;Port=3307;Database=MiniTiendaDB;Uid=root;Pwd=Password1.;";
                }
                
                Console.WriteLine($"Cadena de conexión: {_connectionString}");
                
                _connection = new MySqlConnection(_connectionString);
                Console.WriteLine("Conexión configurada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al inicializar la conexión: " + ex.Message);
                Console.WriteLine("Usando cadena de conexión predeterminada como respaldo.");
                
                // Usar una cadena de conexión predeterminada como respaldo
                _connectionString = "Server=localhost;Port=3307;Database=MiniTiendaDB;Uid=root;Pwd=Password1.;";
                _connection = new MySqlConnection(_connectionString);
            }
        }

        /// <summary>
        /// Obtiene una nueva conexión a la base de datos MySQL.
        /// Este método crea una nueva instancia de MySqlConnection cada vez que se llama,
        /// permitiendo a cada operación gestionar su propio ciclo de vida de conexión.
        /// </summary>
        /// <returns>Una nueva instancia de MySqlConnection configurada con la cadena de conexión</returns>
        /// <remarks>
        /// Es responsabilidad del código que llama a este método:
        /// 1. Abrir la conexión (Open)
        /// 2. Utilizarla para operaciones de base de datos
        /// 3. Cerrarla correctamente (Close) o encapsularla en un bloque using
        /// </remarks>
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public MySqlConnection openConnection()
        {
            try
            {
                _connection.Open();
                return _connection;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }

        public void closeConnection()
        {
            _connection.Close();
        }
    }
} 