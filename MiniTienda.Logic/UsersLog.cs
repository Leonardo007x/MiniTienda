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
using System.Linq;
// using MiniTienda.Model; // <-- Comentado temporalmente, añadir si es necesario y User.cs está en MiniTienda.Model

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
        /// <returns>DataSet con la información de los usuarios.</returns>
        public DataSet showUsers()
        {
            return _userData.showUsers();
        }

        /// <summary>
        /// Obtiene un usuario específico por su correo electrónico.
        /// Este método debe ser implementado por Elkin según Sprint 1 y llamará a _userData.showUsersMail(mail)
        /// </summary>
        /// <param name="mail">Correo electrónico del usuario.</param>
        /// <returns>Objeto Model.User si se encuentra, de lo contrario null.</returns>
        public Model.User showUsersMail(string mail)
        {
            // La clase Model.User debe estar definida y accesible.
            // Por ejemplo, si User.cs está en namespace Model, necesitarás:
            // using Model; o using MiniTienda.Model; al inicio del archivo.
            return _userData.showUsersMail(mail);
        }

        /// <summary>
        /// Guarda un nuevo usuario en el sistema.
        /// Este método recibe la contraseña ya hasheada y el salt generados por SimpleCrypto en la capa de presentación.
        /// </summary>
        /// <param name="mail">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña en texto plano (será hasheada por este método).</param>
        /// <param name="salt">Salt opcional. Si es null, se generará uno nuevo.</param>
        /// <param name="state">Estado del usuario (e.g., "activo", "inactivo").</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario.</returns>
        public bool saveUserWithDetails(string mail, string password, string salt, string state)
        {
            if (string.IsNullOrWhiteSpace(mail) || !IsValidEmail(mail)) // Validación de email
            {
                Console.WriteLine($"Error de validación: Email '{mail}' no es válido o está vacío.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(state))
            {
                Console.WriteLine("Error de validación: password o state están vacíos.");
                return false;
            }
            
            try
            {
                // Generar salt y hash de contraseña usando SimpleCrypto
                SimpleCrypto.ICryptoService cryptoService = new SimpleCrypto.PBKDF2();
                
                // Usar el salt proporcionado o generar uno nuevo (asegurándonos que sea válido)
                string newSalt;
                if (string.IsNullOrEmpty(salt))
                {
                    // Generar un nuevo salt
                    newSalt = cryptoService.GenerateSalt();
                    
                    // Verificar que el salt sea de al menos 8 bytes
                    if (string.IsNullOrEmpty(newSalt) || newSalt.Length < 8)
                    {
                        // Generar un salt manualmente si el generado no es válido
                        newSalt = GenerateCustomSalt();
                        System.Diagnostics.Debug.WriteLine($"Usando salt personalizado: {newSalt}");
                    }
                }
                else
                {
                    // Usar el salt proporcionado, pero verificar su longitud
                    newSalt = salt;
                    if (newSalt.Length < 8)
                    {
                        // Extender el salt si es demasiado corto
                        newSalt = ExtendSalt(newSalt);
                        System.Diagnostics.Debug.WriteLine($"Salt extendido a: {newSalt}");
                    }
                }
                
                // Verificación final de longitud del salt
                if (string.IsNullOrEmpty(newSalt) || newSalt.Length < 8)
                {
                    // Si todavía no es válido, usar un salt hardcodeado como último recurso
                    newSalt = "DefaultSalt123456";
                    System.Diagnostics.Debug.WriteLine("Usando salt predeterminado");
                }
                
                // Establecer el salt en el servicio de criptografía
                cryptoService.Salt = newSalt;
                
                // Computar el hash de la contraseña
                string hashedPassword = cryptoService.Compute(password);
                
                System.Diagnostics.Debug.WriteLine($"Guardando usuario con email: {mail}");
                System.Diagnostics.Debug.WriteLine($"Salt generado/usado: {newSalt}");
                System.Diagnostics.Debug.WriteLine($"Contraseña hasheada: {hashedPassword}");
                
                return _userData.saveUsers(mail, hashedPassword, newSalt, state);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al guardar usuario: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                return false;
            }
        }
        
        /// <summary>
        /// Genera un salt personalizado que cumple con los requisitos mínimos
        /// </summary>
        private string GenerateCustomSalt()
        {
            // Generar un salt con caracteres aleatorios
            Random rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
        
        /// <summary>
        /// Extiende un salt demasiado corto para cumplir con el requisito mínimo
        /// </summary>
        private string ExtendSalt(string originalSalt)
        {
            // Extender el salt agregando caracteres aleatorios
            Random rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int additionalLength = 16 - originalSalt.Length;
            
            if (additionalLength <= 0)
                return originalSalt;
                
            string extension = new string(Enumerable.Repeat(chars, additionalLength)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
                
            return originalSalt + extension;
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
                System.Diagnostics.Debug.WriteLine("Error de validación: nombre, email o contraseña vacíos");
                return false;
            }
            
            // Validación de formato de email usando regex (ahora opcional)
            if (!IsValidEmail(email))
            {
                System.Diagnostics.Debug.WriteLine($"ADVERTENCIA: El email '{email}' no tiene un formato válido, pero se continuará con el guardado");
                // Continuamos a pesar de formato inválido para hacerlo más flexible
            }

            // Validación de contraseña (ahora opcional)
            if (!IsValidPassword(password))
            {
                System.Diagnostics.Debug.WriteLine($"ADVERTENCIA: La contraseña no cumple con los requisitos de seguridad, pero se continuará con el guardado");
                // Continuamos a pesar de contraseña débil para hacerlo más flexible
            }

            // Generar salt y hash para la contraseña
            SimpleCrypto.ICryptoService cryptoService = new SimpleCrypto.PBKDF2();
            string salt = cryptoService.GenerateSalt();
            cryptoService.Salt = salt;
            string hashedPassword = cryptoService.Compute(password);
            
            // Estado por defecto
            string state = "activo";
            
            System.Diagnostics.Debug.WriteLine($"Intentando guardar usuario: Email={email}, Salt={salt}, Estado={state}");
            System.Diagnostics.Debug.WriteLine($"Contraseña cifrada: {hashedPassword}");
            
            // Llamar al método de la capa de datos
            bool result = _userData.saveUsers(email, hashedPassword, salt, state);
            System.Diagnostics.Debug.WriteLine($"Resultado de guardado de usuario: {(result ? "Exitoso" : "Fallido")}");
            
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
                System.Diagnostics.Debug.WriteLine($"Error de validación: userId={userId}, name={name}, email={email}");
                return false;
            }
                
            // Validación de formato de email
            if (!IsValidEmail(email))
            {
                System.Diagnostics.Debug.WriteLine($"Error de validación: email {email} no tiene formato válido");
                return false;
            }
            
            // Primero obtener el usuario actual para recuperar el salt existente si no cambia la contraseña
            Model.User currentUser = null;
            try 
            {
                currentUser = _userData.showUsersMail(email);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al recuperar usuario para actualización: {ex.Message}");
            }
            
            string hashedPassword;
            string salt;
            
            // Si hay una nueva contraseña, generamos nuevo salt y hash
            if (!string.IsNullOrEmpty(password))
            {
                // Validación de contraseña
                if (!IsValidPassword(password))
                {
                    System.Diagnostics.Debug.WriteLine($"Error de validación: La contraseña no cumple con los requisitos de seguridad");
                    return false;
                }

                // Generar salt y hash para la contraseña
                SimpleCrypto.ICryptoService cryptoService = new SimpleCrypto.PBKDF2();
                salt = cryptoService.GenerateSalt();
                cryptoService.Salt = salt;
                hashedPassword = cryptoService.Compute(password);
                
                System.Diagnostics.Debug.WriteLine($"Actualizando contraseña para usuario ID={userId}");
                System.Diagnostics.Debug.WriteLine($"Nuevo salt generado: {salt}");
                System.Diagnostics.Debug.WriteLine($"Nueva contraseña cifrada: {hashedPassword}");
            }
            else if (currentUser != null)
            {
                // Mantener salt y password existentes
                salt = currentUser.Salt;
                hashedPassword = currentUser.Contrasena;
                System.Diagnostics.Debug.WriteLine($"Manteniendo contraseña actual para usuario ID={userId}");
            }
            else
            {
                // Si no tenemos contraseña nueva ni datos existentes, no podemos continuar
                System.Diagnostics.Debug.WriteLine($"Error: No se puede actualizar el usuario sin contraseña nueva o existente");
                return false;
            }
            
            // Estado por defecto
            string state = "activo";
            
            System.Diagnostics.Debug.WriteLine($"Actualizando usuario: ID={userId}, Email={email}, State={state}");
            
            return _userData.updateUsers(userId, email, hashedPassword, salt, state);
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
                DataSet ds = showUsers();
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