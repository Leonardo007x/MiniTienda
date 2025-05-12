
/**
 * Proyecto MiniTienda - Pruebas de la Capa Lógica
 * 
 * Implementación de pruebas para la clase ProvidersLog de la capa lógica.
 * Esta clase proporciona métodos para probar las funcionalidades de gestión de proveedores,
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
    /// Clase para probar las funcionalidades de la clase ProvidersLog
    /// </summary>
    public class TestProvidersLog
    {
        /// <summary>
        /// Instancia de la clase ProvidersLog que será probada
        /// </summary>
        private ProvidersLog objProvidersLog;

        /// <summary>
        /// Constructor de la clase. Inicializa la instancia de ProvidersLog.
        /// </summary>
        public TestProvidersLog()
        {
            objProvidersLog = new ProvidersLog();
        }

        /// <summary>
        /// Prueba el método ShowProviders() para verificar que se obtengan correctamente los proveedores
        /// desde la base de datos.
        /// </summary>
        public void TestShowProviders()
        {
            Console.WriteLine("Prueba de mostrar proveedores");
            try
            {
                DataSet ds = objProvidersLog.ShowProviders();
                if (ds != null && ds.Tables.Count > 0)
                {
                    Console.WriteLine("Prueba exitosa: Se obtuvieron los proveedores correctamente");
                    Console.WriteLine($"Número de proveedores: {ds.Tables[0].Rows.Count}");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudieron obtener los proveedores");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prueba el método SaveProvider() para verificar que los proveedores se guarden correctamente
        /// y que las validaciones funcionen adecuadamente.
        /// </summary>
        public void TestSaveProvider()
        {
            Console.WriteLine("Prueba de guardar proveedor");
            try
            {
                string nit = "12345678-9";
                string name = "Proveedor de Prueba";

                bool result = objProvidersLog.SaveProvider(nit, name);
                
                if (result)
                {
                    Console.WriteLine("Prueba exitosa: Proveedor guardado correctamente");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudo guardar el proveedor");
                }

                // Prueba de validación (nombre vacío)
                Console.WriteLine("Prueba de validación (nombre vacío)");
                try
                {
                    objProvidersLog.SaveProvider(nit, "");
                    Console.WriteLine("Prueba de validación fallida: Se aceptó nombre vacío");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Prueba de validación exitosa: Se rechazó nombre vacío");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado en prueba de validación: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Si intentamos guardar un proveedor con un nombre que ya existe, 
                // podríamos recibir una excepción
                if (ex.Message.Contains("Ya existe un proveedor con este nombre"))
                {
                    Console.WriteLine("Prueba de validación exitosa: Se detectó nombre duplicado");
                }
                else
                {
                    Console.WriteLine($"Error en la prueba: {ex.Message}");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prueba el método UpdateProvider() para verificar que los proveedores se actualicen correctamente
        /// y que las validaciones funcionen adecuadamente.
        /// </summary>
        public void TestUpdateProvider()
        {
            Console.WriteLine("Prueba de actualizar proveedor");
            try
            {
                // Para esta prueba, necesitamos un proveedor existente
                // Asumimos que el ID 1 existe, pero esto podría ajustarse
                int id = 1;
                string nit = "98765432-1";
                string name = "Proveedor Actualizado";

                bool result = objProvidersLog.UpdateProvider(id, nit, name);
                
                if (result)
                {
                    Console.WriteLine("Prueba exitosa: Proveedor actualizado correctamente");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudo actualizar el proveedor");
                }

                // Prueba de validación (ID inválido)
                Console.WriteLine("Prueba de validación (ID inválido)");
                try
                {
                    objProvidersLog.UpdateProvider(0, nit, name);
                    Console.WriteLine("Prueba de validación fallida: Se aceptó ID inválido");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Prueba de validación exitosa: Se rechazó ID inválido");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado en prueba de validación: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Ejecuta todas las pruebas disponibles para la clase ProvidersLog
        /// </summary>
        public void RunAllTests()
        {
            Console.WriteLine("=== PRUEBAS DE PROVIDERSLOG ===");
            TestShowProviders();
            TestSaveProvider();
            TestUpdateProvider();
            Console.WriteLine("=== FIN PRUEBAS DE PROVIDERSLOG ===");
            Console.WriteLine();
        }
    }
} 
