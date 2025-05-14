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
using MiniTienda.Logic;
using System.Data;

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
        /// Carga las estadísticas del sistema desde la base de datos
        /// Muestra el conteo de categorías, productos, proveedores y usuarios
        /// </summary>
        private void LoadStatistics()
        {
            try
            {
                // Aquí iría la lógica para cargar las estadísticas desde la base de datos
                // Por ahora, usamos valores de ejemplo
                lblCategories.Text = "0";
                lblProducts.Text = "0";
                lblProviders.Text = "0";
                lblUsers.Text = "0";
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