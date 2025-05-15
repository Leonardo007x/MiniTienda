<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WFProducts.aspx.cs" Inherits="MiniTienda.Presentation.WFProducts" %>

<%--
    WFProducts.aspx
    Página para la gestión de productos
    Permite crear, editar y eliminar productos del sistema
    Desarrollado por: Elkin
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <!-- Formulario para crear/editar productos -->
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h4><i class="fas fa-box-open"></i> Gestión de Productos</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-row mb-3">
                            <!-- Campo para el nombre del producto -->
                            <div class="col-md-3">
                                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" placeholder="Nombre del producto"></asp:TextBox>
                                <small class="form-text text-muted">Nombre del producto</small>
                            </div>
                            <!-- Campo para el precio del producto -->
                            <div class="col-md-2">
                                <asp:TextBox ID="txtProductPrice" runat="server" CssClass="form-control" placeholder="Precio" TextMode="Number" step="0.01"></asp:TextBox>
                                <small class="form-text text-muted">Precio en $</small>
                            </div>
                            <!-- Selector de categoría -->
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                                <small class="form-text text-muted">Categoría</small>
                            </div>
                            <!-- Selector de proveedor -->
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlProvider" runat="server" CssClass="form-control"></asp:DropDownList>
                                <small class="form-text text-muted">Proveedor</small>
                            </div>
                            <!-- Botón para guardar el producto -->
                            <div class="col-md-1">
                                <asp:Button ID="btnSaveProduct" runat="server" Text="Guardar" CssClass="btn btn-primary btn-block" OnClick="btnSaveProduct_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Tabla de productos existentes -->
        <div class="row mt-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h4>Lista de Productos</h4>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <!-- GridView para mostrar y administrar los productos -->
                            <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-striped table-bordered" 
                                OnRowCommand="gvProducts_RowCommand"
                                OnRowDeleting="gvProducts_RowDeleting"
                                OnRowEditing="gvProducts_RowEditing"
                                OnRowCancelingEdit="gvProducts_RowCancelingEdit"
                                OnRowUpdating="gvProducts_RowUpdating"
                                DataKeyNames="ProductID">
                                <Columns>
                                    <%-- Columna para el ID (solo lectura) --%>
                                    <asp:BoundField DataField="ProductID" HeaderText="ID" ReadOnly="True" />
                                    <%-- Columna para el nombre (editable) --%>
                                    <asp:TemplateField HeaderText="Nombre">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Text='<%# Bind("Name") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%-- Columna para el precio (editable) --%>
                                    <asp:TemplateField HeaderText="Precio">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrice" runat="server" Text='<%# String.Format("${0:0.00}", Eval("Price")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" Text='<%# Bind("Price") %>' TextMode="Number" step="0.01"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%-- Columna para la categoría (editable) --%>
                                    <asp:TemplateField HeaderText="Categoría">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlCategoryEdit" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%-- Columna para el proveedor (editable) --%>
                                    <asp:TemplateField HeaderText="Proveedor">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProvider" runat="server" Text='<%# Eval("ProviderName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlProviderEdit" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%-- Botones de edición --%>
                                    <asp:CommandField ShowEditButton="True" ButtonType="Button" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar" ControlStyle-CssClass="btn btn-sm btn-info" />
                                    <%-- Botón de eliminación --%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Eliminar" CssClass="btn btn-sm btn-danger" 
                                                OnClientClick="return confirm('¿Está seguro que desea eliminar este producto?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <%-- Plantilla para cuando no hay datos --%>
                                <EmptyDataTemplate>
                                    <div class="alert alert-info">
                                        No hay productos registrados.
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