/**
 * Proyecto MiniTienda - Pruebas de la Capa Lógica
 * 
 * Implementación de pruebas para la clase UsersLog de la capa lógica.
 * Esta clase proporciona métodos para probar las funcionalidades de gestión de usuarios,
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
    /// Clase para probar las funcionalidades de la clase UsersLog
    /// </summary>
    public class TestUsersLog
    {
        /// <summary>
        /// Instancia de la clase UsersLog que será probada
        /// </summary>
        private UsersLog objUsersLog;

        /// <summary>
        /// Constructor de la clase. Inicializa la instancia de UsersLog.
        /// </summary>
        public TestUsersLog()
        {
            objUsersLog = new UsersLog();
        }

        /// <summary>
        /// Prueba el método showUsers() para verificar que se obtengan correctamente los usuarios
        /// desde la base de datos.
        /// </summary>
        public void TestShowUsers()
        {
            Console.WriteLine("Prueba de mostrar usuarios");
            try
            {
                DataSet ds = objUsersLog.showUsers();
                if (ds != null && ds.Tables.Count > 0)
                {
                    Console.WriteLine("Prueba exitosa: Se obtuvieron los usuarios correctamente");
                    Console.WriteLine($"Número de usuarios: {ds.Tables[0].Rows.Count}");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudieron obtener los usuarios");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prueba el método saveUsers() para verificar que los usuarios se guarden correctamente
        /// y que las validaciones funcionen adecuadamente.
        /// </summary>
        public void TestSaveUser()
        {
            Console.WriteLine("Prueba de guardar usuario");
            try
            {
                string mail = "usuario_prueba@test.com";
                string password = "password123";
                string salt = "abc123";
                string state = "Activo";

                bool result = objUsersLog.saveUsers(mail, password, salt, state);
                
                if (result)
                {
                    Console.WriteLine("Prueba exitosa: Usuario guardado correctamente");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudo guardar el usuario");
                }

                // Prueba de validación
                Console.WriteLine("Prueba de validación (datos vacíos)");
                bool resultVacio = objUsersLog.saveUsers("", "", "", "");
                if (!resultVacio)
                {
                    Console.WriteLine("Prueba de validación exitosa: Se rechazaron datos vacíos");
                }
                else
                {
                    Console.WriteLine("Prueba de validación fallida: Se aceptaron datos vacíos");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la prueba: {ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Ejecuta todas las pruebas disponibles para la clase UsersLog
        /// </summary>
        public void RunAllTests()
        {
            Console.WriteLine("=== PRUEBAS DE USERSLOG ===");
            TestShowUsers();
            TestSaveUser();
            Console.WriteLine("=== FIN PRUEBAS DE USERSLOG ===");
            Console.WriteLine();
        }
    }
} 