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
    // Clase parcial de la página WFUsers, hereda de System.Web.UI.Page
    public partial class WFUsers : System.Web.UI.Page
    {
        // Instancia de la lógica de usuarios
        UsersLog objUse = new UsersLog();

        // Variables privadas para almacenar datos del usuario
        private string _mail, _contrasena, _salt, _state, _encryptedPassword;
        private int _id;
        private bool executed = false;

        // Evento que se ejecuta al cargar la página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                showUsers(); // Carga los datos de usuarios si no es una recarga (postback)
            }
        }

        // Método para mostrar los usuarios en el GridView
        private void showUsers()
        {
            DataSet objData = new DataSet();
            objData = objUse.showUsers(); // Obtiene los datos desde la capa lógica
            GVUsers.DataSource = objData;
            GVUsers.DataBind(); // Enlaza los datos al GridView
        }

        // Evento del botón "Guardar"
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            // Se utiliza una implementación de cifrado con PBKDF2
            ICryptoService cryptoService = new PBKDF2();

            // Se capturan los datos desde los controles
            _mail = TBMail.Text;
            _contrasena = TBContrasena.Text;
            _state = DDLState.SelectedValue;

            // Se genera la sal y se cifra la contraseña
            _salt = cryptoService.GenerateSalt();
            _encryptedPassword = cryptoService.Compute(_contrasena);

            // Se guarda el usuario en la base de datos
            executed = objUse.saveUsers(_mail, _encryptedPassword, _salt, _state);

            // Se verifica si la operación fue exitosa
            if (executed)
            {
                LblMsj.Text = "Se guardó exitosamente";
                showUsers(); // Refresca la lista de usuarios
            }
            else
            {
                LblMsj.Text = "Error al guardar";
            }
        }

        // Evento del botón "Actualizar"
        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            ICryptoService cryptoService = new PBKDF2();

            // Conversión del ID desde la etiqueta oculta
            _id = Convert.ToInt32(LblId.Text);
            _mail = TBMail.Text;
            _contrasena = TBContrasena.Text;
            _state = DDLState.SelectedValue;

            // Se generan los datos de cifrado
            _salt = cryptoService.GenerateSalt();
            _encryptedPassword = cryptoService.Compute(_contrasena);

            // Actualiza el usuario
            executed = objUse.updateUsers(_id, _mail, _encryptedPassword, _salt, _state);

            if (executed)
            {
                LblMsj.Text = "Se actualizó exitosamente";
                showUsers(); // Recarga la tabla de usuarios
            }
            else
            {
                LblMsj.Text = "Error al actualizar";
            }
        }

        // Evento que se dispara al seleccionar un usuario en el GridView
        protected void GVUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Se asignan los valores de la fila seleccionada a los controles del formulario
            LblId.Text = GVUsers.SelectedRow.Cells[0].Text;
            TBMail.Text = GVUsers.SelectedRow.Cells[1].Text;
            DDLState.SelectedValue = GVUsers.SelectedRow.Cells[4].Text;
            // Nota: la contraseña no se recupera por seguridad
        }
    }
}