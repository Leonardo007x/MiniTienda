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
        MySqlConnection _connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MiniTiendaDB"].ConnectionString);
        // Cadena de conexión obtenida del archivo App.config
        private readonly string _connectionString;

        /// <summary>
        /// Constructor que inicializa la cadena de conexión desde App.config.
        /// Lee la configuración de conexión del archivo de configuración de la aplicación
        /// utilizando la clave "MiniTiendaDB" definida en la sección connectionStrings.
        /// </summary>
        public Persistence()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MiniTiendaDB"].ConnectionString;
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