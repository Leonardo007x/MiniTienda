/**
 * Proyecto MiniTienda - Pruebas de la Capa Lógica
 * 
 * Implementación de pruebas para la clase CategoryLog de la capa lógica.
 * Esta clase proporciona métodos para probar las funcionalidades de gestión de categorías,
 * validando tanto el correcto funcionamiento como los mecanismos de validación.
 * 
 * Autor: Leonardo
 * Fecha: 06/05/2025
 */

using System;
using System.Data;
using MiniTienda.Logic;

namespace MiniTienda.Tests
{
    /// <summary>
    /// Clase para probar las funcionalidades de la clase CategoryLog
    /// </summary>
    public class TestCategoryLog
    {
        /// <summary>
        /// Instancia de la clase CategoryLog que será probada
        /// </summary>
        private CategoryLog objCategoryLog;

        /// <summary>
        /// Constructor de la clase. Inicializa la instancia de CategoryLog.
        /// </summary>
        public TestCategoryLog()
        {
            objCategoryLog = new CategoryLog();
        }

        /// <summary>
        /// Prueba el método showCategories() para verificar que se obtengan correctamente las categorías
        /// desde la base de datos.
        /// </summary>
        public void TestShowCategories()
        {
            Console.WriteLine("Prueba de mostrar categorías");
            try
            {
                DataTable dt = objCategoryLog.showCategories();
                if (dt != null)
                {
                    Console.WriteLine("Prueba exitosa: Se obtuvieron las categorías correctamente");
                    Console.WriteLine($"Número de categorías: {dt.Rows.Count}");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudieron obtener las categorías");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prueba el método saveCategory() para verificar que las categorías se guarden correctamente
        /// y que las validaciones funcionen adecuadamente.
        /// </summary>
        public void TestSaveCategory()
        {
            Console.WriteLine("Prueba de guardar categoría");
            try
            {
                string name = "Categoría de Prueba";
                string description = "Descripción de la categoría de prueba";

                int result = objCategoryLog.saveCategory(name, description);
                
                if (result > 0)
                {
                    Console.WriteLine("Prueba exitosa: Categoría guardada correctamente");
                    Console.WriteLine($"ID de la nueva categoría: {result}");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudo guardar la categoría");
                }

                // Prueba de validación (nombre vacío)
                Console.WriteLine("Prueba de validación (nombre vacío)");
                int resultNombreVacio = objCategoryLog.saveCategory("", description);
                if (resultNombreVacio == 0)
                {
                    Console.WriteLine("Prueba de validación exitosa: Se rechazó nombre vacío");
                }
                else
                {
                    Console.WriteLine("Prueba de validación fallida: Se aceptó nombre vacío");
                }

                // Prueba de validación (descripción vacía)
                Console.WriteLine("Prueba de validación (descripción vacía)");
                int resultDescVacia = objCategoryLog.saveCategory(name, "");
                if (resultDescVacia == 0)
                {
                    Console.WriteLine("Prueba de validación exitosa: Se rechazó descripción vacía");
                }
                else
                {
                    Console.WriteLine("Prueba de validación fallida: Se aceptó descripción vacía");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prueba el método updateCategory() para verificar que las categorías se actualicen correctamente
        /// y que las validaciones funcionen adecuadamente.
        /// </summary>
        public void TestUpdateCategory()
        {
            Console.WriteLine("Prueba de actualizar categoría");
            try
            {
                // Para esta prueba, necesitamos una categoría existente
                // Asumimos que el ID 1 existe, pero esto podría ajustarse
                int id = 1;
                string newName = "Categoría Actualizada";
                string newDescription = "Descripción actualizada";

                int result = objCategoryLog.updateCategory(id, newName, newDescription);
                
                if (result > 0)
                {
                    Console.WriteLine("Prueba exitosa: Categoría actualizada correctamente");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudo actualizar la categoría");
                }

                // Prueba de validación (ID inválido)
                Console.WriteLine("Prueba de validación (ID inválido)");
                int resultIdInvalido = objCategoryLog.updateCategory(0, newName, newDescription);
                if (resultIdInvalido == 0)
                {
                    Console.WriteLine("Prueba de validación exitosa: Se rechazó ID inválido");
                }
                else
                {
                    Console.WriteLine("Prueba de validación fallida: Se aceptó ID inválido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Ejecuta todas las pruebas disponibles para la clase CategoryLog
        /// </summary>
        public void RunAllTests()
        {
            Console.WriteLine("=== PRUEBAS DE CATEGORYLOG ===");
            TestShowCategories();
            TestSaveCategory();
            TestUpdateCategory();
            Console.WriteLine("=== FIN PRUEBAS DE CATEGORYLOG ===");
            Console.WriteLine();
        }
    }
} 