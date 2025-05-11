<%@ Page Title="Proveedores" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WFProviders.aspx.cs" Inherits="MiniTienda.Presentation.WFProviders" %>

<%--
    WFProviders.aspx
    Página para la gestión de proveedores
    Permite crear, editar y eliminar proveedores del sistema
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <!-- Formulario para crear/editar proveedores -->
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h4><i class="fas fa-truck"></i> Gestión de Proveedores</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-row mb-3">
                            <!-- Campo para el nombre del proveedor -->
                            <div class="col-md-3">
                                <asp:TextBox ID="txtProviderName" runat="server" CssClass="form-control" placeholder="Nombre del proveedor"></asp:TextBox>
                                <small class="form-text text-muted">Nombre del proveedor</small>
                            </div>
                            <!-- Campo para el NIT del proveedor -->
                            <div class="col-md-7">
                                <asp:TextBox ID="txtProviderDescription" runat="server" CssClass="form-control" placeholder="NIT"></asp:TextBox>
                                <small class="form-text text-muted">NIT o identificación fiscal del proveedor</small>
                            </div>
                            <!-- Botón para guardar el proveedor -->
                            <div class="col-md-2">
                                <asp:Button ID="btnSaveProvider" runat="server" Text="Guardar" CssClass="btn btn-primary btn-block" OnClick="btnSaveProvider_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Tabla de proveedores existentes -->
        <div class="row mt-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h4>Lista de Proveedores</h4>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <!-- GridView para mostrar y administrar los proveedores -->
                            <asp:GridView ID="gvProviders" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-striped table-bordered" 
                                OnRowCommand="gvProviders_RowCommand"
                                OnRowDeleting="gvProviders_RowDeleting"
                                OnRowEditing="gvProviders_RowEditing"
                                OnRowCancelingEdit="gvProviders_RowCancelingEdit"
                                OnRowUpdating="gvProviders_RowUpdating"
                                DataKeyNames="ProviderID">
                                <Columns>
                                    <!-- Columna para el ID (solo lectura) -->
                                    <asp:BoundField DataField="ProviderID" HeaderText="ID" ReadOnly="True" />
                                    <!-- Columna para el nombre (editable) -->
                                    <asp:TemplateField HeaderText="Nombre">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Text='<%# Bind("Name") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <!-- Columna para el NIT (editable) -->
                                    <asp:TemplateField HeaderText="NIT">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" Text='<%# Bind("Description") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <!-- Botones de edición -->
                                    <asp:CommandField ShowEditButton="True" ButtonType="Button" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar" ControlStyle-CssClass="btn btn-sm btn-info" />
                                    <!-- Botón de eliminación -->
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Eliminar" CssClass="btn btn-sm btn-danger" 
                                                OnClientClick="return confirm('¿Está seguro que desea eliminar este proveedor?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <!-- Plantilla para cuando no hay datos -->
                                <EmptyDataTemplate>
                                    <div class="alert alert-info">
                                        No hay proveedores registrados.
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
