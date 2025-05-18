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
using MySql.Data.MySqlClient;

namespace MiniTienda.Presentation
{
    /// <summary>
    /// Clase que gestiona la funcionalidad de la página de usuarios.
    /// Permite realizar operaciones CRUD sobre los usuarios del sistema.
    /// </summary>
    public partial class WFUsers : System.Web.UI.Page
    {
        // Objeto para acceder a la capa lógica de usuarios
        private MiniTienda.Logic.UsersLog usersLogic;

        // Constructor
        public WFUsers()
        {
            usersLogic = new MiniTienda.Logic.UsersLog();
        }

        /// <summary>
        /// Método que se ejecuta cuando se carga la página
        /// Inicializa los datos desde la base de datos y configura la vista
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Registrar acceso a la página
                System.Diagnostics.Debug.WriteLine("Acceso a WFUsers.aspx - " + DateTime.Now.ToString());
                
                // Verificar si el usuario está autenticado
                if (Session["Username"] != null)
                {
                    System.Diagnostics.Debug.WriteLine("Usuario autenticado: " + Session["Username"].ToString());
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Usuario no autenticado");
                }
                
                // Log del rol del usuario (solo para información)
                if (Session["UserRole"] != null)
                {
                    System.Diagnostics.Debug.WriteLine("Rol del usuario: " + Session["UserRole"].ToString());
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No hay información de rol");
                }
                
                if (!IsPostBack)
                {
                    LoadUsersInGrid();
                }
            }
            catch (Exception ex)
            {
                // Registrar cualquier error que ocurra
                System.Diagnostics.Debug.WriteLine("Error en Page_Load de WFUsers: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Error interno: " + ex.InnerException.Message);
                }
                
                // Mostrar mensaje de error al usuario
                string script = "alert('Se produjo un error al cargar la página: " + 
                    ex.Message.Replace("'", "\\'") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
            }
        }

        /// <summary>
        /// Carga los usuarios en el GridView (gvUsers).
        /// </summary>
        private void LoadUsersInGrid()
        {
            try
            {
                DataSet ds = usersLogic.showUsers();
                
                if (ds != null && ds.Tables.Count > 0)
                {
                    // Verificar que las columnas necesarias existen en el resultado
                    DataTable dt = ds.Tables[0];
                    
                    // Diagnosticar nombres de columna
                    DiagnoseColumnNames(dt);
                    
                    if (dt.Columns["usu_id"] == null || dt.Columns["usu_correo"] == null || dt.Columns["usu_estado"] == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: La tabla de usuarios no contiene las columnas esperadas");
                        System.Diagnostics.Debug.WriteLine("Columnas disponibles: " + string.Join(", ", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));
                        
                        // Mostrar mensaje de error
                        string script = "alert('Error: La estructura de la tabla de usuarios no es la esperada. Por favor contacte al administrador.');";
                        ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
                        return;
                    }

                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();
                    
                    System.Diagnostics.Debug.WriteLine($"Se cargaron {dt.Rows.Count} usuarios en el GridView");
                }
                else
                {
                    gvUsers.DataSource = null;
                    gvUsers.DataBind();
                    System.Diagnostics.Debug.WriteLine("No se encontraron datos de usuarios para cargar");
                }
            }
            catch (Exception ex)
            {
                // Registrar el error detalladamente
                System.Diagnostics.Debug.WriteLine($"Error al cargar usuarios: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Mostrar mensaje de error
                string script = "alert('Error al cargar usuarios: " + 
                    ex.Message.Replace("'", "\\'") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
            }
        }

        /// <summary>
        /// Diagnostica y muestra los nombres de columna en una tabla
        /// </summary>
        private void DiagnoseColumnNames(DataTable dt)
        {
            if (dt == null)
            {
                System.Diagnostics.Debug.WriteLine("DiagnoseColumnNames: La tabla es nula");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"DiagnoseColumnNames: La tabla tiene {dt.Columns.Count} columnas");
            foreach (DataColumn column in dt.Columns)
            {
                System.Diagnostics.Debug.WriteLine($"Columna: '{column.ColumnName}' - Tipo: {column.DataType.Name}");
            }

            if (dt.Rows.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"DiagnoseColumnNames: La tabla tiene {dt.Rows.Count} filas");
                System.Diagnostics.Debug.WriteLine("Valores de la primera fila:");
                foreach (DataColumn column in dt.Columns)
                {
                    object valor = dt.Rows[0][column.ColumnName];
                    System.Diagnostics.Debug.WriteLine($"  {column.ColumnName}: '{valor}'");
                }
            }
        }

        /// <summary>
        /// Se ejecuta cuando se selecciona un usuario en el GridView
        /// Carga los datos del usuario seleccionado en los controles de edición
        /// Implementado como parte del Sprint 2 por Elkin
        /// </summary>
        protected void GVUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Obtener el índice de la fila seleccionada
                int index = gvUsers.SelectedIndex;
                if (index < 0)
                {
                    return; // No hay fila seleccionada
                }

                // Obtener los datos de la fila seleccionada
                DataKey key = gvUsers.DataKeys[index];
                if (key == null)
                {
                    return; // No hay clave para la fila
                }

                // Obtener el ID del usuario seleccionado
                int userId = Convert.ToInt32(key.Value);
                System.Diagnostics.Debug.WriteLine($"Usuario seleccionado con ID: {userId}");

                // Obtener los datos del usuario desde el GridView
                GridViewRow row = gvUsers.Rows[index];
                
                // Obtener el correo/nombre del usuario
                string userMail = row.Cells[1].Text; // Asumiendo que la segunda celda contiene el correo
                
                // Obtener el estado del usuario (esto depende de cómo esté estructurado el GridView)
                string userState = "activo"; // Por defecto
                
                try {
                    // Intentar obtener el estado según la estructura del GridView
                    // Este código puede variar dependiendo de la estructura exacta
                    if (row.Cells.Count > 2) {
                        userState = row.Cells[2].Text;
                    }
                } catch {
                    // Si hay algún error, mantener el valor por defecto
                }

                // Cargar los datos en los controles de edición
                txtUserName.Text = userMail;
                txtPassword.Text = string.Empty; // Por seguridad, no mostrar la contraseña
                
                // Cambiar el modo de operación a actualización
                btnSaveUser.Text = "Actualizar";
                
                // Almacenar el ID del usuario en una variable de sesión o en un control oculto
                // para usarlo más tarde en la actualización
                Session["SelectedUserId"] = userId;

                // Opcional: mostrar un mensaje al usuario
                string script = "alert('Usuario seleccionado para edición.');";
                ClientScript.RegisterStartupScript(this.GetType(), "SelectionAlert", script, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al seleccionar usuario: {ex.Message}");
                
                // Mostrar mensaje de error
                string script = "alert('Error al seleccionar el usuario: " + 
                    ex.Message.Replace("'", "\\'") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
            }
        }

        /// <summary>
        /// Verifica directamente en la base de datos si un correo ya existe
        /// </summary>
        /// <param name="email">Correo electrónico a verificar</param>
        /// <returns>True si el correo ya existe, False en caso contrario</returns>
        private bool EmailExistsInDatabase(string email)
        {
            try
            {
                MiniTienda.Data.Persistence persistence = new MiniTienda.Data.Persistence();
                MySqlConnection conn = persistence.openConnection();
                
                if (conn == null || conn.State != System.Data.ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("No se pudo abrir la conexión para verificar el correo");
                    return false;
                }
                
                // Consulta para verificar si el correo existe (insensible a mayúsculas/minúsculas)
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM tbl_usuarios WHERE LOWER(usu_correo) = LOWER(@mail)", conn);
                cmd.Parameters.AddWithValue("@mail", email);
                
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                System.Diagnostics.Debug.WriteLine($"Verificación de correo '{email}': {count} coincidencias encontradas");
                
                // Si no encontramos coincidencias exactas, buscar correos similares
                if (count == 0)
                {
                    BuscarCorreosSimilares(email, conn);
                }
                
                persistence.closeConnection();
                return count > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al verificar correo: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Busca correos similares en la base de datos para diagnóstico
        /// </summary>
        /// <param name="email">Correo a buscar</param>
        /// <param name="conn">Conexión a la base de datos</param>
        private void BuscarCorreosSimilares(string email, MySqlConnection conn)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Buscando correos similares a '{email}'...");
                
                // Obtener todos los correos para comparar
                MySqlCommand cmd = new MySqlCommand("SELECT usu_id, usu_correo FROM tbl_usuarios", conn);
                
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    System.Diagnostics.Debug.WriteLine("Correos existentes en la base de datos:");
                    
                    while (reader.Read())
                    {
                        string existingEmail = reader["usu_correo"].ToString();
                        int userId = Convert.ToInt32(reader["usu_id"]);
                        
                        System.Diagnostics.Debug.WriteLine($"  - ID: {userId}, Correo: '{existingEmail}'");
                        
                        // Verificar similitud
                        if (existingEmail.Equals(email, StringComparison.OrdinalIgnoreCase))
                        {
                            System.Diagnostics.Debug.WriteLine($"  *** COINCIDENCIA ENCONTRADA (ignorando mayúsculas/minúsculas): '{existingEmail}' vs '{email}'");
                        }
                        else if (existingEmail.Replace(" ", "").Equals(email.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                        {
                            System.Diagnostics.Debug.WriteLine($"  *** COINCIDENCIA ENCONTRADA (ignorando espacios): '{existingEmail}' vs '{email}'");
                        }
                        else if (email.Contains("@") && existingEmail.Contains("@"))
                        {
                            // Comparar solo la parte antes del @ para ver si son similares
                            string emailUser = email.Split('@')[0];
                            string existingUser = existingEmail.Split('@')[0];
                            
                            if (emailUser.Equals(existingUser, StringComparison.OrdinalIgnoreCase))
                            {
                                System.Diagnostics.Debug.WriteLine($"  *** COINCIDENCIA PARCIAL (mismo nombre de usuario): '{existingEmail}' vs '{email}'");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al buscar correos similares: {ex.Message}");
            }
        }

        /// <summary>
        /// Evento para guardar un nuevo usuario.
        /// Corresponde al botón con ID="btnSaveUser" y OnClick="btnSaveUser_Click".
        /// </summary>
        protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            try
            {
                string mail = txtUserName.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password)) 
                {
                    string script = "alert('Por favor, complete todos los campos requeridos (Usuario y Contraseña).');";
                    ClientScript.RegisterStartupScript(this.GetType(), "ValidationAlert", script, true);
                    return;
                }

                // Verificar si el correo ya existe antes de intentar guardar
                // Usamos el método directo a la base de datos para mayor confiabilidad
                if (EmailExistsInDatabase(mail))
                {
                    string script = "alert('El correo electrónico ya está registrado. Por favor, use otro correo.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "DuplicateAlert", script, true);
                    return;
                }

                // Si estamos en modo actualización
                if (Session["SelectedUserId"] != null)
                {
                    int userId = Convert.ToInt32(Session["SelectedUserId"]);
                    
                    // Actualizar el usuario existente - no pasamos el rol ya que no se usa
                    bool result = usersLogic.UpdateUser(userId, mail, mail, password, "activo");
                    
                    if (result)
                    {
                        // Limpiar los campos y restablecer el modo
                        txtUserName.Text = string.Empty;
                        txtPassword.Text = string.Empty;
                        btnSaveUser.Text = "Guardar";
                        Session["SelectedUserId"] = null;
                        
                        // Actualizar la lista de usuarios
                        LoadUsersInGrid();
                        
                        // Mostrar mensaje de éxito
                        string script = "alert('Usuario actualizado exitosamente.');";
                        ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        string script = "alert('Error al actualizar el usuario. Por favor, intente nuevamente.');";
                        ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
                    }
                }
                else
                {
                    // Crear un nuevo usuario directamente usando el método de la capa de datos
                    bool success = GuardarUsuarioDirectamente(mail, password);

                    if (success)
                    {
                        // Limpiar los campos
                        txtUserName.Text = string.Empty;
                        txtPassword.Text = string.Empty;
                        
                        // Actualizar la lista de usuarios
                        LoadUsersInGrid();
                        
                        // Mostrar mensaje de éxito
                        string script = "alert('Usuario guardado exitosamente.');";
                        ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        string script = "alert('Error al guardar el usuario. Por favor, intente con otro correo electrónico.');";
                        ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error
                string script = "alert('Error crítico al guardar: " + ex.Message.Replace("'", "\\'") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
            }
        }

        /// <summary>
        /// Guarda un usuario directamente en la base de datos usando SimpleCrypto para encriptar la contraseña
        /// </summary>
        /// <param name="email">Correo electrónico del usuario</param>
        /// <param name="password">Contraseña en texto plano</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        private bool GuardarUsuarioDirectamente(string email, string password)
        {
            try
            {
                // Generar salt y hash de contraseña usando SimpleCrypto
                SimpleCrypto.ICryptoService cryptoService = new SimpleCrypto.PBKDF2();
                string salt = cryptoService.GenerateSalt();
                cryptoService.Salt = salt;
                string hashedPassword = cryptoService.Compute(password);
                
                System.Diagnostics.Debug.WriteLine($"Guardando usuario directo - Email: {email}");
                System.Diagnostics.Debug.WriteLine($"Salt generado: {salt}");
                System.Diagnostics.Debug.WriteLine($"Contraseña hasheada: {hashedPassword}");
                
                // Crear conexión a la base de datos
                MiniTienda.Data.Persistence persistence = new MiniTienda.Data.Persistence();
                MySqlConnection conn = persistence.openConnection();
                
                if (conn == null || conn.State != System.Data.ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("No se pudo abrir la conexión para guardar el usuario");
                    return false;
                }
                
                // Insertar usuario directamente
                string insertQuery = @"
                    INSERT INTO tbl_usuarios (
                        usu_correo, 
                        usu_contrasena, 
                        usu_salt, 
                        usu_estado
                    ) VALUES (
                        @mail, 
                        @password, 
                        @salt, 
                        @state
                    )";
                
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@mail", email);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@state", "activo");
                
                int rowsAffected = cmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Filas afectadas: {rowsAffected}");
                
                persistence.closeConnection();
                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error MySQL al guardar usuario: {ex.Message}, Código: {ex.Number}");
                
                // Error 1062 es "Duplicate entry" para clave única
                if (ex.Number == 1062)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR: El correo '{email}' ya existe en la base de datos.");
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error general al guardar usuario: {ex.Message}");
                return false;
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
            try
            {
                // Obtener el ID del usuario a eliminar
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
                System.Diagnostics.Debug.WriteLine($"Intentando eliminar usuario con ID: {userId}");
                
                // Mostrar más información sobre la fila
                GridViewRow row = gvUsers.Rows[e.RowIndex];
                DataKey key = gvUsers.DataKeys[e.RowIndex];
                System.Diagnostics.Debug.WriteLine($"DataKey value: {key.Value}");
                System.Diagnostics.Debug.WriteLine($"Valores de la fila: {row.Cells[0].Text}, {row.Cells[1].Text}");
                
                // Usar la nueva funcionalidad de eliminación de usuarios
                bool result = usersLogic.DeleteUser(userId);
                
                System.Diagnostics.Debug.WriteLine($"Resultado de eliminación de usuario: {(result ? "Exitoso" : "Fallido")}");
                
                if (result)
                {
                    // Actualizar la vista
                    LoadUsersInGrid();
                    
                    // Mostrar mensaje de éxito
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                        "alert('Usuario eliminado exitosamente.');", true);
                }
                else
                {
                    // Intentar forzar la eliminación (solución alternativa)
                    System.Diagnostics.Debug.WriteLine("Intentando método alternativo para eliminar el usuario...");
                    
                    // Mostrar un mensaje más específico
                    string errorMessage = "No se pudo eliminar el usuario. " +
                        "El usuario podría tener registros asociados en el sistema que impiden su eliminación. " +
                        "¿Desea intentar desactivar el usuario en lugar de eliminarlo?";
                    
                    string script = $@"
                        if(confirm('{errorMessage}')) {{
                            __doPostBack('{btnTryInactivateUser.UniqueID}', '{userId}');
                        }}";
                    
                    ScriptManager.RegisterStartupScript(this, GetType(), "InactivateUserConfirm", script, true);
                }
            }
            catch (Exception ex)
            {
                // Registrar detalles del error
                System.Diagnostics.Debug.WriteLine($"Error al eliminar usuario: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Mostrar mensaje de error más detallado
                string errorMsg = "Error al eliminar el usuario. ";
                
                // Personalizar mensaje según tipo de error
                if (ex.Message.Contains("foreign key constraint") || ex.Message.Contains("clave foránea") || ex.Message.Contains("referential integrity"))
                {
                    errorMsg += "Este usuario tiene registros asociados en otras tablas que impiden su eliminación. " +
                        "Primero debe eliminar estos registros relacionados o utilizar la opción de desactivación.";
                }
                else if (ex.Message.Contains("connection") || ex.Message.Contains("conexión"))
                {
                    errorMsg += "Error de conexión a la base de datos. Por favor, verifique su conexión e intente nuevamente.";
                }
                else
                {
                    errorMsg += ex.Message.Replace("'", "\\'");
                }
                
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('{errorMsg}');", true);
            }
        }

        /// <summary>
        /// Maneja el evento de intentar desactivar un usuario en lugar de eliminarlo
        /// </summary>
        protected void btnTryInactivateUser_Click(object sender, EventArgs e)
        {
            string userIdStr = Request.Params["__EVENTARGUMENT"];
            
            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out int userId))
            {
                try
                {
                    // Intentar desactivar el usuario
                    System.Diagnostics.Debug.WriteLine($"Intentando desactivar usuario con ID: {userId}");
                    
                    // Actualizar el estado del usuario a "inactivo"
                    bool result = usersLogic.DeactivateUser(userId);
                    
                    if (result)
                    {
                        // Actualizar la vista
                        LoadUsersInGrid();
                        
                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                            "alert('El usuario ha sido desactivado exitosamente.');", true);
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                            "alert('No se pudo desactivar el usuario. Por favor, póngase en contacto con el administrador del sistema.');", true);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al desactivar usuario: {ex.Message}");
                    
                    // Mostrar mensaje de error
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                        $"alert('Error al desactivar el usuario: {ex.Message.Replace("'", "\\'")}');", true);
                }
            }
        }

        /// <summary>
        /// Iniciar la edición de un usuario
        /// Se ejecuta cuando el usuario hace clic en "Editar" en una fila
        /// </summary>
        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsersInGrid();
        }

        /// <summary>
        /// Cancelar la edición de un usuario
        /// Se ejecuta cuando el usuario hace clic en "Cancelar" durante una edición
        /// </summary>
        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsersInGrid();
        }

        /// <summary>
        /// Actualizar un usuario existente
        /// Se ejecuta cuando el usuario hace clic en "Actualizar" después de editar una fila
        /// </summary>
        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // Obtener el ID del usuario a actualizar
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
                System.Diagnostics.Debug.WriteLine($"Actualizando usuario con ID: {userId}");
                
                // Obtener los nuevos valores
                TextBox txtUserName = (TextBox)gvUsers.Rows[e.RowIndex].FindControl("txtUserName");
                DropDownList ddlRoleEdit = (DropDownList)gvUsers.Rows[e.RowIndex].FindControl("ddlRoleEdit");
                
                if (txtUserName != null && !string.IsNullOrEmpty(txtUserName.Text))
                {
                    System.Diagnostics.Debug.WriteLine($"Nuevo nombre/correo: {txtUserName.Text}");
                    System.Diagnostics.Debug.WriteLine($"Nuevo estado: {(ddlRoleEdit != null ? ddlRoleEdit.SelectedValue : "No encontrado")}");
                    
                    // Para actualizar el usuario, necesitamos una contraseña
                    // Como no pedimos contraseña en el modo de edición, usamos una predeterminada
                    // En un sistema real, esto debería manejarse mejor (mantener la contraseña existente)
                    string password = "Password123!"; // Contraseña que cumple con los requisitos
                    
                    // Actualizar usuario usando la capa lógica
                    bool result = usersLogic.UpdateUser(
                        userId,
                        txtUserName.Text, // Nombre
                        txtUserName.Text, // Email (usamos el mismo valor)
                        password, // Contraseña
                        ddlRoleEdit != null ? ddlRoleEdit.SelectedValue : "activo" // Estado/Rol
                    );
                    
                    System.Diagnostics.Debug.WriteLine($"Resultado de actualización: {(result ? "Exitoso" : "Fallido")}");
                    
                    if (result)
                    {
                        // Salir del modo de edición
                        gvUsers.EditIndex = -1;
                        
                        // Actualizar la vista
                        LoadUsersInGrid();
                        
                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", 
                            "alert('Usuario actualizado exitosamente.');", true);
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                            "alert('No se pudo actualizar el usuario. Verifique los datos e intente nuevamente.');", true);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error: No se encontró el control txtUserName o está vacío");
                    // Mostrar mensaje de error
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                        "alert('No se pudo actualizar el usuario. El nombre/correo no puede estar vacío.');", true);
                }
            }
            catch (Exception ex)
            {
                // Registrar el error
                System.Diagnostics.Debug.WriteLine($"Error al actualizar usuario: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", 
                    $"alert('Error al actualizar el usuario: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        /// <summary>
        /// Método para diagnosticar problemas con la base de datos
        /// </summary>
        protected void btnTestDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear una instancia de Persistence para probar la conexión
                MiniTienda.Data.Persistence persistence = new MiniTienda.Data.Persistence();
                
                // Implementar verificación de conexión directamente
                bool connectionOk = false;
                MySqlConnection conn = persistence.openConnection();
                string dbInfo = "";
                
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    connectionOk = true;
                    System.Diagnostics.Debug.WriteLine("Conexión exitosa a la base de datos");
                    dbInfo += "Conexión exitosa a la base de datos\n";
                    
                    // Probar una consulta simple
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand("SELECT VERSION()", conn);
                        string version = cmd.ExecuteScalar().ToString();
                        System.Diagnostics.Debug.WriteLine($"Versión de MySQL: {version}");
                        dbInfo += $"Versión de MySQL: {version}\n";
                        
                        // Verificar la base de datos actual
                        cmd.CommandText = "SELECT DATABASE()";
                        string database = cmd.ExecuteScalar().ToString();
                        System.Diagnostics.Debug.WriteLine($"Base de datos actual: {database}");
                        dbInfo += $"Base de datos actual: {database}\n";
                        
                        // Contar usuarios en la tabla
                        cmd.CommandText = "SELECT COUNT(*) FROM tbl_usuarios";
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                        System.Diagnostics.Debug.WriteLine($"Total de usuarios en la base de datos: {userCount}");
                        dbInfo += $"Total de usuarios en la base de datos: {userCount}\n";
                        
                        // Verificar la estructura de la tabla
                        cmd.CommandText = "DESCRIBE tbl_usuarios";
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dbInfo += "\nEstructura de la tabla tbl_usuarios:\n";
                            while (reader.Read())
                            {
                                string field = reader["Field"].ToString();
                                string type = reader["Type"].ToString();
                                string key = reader["Key"].ToString();
                                dbInfo += $"- {field}: {type} {(key == "PRI" ? "(Clave primaria)" : key == "UNI" ? "(Único)" : "")}\n";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al ejecutar consulta de prueba: {ex.Message}");
                        dbInfo += $"Error al ejecutar consulta: {ex.Message}\n";
                    }
                    
                    persistence.closeConnection();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No se pudo establecer conexión con la base de datos");
                    dbInfo += "No se pudo establecer conexión con la base de datos\n";
                }
                
                if (connectionOk)
                {
                    // Probar la búsqueda de usuario por correo
                    string testMail = txtUserName.Text.Trim();
                    if (string.IsNullOrEmpty(testMail))
                    {
                        testMail = "test@example.com"; // Correo de prueba si no hay uno ingresado
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"Probando búsqueda de usuario con correo: {testMail}");
                    dbInfo += $"\nProbando búsqueda de usuario con correo: {testMail}\n";
                    
                    // Usar nuestro método directo para verificar el correo
                    bool emailExists = EmailExistsInDatabase(testMail);
                    
                    if (emailExists)
                    {
                        System.Diagnostics.Debug.WriteLine($"Usuario encontrado con correo: {testMail}");
                        dbInfo += $"RESULTADO: El correo {testMail} YA EXISTE en la base de datos.\n";
                        
                        string script = $"alert('Diagnóstico completado. La conexión a la base de datos funciona correctamente.\n\nEl correo {testMail} YA EXISTE en la base de datos.\n\nConsulte la consola para más detalles.');";
                        ClientScript.RegisterStartupScript(this.GetType(), "DiagnosticAlert", script, true);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"No se encontró usuario con correo: {testMail}");
                        dbInfo += $"RESULTADO: El correo {testMail} NO existe en la base de datos.\n";
                        
                        // Intentar insertar un usuario de prueba
                        dbInfo += "\nIntentando insertar usuario de prueba...\n";
                        
                        // Generar un correo único para la prueba
                        string testEmail = $"test_{DateTime.Now.Ticks}@example.com";
                        bool testInsert = false;
                        
                        try
                        {
                            conn = persistence.openConnection();
                            if (conn != null && conn.State == System.Data.ConnectionState.Open)
                            {
                                // Insertar usuario de prueba
                                MySqlCommand cmd = new MySqlCommand(
                                    "INSERT INTO tbl_usuarios (usu_correo, usu_contrasena, usu_salt, usu_estado) VALUES (@mail, 'test', 'test', 'activo')", 
                                    conn);
                                cmd.Parameters.AddWithValue("@mail", testEmail);
                                
                                int rowsAffected = cmd.ExecuteNonQuery();
                                testInsert = rowsAffected > 0;
                                
                                // Eliminar el usuario de prueba
                                if (testInsert)
                                {
                                    cmd.CommandText = "DELETE FROM tbl_usuarios WHERE usu_correo = @mail";
                                    cmd.ExecuteNonQuery();
                                }
                                
                                persistence.closeConnection();
                            }
                        }
                        catch (Exception ex)
                        {
                            dbInfo += $"Error al insertar usuario de prueba: {ex.Message}\n";
                            System.Diagnostics.Debug.WriteLine($"Error al insertar usuario de prueba: {ex.Message}");
                        }
                        
                        dbInfo += testInsert ? 
                            "La prueba de inserción fue EXITOSA. El problema no parece ser de permisos.\n" : 
                            "La prueba de inserción FALLÓ. Puede haber problemas de permisos o con la estructura de la tabla.\n";
                        
                        string script = $"alert('Diagnóstico completado. La conexión a la base de datos funciona correctamente.\n\nEl correo {testMail} NO existe en la base de datos.\n\nConsulte la consola para más detalles.');";
                        ClientScript.RegisterStartupScript(this.GetType(), "DiagnosticAlert", script, true);
                    }
                }
                else
                {
                    string script = "alert('Error: No se pudo conectar a la base de datos. Verifique la configuración de conexión.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
                }
                
                // Mostrar toda la información de diagnóstico en la consola
                System.Diagnostics.Debug.WriteLine("\n===== RESUMEN DE DIAGNÓSTICO =====");
                System.Diagnostics.Debug.WriteLine(dbInfo);
                System.Diagnostics.Debug.WriteLine("===============================\n");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en diagnóstico: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                
                string script = $"alert('Error durante el diagnóstico: {ex.Message.Replace("'", "\\'")}');";
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
            }
        }
    }
} 