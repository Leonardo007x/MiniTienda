/*
 * WFUsers.aspx.cs
 * Controlador para la página de gestión de usuarios
 * Implementa la funcionalidad para crear, editar y eliminar usuarios del sistema
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
    /// Clase que gestiona la funcionalidad de la página de usuarios.
    /// Permite realizar operaciones CRUD sobre los usuarios del sistema.
    /// </summary>
    public partial class WFUsers : System.Web.UI.Page
    {
        // Datos de muestra para mostrar en la vista
        private DataTable usersTable;

        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// Inicializa los datos de muestra y configura la vista
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateSampleData();
                BindUsersGrid();
            }
        }

        /// <summary>
        /// Método para crear datos de muestra para la demostración
        /// En un sistema real, estos datos vendrían de la base de datos
        /// </summary>
        private void CreateSampleData()
        {
            usersTable = new DataTable();
            usersTable.Columns.Add("UserID", typeof(int));
            usersTable.Columns.Add("UserName", typeof(string));
            usersTable.Columns.Add("Password", typeof(string));
            usersTable.Columns.Add("Role", typeof(string));

            // Agregar algunos usuarios de muestra
            // Nota: En un sistema real, las contraseñas estarían cifradas con SimpleCrypto
            usersTable.Rows.Add(1, "admin@ejemplo.com", "contraseña_cifrada_1", "Admin");
            usersTable.Rows.Add(2, "usuario@ejemplo.com", "contraseña_cifrada_2", "User");
            usersTable.Rows.Add(3, "invitado@ejemplo.com", "contraseña_cifrada_3", "Guest");

            // Guardar la tabla en sesión
            Session["Users"] = usersTable;
        }

        /// <summary>
        /// Enlazar los datos a la cuadrícula (GridView)
        /// Actualiza la vista con los datos más recientes
        /// </summary>
        private void BindUsersGrid()
        {
            if (Session["Users"] != null)
            {
                gvUsers.DataSource = Session["Users"];
                gvUsers.DataBind();
            }
        }

        /// <summary>
        /// Guardar un nuevo usuario
        /// Se ejecuta cuando el usuario hace clic en el botón "Guardar"
        /// </summary>
        protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text))
            {
                DataTable dt = (DataTable)Session["Users"];
                
                // Determinar el nuevo ID (en un sistema real se generaría por la base de datos)
                int newId = 1;
                if (dt.Rows.Count > 0)
                {
                    newId = dt.AsEnumerable().Max(row => row.Field<int>("UserID")) + 1;
                }
                
                // En un sistema real, se usaría SimpleCrypto para cifrar la contraseña
                // SimpleCrypto.PBKDF2 crypto = new SimpleCrypto.PBKDF2();
                // string encryptedPassword = crypto.Compute(txtPassword.Text);
                string encryptedPassword = "contraseña_cifrada_" + newId; // Simulación
                
                // Agregar el nuevo usuario
                dt.Rows.Add(newId, txtUserName.Text, encryptedPassword, ddlRole.SelectedValue);
                
                // Actualizar la sesión
                Session["Users"] = dt;
                
                // Actualizar la vista
                BindUsersGrid();
                
                // Limpiar los controles
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                ddlRole.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Manejar comandos de fila del GridView
        /// Permite responder a eventos personalizados en las filas
        /// </summary>
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Implementar si se necesitan comandos personalizados adicionales
        }

        /// <summary>
        /// Eliminar un usuario
        /// Se ejecuta cuando el usuario hace clic en "Eliminar" en una fila
        /// </summary>
        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)Session["Users"];
            
            // Obtener el ID del usuario a eliminar
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            
            // Encontrar la fila a eliminar
            DataRow[] rows = dt.Select("UserID = " + userId);
            if (rows.Length > 0)
            {
                // Eliminar la fila
                rows[0].Delete();
                dt.AcceptChanges();
                
                // Actualizar la sesión
                Session["Users"] = dt;
                
                // Actualizar la vista
                BindUsersGrid();
            }
        }

        /// <summary>
        /// Iniciar la edición de un usuario
        /// Se ejecuta cuando el usuario hace clic en "Editar" en una fila
        /// </summary>
        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            BindUsersGrid();
        }

        /// <summary>
        /// Cancelar la edición de un usuario
        /// Se ejecuta cuando el usuario hace clic en "Cancelar" durante una edición
        /// </summary>
        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            BindUsersGrid();
        }

        /// <summary>
        /// Actualizar un usuario existente
        /// Se ejecuta cuando el usuario hace clic en "Actualizar" después de editar una fila
        /// </summary>
        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session["Users"];
            
            // Obtener el ID del usuario a actualizar
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            
            // Obtener los nuevos valores
            TextBox txtUserName = (TextBox)gvUsers.Rows[e.RowIndex].FindControl("txtUserName");
            DropDownList ddlRoleEdit = (DropDownList)gvUsers.Rows[e.RowIndex].FindControl("ddlRoleEdit");
            
            // Encontrar la fila a actualizar
            DataRow[] rows = dt.Select("UserID = " + userId);
            if (rows.Length > 0)
            {
                // Actualizar los valores
                rows[0]["UserName"] = txtUserName.Text;
                rows[0]["Role"] = ddlRoleEdit.SelectedValue;
                // Nota: No actualizamos la contraseña en este flujo de edición
                dt.AcceptChanges();
                
                // Actualizar la sesión
                Session["Users"] = dt;
                
                // Salir del modo de edición
                gvUsers.EditIndex = -1;
                
                // Actualizar la vista
                BindUsersGrid();
            }
        }
    }
} 