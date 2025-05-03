/**
 * Proyecto MiniTienda - Capa de Acceso a Datos
 * 
 * Implementación del patrón DAO (Data Access Object) para la gestión de categorías.
 * Esta clase proporciona métodos CRUD (Create, Read, Update, Delete) para manipular
 * los datos de categorías en la base de datos MySQL.
 * 
 * Autor: Leonardo
 * Fecha: 02/05/2025
 */

using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MiniTienda.Data
{
    /// <summary>
    /// Clase para gestionar operaciones de datos de categorías en la base de datos.
    /// Implementa el patrón DAO para abstraer la lógica de acceso a datos de la lógica de negocio.
    /// Utiliza procedimientos almacenados para todas las operaciones, mejorando la seguridad y el rendimiento.
    /// </summary>
    public class CategoryData
    {
        /// <summary>
        /// Instancia de la clase Persistence que proporciona acceso a la base de datos
        /// </summary>
        private readonly Persistence _persistence;

        /// <summary>
        /// Constructor que inicializa una nueva instancia de la clase CategoryData.
        /// Crea una instancia de Persistence para gestionar las conexiones a la base de datos.
        /// </summary>
        public CategoryData()
        {
            _persistence = new Persistence();
        }

        /// <summary>
        /// Obtiene todas las categorías almacenadas en la base de datos.
        /// Ejecuta el procedimiento almacenado 'sp_show_categories' que selecciona
        /// todos los registros de la tabla de categorías.
        /// </summary>
        /// <returns>DataTable con las categorías (columnas: id, description)</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error al obtener las categorías</exception>
        /// <remarks>
        /// El procedimiento almacenado devuelve los datos ordenados por descripción de categoría
        /// </remarks>
        public DataTable ShowCategories()
        {
            try
            {
                // Obtener conexión a la base de datos
                using (var connection = _persistence.GetConnection())
                {
                    // Configurar comando para llamar al procedimiento almacenado
                    using (var command = new MySqlCommand("sp_show_categories", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        // Preparar tabla para recibir los datos
                        var dataTable = new DataTable();
                        connection.Open();
                        
                        // Usar adaptador para llenar la tabla con los resultados
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                        
                        return dataTable;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Capturar y relanzar excepciones específicas de MySQL con mensaje informativo
                throw new Exception("Error al obtener las categorías: " + ex.Message);
            }
        }

        /// <summary>
        /// Guarda una nueva categoría en la base de datos.
        /// Ejecuta el procedimiento almacenado 'sp_save_category' que inserta
        /// un nuevo registro en la tabla de categorías.
        /// </summary>
        /// <param name="name">Nombre de la categoría (no utilizado en la versión actual)</param>
        /// <param name="description">Descripción de la categoría</param>
        /// <returns>ID de la categoría creada (valor autogenerado)</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error al guardar la categoría</exception>
        /// <remarks>
        /// El parámetro 'name' se mantiene por compatibilidad con la interfaz, pero actualmente
        /// solo se utiliza el parámetro 'description' en la tabla de categorías
        /// </remarks>
        public int SaveCategory(string name, string description)
        {
            try
            {
                // Obtener conexión a la base de datos
                using (var connection = _persistence.GetConnection())
                {
                    // Configurar comando para llamar al procedimiento almacenado
                    using (var command = new MySqlCommand("sp_save_category", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        // Agregar parámetros al comando
                        command.Parameters.AddWithValue("p_name", name);
                        command.Parameters.AddWithValue("p_description", description);
                        
                        connection.Open();
                        
                        // Ejecutar y obtener el ID insertado
                        var result = Convert.ToInt32(command.ExecuteScalar());
                        return result;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Capturar y relanzar excepciones específicas de MySQL con mensaje informativo
                throw new Exception("Error al guardar la categoría: " + ex.Message);
            }
        }

        /// <summary>
        /// Actualiza una categoría existente en la base de datos.
        /// Ejecuta el procedimiento almacenado 'sp_update_category' que modifica
        /// un registro en la tabla de categorías según su ID.
        /// </summary>
        /// <param name="id">ID de la categoría a actualizar</param>
        /// <param name="name">Nuevo nombre (no utilizado en la versión actual)</param>
        /// <param name="description">Nueva descripción</param>
        /// <returns>Número de filas afectadas (1 si la actualización fue exitosa, 0 si no se encontró la categoría)</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error al actualizar la categoría</exception>
        /// <remarks>
        /// El parámetro 'name' se mantiene por compatibilidad con la interfaz, pero actualmente
        /// solo se utiliza el parámetro 'description' en la tabla de categorías
        /// </remarks>
        public int UpdateCategory(int id, string name, string description)
        {
            try
            {
                // Obtener conexión a la base de datos
                using (var connection = _persistence.GetConnection())
                {
                    // Configurar comando para llamar al procedimiento almacenado
                    using (var command = new MySqlCommand("sp_update_category", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        // Agregar parámetros al comando
                        command.Parameters.AddWithValue("p_id", id);
                        command.Parameters.AddWithValue("p_name", name);
                        command.Parameters.AddWithValue("p_description", description);
                        
                        connection.Open();
                        
                        // Ejecutar y obtener el número de filas afectadas
                        var result = Convert.ToInt32(command.ExecuteScalar());
                        return result;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Capturar y relanzar excepciones específicas de MySQL con mensaje informativo
                throw new Exception("Error al actualizar la categoría: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina una categoría por su ID de la base de datos.
        /// Ejecuta el procedimiento almacenado 'sp_delete_category' que elimina
        /// un registro de la tabla de categorías según su ID.
        /// </summary>
        /// <param name="id">ID de la categoría a eliminar</param>
        /// <returns>Número de filas afectadas (1 si la eliminación fue exitosa, 0 si no se encontró la categoría)</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error al eliminar la categoría</exception>
        /// <remarks>
        /// Se debe tener precaución al eliminar categorías que puedan estar referenciadas por otros registros,
        /// como productos, para evitar errores de integridad referencial
        /// </remarks>
        public int DeleteCategory(int id)
        {
            try
            {
                // Obtener conexión a la base de datos
                using (var connection = _persistence.GetConnection())
                {
                    // Configurar comando para llamar al procedimiento almacenado
                    using (var command = new MySqlCommand("sp_delete_category", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        // Agregar parámetro al comando
                        command.Parameters.AddWithValue("p_id", id);
                        
                        connection.Open();
                        
                        // Ejecutar y obtener el número de filas afectadas
                        var result = Convert.ToInt32(command.ExecuteScalar());
                        return result;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Capturar y relanzar excepciones específicas de MySQL con mensaje informativo
                throw new Exception("Error al eliminar la categoría: " + ex.Message);
            }
        }
    }
} 