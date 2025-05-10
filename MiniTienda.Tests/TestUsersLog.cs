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
        /// Prueba el método GetUsers() para verificar que se obtengan correctamente los usuarios
        /// desde la base de datos.
        /// </summary>
        public void TestShowUsers()
        {
            Console.WriteLine("Prueba de mostrar usuarios");
            try
            {
                DataSet ds = objUsersLog.GetUsers();
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
        /// Prueba el método SaveUser() para verificar que los usuarios se guarden correctamente
        /// y que las validaciones funcionen adecuadamente.
        /// </summary>
        public void TestSaveUser()
        {
            Console.WriteLine("Prueba de guardar usuario");
            try
            {
                string name = "Usuario Test";
                string email = "test@example.com";
                string password = "Test1234!";
                string role = "Cliente";

                bool result = objUsersLog.SaveUser(name, email, password, role);
                
                if (result)
                {
                    Console.WriteLine("Prueba exitosa: Usuario guardado correctamente");
                }
                else
                {
                    Console.WriteLine("Prueba fallida: No se pudo guardar el usuario");
                }

                // Prueba de validación para email inválido
                Console.WriteLine("Prueba de validación (email inválido)");
                bool resultEmailInvalido = objUsersLog.SaveUser(name, "correo-invalido", password, role);
                if (!resultEmailInvalido)
                {
                    Console.WriteLine("Prueba de validación exitosa: Se rechazó email inválido");
                }
                else
                {
                    Console.WriteLine("Prueba de validación fallida: Se aceptó email inválido");
                }

                // Prueba de validación para contraseña débil
                Console.WriteLine("Prueba de validación (contraseña débil)");
                bool resultPasswordDebil = objUsersLog.SaveUser(name, email, "123", role);
                if (!resultPasswordDebil)
                {
                    Console.WriteLine("Prueba de validación exitosa: Se rechazó contraseña débil");
                }
                else
                {
                    Console.WriteLine("Prueba de validación fallida: Se aceptó contraseña débil");
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