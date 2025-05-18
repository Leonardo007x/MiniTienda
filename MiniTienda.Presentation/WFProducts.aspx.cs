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
        // Objeto para acceder a la capa lógica de productos
        private MiniTienda.Logic.ProductsLog productsLogic;
        // Objeto para acceder a la capa lógica de categorías
        private MiniTienda.Logic.CategoryLog categoryLogic;
        // Objeto para acceder a la capa lógica de proveedores
        private MiniTienda.Logic.ProvidersLog providersLogic;

        // Constructor
        public WFProducts()
        {
            productsLogic = new MiniTienda.Logic.ProductsLog();
            categoryLogic = new MiniTienda.Logic.CategoryLog();
            providersLogic = new MiniTienda.Logic.ProvidersLog();
        }

        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// Carga los datos desde la base de datos y configura la vista
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategoriesDropDown();
                LoadProvidersDropDown();
                BindProductsGrid();
            }
        }

        /// <summary>
        /// Cargar el DropDownList de categorías con los datos de la base de datos
        /// </summary>
        private void LoadCategoriesDropDown()
        {
            try
            {
                DataTable categoriesTable = categoryLogic.showCategories();
                
                // Configurar el DropDownList
                ddlCategory.DataSource = categoriesTable;
                ddlCategory.DataTextField = "description";
                ddlCategory.DataValueField = "id";
                ddlCategory.DataBind();

                // Agregar la opción por defecto
                ddlCategory.Items.Insert(0, new ListItem("-- Seleccione una categoría --", "0"));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al cargar categorías: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        /// <summary>
        /// Cargar el DropDownList de proveedores con los datos de la base de datos
        /// </summary>
        private void LoadProvidersDropDown()
        {
            try
            {
                // Obtener proveedores desde la capa lógica
                DataSet ds = providersLogic.ShowProviders();
                
                // Configurar tabla para el DropDownList
                DataTable providersTable = new DataTable();
                providersTable.Columns.Add("ProviderID", typeof(int));
                providersTable.Columns.Add("Name", typeof(string));
                
                // Transformar datos del formato de la BD al formato del DropDownList
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        int id = Convert.ToInt32(row["prov_id"]);
                        string name = row["prov_nombre"].ToString();
                        
                        providersTable.Rows.Add(id, name);
                    }
                }
                
                // Configurar el DropDownList
                ddlProvider.DataSource = providersTable;
                ddlProvider.DataTextField = "Name";
                ddlProvider.DataValueField = "ProviderID";
                ddlProvider.DataBind();
                
                // Agregar la opción por defecto
                ddlProvider.Items.Insert(0, new ListItem("-- Seleccione un proveedor --", "0"));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al cargar proveedores: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        /// <summary>
        /// Enlazar los datos a la cuadrícula (GridView)
        /// Actualiza la vista con los datos de la base de datos
        /// </summary>
        private void BindProductsGrid()
        {
            try
            {
                // Registrar información de diagnóstico
                System.Diagnostics.Debug.WriteLine("Iniciando carga de productos desde la base de datos");
                
                // Obtener productos desde la capa lógica
                DataSet ds = productsLogic.GetProducts();
                System.Diagnostics.Debug.WriteLine($"DataSet recibido: {(ds != null ? "No es nulo" : "Es nulo")}");
                if (ds != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Cantidad de tablas: {ds.Tables.Count}");
                    if (ds.Tables.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Cantidad de filas en tabla 0: {ds.Tables[0].Rows.Count}");
                        
                        // Usar un enfoque alternativo para listar las columnas
                        List<string> columnNames = new List<string>();
                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {
                            columnNames.Add(col.ColumnName);
                        }
                        System.Diagnostics.Debug.WriteLine($"Columnas en tabla 0: {string.Join(", ", columnNames)}");
                    }
            }

                // Crear una tabla con estructura idéntica a la base de datos para diagnóstico
                DataTable rawTable = new DataTable();
                if (ds != null && ds.Tables.Count > 0)
                {
                    rawTable = ds.Tables[0].Copy();
                    System.Diagnostics.Debug.WriteLine($"Tabla copiada con {rawTable.Rows.Count} filas");
                }
                
                // Configurar tabla para el GridView
                DataTable productsTable = new DataTable();
            productsTable.Columns.Add("ProductID", typeof(int));
            productsTable.Columns.Add("Name", typeof(string));
            productsTable.Columns.Add("Price", typeof(decimal));
            productsTable.Columns.Add("Stock", typeof(int));
            productsTable.Columns.Add("CategoryID", typeof(int));
            productsTable.Columns.Add("CategoryName", typeof(string));
            productsTable.Columns.Add("ProviderID", typeof(int));
            productsTable.Columns.Add("ProviderName", typeof(string));

                // Transformar datos del formato de la BD al formato del GridView
                // Según la captura: pro_id, pro_codigo, pro_descripcion, pro_cantidad, pro_precio, tbl_categorias_cat_id, tbl_proveedores_prov_id
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        try
                        {
                            // Registrar toda la fila para diagnóstico
                            System.Diagnostics.Debug.WriteLine("Contenido completo de la fila del producto:");
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                System.Diagnostics.Debug.WriteLine($"   {col.ColumnName}: {row[col]}");
                            }
                            
                            // Variables para almacenar los datos del producto
                            int id = 0;
                            string name = "Sin nombre";
                            decimal price = 0;
                            int categoryId = 0;
                            string categoryName = "Sin categoría";
                            int providerId = 0;
                            string providerName = "Sin proveedor";
                            int stock = 0;
                            
                            // Intentar obtener el ID con diferentes nombres de columna posibles
                            if (row.Table.Columns.Contains("pro_id"))
                            {
                                id = row["pro_id"] != DBNull.Value ? Convert.ToInt32(row["pro_id"]) : 0;
                                System.Diagnostics.Debug.WriteLine($"ID obtenido de pro_id: {id}");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("La columna pro_id no existe en el resultado");
                                
                                // Buscar cualquier columna que pueda contener un ID
                                foreach (DataColumn col in row.Table.Columns)
                                {
                                    if (col.ColumnName.EndsWith("_id") || col.ColumnName.StartsWith("id"))
                                    {
                                        id = row[col] != DBNull.Value ? Convert.ToInt32(row[col]) : 0;
                                        System.Diagnostics.Debug.WriteLine($"ID obtenido de columna alternativa {col.ColumnName}: {id}");
                                        break;
                                    }
                                }
                            }
                            
                            // Obtener descripción/nombre del producto
                            if (row.Table.Columns.Contains("pro_descripcion"))
                            {
                                name = row["pro_descripcion"] != DBNull.Value ? row["pro_descripcion"].ToString() : "Sin nombre";
        }

                            // Obtener precio del producto
                            if (row.Table.Columns.Contains("pro_precio"))
                            {
                                price = row["pro_precio"] != DBNull.Value ? Convert.ToDecimal(row["pro_precio"]) : 0;
                            }
                            
                            // Obtener stock del producto
                            if (row.Table.Columns.Contains("pro_cantidad"))
                            {
                                stock = row["pro_cantidad"] != DBNull.Value ? Convert.ToInt32(row["pro_cantidad"]) : 0;
                            }
                            
                            // Obtener categoría ID
                            if (row.Table.Columns.Contains("tbl_categorias_cat_id"))
                            {
                                categoryId = row["tbl_categorias_cat_id"] != DBNull.Value ? Convert.ToInt32(row["tbl_categorias_cat_id"]) : 0;
                            }
                            
                            // Obtener nombre de categoría
                            if (row.Table.Columns.Contains("cat_descripcion"))
        {
                                categoryName = row["cat_descripcion"] != DBNull.Value ? row["cat_descripcion"].ToString() : "Sin categoría";
                            }
                            else
                            {
                                // Si no está en el resultado, búscalo desde la capa lógica
                                DataTable categories = categoryLogic.showCategories();
                                if (categories != null && categoryId > 0)
                                {
                                    foreach (DataRow catRow in categories.Rows)
                                    {
                                        if (Convert.ToInt32(catRow["id"]) == categoryId)
                                        {
                                            categoryName = catRow["description"].ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                            
                            // Obtener proveedor ID
                            if (row.Table.Columns.Contains("tbl_proveedores_prov_id"))
                            {
                                providerId = row["tbl_proveedores_prov_id"] != DBNull.Value ? Convert.ToInt32(row["tbl_proveedores_prov_id"]) : 0;
        }

                            // Obtener nombre de proveedor
                            if (row.Table.Columns.Contains("prov_nombre"))
                            {
                                providerName = row["prov_nombre"] != DBNull.Value ? row["prov_nombre"].ToString() : "Sin proveedor";
                            }
                            else
                            {
                                // Si no está en el resultado, búscalo desde la capa lógica
                                DataSet providers = providersLogic.ShowProviders();
                                if (providers != null && providers.Tables.Count > 0 && providerId > 0)
                                {
                                    foreach (DataRow provRow in providers.Tables[0].Rows)
                                    {
                                        if (Convert.ToInt32(provRow["prov_id"]) == providerId)
        {
                                            providerName = provRow["prov_nombre"].ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                            
                            // Registrar información del producto que se añadirá al GridView
                            System.Diagnostics.Debug.WriteLine($"Añadiendo producto: ID={id}, Nombre={name}, Precio={price}, Stock={stock}, " +
                                $"CategoríaID={categoryId}, CategoríaNombre={categoryName}, " +
                                $"ProveedorID={providerId}, ProveedorNombre={providerName}");
                            
                            // Añadir los datos a la tabla que usa el GridView
                            productsTable.Rows.Add(id, name, price, stock, categoryId, categoryName, providerId, providerName);
        }
                        catch (Exception ex)
                        {
                            // Registrar detalles específicos del error para esta fila
                            System.Diagnostics.Debug.WriteLine($"Error al procesar fila de producto: {ex.Message}");
                            
                            // Listar las columnas disponibles
                            List<string> columnNames = new List<string>();
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                columnNames.Add(col.ColumnName);
                            }
                            System.Diagnostics.Debug.WriteLine($"Columnas disponibles: {string.Join(", ", columnNames)}");
                            
                            // Listar los valores de la fila
                            List<string> values = new List<string>();
                            foreach (var item in row.ItemArray)
                            {
                                values.Add(item?.ToString() ?? "null");
                            }
                            System.Diagnostics.Debug.WriteLine($"Valores: {string.Join(", ", values)}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No se encontraron productos en la base de datos o la consulta no devolvió resultados");
                }
                
                // Asignar datos al GridView
                gvProducts.DataSource = productsTable;
                gvProducts.DataBind();
                
                // Registrar cantidad final de productos en la tabla
                System.Diagnostics.Debug.WriteLine($"Total de productos cargados en el GridView: {productsTable.Rows.Count}");
            }
            catch (Exception ex)
            {
                // Registrar el error detallado
                System.Diagnostics.Debug.WriteLine($"Error al cargar productos: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Mostrar mensaje de error más detallado para diagnóstico
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al cargar productos: {ex.Message.Replace("'", "\\'")}\\n" +
                    $"Revise los logs para más detalles.');", true);
            }
        }

        /// <summary>
        /// Guardar un nuevo producto
        /// Se ejecuta cuando el usuario hace clic en el botón "Guardar"
        /// </summary>
        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            try
            {
                // Registrar inicio de operación
                System.Diagnostics.Debug.WriteLine("Iniciando guardado de producto");
                System.Diagnostics.Debug.WriteLine($"Nombre: {txtProductName.Text}");
                System.Diagnostics.Debug.WriteLine($"Precio: {txtProductPrice.Text}");
                System.Diagnostics.Debug.WriteLine($"Categoría seleccionada: {ddlCategory.SelectedValue} (índice: {ddlCategory.SelectedIndex})");
                System.Diagnostics.Debug.WriteLine($"Proveedor seleccionado: {ddlProvider.SelectedValue} (índice: {ddlProvider.SelectedIndex})");
                
            if (!string.IsNullOrEmpty(txtProductName.Text) && 
                !string.IsNullOrEmpty(txtProductPrice.Text) && 
                ddlCategory.SelectedIndex > 0 && 
                ddlProvider.SelectedIndex > 0)
            {
                    decimal price;
                    if (!decimal.TryParse(txtProductPrice.Text, out price) || price <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                            "alert('El precio debe ser un número positivo.');", true);
                        return;
                    }
                    
                    // Por defecto, usamos un stock inicial de 10 para nuevos productos
                    int stock = 10;
                    
                    // Obtener los IDs seleccionados
                int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                    int providerId = Convert.ToInt32(ddlProvider.SelectedValue);
                    
                    System.Diagnostics.Debug.WriteLine($"Guardando producto con datos: Nombre={txtProductName.Text}, " +
                        $"Precio={price}, Stock={stock}, CategoríaID={categoryId}, ProveedorID={providerId}");
                
                    // Forma directa sin confirmación para evitar problemas de JavaScript
                    // Guardar producto usando la capa lógica
                    bool result = productsLogic.SaveProduct(
                    txtProductName.Text, 
                        price,
                        stock,
                    categoryId, 
                        providerId
                );
                
                    System.Diagnostics.Debug.WriteLine($"Resultado de guardado: {(result ? "Éxito" : "Fallo")}");
                
                    if (result)
                    {
                // Limpiar los controles
                txtProductName.Text = string.Empty;
                txtProductPrice.Text = string.Empty;
                ddlCategory.SelectedIndex = 0;
                ddlProvider.SelectedIndex = 0;
                        
                        // Actualizar la vista
                        BindProductsGrid();
                        
                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                            "alert('Producto guardado exitosamente.');", true);
                    }
                    else
                    {
                        // Mostrar mensaje de error más detallado
                        ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                            "alert('No se pudo guardar el producto. Puede ser por un problema de conexión a la base de datos o porque el producto ya existe. Verifique los datos e intente nuevamente.');", true);
                    }
                }
                else
                {
                    // Recopilar información sobre qué campos faltan
                    List<string> camposFaltantes = new List<string>();
                    if (string.IsNullOrEmpty(txtProductName.Text)) camposFaltantes.Add("Nombre del producto");
                    if (string.IsNullOrEmpty(txtProductPrice.Text)) camposFaltantes.Add("Precio");
                    if (ddlCategory.SelectedIndex <= 0) camposFaltantes.Add("Categoría");
                    if (ddlProvider.SelectedIndex <= 0) camposFaltantes.Add("Proveedor");
                    
                    string mensajeError = "Todos los campos son obligatorios. ";
                    if (camposFaltantes.Count > 0)
                    {
                        mensajeError += "Falta completar: " + string.Join(", ", camposFaltantes);
                    }
                    
                    // Mostrar mensaje de error si faltan datos
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                        $"alert('{mensajeError}');", true);
                }
            }
            catch (Exception ex)
            {
                // Registrar detalles del error
                System.Diagnostics.Debug.WriteLine($"Error al guardar el producto: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Mostrar mensaje de error detallado
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al guardar el producto: {ex.Message.Replace("'", "\\'")}\\n" +
                    $"Revise los logs para más detalles.');", true);
            }
        }

        /// <summary>
        /// Manejar comandos de fila del GridView
        /// </summary>
        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Implementar si se necesitan comandos personalizados adicionales
        }

        /// <summary>
        /// Eliminar un producto
        /// </summary>
        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
            // Obtener el ID del producto a eliminar
            int productId = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
                System.Diagnostics.Debug.WriteLine($"Intentando eliminar producto con ID: {productId}");
                
                // Eliminar el producto usando la capa lógica
                bool result = productsLogic.DeleteProduct(productId);
                
                if (result)
                {
                // Actualizar la vista
                BindProductsGrid();
                    
                    // Mostrar mensaje de éxito
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                        "alert('Producto eliminado exitosamente.');", true);
                }
                else
                {
                    // Mostrar mensaje de error
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                        "alert('No se pudo eliminar el producto. Es posible que tenga dependencias en el sistema.');", true);
                }
            }
            catch (Exception ex)
            {
                // Registrar detalles del error
                System.Diagnostics.Debug.WriteLine($"Error al eliminar producto: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al procesar la solicitud: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        /// <summary>
        /// Iniciar la edición de un producto
        /// </summary>
        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            BindProductsGrid();
            
            // Cargar los dropdowns de categorías y proveedores en modo edición
            try 
            {
            DropDownList ddlCategoryEdit = (DropDownList)gvProducts.Rows[e.NewEditIndex].FindControl("ddlCategoryEdit");
            DropDownList ddlProviderEdit = (DropDownList)gvProducts.Rows[e.NewEditIndex].FindControl("ddlProviderEdit");
            
            if (ddlCategoryEdit != null)
            {
                    // Cargar categorías
                    DataTable categoriesTable = categoryLogic.showCategories();
                    
                ddlCategoryEdit.DataSource = categoriesTable;
                    ddlCategoryEdit.DataTextField = "description";
                    ddlCategoryEdit.DataValueField = "id";
                ddlCategoryEdit.DataBind();
                
                    // Obtener el ID de la categoría actual del producto
                    string categoryId = gvProducts.DataKeys[e.NewEditIndex].Values["CategoryID"].ToString();
                    // Seleccionar la categoría actual
                    ddlCategoryEdit.SelectedValue = categoryId;
                }
                
                if (ddlProviderEdit != null)
                {
                    // Obtener proveedores desde la capa lógica
                    DataSet ds = providersLogic.ShowProviders();
                    
                    // Configurar tabla para el DropDownList
                    DataTable providersTable = new DataTable();
                    providersTable.Columns.Add("ProviderID", typeof(int));
                    providersTable.Columns.Add("Name", typeof(string));
                    
                    // Transformar datos del formato de la BD al formato del DropDownList
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            int id = Convert.ToInt32(row["prov_id"]);
                            string name = row["prov_nombre"].ToString();
                            
                            providersTable.Rows.Add(id, name);
                        }
                    }
                    
                ddlProviderEdit.DataSource = providersTable;
                ddlProviderEdit.DataTextField = "Name";
                ddlProviderEdit.DataValueField = "ProviderID";
                ddlProviderEdit.DataBind();
                
                    // Obtener el ID del proveedor actual del producto
                    string providerId = gvProducts.DataKeys[e.NewEditIndex].Values["ProviderID"].ToString();
                    // Seleccionar el proveedor actual
                    ddlProviderEdit.SelectedValue = providerId;
                }
            }
            catch (Exception ex)
                    {
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al cargar datos para edición: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        /// <summary>
        /// Cancelar la edición de un producto
        /// </summary>
        protected void gvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1;
            BindProductsGrid();
        }

        /// <summary>
        /// Actualizar un producto existente
        /// </summary>
        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
            // Obtener el ID del producto a actualizar
            int productId = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
            
            // Obtener los nuevos valores
            TextBox txtName = (TextBox)gvProducts.Rows[e.RowIndex].FindControl("txtName");
            TextBox txtPrice = (TextBox)gvProducts.Rows[e.RowIndex].FindControl("txtPrice");
            DropDownList ddlCategoryEdit = (DropDownList)gvProducts.Rows[e.RowIndex].FindControl("ddlCategoryEdit");
            DropDownList ddlProviderEdit = (DropDownList)gvProducts.Rows[e.RowIndex].FindControl("ddlProviderEdit");
            
                if (txtName != null && !string.IsNullOrEmpty(txtName.Text) && 
                    txtPrice != null && !string.IsNullOrEmpty(txtPrice.Text) && 
                    ddlCategoryEdit != null && ddlProviderEdit != null)
            {
                    decimal price;
                    if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                            "alert('El precio debe ser un número positivo.');", true);
                        return;
                    }
                    
                    // Obtener el stock actual del producto desde DataKeys
                    int stock = 0;
                    if (gvProducts.DataKeys[e.RowIndex].Values["Stock"] != null && gvProducts.DataKeys[e.RowIndex].Values["Stock"] != DBNull.Value)
                    {
                        stock = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Values["Stock"]);
                    }
                    else
                    {
                        // Opcional: manejar el caso donde el stock no se pudo recuperar, aunque no debería pasar si BindProductsGrid y DataKeyNames están bien.
                        System.Diagnostics.Debug.WriteLine($"Advertencia: No se pudo recuperar el Stock para el producto ID: {productId} durante la actualización. Usando stock = 0.");
                    }
                
                    // Actualizar producto usando la capa lógica
                    bool result = productsLogic.UpdateProduct(
                        productId,
                        txtName.Text,
                        price,
                        stock,
                        Convert.ToInt32(ddlCategoryEdit.SelectedValue),
                        Convert.ToInt32(ddlProviderEdit.SelectedValue)
                    );
                    
                    if (result)
                    {
                // Salir del modo de edición
                gvProducts.EditIndex = -1;
                
                // Actualizar la vista
                BindProductsGrid();
                        
                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                            "alert('Producto actualizado exitosamente.');", true);
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                            "alert('No se pudo actualizar el producto. Verifique los datos e intente nuevamente.');", true);
                    }
                }
                else
                {
                    // Mostrar mensaje de error si faltan datos
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                        "alert('Todos los campos son obligatorios.');", true);
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al actualizar el producto: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }
    }
} 