/*
 * Main.Master.cs
 * Controlador para la página maestra del sistema
 * Gestiona la navegación, permisos por rol y controla el acceso a las diferentes secciones
 */

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Clase que gestiona la funcionalidad de la página maestra.
    /// Controla la visibilidad de los menús según el rol del usuario y verifica permisos de acceso.
    /// </summary>
    public partial class Main : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Método que se ejecuta cuando se carga la página maestra
        /// Verifica si el usuario ha iniciado sesión y configura la interfaz según su rol
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar si el usuario ha iniciado sesión
            if (Session["UserID"] == null)
            {
                // Si no ha iniciado sesión y no está en la página de login, redirigir a login
                if (!HttpContext.Current.Request.Url.AbsolutePath.EndsWith("Login.aspx"))
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
            else
            {
                // Mostrar información del usuario actual
                if (Session["Username"] != null)
                {
                    lblCurrentUser.Text = Session["Username"].ToString();
                }

                if (Session["Role"] != null)
                {
                    lblCurrentRole.Text = Session["Role"].ToString();
                    // Asegurar compatibilidad con WFUsers.aspx.cs
                    Session["UserRole"] = Session["Role"];
                }

                // Obtenemos el rol del usuario para gestionar los permisos
                string userRole = Session["Role"]?.ToString().ToLower() ?? "";

                // Configurar visibilidad de menús según el rol
                ConfigureMenusByRole(userRole);

                // Haciendo visible todos los menús para permitir acceso a todas las páginas
                menuCategorias.Visible = true;
                menuProveedores.Visible = true;
                menuProductos.Visible = true;
                menuUsuarios.Visible = true;

                // Obtenemos la página actual
                string currentPage = HttpContext.Current.Request.Url.AbsolutePath.ToLower();

                // Implementar restricciones de acceso basadas en roles - Desactivado temporalmente
                bool hasAccess = true;

                // Control de acceso por roles - Desactivado temporalmente
                /*
                if (currentPage.Contains("wfusers.aspx") && 
                    !(userRole == "admin" || userRole == "administrador"))
                {
                    // Solo los administradores pueden gestionar usuarios
                    hasAccess = false;
                }
                else if ((currentPage.Contains("wfcategories.aspx") || 
                          currentPage.Contains("wfproviders.aspx")) && 
                         !(userRole == "admin" || userRole == "administrador" || 
                           userRole == "almacén" || userRole == "almacen"))
                {
                    // Administradores y personal de almacén pueden gestionar categorías y proveedores
                    hasAccess = false;
                }
                */

                // Si el usuario no tiene acceso a la página actual, redirigir a la página de inicio
                if (!hasAccess)
                {
                    Response.Redirect("~/Index.aspx");
                }
            }
        }

        /// <summary>
        /// Configura la visibilidad de los elementos del menú según el rol del usuario
        /// Cada rol tiene permisos específicos para acceder a distintas secciones del sistema
        /// </summary>
        /// <param name="role">Rol del usuario actual</param>
        private void ConfigureMenusByRole(string role)
        {
            // Todos los usuarios tienen acceso a la página de inicio
            menuInicio.Visible = true;
            
            // Configurar visibilidad de los menús según el rol
            if (role == "admin" || role == "administrador")
            {
                // Los administradores tienen acceso a todos los menús
                menuCategorias.Visible = true;
                menuProveedores.Visible = true;
                menuProductos.Visible = true;
                menuUsuarios.Visible = true;
            }
            else if (role == "almacén" || role == "almacen")
            {
                // El personal de almacén puede gestionar categorías, proveedores y productos
                menuCategorias.Visible = true;
                menuProveedores.Visible = true;
                menuProductos.Visible = true;
                menuUsuarios.Visible = false;
            }
            else if (role == "vendedor")
            {
                // Los vendedores solo pueden ver productos
                menuCategorias.Visible = false;
                menuProveedores.Visible = false;
                menuProductos.Visible = true;
                menuUsuarios.Visible = false;
            }
            else
            {
                // Para cualquier otro rol, restringir acceso a menús administrativos
                menuCategorias.Visible = false;
                menuProveedores.Visible = false;
                menuProductos.Visible = false;
                menuUsuarios.Visible = false;
            }
        }

        /// <summary>
        /// Método que se ejecuta cuando el usuario cierra sesión
        /// Limpia la información de sesión y redirecciona a la página de login
        /// </summary>
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Limpiar sesión
            Session.Clear();
            Session.Abandon();

            // Redirigir a la página de login
            Response.Redirect("~/Login.aspx");
        }
    }
} 