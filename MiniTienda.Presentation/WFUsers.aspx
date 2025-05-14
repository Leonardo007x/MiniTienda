
<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WFUsers.aspx.cs" Inherits="MiniTienda.Presentation.WFUsers" %>

<%--
    WFUsers.aspx
    Página para la gestión de usuarios del sistema
    Permite crear, editar y eliminar usuarios
    Desarrollado por: Elkin
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <!-- Botón oculto para desactivar usuarios -->
        <asp:Button ID="btnTryInactivateUser" runat="server" Text="Desactivar" style="display:none;" OnClick="btnTryInactivateUser_Click" />
        
        <!-- Formulario para crear/editar usuarios -->
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h4><i class="fas fa-users"></i> Gestión de Usuarios</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-row mb-3">
                            <!-- Campo para el nombre de usuario -->
                            <div class="col-md-3">
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="Nombre de usuario"></asp:TextBox>
                                <small class="form-text text-muted">Nombre de usuario o email</small>
                            </div>
                            <!-- Campo para la contraseña -->
                            <div class="col-md-3">
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Contraseña"></asp:TextBox>
                                <small class="form-text text-muted">Contraseña segura</small>
                            </div>
                            <!-- Selector de rol -->
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="Admin">Administrador</asp:ListItem>
                                    <asp:ListItem Value="User">Usuario</asp:ListItem>
                                    <asp:ListItem Value="Guest">Invitado</asp:ListItem>
                                </asp:DropDownList>
                                <small class="form-text text-muted">Rol del usuario</small>
                            </div>
                            <!-- Botón para guardar el usuario -->
                            <div class="col-md-2">
                                <asp:Button ID="btnSaveUser" runat="server" Text="Guardar" CssClass="btn btn-primary btn-block" OnClick="btnSaveUser_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Tabla de usuarios existentes -->
        <div class="row mt-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h4>Lista de Usuarios</h4>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <!-- GridView para mostrar y administrar los usuarios -->
                            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-striped table-bordered" 
                                OnRowCommand="gvUsers_RowCommand"
                                OnRowDeleting="gvUsers_RowDeleting"
                                OnRowEditing="gvUsers_RowEditing"
                                OnRowCancelingEdit="gvUsers_RowCancelingEdit"
                                OnRowUpdating="gvUsers_RowUpdating"
                                DataKeyNames="UserID">
                                <Columns>
                                    <asp:BoundField DataField="UserID" HeaderText="ID" ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Usuario">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" Text='<%# Bind("UserName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rol">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRole" runat="server" Text='<%# Eval("Role") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlRoleEdit" runat="server" CssClass="form-control" SelectedValue='<%# Bind("Role") %>'>
                                                <asp:ListItem Value="Admin">Administrador</asp:ListItem>
                                                <asp:ListItem Value="User">Usuario</asp:ListItem>
                                                <asp:ListItem Value="Guest">Invitado</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowEditButton="True" ButtonType="Button" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar" ControlStyle-CssClass="btn btn-sm btn-info" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Eliminar" CssClass="btn btn-sm btn-danger" 
                                                OnClientClick="return confirm('¿Está seguro que desea eliminar este usuario?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert alert-info">
                                        No hay usuarios registrados.
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 
