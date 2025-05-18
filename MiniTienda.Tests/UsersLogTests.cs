/**
 * Proyecto MiniTienda - Pruebas Unitarias de la Capa Lógica
 * 
 * Implementación de pruebas unitarias para la clase UsersLog de la capa lógica.
 * Esta clase utiliza MSTest para validar el correcto funcionamiento de la gestión de usuarios
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
    /// Clase de pruebas unitarias para validar la funcionalidad de UsersLog
    /// </summary>
    [TestClass]
    public class UsersLogTests
    {
        /// <summary>
        /// Instancia de la clase UsersLog que será probada
        /// </summary>
        private UsersLog _usersLog;

        /// <summary>
        /// Método de inicialización que se ejecuta antes de cada prueba
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _usersLog = new UsersLog();
        }

        /// <summary>
        /// Verifica que el método showUsers() retorne un objeto DataSet válido
        /// </summary>
        [TestMethod]
        public void GetUsers_ShouldReturnDataSet()
        {
            // Act
            DataSet result = _usersLog.showUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DataSet));
        }

        /// <summary>
        /// Verifica que SaveUser() devuelva true cuando se proporcionan datos válidos
        /// </summary>
        [TestMethod]
        public void SaveUser_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            string name = "Usuario Test";
            string email = "test@example.com";
            string password = "Test1234!";
            string role = "Cliente";

            // Act
            bool result = _usersLog.SaveUser(name, email, password, role);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifica que SaveUser() devuelva false cuando el email tiene formato inválido
        /// </summary>
        [TestMethod]
        public void SaveUser_WithInvalidEmail_ShouldReturnFalse()
        {
            // Arrange
            string name = "Usuario Test";
            string email = "correo-invalido"; // Email con formato inválido
            string password = "Test1234!";
            string role = "Cliente";

            // Act
            bool result = _usersLog.SaveUser(name, email, password, role);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifica que SaveUser() devuelva false cuando la contraseña es débil
        /// </summary>
        [TestMethod]
        public void SaveUser_WithWeakPassword_ShouldReturnFalse()
        {
            // Arrange
            string name = "Usuario Test";
            string email = "test@example.com";
            string password = "123"; // Contraseña débil
            string role = "Cliente";

            // Act
            bool result = _usersLog.SaveUser(name, email, password, role);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifica que SaveUser() devuelva false cuando el nombre está vacío
        /// </summary>
        [TestMethod]
        public void SaveUser_WithEmptyName_ShouldReturnFalse()
        {
            // Arrange
            string name = ""; // Nombre vacío
            string email = "test@example.com";
            string password = "Test1234!";
            string role = "Cliente";

            // Act
            bool result = _usersLog.SaveUser(name, email, password, role);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifica que SaveUser() devuelva false cuando el rol es inválido
        /// </summary>
        [TestMethod]
        public void SaveUser_WithInvalidRole_ShouldReturnFalse()
        {
            // Arrange
            string name = "Usuario Test";
            string email = "test@example.com";
            string password = "Test1234!";
            string role = "RolInvalido"; // Rol que no existe en el sistema

            // Act
            bool result = _usersLog.SaveUser(name, email, password, role);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifica que UpdateUser() devuelva true cuando se proporcionan datos válidos
        /// </summary>
        [TestMethod]
        public void UpdateUser_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            int userId = 1; // Asegúrate de que este ID exista en tu BD de pruebas
            string name = "Usuario Actualizado";
            string email = "updated@example.com";
            string password = "UpdatedPass123!";
            string role = "Administrador";

            // Act
            bool result = _usersLog.UpdateUser(userId, name, email, password, role);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifica que UpdateUser() devuelva false cuando el ID del usuario es inválido
        /// </summary>
        [TestMethod]
        public void UpdateUser_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            int userId = -1; // ID inválido
            string name = "Usuario Actualizado";
            string email = "updated@example.com";
            string password = "UpdatedPass123!";
            string role = "Administrador";

            // Act
            bool result = _usersLog.UpdateUser(userId, name, email, password, role);

            // Assert
            Assert.IsFalse(result);
        }
    }
} 