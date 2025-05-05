/**
 * Proyecto MiniTienda - Capa Lógica de Negocio
 * 
 * Implementación de la lógica de negocio para la gestión de productos.
 * Esta clase proporciona métodos para operaciones CRUD, validando los datos
 * antes de enviarlos a la capa de datos.
 * 
 * Autor: Leonardo
 * Fecha: 04/05/2025 
 */

using System;
using System.Data;
using MiniTienda.Data;

namespace MiniTienda.Logic
{
    /// <summary>
    /// Clase para gestionar la lógica de negocio relacionada con productos.
    /// Implementa validaciones y se comunica con la capa de datos.
    /// </summary>
    public class ProductsLog
    {
        /// <summary>
        /// Instancia de la clase ProductsData que proporciona acceso a los datos de productos
        /// </summary>
        ProductsData objPrep = new ProductsData();

        /// <summary>
        /// Obtiene todos los productos desde la capa de datos.
        /// </summary>
        /// <returns>DataSet con la información de los productos</returns>
        public DataSet showProducts()
        {
            return objPrep.showProducts();
        }

        /// <summary>
        /// Guarda un nuevo producto en el sistema después de validar los datos.
        /// </summary>
        /// <param name="_code">Código único del producto</param>
        /// <param name="_description">Descripción del producto</param>
        /// <param name="_quantity">Cantidad en inventario</param>
        /// <param name="_price">Precio del producto</param>
        /// <param name="_idCategory">ID de la categoría asociada</param>
        /// <param name="_idProvider">ID del proveedor asociado</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario o si los datos no son válidos</returns>
        public bool saveProducts(string _code, string _description, int _quantity, double _price, int _idCategory, int _idProvider)
        {
            // Validación de datos
            if (string.IsNullOrEmpty(_code))
                return false;
            
            if (string.IsNullOrEmpty(_description))
                return false;
            
            if (_quantity < 0)
                return false;
            
            if (_price <= 0)
                return false;
            
            if (_idCategory <= 0)
                return false;
            
            if (_idProvider <= 0)
                return false;
                
            return objPrep.saveProducts(_code, _description, _quantity, _price, _idCategory, _idProvider);
        }

        /// <summary>
        /// Actualiza los datos de un producto existente después de validar los datos.
        /// </summary>
        /// <param name="_id">ID del producto a actualizar</param>
        /// <param name="_code">Nuevo código del producto</param>
        /// <param name="_description">Nueva descripción</param>
        /// <param name="_quantity">Nueva cantidad en inventario</param>
        /// <param name="_price">Nuevo precio</param>
        /// <param name="_idCategory">Nueva categoría asociada</param>
        /// <param name="_idProvider">Nuevo proveedor asociado</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario o si los datos no son válidos</returns>
        public bool updateProducts(int _id, string _code, string _description, int _quantity, double _price, int _idCategory, int _idProvider)
        {
            // Validación de datos
            if (_id <= 0)
                return false;
                
            if (string.IsNullOrEmpty(_code))
                return false;
            
            if (string.IsNullOrEmpty(_description))
                return false;
            
            if (_quantity < 0)
                return false;
            
            if (_price <= 0)
                return false;
            
            if (_idCategory <= 0)
                return false;
            
            if (_idProvider <= 0)
                return false;
                
            return objPrep.updateProducts(_id, _code, _description, _quantity, _price, _idCategory, _idProvider);
        }
    }
} 