/**
 * Proyecto MiniTienda - Pruebas Unitarias de la Capa Lógica
 * 
 * Implementación de pruebas unitarias para la clase ProductsLog de la capa lógica.
 * Esta clase utiliza MSTest para validar el correcto funcionamiento de la gestión de productos
 * y verificar que los mecanismos de validación funcionen correctamente.
 * 
 * Autor: Leonardo
 * Fecha: 09/05/2025    
 */

using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniTienda.Logic;

namespace MiniTienda.Tests
{
    /// <summary>
    /// Clase de pruebas unitarias para validar la funcionalidad de ProductsLog
    /// </summary>
    [TestClass]
    public class ProductsLogTests
    {
        /// <summary>
        /// Instancia de la clase ProductsLog que será probada
        /// </summary>
        private ProductsLog _productsLog;

        /// <summary>
        /// Método de inicialización que se ejecuta antes de cada prueba
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _productsLog = new ProductsLog();
        }

        /// <summary>
        /// Verifica que el método GetProducts() retorne un objeto DataSet válido
        /// </summary>
        [TestMethod]
        public void GetProducts_ShouldReturnDataSet()
        {
            // Act
            DataSet result = _productsLog.GetProducts();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DataSet));
        }

        /// <summary>
        /// Verifica que SaveProduct() devuelva true cuando se proporcionan datos válidos
        /// </summary>
        [TestMethod]
        public void SaveProduct_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            string name = "Producto de Prueba";
            decimal price = 19.99m;
            int stock = 100;
            int categoryId = 1; // Asegúrate de que este ID exista en tu BD de pruebas
            int providerId = 1; // Asegúrate de que este ID exista en tu BD de pruebas

            // Act
            bool result = _productsLog.SaveProduct(name, price, stock, categoryId, providerId);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifica que SaveProduct() devuelva false cuando el nombre está vacío
        /// </summary>
        [TestMethod]
        public void SaveProduct_WithEmptyName_ShouldReturnFalse()
        {
            // Arrange
            string name = "";
            decimal price = 19.99m;
            int stock = 100;
            int categoryId = 1;
            int providerId = 1;

            // Act
            bool result = _productsLog.SaveProduct(name, price, stock, categoryId, providerId);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifica que SaveProduct() devuelva false cuando el precio es negativo
        /// </summary>
        [TestMethod]
        public void SaveProduct_WithNegativePrice_ShouldReturnFalse()
        {
            // Arrange
            string name = "Producto de Prueba";
            decimal price = -10.0m; // Precio negativo no permitido
            int stock = 100;
            int categoryId = 1;
            int providerId = 1;

            // Act
            bool result = _productsLog.SaveProduct(name, price, stock, categoryId, providerId);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifica que SaveProduct() devuelva false cuando el stock es negativo
        /// </summary>
        [TestMethod]
        public void SaveProduct_WithNegativeStock_ShouldReturnFalse()
        {
            // Arrange
            string name = "Producto de Prueba";
            decimal price = 19.99m;
            int stock = -5; // Stock negativo no permitido
            int categoryId = 1;
            int providerId = 1;

            // Act
            bool result = _productsLog.SaveProduct(name, price, stock, categoryId, providerId);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifica que SaveProduct() devuelva false cuando el ID de categoría no existe
        /// </summary>
        [TestMethod]
        public void SaveProduct_WithInvalidCategoryId_ShouldReturnFalse()
        {
            // Arrange
            string name = "Producto de Prueba";
            decimal price = 19.99m;
            int stock = 100;
            int categoryId = 9999; // ID de categoría que no existe
            int providerId = 1;

            // Act
            bool result = _productsLog.SaveProduct(name, price, stock, categoryId, providerId);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifica que UpdateProduct() devuelva true cuando se proporcionan datos válidos
        /// </summary>
        [TestMethod]
        public void UpdateProduct_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            int productId = 1; // Asegúrate de que este ID exista en tu BD de pruebas
            string name = "Producto Actualizado";
            decimal price = 29.99m;
            int stock = 200;
            int categoryId = 1;
            int providerId = 1;

            // Act
            bool result = _productsLog.UpdateProduct(productId, name, price, stock, categoryId, providerId);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifica que UpdateProduct() devuelva false cuando el ID del producto es inválido
        /// </summary>
        [TestMethod]
        public void UpdateProduct_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            int productId = -1; // ID inválido
            string name = "Producto Actualizado";
            decimal price = 29.99m;
            int stock = 200;
            int categoryId = 1;
            int providerId = 1;

            // Act
            bool result = _productsLog.UpdateProduct(productId, name, price, stock, categoryId, providerId);

            // Assert
            Assert.IsFalse(result);
        }
    }
} 