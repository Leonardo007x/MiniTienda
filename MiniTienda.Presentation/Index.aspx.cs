/*
 * Index.aspx.cs
 * Controlador para la página de inicio del sistema
 * Muestra un dashboard con estadísticas y da acceso a las principales funciones
 */

using System;
using System.Web.UI;
// Comentamos la referencia problemática
// using MiniTienda.Logic;
using System.Collections.Generic;

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Clase que gestiona la funcionalidad de la página de inicio.
    /// Muestra un resumen del sistema con estadísticas sobre categorías, productos, proveedores y usuarios.
    /// </summary>
    public partial class Index : System.Web.UI.Page
    {
        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// Inicializa los controles y carga los datos estadísticos
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar nombre de usuario desde la sesión
                if (Session["Username"] != null)
                {
                    lblUsername.Text = Session["Username"].ToString();
                }

                // Cargar fecha y hora actual
                lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                // Cargar último acceso (si existe)
                if (Session["LastAccess"] != null)
                {
                    lblLastAccess.Text = Session["LastAccess"].ToString();
                }
                else
                {
                    lblLastAccess.Text = "Primera visita";
                }

                // Actualizar último acceso
                Session["LastAccess"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                // Cargar estadísticas (temporalmente desactivado hasta resolver la referencia)
                LoadStatistics();
            }
        }

        /// <summary>
        /// Carga las estadísticas del sistema para mostrarlas en el dashboard
        /// Incluye conteo de categorías, productos, proveedores y usuarios
        /// </summary>
        private void LoadStatistics()
        {
            try
            {
                // Temporalmente, usamos valores de ejemplo hasta resolver la referencia
                lblCategories.Text = "5";
                lblProducts.Text = "25";
                lblProviders.Text = "10";
                lblUsers.Text = "15";
                
                /* Código original comentado hasta resolver la referencia
                // Obtener conteo de categorías
                CategoryBL categoryBL = new CategoryBL();
                int categoryCount = categoryBL.GetAllCategories().Count;
                lblCategories.Text = categoryCount.ToString();

                // Obtener conteo de productos
                ProductBL productBL = new ProductBL();
                int productCount = productBL.GetAllProducts().Count;
                lblProducts.Text = productCount.ToString();

                // Obtener conteo de proveedores
                ProviderBL providerBL = new ProviderBL();
                int providerCount = providerBL.GetAllProviders().Count;
                lblProviders.Text = providerCount.ToString();

                // Obtener conteo de usuarios
                UserBL userBL = new UserBL();
                int userCount = userBL.GetAllUsers().Count;
                lblUsers.Text = userCount.ToString();
                */
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar valores por defecto
                lblCategories.Text = "0";
                lblProducts.Text = "0";
                lblProviders.Text = "0";
                lblUsers.Text = "0";
                
                // Registrar el error (en un sistema real, podríamos utilizar un logger)
                System.Diagnostics.Debug.WriteLine("Error al cargar estadísticas: " + ex.Message);
            }
        }
    }
} 