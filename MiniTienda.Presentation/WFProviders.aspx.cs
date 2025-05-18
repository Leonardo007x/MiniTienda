/*
 * WFProviders.aspx.cs
 * Controlador para la página de gestión de proveedores
 * Implementa la funcionalidad para crear, editar y eliminar proveedores
 */
//test brayan cruz
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
    /// Clase que gestiona la funcionalidad de la página de proveedores.
    /// Permite realizar operaciones CRUD sobre los proveedores.
    /// </summary>
    public partial class WFProviders : System.Web.UI.Page
    {
        // Objeto para acceder a la capa lógica de proveedores
        private MiniTienda.Logic.ProvidersLog providersLogic;

        // Constructor
        public WFProviders()
        {
            providersLogic = new MiniTienda.Logic.ProvidersLog();
        }

        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// Carga los datos de proveedores desde la base de datos
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProvidersGrid();
            }
        }

        /// <summary>
        /// Enlazar los datos a la cuadrícula (GridView)
        /// Carga los datos desde la capa lógica
        /// </summary>
        private void BindProvidersGrid()
        {
            try
            {
                // Obtener proveedores desde la capa lógica
                DataSet ds = providersLogic.ShowProviders();
                
                // Configurar tabla para el GridView
                DataTable providersTable = new DataTable();
                providersTable.Columns.Add("ProviderID", typeof(int));
                providersTable.Columns.Add("Name", typeof(string));
                providersTable.Columns.Add("Description", typeof(string));
                
                // Transformar datos del formato de la BD al formato del GridView
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        int id = Convert.ToInt32(row["prov_id"]);
                        string name = row["prov_nombre"].ToString();
                        string nit = row["prov_nit"].ToString();
                        
                        providersTable.Rows.Add(id, name, nit);
                    }
                }
                
                // Asignar datos al GridView
                gvProviders.DataSource = providersTable;
                gvProviders.DataKeyNames = new string[] { "ProviderID" };
                gvProviders.DataBind();
            }
            catch (Exception ex)
            {
                // Manejar errores
                ShowErrorMessage("Error al cargar proveedores: " + ex.Message);
            }
        }

        /// <summary>
        /// Guardar un nuevo proveedor
        /// Se ejecuta cuando el usuario hace clic en el botón "Guardar"
        /// </summary>
        protected void btnSaveProvider_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtProviderName.Text))
                {
                    // Guardar proveedor usando la capa lógica
                    bool result = providersLogic.SaveProvider(
                        txtProviderDescription.Text, // NIT (guardado en el campo descripción)
                        txtProviderName.Text         // Nombre
                    );
                    
                    if (result)
                    {
                        // Limpiar campos después de guardar
                        txtProviderName.Text = string.Empty;
                        txtProviderDescription.Text = string.Empty;
                        
                        // Actualizar la vista
                        BindProvidersGrid();
                        
                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                            "alert('Proveedor guardado exitosamente.');", true);
                    }
                }
                else
                {
                    ShowErrorMessage("El nombre del proveedor es obligatorio.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error al guardar el proveedor: " + ex.Message);
            }
        }

        /// <summary>
        /// Gestionar comandos personalizados en el GridView
        /// </summary>
        protected void gvProviders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Implementar si se necesitan comandos personalizados
        }

        /// <summary>
        /// Eliminar un proveedor
        /// Se ejecuta cuando el usuario hace clic en "Eliminar" en una fila
        /// </summary>
        protected void gvProviders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Obtener el ID del proveedor a eliminar
                int providerId = Convert.ToInt32(gvProviders.DataKeys[e.RowIndex].Value);
                
                // Mostrar mensaje de confirmación en consola para depuración
                System.Diagnostics.Debug.WriteLine($"Intentando eliminar proveedor con ID: {providerId}");
                
                // Eliminar proveedor usando la capa lógica
                bool result = providersLogic.DeleteProvider(providerId);
                
                if (result)
                {
                    // Actualizar la vista
                    BindProvidersGrid();
                    
                    // Mostrar mensaje de éxito
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                        "alert('Proveedor eliminado exitosamente.');", true);
                }
                else
                {
                    // Si no se pudo eliminar, mostrar mensaje de error
                    ShowErrorMessage("No se pudo eliminar el proveedor. Puede estar siendo utilizado por otros registros.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error al eliminar el proveedor: " + ex.Message);
            }
        }

        /// <summary>
        /// Iniciar la edición de un proveedor
        /// Se ejecuta cuando el usuario hace clic en "Editar" en una fila
        /// </summary>
        protected void gvProviders_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProviders.EditIndex = e.NewEditIndex;
            BindProvidersGrid();
        }

        /// <summary>
        /// Cancelar la edición de un proveedor
        /// Se ejecuta cuando el usuario hace clic en "Cancelar" durante una edición
        /// </summary>
        protected void gvProviders_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProviders.EditIndex = -1;
            BindProvidersGrid();
        }

        /// <summary>
        /// Actualizar un proveedor existente
        /// Se ejecuta cuando el usuario hace clic en "Actualizar" después de editar una fila
        /// </summary>
        protected void gvProviders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // Obtener el ID del proveedor a actualizar
                int providerId = Convert.ToInt32(gvProviders.DataKeys[e.RowIndex].Value);
                
                // Obtener los nuevos valores de los controles en la fila editada
                TextBox txtName = (TextBox)gvProviders.Rows[e.RowIndex].FindControl("txtName");
                TextBox txtDescription = (TextBox)gvProviders.Rows[e.RowIndex].FindControl("txtDescription");
                
                if (txtName != null && !string.IsNullOrEmpty(txtName.Text))
                {
                    // Actualizar proveedor usando la capa lógica
                    bool result = providersLogic.UpdateProvider(
                        providerId,
                        txtDescription.Text, // NIT (guardado en el campo descripción)
                        txtName.Text         // Nombre
                    );
                    
                    if (result)
                    {
                        // Salir del modo de edición
                        gvProviders.EditIndex = -1;
                        
                        // Actualizar la vista
                        BindProvidersGrid();
                        
                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                            "alert('Proveedor actualizado exitosamente.');", true);
                    }
                }
                else
                {
                    ShowErrorMessage("El nombre del proveedor es obligatorio.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error al actualizar el proveedor: " + ex.Message);
            }
        }
        
        /// <summary>
        /// Método auxiliar para mostrar mensajes de error
        /// </summary>
        private void ShowErrorMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                $"alert('{message.Replace("'", "\\'")}');", true);
        }
    }
}
