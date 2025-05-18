/**
 * Proyecto MiniTienda - Capa de Presentación
 * 
 * Lógica para la página de error genérica.
 * Muestra información sobre el error ocurrido de manera amigable al usuario.
 * 
 * Autor: Leonardo
 * Fecha: 15/05/2025            
 * Versión: 1.0 (Sprint 3)
 */

using System;
using System.Web;
using System.Web.UI;

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Clase que implementa la lógica para la página de error genérica.
    /// </summary>
    public partial class Error : System.Web.UI.Page
    {
        /// <summary>
        /// Método que se ejecuta cuando se carga la página.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Obtener el código de error desde la querystring (si existe)
                string errorCode = Request.QueryString["code"];
                
                if (!string.IsNullOrEmpty(errorCode))
                {
                    switch (errorCode)
                    {
                        case "404":
                            LblMensajeError.Text = "La página que está buscando no existe.";
                            LblCodigoError.Text = "Error 404 - Página no encontrada";
                            break;
                        case "500":
                            LblMensajeError.Text = "Ha ocurrido un error interno en el servidor.";
                            LblCodigoError.Text = "Error 500 - Error interno del servidor";
                            break;
                        default:
                            LblMensajeError.Text = "Ha ocurrido un error inesperado.";
                            LblCodigoError.Text = $"Error {errorCode}";
                            break;
                    }
                }
                
                // Obtener información adicional del error desde la sesión (si existe)
                if (Session["ErrorMessage"] != null)
                {
                    string errorMessage = Session["ErrorMessage"].ToString();
                    
                    // En ambiente de desarrollo, mostrar el detalle del error
                    if (HttpContext.Current.IsDebuggingEnabled)
                    {
                        LblDetalleError.Text = errorMessage;
                        LblDetalleError.Visible = true;
                    }
                    
                    // Limpiar el mensaje de error de la sesión
                    Session["ErrorMessage"] = null;
                }
                
                // Registrar el error en el log (en un entorno real, se usaría un sistema de logging)
                System.Diagnostics.Debug.WriteLine($"Error page accessed - Code: {errorCode ?? "unknown"} - Time: {DateTime.Now}");
            }
        }

        /// <summary>
        /// Método que se ejecuta cuando se hace clic en el botón "Volver al Inicio".
        /// </summary>
        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            // Si el usuario está autenticado, redirigir a la página principal
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("Index.aspx");
            }
            else
            {
                // Si no está autenticado, redirigir a la página de inicio de sesión
                Response.Redirect("Default.aspx");
            }
        }
    }
} 