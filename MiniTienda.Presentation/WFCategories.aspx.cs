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
using MiniTienda.Logic;

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Clase que gestiona la funcionalidad de la página de categorías.
    /// Permite realizar operaciones CRUD sobre las categorías de productos.
    /// </summary>
    public partial class WFCategories : System.Web.UI.Page
    {
        // Objeto para acceder a la capa lógica de categorías
        private CategoryLog categoryLogic = new CategoryLog();

        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// Carga los datos desde la base de datos y configura la vista
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategoriesGrid();
            }
        }

        /// <summary>
        /// Enlazar los datos a la cuadrícula (GridView)
        /// Actualiza la vista con los datos de la base de datos
        /// </summary>
        private void BindCategoriesGrid()
        {
            try
            {
                // Obtener categorías desde la capa lógica
                DataTable categoriesTable = categoryLogic.showCategories();
                
                // Configurar tabla para el GridView
                DataTable gridTable = new DataTable();
                gridTable.Columns.Add("CategoryID", typeof(int));
                gridTable.Columns.Add("Name", typeof(string));
                
                // Transformar datos del formato de la BD al formato del GridView
                if (categoriesTable != null && categoriesTable.Rows.Count > 0)
                {
                    foreach (DataRow row in categoriesTable.Rows)
                    {
                        int id = Convert.ToInt32(row["id"]);
                        string description = row["description"].ToString();
                        
                        // Usamos description como Name
                        gridTable.Rows.Add(id, description);
                    }
                }
                
                // Asignar datos al GridView
                gvCategories.DataSource = gridTable;
                gvCategories.DataBind();
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al cargar categorías: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        /// <summary>
        /// Guardar una nueva categoría
        /// Se ejecuta cuando el usuario hace clic en el botón "Guardar"
        /// </summary>
        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCategoryName.Text))
                {
                    // Guardar categoría usando la capa lógica
                    int result = categoryLogic.saveCategory(
                        txtCategoryName.Text,
                        txtCategoryName.Text  // Usamos el mismo valor para descripción
                    );
                    
                    if (result > 0)
                    {
                        // Limpiar los controles
                        txtCategoryName.Text = string.Empty;
                        
                        // Actualizar la vista
                        BindCategoriesGrid();
                        
                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                            "alert('Categoría guardada exitosamente.');", true);
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                            "alert('No se pudo guardar la categoría. Por favor intente nuevamente.');", true);
                    }
                }
                else
                {
                    // Mostrar mensaje de error si el nombre está vacío
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                        "alert('El nombre de la categoría es obligatorio.');", true);
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al guardar la categoría: {ex.Message.Replace("'", "\\'")}');", true);
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
            try
            {
                // Obtener el ID de la categoría a eliminar
                int categoryId = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
                
                // Eliminar categoría usando la capa lógica
                int result = categoryLogic.deleteCategory(categoryId);
                
                if (result > 0)
                {
                    // Actualizar la vista
                    BindCategoriesGrid();
                    
                    // Mostrar mensaje de éxito
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                        "alert('Categoría eliminada exitosamente.');", true);
                }
                else
                {
                    // Mostrar mensaje de error
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                        "alert('No se pudo eliminar la categoría. Es posible que tenga productos asociados.');", true);
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al eliminar la categoría: {ex.Message.Replace("'", "\\'")}');", true);
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
            try
            {
                // Obtener el ID de la categoría a actualizar
                int categoryId = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
                
                // Obtener los nuevos valores
                TextBox txtName = (TextBox)gvCategories.Rows[e.RowIndex].FindControl("txtName");
                
                if (txtName != null && !string.IsNullOrEmpty(txtName.Text))
                {
                    // Actualizar categoría usando la capa lógica
                    int result = categoryLogic.updateCategory(
                        categoryId,
                        txtName.Text,
                        txtName.Text  // Usamos el mismo valor para nombre y descripción
                    );
                    
                    if (result > 0)
                    {
                        // Salir del modo de edición
                        gvCategories.EditIndex = -1;
                        
                        // Actualizar la vista
                        BindCategoriesGrid();
                        
                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                            "alert('Categoría actualizada exitosamente.');", true);
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                            "alert('No se pudo actualizar la categoría. Por favor intente nuevamente.');", true);
                    }
                }
                else
                {
                    // Mostrar mensaje de error si el nombre está vacío
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                        "alert('El nombre de la categoría es obligatorio.');", true);
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al actualizar la categoría: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }
    }
} 