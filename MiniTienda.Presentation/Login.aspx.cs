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

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Clase que gestiona la funcionalidad de la página de inicio de sesión.
    /// Controla la autenticación de usuarios y redirecciona según el rol.
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {
        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// Verifica si ya existe una sesión activa y redirecciona si es necesario
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Si el usuario ya está autenticado, redirigirlo a la página principal
            if (Session["UserID"] != null)
            {
                Response.Redirect("~/Index.aspx");
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
                    // Para facilitar las pruebas, mantenemos el usuario hardcoded
                    if (email == "admin@mitienda.com" && password == "admin")
                    {
                        // Guardar información del usuario en sesión
                        Session["UserID"] = 1;
                        Session["Username"] = "Administrador";
                        Session["UserRole"] = "Admin";

                        // Redirigir a la página principal
                        Response.Redirect("~/Index.aspx");
                        return;
                    }

                    // Conexión con la base de datos para autenticar al usuario
                    string connectionString = ConfigurationManager.ConnectionStrings["MiniTiendaDB"].ConnectionString;
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        
                        // Verificación de usuario en la base de datos
                        using (MySqlCommand command = new MySqlCommand("SELECT * FROM tbl_usuarios WHERE usu_correo = @Email", connection))
                        {
                            command.Parameters.AddWithValue("@Email", email);
                            
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int userId = Convert.ToInt32(reader["usu_id"]);
                                    string estado = reader["usu_estado"].ToString();
                                    
                                    // Verificar si el usuario está activo
                                    if (estado.ToLower() != "activo")
                                    {
                                        lblError.Text = "La cuenta no está activa";
                                        lblError.Visible = true;
                                        return;
                                    }

                                    // Verificación de contraseña
                                    string storedPassword = reader["usu_contrasena"].ToString();
                                    
                                    // Verificar si la contraseña ingresada coincide con la almacenada
                                    if (password == storedPassword)
                                    {
                                        // Contraseña correcta, iniciar sesión
                                        Session["UserID"] = userId;
                                        Session["Username"] = email;
                                        Session["UserRole"] = "usuario";
                                        Response.Redirect("~/Index.aspx");
                                    }
                                    else
                                    {
                                        // Contraseña incorrecta
                                        lblError.Text = "Contraseña incorrecta";
                                        lblError.Visible = true;
                                    }
                                }
                                else
                                {
                                    // Usuario no encontrado
                                    lblError.Text = "Correo no encontrado en la base de datos";
                                    lblError.Visible = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Error en la autenticación
                    lblError.Text = "Error al iniciar sesión: " + ex.Message;
                    lblError.Visible = true;
                }
            }
            else
            {
                // Campos vacíos
                lblError.Text = "Debe ingresar correo y contraseña";
                lblError.Visible = true;
            }
        }
        
        private bool VerificarContrasena(string password, string storedHash, string salt)
        {
            try
            {
                // Recrear el hash con la misma sal y contraseña ingresada
                using (var sha256 = SHA256.Create())
                {
                    // Combinar la contraseña con la sal
                    string passwordWithSalt = password + salt;
                    
                    // Convertir a bytes
                    byte[] bytes = Encoding.UTF8.GetBytes(passwordWithSalt);
                    
                    // Calcular el hash
                    byte[] hash = sha256.ComputeHash(bytes);
                    
                    // Convertir a string
                    string computedHash = Convert.ToBase64String(hash);
                    
                    // Comparar con el hash almacenado
                    return storedHash == computedHash;
                }
            }
            catch
            {
                // En caso de error, denegar acceso
                return false;
            }
        }
    }
} 