/*
 * WFCategories.aspx.cs
 * Controlador para la página de gestión de categorías
 * Implementa la funcionalidad para crear, editar y eliminar categorías de productos
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Clase que gestiona la funcionalidad de la página de categorías.
    /// Permite realizar operaciones CRUD sobre las categorías de productos.
    /// </summary>
    public partial class WFCategories : System.Web.UI.Page
    {
        // Datos de muestra para mostrar en la vista
        private DataTable categoriesTable;

        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// Inicializa los datos de muestra y configura la vista
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateSampleData();
                BindCategoriesGrid();
            }
        }

        /// <summary>
        /// Método para crear datos de muestra para la demostración
        /// En un sistema real, estos datos vendrían de la base de datos
        /// </summary>
        private void CreateSampleData()
        {
            categoriesTable = new DataTable();
            categoriesTable.Columns.Add("CategoryID", typeof(int));
            categoriesTable.Columns.Add("Name", typeof(string));
            categoriesTable.Columns.Add("Description", typeof(string));

            // Agregar algunas categorías de muestra
            categoriesTable.Rows.Add(1, "Electrónicos", "Dispositivos electrónicos y accesorios");
            categoriesTable.Rows.Add(2, "Ropa", "Ropa para hombres, mujeres y niños");
            categoriesTable.Rows.Add(3, "Hogar", "Artículos para el hogar y decoración");
            categoriesTable.Rows.Add(4, "Alimentos", "Productos alimenticios");
            categoriesTable.Rows.Add(5, "Juguetes", "Juguetes para niños y adultos");

            // Guardar la tabla en sesión
            Session["Categories"] = categoriesTable;
        }

        /// <summary>
        /// Enlazar los datos a la cuadrícula (GridView)
        /// Actualiza la vista con los datos más recientes
        /// </summary>
        private void BindCategoriesGrid()
        {
            if (Session["Categories"] != null)
            {
                gvCategories.DataSource = Session["Categories"];
                gvCategories.DataBind();
            }
        }

        /// <summary>
        /// Guardar una nueva categoría
        /// Se ejecuta cuando el usuario hace clic en el botón "Guardar"
        /// </summary>
        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCategoryName.Text))
            {
                DataTable dt = (DataTable)Session["Categories"];
                
                // Determinar el nuevo ID (en un sistema real se generaría por la base de datos)
                int newId = 1;
                if (dt.Rows.Count > 0)
                {
                    newId = dt.AsEnumerable().Max(row => row.Field<int>("CategoryID")) + 1;
                }
                
                // Agregar la nueva categoría
                dt.Rows.Add(newId, txtCategoryName.Text, txtCategoryDescription.Text);
                
                // Actualizar la sesión
                Session["Categories"] = dt;
                
                // Actualizar la vista
                BindCategoriesGrid();
                
                // Limpiar los controles
                txtCategoryName.Text = string.Empty;
                txtCategoryDescription.Text = string.Empty;
            }
        }

        /// <summary>
        /// Manejar comandos de fila del GridView
        /// Permite responder a eventos personalizados en las filas
        /// </summary>
        protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Implementar si se necesitan comandos personalizados adicionales
        }

        /// <summary>
        /// Eliminar una categoría
        /// Se ejecuta cuando el usuario hace clic en "Eliminar" en una fila
        /// </summary>
        protected void gvCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)Session["Categories"];
            
            // Obtener el ID de la categoría a eliminar
            int categoryId = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
            
            // Encontrar la fila a eliminar
            DataRow[] rows = dt.Select("CategoryID = " + categoryId);
            if (rows.Length > 0)
            {
                // Eliminar la fila
                rows[0].Delete();
                dt.AcceptChanges();
                
                // Actualizar la sesión
                Session["Categories"] = dt;
                
                // Actualizar la vista
                BindCategoriesGrid();
            }
        }

        /// <summary>
        /// Iniciar la edición de una categoría
        /// Se ejecuta cuando el usuario hace clic en "Editar" en una fila
        /// </summary>
        protected void gvCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCategories.EditIndex = e.NewEditIndex;
            BindCategoriesGrid();
        }

        /// <summary>
        /// Cancelar la edición de una categoría
        /// Se ejecuta cuando el usuario hace clic en "Cancelar" durante una edición
        /// </summary>
        protected void gvCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCategories.EditIndex = -1;
            BindCategoriesGrid();
        }

        /// <summary>
        /// Actualizar una categoría existente
        /// Se ejecuta cuando el usuario hace clic en "Actualizar" después de editar una fila
        /// </summary>
        protected void gvCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session["Categories"];
            
            // Obtener el ID de la categoría a actualizar
            int categoryId = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
            
            // Obtener los nuevos valores
            TextBox txtName = (TextBox)gvCategories.Rows[e.RowIndex].FindControl("txtName");
            TextBox txtDescription = (TextBox)gvCategories.Rows[e.RowIndex].FindControl("txtDescription");
            
            // Encontrar la fila a actualizar
            DataRow[] rows = dt.Select("CategoryID = " + categoryId);
            if (rows.Length > 0)
            {
                // Actualizar los valores
                rows[0]["Name"] = txtName.Text;
                rows[0]["Description"] = txtDescription.Text;
                dt.AcceptChanges();
                
                // Actualizar la sesión
                Session["Categories"] = dt;
                
                // Salir del modo de edición
                gvCategories.EditIndex = -1;
                
                // Actualizar la vista
                BindCategoriesGrid();
            }
        }
    }
} 