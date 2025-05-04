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
        /// Ejecuta el procedimiento almacenado 'spSelectCategory' que selecciona
        /// todos los registros de la tabla de categorías.
        /// </summary>
        /// <returns>DataTable con las categorías (columnas: id, description)</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error al obtener las categorías</exception>
        public DataTable ShowCategories()
        {
            try
            {
                // Obtener conexión a la base de datos
                using (var connection = _persistence.GetConnection())
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine($"Base de datos: {connection.Database}");
                        
                        // Ejecutar el procedimiento almacenado spSelectCategory
                        using (var command = new MySqlCommand("spSelectCategory", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            
                            // Imprimir la consulta SQL que estamos ejecutando
                            Console.WriteLine($"Ejecutando procedimiento: spSelectCategory");
                            
                            // Preparar tabla para recibir los datos
                            var dataTable = new DataTable();
                            
                            // Definir manualmente las columnas de la tabla de datos
                            dataTable.Columns.Add("id", typeof(int));
                            dataTable.Columns.Add("description", typeof(string));
                            
                            // Leer los datos y llenar la tabla con los valores correctos
                            using (var reader = command.ExecuteReader())
                            {
                                bool hasRows = reader.HasRows;
                                Console.WriteLine($"La consulta devolvió registros: {hasRows}");
                                
                                int rowCount = 0;
                                int colIndex = 0;
                                
                                // Obtener el índice de la columna cat_descripcion
                                if (hasRows)
                                {
                                    colIndex = reader.GetOrdinal("cat_descripcion");
                                }
                                
                                while (reader.Read())
                                {
                                    rowCount++;
                                    // Imprimir cada registro que estamos leyendo para diagnóstico
                                    Console.WriteLine($"Leyendo registro: Descripcion={reader[colIndex]}");
                                    
                                    // Crear una nueva fila y asignar valores con los nombres de columna que esperamos
                                    DataRow row = dataTable.NewRow();
                                    // El procedimiento no devuelve el ID, por lo que usamos el rowCount como id temporal
                                    row["id"] = rowCount;
                                    row["description"] = reader[colIndex];
                                    dataTable.Rows.Add(row);
                                }
                                
                                Console.WriteLine($"Total de registros procesados: {rowCount}");
                            }
                        
                            return dataTable;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al obtener categorías: {ex.Message}");
                        throw;
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
        /// Ejecuta el procedimiento almacenado 'spInsertCategory' que inserta
        /// un nuevo registro en la tabla de categorías.
        /// </summary>
        /// <param name="name">Nombre de la categoría (no utilizado en la versión actual)</param>
        /// <param name="description">Descripción de la categoría</param>
        /// <returns>ID de la categoría creada (valor autogenerado)</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error al guardar la categoría</exception>
        public int SaveCategory(string name, string description)
        {
            try
            {
                // Verificar que los parámetros no sean nulos
                if (string.IsNullOrEmpty(description))
                {
                    throw new ArgumentException("La descripción de la categoría no puede estar vacía");
                }
                
                // Usamos solo la descripción, ignorando el nombre si está disponible
                string categoryDescription = !string.IsNullOrEmpty(description) ? description : name;
                
                // Obtener conexión a la base de datos
                using (var connection = _persistence.GetConnection())
                {
                    connection.Open();
                    
                    // Configurar comando para llamar al procedimiento almacenado
                    using (var command = new MySqlCommand("spInsertCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        // Agregar parámetros al comando - el procedimiento solo acepta p_description
                        command.Parameters.AddWithValue("p_description", categoryDescription);
                        
                        // Ejecutar el procedimiento almacenado
                        command.ExecuteNonQuery();
                        
                        // Obtener el ID del último registro insertado
                        using (var lastIdCommand = new MySqlCommand("SELECT LAST_INSERT_ID()", connection))
                        {
                            int insertedId = Convert.ToInt32(lastIdCommand.ExecuteScalar());
                            Console.WriteLine($"Categoría insertada con ID: {insertedId}");
                            return insertedId;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Capturar y relanzar excepciones específicas de MySQL con mensaje informativo
                Console.WriteLine($"Error de MySQL: {ex.Message}");
                throw new Exception("Error al guardar la categoría: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                throw new Exception("Error al guardar la categoría: " + ex.Message);
            }
        }

        /// <summary>
        /// Actualiza una categoría existente en la base de datos.
        /// Ejecuta el procedimiento almacenado 'spUpdateCategory' que modifica
        /// un registro en la tabla de categorías según su ID.
        /// </summary>
        /// <param name="id">ID de la categoría a actualizar</param>
        /// <param name="name">Nuevo nombre (no utilizado en la versión actual)</param>
        /// <param name="description">Nueva descripción</param>
        /// <returns>Número de filas afectadas (1 si la actualización fue exitosa, 0 si no se encontró la categoría)</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error al actualizar la categoría</exception>
        public int UpdateCategory(int id, string name, string description)
        {
            try
            {
                // Usamos solo la descripción, ignorando el nombre si está disponible
                string categoryDescription = !string.IsNullOrEmpty(description) ? description : name;
                
                // Obtener conexión a la base de datos
                using (var connection = _persistence.GetConnection())
                {
                    // Configurar comando para llamar al procedimiento almacenado
                    using (var command = new MySqlCommand("spUpdateCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        // Agregar parámetros al comando
                        command.Parameters.AddWithValue("p_id", id);
                        command.Parameters.AddWithValue("p_description", categoryDescription);
                        
                        connection.Open();
                        
                        // Ejecutar y obtener el número de filas afectadas
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected;
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
        /// Ejecuta el procedimiento almacenado 'spDeleteCategory' que elimina
        /// un registro de la tabla de categorías según su ID.
        /// </summary>
        /// <param name="id">ID de la categoría a eliminar</param>
        /// <returns>Número de filas afectadas (1 si la eliminación fue exitosa, 0 si no se encontró la categoría)</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error al eliminar la categoría</exception>
        public int DeleteCategory(int id)
        {
            try
            {
                // Obtener conexión a la base de datos
                using (var connection = _persistence.GetConnection())
                {
                    // Configurar comando para llamar al procedimiento almacenado
                    using (var command = new MySqlCommand("spDeleteCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        // Agregar parámetro al comando
                        command.Parameters.AddWithValue("p_id", id);
                        
                        connection.Open();
                        
                        // Ejecutar y obtener el número de filas afectadas
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Capturar y relanzar excepciones específicas de MySQL con mensaje informativo
                throw new Exception("Error al eliminar la categoría: " + ex.Message);
            }
        }

        /// <summary>
        /// Busca categorías que comiencen con una letra o texto especificado.
        /// Ejecuta el procedimiento almacenado 'spSearchCategory' que busca
        /// registros en la tabla de categorías según la letra inicial.
        /// </summary>
        /// <param name="letter">Letra o texto inicial para la búsqueda</param>
        /// <returns>DataTable con las categorías encontradas (columnas: id, description)</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error al buscar las categorías</exception>
        public DataTable SearchCategoriesByLetter(string letter)
        {
            try
            {
                // Obtener conexión a la base de datos
                using (var connection = _persistence.GetConnection())
                {
                    connection.Open();
                    
                    // Configurar comando para llamar al procedimiento almacenado
                    using (var command = new MySqlCommand("spSearchCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        // Agregar parámetro al comando
                        command.Parameters.AddWithValue("p_letter", letter);
                        
                        // Preparar tabla para recibir los datos
                        var dataTable = new DataTable();
                        
                        // Definir manualmente las columnas de la tabla de datos
                        dataTable.Columns.Add("id", typeof(int));
                        dataTable.Columns.Add("description", typeof(string));
                        
                        // Leer los datos y llenar la tabla con los valores correctos
                        using (var reader = command.ExecuteReader())
                        {
                            int rowCount = 0;
                            
                            while (reader.Read())
                            {
                                rowCount++;
                                
                                // Crear una nueva fila y asignar valores con los nombres de columna que esperamos
                                DataRow row = dataTable.NewRow();
                                // El procedimiento no devuelve el ID, por lo que usamos el rowCount como id temporal
                                row["id"] = rowCount;
                                row["description"] = reader["cat_descripcion"];
                                dataTable.Rows.Add(row);
                            }
                        }
                        
                        return dataTable;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Capturar y relanzar excepciones específicas de MySQL con mensaje informativo
                throw new Exception("Error al buscar categorías: " + ex.Message);
            }
        }

        /// <summary>
        /// Cuenta el número de categorías que comienzan con una letra o texto especificado.
        /// Ejecuta el procedimiento almacenado 'spCountCategory' que cuenta
        /// registros en la tabla de categorías según la letra inicial.
        /// </summary>
        /// <param name="letter">Letra o texto inicial para la búsqueda</param>
        /// <returns>Número de categorías encontradas</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error al contar las categorías</exception>
        public int CountCategoriesByLetter(string letter)
        {
            try
            {
                // Obtener conexión a la base de datos
                using (var connection = _persistence.GetConnection())
                {
                    connection.Open();
                    
                    // Configurar comando para llamar al procedimiento almacenado
                    using (var command = new MySqlCommand("spCountCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        // Agregar parámetro de entrada
                        command.Parameters.AddWithValue("p_letter", letter);
                        
                        // Agregar parámetro de salida
                        MySqlParameter outputParam = new MySqlParameter("p_quantity", MySqlDbType.Int32);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);
                        
                        // Ejecutar el procedimiento almacenado
                        command.ExecuteNonQuery();
                        
                        // Obtener el valor del parámetro de salida
                        int count = Convert.ToInt32(command.Parameters["p_quantity"].Value);
                        return count;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Capturar y relanzar excepciones específicas de MySQL con mensaje informativo
                throw new Exception("Error al contar categorías: " + ex.Message);
            }
        }
    }
} 