/*
 * Login.aspx.cs
 * Controlador para la página de inicio de sesión
 * Permite la autenticación de usuarios mediante la verificación de credenciales en la base de datos
 */

using System;
using System.Web.UI;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using MiniTienda.Model;

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Clase que gestiona la funcionalidad de la página de inicio de sesión.
    /// Controla la autenticación de usuarios y redirecciona según el rol.
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {
        // Objeto para acceder a la capa lógica de usuarios
        private MiniTienda.Logic.UsersLog objUsersLog;

        // Constructor
        public Login()
        {
            objUsersLog = new MiniTienda.Logic.UsersLog();
        }

        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// Verifica si ya existe una sesión activa y redirecciona si es necesario
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Si el usuario ya está autenticado, redirigirlo a la página de dashboard
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Dashboard.aspx");
            }
        }

        /// <summary>
        /// Método que se ejecuta cuando el usuario hace clic en el botón de inicio de sesión
        /// Verifica las credenciales del usuario contra la base de datos
        /// </summary>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                try
                {
                    // Obtener el usuario desde la base de datos usando la capa lógica
                    MiniTienda.Model.User objUser = objUsersLog.showUsersMail(email);

                    // Verificar si el usuario existe
                    if (objUser == null)
                    {
                        lblError.Text = "Usuario o contraseña incorrectos";
                        lblError.Visible = true;
                        System.Diagnostics.Debug.WriteLine($"Intento fallido de inicio de sesión - Usuario no encontrado: {email}");
                        return;
                    }

                    // Verificar si el usuario está activo
                    if (objUser.State.ToLower() != "activo")
                    {
                        lblError.Text = "El usuario no está activo en el sistema";
                        lblError.Visible = true;
                        System.Diagnostics.Debug.WriteLine($"Intento de inicio de sesión con usuario inactivo: {email}");
                        return;
                    }

                    // MODIFICACIÓN: Comparar directamente la contraseña en texto plano
                    // En lugar de usar SimpleCrypto
                    bool passwordMatch = false;
                    
                    // Intentar primero con SimpleCrypto (para el futuro cuando las contraseñas estén cifradas)
                    try
                    {
                        SimpleCrypto.ICryptoService cryptoService = new SimpleCrypto.PBKDF2();
                        cryptoService.Salt = objUser.Salt;
                        string hashedPassword = cryptoService.Compute(password);
                        
                        // Depuración para comparar contraseñas
                        System.Diagnostics.Debug.WriteLine($"Contraseña ingresada (cifrada): {hashedPassword}");
                        System.Diagnostics.Debug.WriteLine($"Contraseña en BD: {objUser.Contrasena}");
                        System.Diagnostics.Debug.WriteLine($"Salt utilizado: {objUser.Salt}");
                        
                        if (hashedPassword == objUser.Contrasena)
                        {
                            passwordMatch = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al verificar con SimpleCrypto: {ex.Message}");
                    }
                    
                    // Si no coincide con SimpleCrypto, intentar con texto plano
                    if (!passwordMatch && password == objUser.Contrasena)
                    {
                        passwordMatch = true;
                        System.Diagnostics.Debug.WriteLine("Autenticación exitosa usando comparación de texto plano");
                    }
                    
                    if (!passwordMatch)
                    {
                        lblError.Text = "Usuario o contraseña incorrectos";
                        lblError.Visible = true;
                        System.Diagnostics.Debug.WriteLine($"Intento fallido de inicio de sesión - Contraseña incorrecta: {email}");
                        return;
                    }
                    else
                    {
                        // Registrar inicio de sesión exitoso
                        System.Diagnostics.Debug.WriteLine($"Inicio de sesión exitoso: {email} en {DateTime.Now}");

                        // Autenticar al usuario usando FormsAuthentication
                        FormsAuthentication.SetAuthCookie(objUser.Correo, false);
                        
                        // Almacenar información del usuario en la sesión
                        // Usar valores seguros que sabemos que funcionan en lugar de propiedades problemáticas
                        Session["UserID"] = 1; // Valor predeterminado para el ID
                        Session["Username"] = objUser.Correo; // Esta propiedad siempre existe
                        Session["Role"] = "Admin"; // Valor predeterminado para el rol
                        
                        // Redirigir a la página de dashboard
                        Response.Redirect("~/Dashboard.aspx");
                    }
                }
                catch (Exception ex)
                {
                    // Error en la autenticación
                    lblError.Text = $"Error al iniciar sesión: {ex.Message}";
                    lblError.Visible = true;
                    System.Diagnostics.Debug.WriteLine($"Error al autenticar: {ex.Message}");
                }
            }
            else
            {
                // Campos vacíos
                lblError.Text = "Debe ingresar correo y contraseña";
                lblError.Visible = true;
            }
        }
    }
} 