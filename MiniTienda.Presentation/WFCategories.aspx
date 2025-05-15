<%@ Page Title="Categorías" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WFCategories.aspx.cs" Inherits="MiniTienda.Presentation.WFCategories" %>

<%--
    WFCategories.aspx
    Página para la gestión de categorías de productos
    Permite crear, editar y eliminar categorías del sistema
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <!-- Formulario para crear/editar categorías -->
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h4><i class="fas fa-tags"></i> Gestión de Categorías</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-row mb-3">
                            <!-- Campo para el nombre de la categoría -->
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="Nombre de categoría"></asp:TextBox>
                            </div>
                            <!-- Botón para guardar la categoría -->
                            <div class="col-md-2">
                                <asp:Button ID="btnSaveCategory" runat="server" Text="Guardar" CssClass="btn btn-primary btn-block" OnClick="btnSaveCategory_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Tabla de categorías existentes -->
        <div class="row mt-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h4>Lista de Categorías</h4>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <!-- GridView para mostrar y administrar las categorías -->
                            <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-striped table-bordered" 
                                OnRowCommand="gvCategories_RowCommand"
                                OnRowDeleting="gvCategories_RowDeleting"
                                OnRowEditing="gvCategories_RowEditing"
                                OnRowCancelingEdit="gvCategories_RowCancelingEdit"
                                OnRowUpdating="gvCategories_RowUpdating"
                                DataKeyNames="CategoryID">
                                <Columns>
                                    <%-- Columna para el ID (solo lectura) --%>
                                    <asp:BoundField DataField="CategoryID" HeaderText="ID" ReadOnly="True" />
                                    <%-- Columna para el nombre (editable) --%>
                                    <asp:TemplateField HeaderText="Nombre">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Text='<%# Bind("Name") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%-- Botones de edición --%>
                                    <asp:CommandField ShowEditButton="True" ButtonType="Button" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar" ControlStyle-CssClass="btn btn-sm btn-info" />
                                    <%-- Botón de eliminación --%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Eliminar" CssClass="btn btn-sm btn-danger" 
                                                OnClientClick="return confirm('¿Está seguro que desea eliminar esta categoría?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <%-- Plantilla para cuando no hay datos --%>
                                <EmptyDataTemplate>
                                    <div class="alert alert-info">
                                        No hay categorías registradas.
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