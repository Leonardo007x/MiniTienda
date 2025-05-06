/**
 * Proyecto MiniTienda - Capa Lógica de Negocio
 * 
 * Implementación de la lógica de negocio para la gestión de proveedores.
 * Esta clase proporciona métodos para operaciones CRUD, validando los datos
 * antes de enviarlos a la capa de datos.
 * 
 * Autor: Brayan
 * Fecha: 04/05/2025
 */

using MiniTienda.Data;
using System;
using System.Data;

namespace MiniTienda.Logic
{
    /// <summary>
    /// Clase para gestionar la lógica de negocio relacionada con proveedores.
    /// Implementa validaciones y se comunica con la capa de datos.
    /// </summary>
    public class ProvidersLog
    {
        /// <summary>
        /// Instancia de la clase ProvidersData que proporciona acceso a los datos de proveedores
        /// </summary>
        ProvidersData objPrv = new ProvidersData();

        /// <summary>
        /// Obtiene todos los proveedores para mostrar en un control desplegable (DropDownList).
        /// </summary>
        /// <returns>DataSet con la información de los proveedores</returns>
        public DataSet ShowProvidersDDL()
        {
            return objPrv.ShowProvidersDDL();
        }

        /// <summary>
        /// Obtiene todos los proveedores desde la capa de datos.
        /// </summary>
        /// <returns>DataSet con la información de los proveedores</returns>
        public DataSet ShowProviders()
        {
            return objPrv.ShowProviders();
        }

        /// <summary>
        /// Guarda un nuevo proveedor en el sistema después de validar los datos.
        /// Incluye validaciones para evitar nombres vacíos y nombres duplicados.
        /// </summary>
        /// <param name="nit">NIT o identificación fiscal del proveedor</param>
        /// <param name="name">Nombre del proveedor</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        /// <exception cref="ArgumentException">Se lanza si el nombre está vacío</exception>
        /// <exception cref="Exception">Se lanza si ya existe un proveedor con el mismo nombre</exception>
        public bool SaveProvider(string nit, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del proveedor no puede estar vacío.");

            // Evitar nombres duplicados
            var existingProviders = objPrv.ShowProviders();
            foreach (DataRow row in existingProviders.Tables[0].Rows)
            {
                if (row["prov_nombre"].ToString().Equals(name, StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Ya existe un proveedor con este nombre.");
            }

            return objPrv.SaveProvider(nit, name);
        }

        /// <summary>
        /// Actualiza los datos de un proveedor existente después de validar los datos.
        /// </summary>
        /// <param name="id">ID del proveedor a actualizar</param>
        /// <param name="nit">Nuevo NIT o identificación fiscal</param>
        /// <param name="name">Nuevo nombre</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        /// <exception cref="ArgumentException">Se lanza si el ID no es válido o el nombre está vacío</exception>
        public bool UpdateProvider(int id, string nit, string name)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío.");

            return objPrv.UpdateProvider(id, nit, name);
        }

        /// <summary>
        /// Elimina un proveedor del sistema.
        /// </summary>
        /// <param name="id">ID del proveedor a eliminar</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        /// <exception cref="ArgumentException">Se lanza si el ID no es válido</exception>
        public bool DeleteProvider(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido.");

            return objPrv.DeleteProvider(id); 
        }
    }
}
