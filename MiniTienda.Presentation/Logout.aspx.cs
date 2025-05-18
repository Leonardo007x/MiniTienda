/**
 * Proyecto MiniTienda - Capa de Presentación
 * 
 * Lógica para la página de cierre de sesión.
 * Implementa la funcionalidad para cerrar sesión y redirigir al usuario.
 * 
 * Autor: Elkin
 * Fecha: 12/05/2025
 */

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Clase que implementa la lógica para cerrar sesión.
    /// </summary>
    public partial class Logout : System.Web.UI.Page
    {
        /// <summary>
        /// Método que se ejecuta cuando se carga la página.
        /// Cierra la sesión del usuario y lo redirige a la página de inicio de sesión.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Registrar el cierre de sesión para auditoría (en un entorno real, esto iría a un log o base de datos)
                System.Diagnostics.Debug.WriteLine($"Usuario {User.Identity.Name} cerró sesión el {DateTime.Now}");
                
                // Limpiar variables de sesión
                Session.Clear();
                
                // Abandonar la sesión actual
                Session.Abandon();
                
                // Eliminar la cookie de autenticación
                FormsAuthentication.SignOut();
                
                // Agregar un pequeño retraso para simular el proceso de cierre de sesión
                // y para permitir que se muestre la animación en la página
                System.Threading.Thread.Sleep(1000);
                
                // Redirigir al usuario a la página de inicio de sesión
                Response.Redirect("Login.aspx", true);
            }
            catch (Exception ex)
            {
                // Registrar cualquier error que pueda ocurrir
                System.Diagnostics.Debug.WriteLine($"Error en el cierre de sesión: {ex.Message}");
                
                // Aún con error, intentar redirigir al usuario a la página de inicio de sesión
                Response.Redirect("Login.aspx", true);
            }
        }
    }
} 