/*
 * AdminSetup.aspx.cs
 * Utilidad para crear un usuario administrador en caso de que no exista
 * Debe utilizarse únicamente por personal autorizado
 */

using System;
using System.Web.UI;

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Página de utilidad para crear un usuario administrador en la base de datos
    /// Solo debe ser accesible para administradores del sistema
    /// </summary>
    public partial class AdminSetup : System.Web.UI.Page
    {
        // Objeto para acceder a la capa lógica de usuarios
        private MiniTienda.Logic.UsersLog objUsersLog;

        // Constructor
        public AdminSetup()
        {
            objUsersLog = new MiniTienda.Logic.UsersLog();
        }

        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMensaje.Visible = false;
            }
        }

        /// <summary>
        /// Método que se ejecuta al hacer clic en el botón para crear un usuario administrador
        /// </summary>
        protected void btnCrearAdmin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            
            // Validaciones básicas
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MostrarMensaje("Debe ingresar un correo y contraseña válidos", "danger");
                return;
            }
            
            try
            {
                // Verificar si el usuario ya existe
                MiniTienda.Model.User existingUser = objUsersLog.showUsersMail(email);
                
                if (existingUser != null)
                {
                    // Si el usuario existe, actualizamos su contraseña
                    ResetearContrasenaUsuario(existingUser, password);
                    MostrarMensaje($"Se ha actualizado la contraseña para el usuario {email}", "success");
                }
                else
                {
                    // Si el usuario no existe, lo creamos
                    bool result = objUsersLog.saveUserWithDetails(email, password, null, "activo");
                    
                    if (result)
                    {
                        MostrarMensaje($"Se ha creado correctamente el usuario {email}", "success");
                        
                        // Verificar que se haya creado correctamente
                        MiniTienda.Model.User newUser = objUsersLog.showUsersMail(email);
                        if (newUser != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Usuario creado: {newUser.Correo}");
                            System.Diagnostics.Debug.WriteLine($"Salt: {newUser.Salt}");
                            System.Diagnostics.Debug.WriteLine($"Contraseña: {newUser.Contrasena}");
                        }
                    }
                    else
                    {
                        MostrarMensaje("No se pudo crear el usuario. Revise los logs para más información.", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al crear/actualizar usuario: {ex.Message}", "danger");
                System.Diagnostics.Debug.WriteLine($"Error en AdminSetup: {ex.ToString()}");
            }
        }
        
        /// <summary>
        /// Resetea la contraseña de un usuario existente
        /// </summary>
        private void ResetearContrasenaUsuario(MiniTienda.Model.User user, string newPassword)
        {
            if (user == null) return;
            
            // Buscar el ID del usuario
            System.Data.DataSet ds = objUsersLog.showUsers();
            int userId = -1;
            
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    if (row["usu_correo"].ToString() == user.Correo)
                    {
                        userId = Convert.ToInt32(row["usu_id"]);
                        break;
                    }
                }
            }
            
            if (userId > 0)
            {
                // Actualizar la contraseña
                bool success = objUsersLog.UpdateUser(userId, user.Correo, user.Correo, newPassword, "admin");
                System.Diagnostics.Debug.WriteLine($"Resultado de actualización de contraseña: {(success ? "Exitoso" : "Fallido")}");
            }
            else
            {
                throw new Exception("No se pudo obtener el ID del usuario para actualizar su contraseña");
            }
        }
        
        /// <summary>
        /// Muestra un mensaje al usuario
        /// </summary>
        private void MostrarMensaje(string mensaje, string tipo)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = $"alert alert-{tipo}";
            lblMensaje.Visible = true;
        }
    }
} 