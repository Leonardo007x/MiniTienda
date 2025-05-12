/**
 * Proyecto MiniTienda - Capa Lógica de Negocio
 * 
 * Implementación de la lógica de negocio para la gestión de categorías.
 * Esta clase proporciona métodos para operaciones CRUD, validando los datos
 * antes de enviarlos a la capa de datos.
 * 
 * Autor: Elkin
 * Fecha: 04/05/2025
 */

using System;
using System.Data;
using MiniTienda.Data;

namespace MiniTienda.Logic
{
    /// <summary>
    /// Clase para gestionar la lógica de negocio relacionada con categorías.
    /// Implementa validaciones y se comunica con la capa de datos.
    /// </summary>
    public class CategoryLog
    {
        /// <summary>
        /// Instancia de la clase CategoryData que proporciona acceso a los datos de categorías
        /// </summary>
        CategoryData objCat = new CategoryData();

        /// <summary>
        /// Obtiene todas las categorías desde la capa de datos.
        /// </summary>
        /// <returns>DataTable con la información de las categorías</returns>
        public DataTable showCategories()
        {
            return objCat.ShowCategories();
        }

        /// <summary>
        /// Guarda una nueva categoría en el sistema después de validar los datos.
        /// </summary>
        /// <param name="_name">Nombre de la categoría</param>
        /// <param name="_descripcion">Descripción de la categoría</param>
        /// <returns>ID de la nueva categoría si la operación fue exitosa, 0 en caso de error</returns>
        public int saveCategory(string _name, string _descripcion)
        {
            // Validación: Nombre no puede estar vacío
            if (string.IsNullOrEmpty(_name))
                return 0;
            
            // Validación: Descripción no puede estar vacía
            if (string.IsNullOrEmpty(_descripcion))
                return 0;

            // Validación: Longitud del nombre (evitar nombres demasiado largos)
            if (_name.Length > 100)
                return 0;
            
            return objCat.SaveCategory(_name, _descripcion);
        }

        /// <summary>
        /// Actualiza una categoría existente después de validar los datos.
        /// </summary>
        /// <param name="_id">ID de la categoría a actualizar</param>
        /// <param name="_name">Nuevo nombre</param>
        /// <param name="_descripcion">Nueva descripción</param>
        /// <returns>Cantidad de filas afectadas. Mayor que 0 indica éxito.</returns>
        public int updateCategory(int _id, string _name, string _descripcion)
        {
            // Validación: ID debe ser positivo
            if (_id <= 0)
                return 0;
                
            // Validación: Nombre no puede estar vacío
            if (string.IsNullOrEmpty(_name))
                return 0;
            
            // Validación: Descripción no puede estar vacía
            if (string.IsNullOrEmpty(_descripcion))
                return 0;

            // Validación: Longitud del nombre (evitar nombres demasiado largos)
            if (_name.Length > 100)
                return 0;
            
            return objCat.UpdateCategory(_id, _name, _descripcion);
        }

        /// <summary>
        /// Elimina una categoría del sistema.
        /// </summary>
        /// <param name="_id">ID de la categoría a eliminar</param>
        /// <returns>Cantidad de filas afectadas. Mayor que 0 indica éxito.</returns>
        public int deleteCategory(int _id)
        {
            // Validación: ID debe ser positivo
            if (_id <= 0)
                return 0;
                
            return objCat.DeleteCategory(_id);
        }
    }
}
