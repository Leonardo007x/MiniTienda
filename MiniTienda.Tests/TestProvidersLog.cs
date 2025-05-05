/**
 * Proyecto MiniTienda - Pruebas de la Capa Lógica
 * 
 * Implementación de pruebas para la clase ProvidersLog de la capa lógica.
 * Esta clase proporciona métodos para probar las funcionalidades de gestión de proveedores.
 * 
 * Autor: Brayan
 * Fecha: 02/05/2025
 */

using System;
using System.Data;
using MiniTienda.Logic;

namespace MiniTienda.Tests
{
    /// <summary>
    /// Clase para probar las funcionalidades de la clase ProvidersLog
    /// </summary>
    public class TestProvidersLog
    {
        /// <summary>
        /// Instancia de la clase ProvidersLog que será probada
        /// </summary>
        private ProvidersLog objProvidersLog;

        public TestProvidersLog()
        {
            objProvidersLog = new ProvidersLog();
        }

        public void TestShowProviders()
        {
            Console.WriteLine("Prueba de mostrar proveedores");
            try
            {
                DataSet ds = objProvidersLog.ShowProviders();
                if (ds != null && ds.Tables.Count > 0)
                {
                    Console.WriteLine("Proveedores obtenidos correctamente");
                    Console.WriteLine($"Cantidad: {ds.Tables[0].Rows.Count}");
                }
                else
                {
                    Console.WriteLine("No se encontraron proveedores");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener proveedores: {ex.Message}");
            }
            Console.WriteLine();
        }

        public void TestSaveProvider()
        {
            Console.WriteLine("Prueba de guardar proveedor");
            try
            {
                string nit = "1234567890";
                string nombre = "Proveedor de Prueba";

                bool result = objProvidersLog.SaveProvider(nit, nombre);
                if (result)
                {
                    Console.WriteLine("Proveedor guardado exitosamente");
                }
                else
                {
                    Console.WriteLine("No se pudo guardar el proveedor");
                }

                // Prueba de validación
                Console.WriteLine("Prueba con datos vacíos:");
                bool validacion = objProvidersLog.SaveProvider("", "");
                if (!validacion)
                {
                    Console.WriteLine("Validación exitosa: no se aceptaron datos vacíos");
                }
                else
                {
                    Console.WriteLine("Validación fallida: se aceptaron datos inválidos");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar proveedor: {ex.Message}");
            }
            Console.WriteLine();
        }

        public void RunAllTests()
        {
            Console.WriteLine("=== PRUEBAS DE PROVIDERSLOG ===");
            TestShowProviders();
            TestSaveProvider();
            Console.WriteLine("=== FIN PRUEBAS DE PROVIDERSLOG ===");
        }
    }
}
