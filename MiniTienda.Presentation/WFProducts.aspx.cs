/*
 * WFProducts.aspx.cs
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
    public partial class WFProducts : System.Web.UI.Page
    {
        // Instancias de las clases lógicas que se encargan de manejar los productos, categorías y proveedores
        ProductsLog objPro = new ProductsLog();
        CategoryLog objCat = new CategoryLog();
        ProvidersLog objPrv = new ProvidersLog();

        // Variables para almacenar información del producto
        private string _code, _description;
        private int _id, _quantity, _fkCategory, _fkProduct;
        private double _price;
        private bool executed = false;

        // Evento que se ejecuta al cargar la página
        protected void Page_Load(object sender, EventArgs e)
        {
            // Si no es un postback, se cargan los productos, categorías y proveedores en sus respectivos controles
            if (!Page.IsPostBack)
            {
                showProducts();
                showCategoriesDDL();
                showProvidersDDL();
            }
        }

        // Método para cargar los productos en el GridView
        private void showProducts()
        {
            DataSet objData = new DataSet();
            objData = objPro.showProducts(); // Llama al método que retorna los productos
            GVProducts.DataSource = objData;
            GVProducts.DataBind(); // Vuelve a enlazar los datos
        }

        // Método para llenar el DropDownList de categorías
        private void showCategoriesDDL()
        {
            DDLCategories.DataSource = objCat.showCategories();
            DDLCategories.DataValueField = "cat_id"; // Campo que se usará como valor
            DDLCategories.DataTextField = "cat_descripcion"; // Campo visible al usuario
            DDLCategories.DataBind();
            DDLCategories.Items.Insert(0, "Seleccione"); // Inserta una opción inicial
        }

        // Método para llenar el DropDownList de proveedores
        private void showProvidersDDL()
        {
            DDLProviders.DataSource = objPrv.showProvidersDDL();
            DDLProviders.DataValueField = "prov_id";
            DDLProviders.DataTextField = "prov_nombre";
            DDLProviders.DataBind();
            DDLProviders.Items.Insert(0, "Seleccione");
        }

        // Evento que oculta columnas del GridView que no se desean mostrar
        protected void GVProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Oculta las columnas con índices 1, 6 y 8 en el encabezado y en cada fila
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
        }

        // Evento del botón Guardar
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            // Obtiene los valores de los controles
            _code = TBCode.Text;
            _description = TBDescription.Text;
            _quantity = Convert.ToInt32(TBQuantity.Text);
            _price = Convert.ToDouble(TBPrice.Text);
            _fkCategory = Convert.ToInt32(DDLCategories.SelectedValue);
            _fkProduct = Convert.ToInt32(DDLProviders.SelectedValue);

            // Llama al método para guardar y almacena el resultado
            executed = objPro.saveProducts(_code, _description, _quantity, _price, _fkCategory, _fkProduct);

            if (executed)
            {
                LblMsg.Text = "Se guardó exitosamente";
                showProducts(); // Recarga la lista de productos
            }
            else
            {
                LblMsg.Text = "Error al guardar";
            }
        }

        // Evento del botón Actualizar
        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            // Obtiene los valores del formulario
            _id = Convert.ToInt32(TBId.Text);
            _code = TBCode.Text;
            _description = TBDescription.Text;
            _quantity = Convert.ToInt32(TBQuantity.Text);
            _price = Convert.ToDouble(TBPrice.Text);
            _fkCategory = Convert.ToInt32(DDLCategories.SelectedValue);
            _fkProduct = Convert.ToInt32(DDLProviders.SelectedValue);

            // Llama al método para actualizar y almacena el resultado
            executed = objPro.updateProducts(_id, _code, _description, _quantity, _price, _fkCategory, _fkProduct);

            if (executed)
            {
                LblMsg.Text = "Se actualizó exitosamente";
                showProducts(); // Recarga los productos
            }
            else
            {
                LblMsg.Text = "Error al actualizar";
            }
        }

        // Evento que se ejecuta al seleccionar un registro en el GridView
        protected void GVProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Asigna los valores seleccionados a los controles del formulario
            TBId.Text = GVProducts.SelectedRow.Cells[1].Text;
            TBCode.Text = GVProducts.SelectedRow.Cells[2].Text;
            TBDescription.Text = GVProducts.SelectedRow.Cells[3].Text;
            TBQuantity.Text = GVProducts.SelectedRow.Cells[4].Text;
            TBPrice.Text = GVProducts.SelectedRow.Cells[5].Text;
            DDLCategories.SelectedValue = GVProducts.SelectedRow.Cells[6].Text;
            DDLProviders.SelectedValue = GVProducts.SelectedRow.Cells[8].Text;
        }
    }
}