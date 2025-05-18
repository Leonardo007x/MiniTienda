/**
 * Proyecto MiniTienda - Capa de Presentación
 * 
 * Lógica para la página de inicio del sistema.
 * Muestra información de bienvenida al usuario autenticado.
 * 

 * Fecha: 15/05/2025
 * Versión: 1.1 (Sprint 3)
 */

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
// Comentamos la referencia problemática
// using MiniTienda.Logic;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Clase que gestiona la funcionalidad de la página principal del sistema.
    /// </summary>
    public partial class Index : System.Web.UI.Page
    {
        /// <summary>
        /// Método que se ejecuta cuando se carga la página.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Validar que el usuario está autenticado
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Mostrar el nombre de usuario autenticado
                if (lblUsername != null)
                {
                    lblUsername.Text = User.Identity.Name;
                }

                // Registrar acceso en el log (en un entorno real se registraría en la BD)
                System.Diagnostics.Debug.WriteLine($"Usuario {User.Identity.Name} accedió a la aplicación el {DateTime.Now}");
            }
        }

        /// <summary>
        /// Maneja el evento de clic en el botón de cerrar sesión.
        /// Cierra la sesión del usuario y lo redirige a la página de inicio de sesión.
        /// </summary>
        protected void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                // Registrar cierre de sesión (en un entorno real se registraría en la BD)
                DateTime ahora = DateTime.Now;
                System.Diagnostics.Debug.WriteLine($"Usuario {User.Identity.Name} cerró sesión el {ahora}");
                
                // Cerrar la sesión del usuario
                Session.Abandon();
                FormsAuthentication.SignOut();
                
                // Redirigir a la página de inicio de sesión
                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            {
                // Registrar error
                System.Diagnostics.Debug.WriteLine($"Error al cerrar sesión: {ex.Message}");
                
                // Intentar redirigir de todos modos
                Response.Redirect("Login.aspx");
            }
        }
    }
} 