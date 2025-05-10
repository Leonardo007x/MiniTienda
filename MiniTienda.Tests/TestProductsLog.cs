/**
 * Proyecto MiniTienda - Pruebas de la Capa Lógica
 * 
 * Implementación de pruebas para la clase ProductsLog de la capa lógica.
 * Esta clase proporciona métodos para probar las funcionalidades de gestión de productos,
 * validando tanto el correcto funcionamiento como los mecanismos de validación.
 * 
 * Autor: Leonardo
 * Fecha: 02/05/2025
 */

using System;
using System.Data;
using MiniTienda.Logic;

namespace MiniTienda.Tests
{
    /// <summary>
    /// Clase para probar las funcionalidades de la clase ProductsLog
    /// </summary>
    public class TestProductsLog
    {
        /// <summary>
        /// Instancia de la clase ProductsLog que será probada
        /// </summary>
        private ProductsLog objProductsLog;

        /// <summary>
        /// Constructor de la clase. Inicializa la instancia de ProductsLog.
        /// </summary>
        public TestProductsLog()
        {
            objProductsLog = new ProductsLog();
        }

        /// <summary>
        /// Prueba el método GetProducts() para verificar que se obtengan correctamente los productos
        /// desde la base de datos.
        /// </summary>
        public void TestShowProducts()
        {
            Console.WriteLine("Prueba de mostrar productos");
            try
            {
                DataSet ds = objProductsLog.GetProducts();
                if (ds != null && ds.Tables.Count > 0)
                {
                    Console.WriteLine("Prueba exitosa: Se obtuvieron los productos correctamente");
                    Console.WriteLine($"Número de productos: {ds.Tables[0].Rows.Count}");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudieron obtener los productos");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prueba el método SaveProduct() para verificar que los productos se guarden correctamente
        /// y que las validaciones funcionen adecuadamente para casos como nombre vacío o precio negativo.
        /// </summary>
        public void TestSaveProduct()
        {
            Console.WriteLine("Prueba de guardar producto");
            try
            {
                string name = "Producto de prueba";
                decimal price = 25.99m;
                int stock = 10;
                int categoryId = 1;
                int providerId = 1;

                bool result = objProductsLog.SaveProduct(name, price, stock, categoryId, providerId);
                
                if (result)
                {
                    Console.WriteLine("Prueba exitosa: Producto guardado correctamente");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudo guardar el producto");
                }

                // Prueba de validación para nombre vacío
                Console.WriteLine("Prueba de validación (nombre vacío)");
                bool resultCodigoVacio = objProductsLog.SaveProduct("", price, stock, categoryId, providerId);
                if (!resultCodigoVacio)
                {
                    Console.WriteLine("Prueba de validación exitosa: Se rechazó nombre vacío");
                }
                else
                {
                    Console.WriteLine("Prueba de validación fallida: Se aceptó nombre vacío");
                }

                // Prueba de validación para precio negativo
                Console.WriteLine("Prueba de validación (precio negativo)");
                bool resultPrecioNegativo = objProductsLog.SaveProduct(name, -10.0m, stock, categoryId, providerId);
                if (!resultPrecioNegativo)
                {
                    Console.WriteLine("Prueba de validación exitosa: Se rechazó precio negativo");
                }
                else
                {
                    Console.WriteLine("Prueba de validación fallida: Se aceptó precio negativo");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Ejecuta todas las pruebas disponibles para la clase ProductsLog
        /// </summary>
        public void RunAllTests()
        {
            Console.WriteLine("=== PRUEBAS DE PRODUCTSLOG ===");
            TestShowProducts();
            TestSaveProduct();
            Console.WriteLine("=== FIN PRUEBAS DE PRODUCTSLOG ===");
            Console.WriteLine();
        }
    }
} 