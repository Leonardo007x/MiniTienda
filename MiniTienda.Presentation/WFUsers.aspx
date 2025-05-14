
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
                            <!-- Campo oculto para ID -->
                            <asp:Label ID="LblId" runat="server" Text="" Visible="false"></asp:Label>

                            <!-- Correo -->
                            <div class="col-md-3">
                                <asp:TextBox ID="TBMail" runat="server" CssClass="form-control" placeholder="Correo electrónico"></asp:TextBox>
                                <small class="form-text text-muted">Correo del usuario</small>
                            </div>

                            <!-- Contraseña -->
                            <div class="col-md-3">
                                <asp:TextBox ID="TBContrasena" runat="server" CssClass="form-control" TextMode="Password" placeholder="Contraseña"></asp:TextBox>
                                <small class="form-text text-muted">Contraseña segura</small>
                            </div>

                            <!-- Estado -->
                            <div class="col-md-2">
                                <asp:DropDownList ID="DDLState" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0">Seleccione uno</asp:ListItem>
                                    <asp:ListItem Value="Activo">Activo</asp:ListItem>
                                    <asp:ListItem Value="Inactivo">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                                <small class="form-text text-muted">Estado del usuario</small>
                            </div>

                            <!-- Botones -->
                            <div class="col-md-2">
                                <asp:Button ID="BtnSave" runat="server" Text="Guardar" CssClass="btn btn-success btn-block" OnClick="BtnSave_Click" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="BtnUpdate" runat="server" Text="Actualizar" CssClass="btn btn-info btn-block" OnClick="BtnUpdate_Click" />
                            </div>
                        </div>

                        <!-- Mensaje -->
                        <asp:Label ID="LblMsj" runat="server" CssClass="text-success"></asp:Label>
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
                            <!-- GridView para mostrar usuarios -->
                            <asp:GridView ID="GVUsers" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-bordered table-striped"
                                OnSelectedIndexChanged="GVUsers_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="usu_id" HeaderText="Id" />
                                    <asp:BoundField DataField="usu_correo" HeaderText="Correo" />
                                    <asp:BoundField DataField="usu_contrasena" HeaderText="Contraseña" />
                                    <asp:BoundField DataField="usu_salt" HeaderText="Salt" />
                                    <asp:BoundField DataField="usu_estado" HeaderText="Estado" />
                                    <asp:CommandField ShowSelectButton="True" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>