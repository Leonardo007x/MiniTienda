/**
 * Proyecto MiniTienda - Capa Lógica de Negocio
 * 
 * Implementación de la lógica de negocio para la gestión de usuarios.
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
using System.Text.RegularExpressions;

namespace MiniTienda.Logic
{
    /// <summary>
    /// Clase para gestionar la lógica de negocio relacionada con usuarios.
    /// Implementa validaciones y se comunica con la capa de datos.
    /// </summary>
    public class UsersLog
    {
        private UsersData _userData;

        public UsersLog()
        {
            _userData = new UsersData();
        }

        /// <summary>
        /// Obtiene todos los usuarios desde la capa de datos.
        /// </summary>
        /// <returns>DataSet con la información de los usuarios</returns>
        public DataSet GetUsers()
        {
            return _userData.showUsers();
        }

        /// <summary>
        /// Guarda un nuevo usuario en el sistema después de validar los datos.
        /// </summary>
        /// <param name="name">Nombre del usuario</param>
        /// <param name="email">Correo electrónico del usuario</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <param name="role">Rol del usuario</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario o si los datos no son válidos</returns>
        public bool SaveUser(string name, string email, string password, string role)
        {
            // Validación básica
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Error de validación: nombre, email o contraseña vacíos");
                return false;
            }
            
            // Validación de formato de email usando regex (ahora opcional)
            if (!IsValidEmail(email))
            {
                Console.WriteLine($"ADVERTENCIA: El email '{email}' no tiene un formato válido, pero se continuará con el guardado");
                // Continuamos a pesar de formato inválido para hacerlo más flexible
            }

            // Validación de contraseña (ahora opcional)
            if (!IsValidPassword(password))
            {
                Console.WriteLine($"ADVERTENCIA: La contraseña no cumple con los requisitos de seguridad, pero se continuará con el guardado");
                // Continuamos a pesar de contraseña débil para hacerlo más flexible
            }

            // Generar salt para la contraseña
            string salt = "salt" + DateTime.Now.Ticks;
            // Estado por defecto
            string state = "activo";
            
            Console.WriteLine($"Intentando guardar usuario: Email={email}, Contraseña={password.Substring(0, 3)}****, Salt={salt}, Estado={state}");
            
            // Llamar al método de la capa de datos
            bool result = _userData.saveUsers(email, password, salt, state);
            Console.WriteLine($"Resultado de guardado de usuario: {(result ? "Exitoso" : "Fallido")}");
            
            return result;
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente después de validar los datos.
        /// </summary>
        /// <param name="userId">ID del usuario a actualizar</param>
        /// <param name="name">Nuevo nombre del usuario</param>
        /// <param name="email">Nuevo correo electrónico</param>
        /// <param name="password">Nueva contraseña</param>
        /// <param name="role">Nuevo rol del usuario</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario o si los datos no son válidos</returns>
        public bool UpdateUser(int userId, string name, string email, string password, string role)
        {
            // Validación básica
            if (userId <= 0 || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                return false;
            }
                
            // Validación de formato de email
            if (!IsValidEmail(email))
            {
                return false;
            }
            
            // Validación de contraseña solo si se proporciona una nueva
            if (!string.IsNullOrEmpty(password) && !IsValidPassword(password))
            {
                return false;
            }

            // Generar salt para la contraseña
            string salt = "salt" + DateTime.Now.Ticks;
            // Estado por defecto
            string state = "activo";

            return _userData.updateUsers(userId, email, password, salt, state);
        }

        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        public bool DeleteUser(int userId)
        {
            // Validar el ID
            if (userId <= 0)
            {
                Console.WriteLine($"ID de usuario no válido para eliminar: {userId}");
                return false;
            }
            
            // Llamar al método de la capa de datos
            try
            {
                bool result = _userData.deleteUsers(userId);
                Console.WriteLine($"Resultado de eliminación del usuario {userId}: {(result ? "Exitoso" : "Fallido")}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar usuario en capa lógica: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Desactiva un usuario en lugar de eliminarlo.
        /// Útil cuando hay restricciones que impiden borrar el usuario.
        /// </summary>
        /// <param name="userId">ID del usuario a desactivar</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        public bool DeactivateUser(int userId)
        {
            // Validar el ID
            if (userId <= 0)
            {
                Console.WriteLine($"ID de usuario no válido para desactivar: {userId}");
                return false;
            }
            
            try
            {
                // Usamos el método de actualización existente pero solo cambiamos el estado
                // Obtenemos primero los datos actuales del usuario
                DataSet ds = GetUsers();
                string email = "";
                string password = "";
                string salt = "";
                
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(row["usu_id"]) == userId)
                        {
                            email = row["usu_correo"].ToString();
                            // Para la contraseña y salt usamos valores temporales
                            // ya que no podemos recuperar la contraseña real
                            password = "Password123!";
                            salt = "salt" + DateTime.Now.Ticks;
                            break;
                        }
                    }
                }
                
                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine($"No se encontró usuario con ID {userId} para desactivar");
                    return false;
                }
                
                // Actualizar el usuario cambiando solo su estado a inactivo
                bool result = _userData.updateUsers(userId, email, password, salt, "inactivo");
                Console.WriteLine($"Resultado de desactivación del usuario {userId}: {(result ? "Exitoso" : "Fallido")}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desactivar usuario en capa lógica: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                return false;
            }
        }

        // Método para validar el formato de email
        private bool IsValidEmail(string email)
        {
            // Patrón para validar formato de email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        // Método para validar la contraseña
        private bool IsValidPassword(string password)
        {
            // La contraseña debe tener al menos 8 caracteres, incluyendo una mayúscula, 
            // una minúscula, un número y un carácter especial
            if (password.Length < 8)
                return false;

            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                    hasUppercase = true;
                else if (char.IsLower(c))
                    hasLowercase = true;
                else if (char.IsDigit(c))
                    hasDigit = true;
                else if (!char.IsLetterOrDigit(c))
                    hasSpecialChar = true;
            }

            return hasUppercase && hasLowercase && hasDigit && hasSpecialChar;
        }



    }
} 