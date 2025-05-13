/*
 * WFProducts.aspx.cs
 * Controlador para la página de gestión de productos
 * Implementa la funcionalidad para crear, editar y eliminar productos
 * Desarrollado por: Elkin
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
    /// Clase que gestiona la funcionalidad de la página de productos.
    /// Permite realizar operaciones CRUD sobre los productos.
    /// </summary>
    public partial class WFProducts : System.Web.UI.Page
    {
        // Datos de muestra para mostrar en la vista
        private DataTable productsTable;
        private DataTable categoriesTable;
        private DataTable providersTable;

        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// Inicializa los datos de muestra y configura la vista
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateSampleData();
                LoadCategoriesDropDown();
                LoadProvidersDropDown();
                BindProductsGrid();
            }
        }

        /// <summary>
        /// Método para crear datos de muestra para la demostración
        /// En un sistema real, estos datos vendrían de la base de datos
        /// </summary>
        private void CreateSampleData()
        {
            // Tabla de categorías (podría venir de la sesión si WFCategories ya la creó)
            if (Session["Categories"] == null)
            {
                categoriesTable = new DataTable();
                categoriesTable.Columns.Add("CategoryID", typeof(int));
                categoriesTable.Columns.Add("Name", typeof(string));
                categoriesTable.Columns.Add("Description", typeof(string));

                // Agregar algunas categorías de muestra
                categoriesTable.Rows.Add(1, "Electrónicos", "Dispositivos electrónicos y accesorios");
                categoriesTable.Rows.Add(2, "Ropa", "Ropa para hombres, mujeres y niños");
                categoriesTable.Rows.Add(3, "Hogar", "Artículos para el hogar y decoración");

                Session["Categories"] = categoriesTable;
            }
            else
            {
                categoriesTable = (DataTable)Session["Categories"];
            }

            // Tabla de proveedores (podría venir de la sesión si WFProviders ya la creó)
            if (Session["Providers"] == null)
            {
                providersTable = new DataTable();
                providersTable.Columns.Add("ProviderID", typeof(int));
                providersTable.Columns.Add("Name", typeof(string));
                providersTable.Columns.Add("Description", typeof(string));

                // Agregar algunos proveedores de muestra
                providersTable.Rows.Add(1, "TechSupplies", "12345678-9");
                providersTable.Rows.Add(2, "ModaTotal", "98765432-1");
                providersTable.Rows.Add(3, "HomeDecor", "56789012-3");

                Session["Providers"] = providersTable;
            }
            else
            {
                providersTable = (DataTable)Session["Providers"];
            }

            // Tabla de productos
            productsTable = new DataTable();
            productsTable.Columns.Add("ProductID", typeof(int));
            productsTable.Columns.Add("Name", typeof(string));
            productsTable.Columns.Add("Price", typeof(decimal));
            productsTable.Columns.Add("CategoryID", typeof(int));
            productsTable.Columns.Add("CategoryName", typeof(string));
            productsTable.Columns.Add("ProviderID", typeof(int));
            productsTable.Columns.Add("ProviderName", typeof(string));

            // Agregar algunos productos de muestra
            productsTable.Rows.Add(1, "Smartphone XYZ", 1299.99, 1, "Electrónicos", 1, "TechSupplies");
            productsTable.Rows.Add(2, "Camisa Premium", 59.95, 2, "Ropa", 2, "ModaTotal");
            productsTable.Rows.Add(3, "Lámpara LED", 129.50, 3, "Hogar", 3, "HomeDecor");
            productsTable.Rows.Add(4, "Tablet Ultra", 899.99, 1, "Electrónicos", 1, "TechSupplies");
            productsTable.Rows.Add(5, "Vaso Decorativo", 25.75, 3, "Hogar", 3, "HomeDecor");

            // Guardar la tabla en sesión
            Session["Products"] = productsTable;
        }

        /// <summary>
        /// Cargar el DropDownList de categorías con los datos de la tabla de categorías
        /// </summary>
        private void LoadCategoriesDropDown()
        {
            ddlCategory.DataSource = categoriesTable;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "CategoryID";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("-- Seleccione una categoría --", "0"));
        }

        /// <summary>
        /// Cargar el DropDownList de proveedores con los datos de la tabla de proveedores
        /// </summary>
        private void LoadProvidersDropDown()
        {
            ddlProvider.DataSource = providersTable;
            ddlProvider.DataTextField = "Name";
            ddlProvider.DataValueField = "ProviderID";
            ddlProvider.DataBind();
            ddlProvider.Items.Insert(0, new ListItem("-- Seleccione un proveedor --", "0"));
        }

        /// <summary>
        /// Enlazar los datos a la cuadrícula (GridView)
        /// Actualiza la vista con los datos más recientes
        /// </summary>
        private void BindProductsGrid()
        {
            if (Session["Products"] != null)
            {
                gvProducts.DataSource = Session["Products"];
                gvProducts.DataBind();
            }
        }

        /// <summary>
        /// Guardar un nuevo producto
        /// Se ejecuta cuando el usuario hace clic en el botón "Guardar"
        /// </summary>
        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProductName.Text) && 
                !string.IsNullOrEmpty(txtProductPrice.Text) && 
                ddlCategory.SelectedIndex > 0 && 
                ddlProvider.SelectedIndex > 0)
            {
                DataTable dt = (DataTable)Session["Products"];
                
                // Determinar el nuevo ID (en un sistema real se generaría por la base de datos)
                int newId = 1;
                if (dt.Rows.Count > 0)
                {
                    newId = dt.AsEnumerable().Max(row => row.Field<int>("ProductID")) + 1;
                }
                
                // Obtener información de la categoría seleccionada
                int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                string categoryName = ddlCategory.SelectedItem.Text;
                
                // Obtener información del proveedor seleccionado
                int providerId = Convert.ToInt32(ddlProvider.SelectedValue);
                string providerName = ddlProvider.SelectedItem.Text;
                
                // Agregar el nuevo producto
                dt.Rows.Add(
                    newId, 
                    txtProductName.Text, 
                    Convert.ToDecimal(txtProductPrice.Text), 
                    categoryId, 
                    categoryName, 
                    providerId, 
                    providerName
                );
                
                // Actualizar la sesión
                Session["Products"] = dt;
                
                // Actualizar la vista
                BindProductsGrid();
                
                // Limpiar los controles
                txtProductName.Text = string.Empty;
                txtProductPrice.Text = string.Empty;
                ddlCategory.SelectedIndex = 0;
                ddlProvider.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Manejar comandos de fila del GridView
        /// Permite responder a eventos personalizados en las filas
        /// </summary>
        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Implementar si se necesitan comandos personalizados adicionales
        }

        /// <summary>
        /// Eliminar un producto
        /// Se ejecuta cuando el usuario hace clic en "Eliminar" en una fila
        /// </summary>
        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)Session["Products"];
            
            // Obtener el ID del producto a eliminar
            int productId = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
            
            // Encontrar la fila a eliminar
            DataRow[] rows = dt.Select("ProductID = " + productId);
            if (rows.Length > 0)
            {
                // Eliminar la fila
                rows[0].Delete();
                dt.AcceptChanges();
                
                // Actualizar la sesión
                Session["Products"] = dt;
                
                // Actualizar la vista
                BindProductsGrid();
            }
        }

        /// <summary>
        /// Iniciar la edición de un producto
        /// Se ejecuta cuando el usuario hace clic en "Editar" en una fila
        /// </summary>
        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            BindProductsGrid();
            
            // Cargar los dropdowns de categorías y proveedores en modo edición
            DropDownList ddlCategoryEdit = (DropDownList)gvProducts.Rows[e.NewEditIndex].FindControl("ddlCategoryEdit");
            DropDownList ddlProviderEdit = (DropDownList)gvProducts.Rows[e.NewEditIndex].FindControl("ddlProviderEdit");
            
            if (ddlCategoryEdit != null)
            {
                ddlCategoryEdit.DataSource = categoriesTable;
                ddlCategoryEdit.DataTextField = "Name";
                ddlCategoryEdit.DataValueField = "CategoryID";
                ddlCategoryEdit.DataBind();
                
                // Seleccionar la categoría actual del producto
                DataTable dt = (DataTable)Session["Products"];
                int productId = Convert.ToInt32(gvProducts.DataKeys[e.NewEditIndex].Value);
                DataRow[] rows = dt.Select("ProductID = " + productId);
                if (rows.Length > 0)
                {
                    int categoryId = Convert.ToInt32(rows[0]["CategoryID"]);
                    ListItem item = ddlCategoryEdit.Items.FindByValue(categoryId.ToString());
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
            
            if (ddlProviderEdit != null)
            {
                ddlProviderEdit.DataSource = providersTable;
                ddlProviderEdit.DataTextField = "Name";
                ddlProviderEdit.DataValueField = "ProviderID";
                ddlProviderEdit.DataBind();
                
                // Seleccionar el proveedor actual del producto
                DataTable dt = (DataTable)Session["Products"];
                int productId = Convert.ToInt32(gvProducts.DataKeys[e.NewEditIndex].Value);
                DataRow[] rows = dt.Select("ProductID = " + productId);
                if (rows.Length > 0)
                {
                    int providerId = Convert.ToInt32(rows[0]["ProviderID"]);
                    ListItem item = ddlProviderEdit.Items.FindByValue(providerId.ToString());
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Cancelar la edición de un producto
        /// Se ejecuta cuando el usuario hace clic en "Cancelar" durante una edición
        /// </summary>
        protected void gvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1;
            BindProductsGrid();
        }

        /// <summary>
        /// Actualizar un producto existente
        /// Se ejecuta cuando el usuario hace clic en "Actualizar" después de editar una fila
        /// </summary>
        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session["Products"];
            
            // Obtener el ID del producto a actualizar
            int productId = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
            
            // Obtener los nuevos valores
            TextBox txtName = (TextBox)gvProducts.Rows[e.RowIndex].FindControl("txtName");
            TextBox txtPrice = (TextBox)gvProducts.Rows[e.RowIndex].FindControl("txtPrice");
            DropDownList ddlCategoryEdit = (DropDownList)gvProducts.Rows[e.RowIndex].FindControl("ddlCategoryEdit");
            DropDownList ddlProviderEdit = (DropDownList)gvProducts.Rows[e.RowIndex].FindControl("ddlProviderEdit");
            
            // Encontrar la fila a actualizar
            DataRow[] rows = dt.Select("ProductID = " + productId);
            if (rows.Length > 0)
            {
                // Actualizar los valores
                rows[0]["Name"] = txtName.Text;
                rows[0]["Price"] = Convert.ToDecimal(txtPrice.Text);
                
                // Actualizar categoría
                int categoryId = Convert.ToInt32(ddlCategoryEdit.SelectedValue);
                string categoryName = ddlCategoryEdit.SelectedItem.Text;
                rows[0]["CategoryID"] = categoryId;
                rows[0]["CategoryName"] = categoryName;
                
                // Actualizar proveedor
                int providerId = Convert.ToInt32(ddlProviderEdit.SelectedValue);
                string providerName = ddlProviderEdit.SelectedItem.Text;
                rows[0]["ProviderID"] = providerId;
                rows[0]["ProviderName"] = providerName;
                
                dt.AcceptChanges();
                
                // Actualizar la sesión
                Session["Products"] = dt;
                
                // Salir del modo de edición
                gvProducts.EditIndex = -1;
                
                // Actualizar la vista
                BindProductsGrid();
            }
        }
    }
} 