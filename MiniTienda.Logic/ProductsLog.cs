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
using System.Collections.Generic;
using MiniTienda.Data;
using System.Data;

namespace MiniTienda.Logic
{
    /// <summary>
    /// Clase para gestionar la lógica de negocio relacionada con productos.
    /// Implementa validaciones y se comunica con la capa de datos.
    /// </summary>
    public class ProductsLog
    {
        private ProductsData _productData;
        private CategoryData _categoryData;
        private ProvidersData _providerData;

        // Límites y constantes
        private const int MAX_PRODUCT_NAME_LENGTH = 100;
        private const decimal MAX_PRODUCT_PRICE = 1000000.00m;
        private const int MAX_STOCK = 100000;

        public ProductsLog()
        {
            _productData = new ProductsData();
            _categoryData = new CategoryData();
            _providerData = new ProvidersData();
        }

        /// <summary>
        /// Obtiene todos los productos desde la capa de datos.
        /// </summary>
        /// <returns>DataSet con la información de los productos</returns>
        public DataSet GetProducts()
        {
            return _productData.showProducts();
        }

        /// <summary>
        /// Guarda un nuevo producto en el sistema después de validar los datos.
        /// </summary>
        /// <param name="name">Nombre del producto</param>
        /// <param name="price">Precio del producto</param>
        /// <param name="stock">Cantidad en inventario</param>
        /// <param name="categoryId">ID de la categoría asociada</param>
        /// <param name="providerId">ID del proveedor asociado</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario o si los datos no son válidos</returns>
        public bool SaveProduct(string name, decimal price, int stock, int categoryId, int providerId)
        {
            // Validación de datos básica
            if (!ValidateProductData(name, price, stock, categoryId, providerId))
            {
                return false;
            }

            // Generar un código para el producto
            string code = "PRD" + DateTime.Now.Ticks.ToString().Substring(10);
            
            // Convertir decimal a double para compatibilidad
            double priceAsDouble = Convert.ToDouble(price);

            return _productData.saveProducts(code, name, stock, priceAsDouble, categoryId, providerId);
        }

        /// <summary>
        /// Actualiza los datos de un producto existente después de validar los datos.
        /// </summary>
        /// <param name="productId">ID del producto a actualizar</param>
        /// <param name="name">Nuevo nombre del producto</param>
        /// <param name="price">Nuevo precio del producto</param>
        /// <param name="stock">Nueva cantidad en inventario</param>
        /// <param name="categoryId">Nueva categoría asociada</param>
        /// <param name="providerId">Nuevo proveedor asociado</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario o si los datos no son válidos</returns>
        public bool UpdateProduct(int productId, string name, decimal price, int stock, int categoryId, int providerId)
        {
            // Validación básica
            if (productId <= 0 || !ValidateProductData(name, price, stock, categoryId, providerId))
            {
                return false;
            }

            // Generar un código para el producto actualizado
            string code = "PRD" + DateTime.Now.Ticks.ToString().Substring(10);
            
            // Convertir decimal a double para compatibilidad
            double priceAsDouble = Convert.ToDouble(price);

            return _productData.updateProducts(productId, code, name, stock, priceAsDouble, categoryId, providerId);
        }

        // Método para validar todos los datos del producto
        private bool ValidateProductData(string name, decimal price, int stock, int categoryId, int providerId)
        {
            // Validación del nombre
            if (string.IsNullOrEmpty(name) || name.Length > MAX_PRODUCT_NAME_LENGTH)
            {
                return false;
            }
            
            // Validación del precio
            if (price <= 0 || price > MAX_PRODUCT_PRICE)
            {
                return false;
            }
            
            // Validación del stock
            if (stock < 0 || stock > MAX_STOCK)
            {
                return false;
            }
            
            // Validación de categoría y proveedor
            if (!CategoryExists(categoryId) || !ProviderExists(providerId))
            {
                return false;
            }

            return true;
        }

        // Método para verificar si existe la categoría
        private bool CategoryExists(int categoryId)
        {
            try
            {
                DataTable categories = _categoryData.ShowCategories();
                foreach (DataRow row in categories.Rows)
                {
                    if (Convert.ToInt32(row["id"]) == categoryId)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Método para verificar si existe el proveedor
        private bool ProviderExists(int providerId)
        {
            try
            {
                DataSet providers = _providerData.ShowProviders();
                if (providers.Tables.Count > 0)
                {
                    foreach (DataRow row in providers.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(row["prov_id"]) == providerId)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
} 