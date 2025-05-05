
using System;
using System.Data;
using MiniTienda.Logic;

namespace MiniTienda.Tests
{
    /// Clase para probar las funcionalidades de la clase CategoryLog
    public class TestCategoryLog
    {
        private CategoryLog objCategoryLog;

 
        /// Constructor de la clase. Inicializa la instancia de CategoryLog.
        public TestCategoryLog()
        {
            objCategoryLog = new CategoryLog();
        }

        /// Prueba el método showCategories() para verificar que se obtengan correctamente las categorías.
        public void TestShowCategories()
        {
            Console.WriteLine("Prueba de mostrar categorías");
            try
            {
                DataTable dt = objCategoryLog.showCategories();
                if (dt != null && dt.Rows.Count > 0)
                {
                    Console.WriteLine("Categorías obtenidas correctamente");
                    Console.WriteLine($"Total de categorías: {dt.Rows.Count}");
                }
                else
                {
                    Console.WriteLine("No se encontraron categorías");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }

        /// Prueba los métodos saveCategory, updateCategory y deleteCategory.
        public void TestCRUDCategory()
        {
            Console.WriteLine("Prueba de creación, actualización y eliminación de categoría");

            int insertedId = -1;
            try
            {
                // 1. Guardar categoría
                Console.WriteLine("➡ Guardando categoría...");
                insertedId = objCategoryLog.saveCategory("Categoria de prueba", "Descripcion de la categoria de prueba");
                if (insertedId > 0)
                    Console.WriteLine($" Categoría guardada con ID: {insertedId}");
                else
                    Console.WriteLine(" Error al guardar categoría");

                // 2. Actualizar categoría
                Console.WriteLine("➡️ Actualizando categoría...");
                int updateResult = objCategoryLog.updateCategory(insertedId, "Prueba para actualizar categoria", "Descripción actualizada");
                if (updateResult > 0)
                    Console.WriteLine(" Categoría actualizada correctamente");
                else
                    Console.WriteLine(" Error al actualizar categoría");

                // 3. Eliminar categoría
                Console.WriteLine(" Eliminando categoría...");
                int deleteResult = objCategoryLog.deleteCategory(insertedId);
                if (deleteResult > 0)
                    Console.WriteLine("Categoría eliminada correctamente");
                else
                    Console.WriteLine("Error al eliminar categoría");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }


        /// Ejecuta todas las pruebas disponibles para CategoryLog
        public void RunAllTests()
        {
            Console.WriteLine("=== PRUEBAS DE CATEGORYLOG ===");
            TestShowCategories();
            TestCRUDCategory();
            Console.WriteLine("=== FIN PRUEBAS DE CATEGORYLOG ===");
            Console.WriteLine();
        }
    }
}
