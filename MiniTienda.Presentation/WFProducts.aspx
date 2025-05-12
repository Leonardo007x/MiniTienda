<%@ Page Title="Products" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WFProducts.aspx.cs" Inherits="MiniTienda.Presentation.WFProducts" %>

<%--
    WFProducts.aspx
    Página para la gestión de categorías de productos
    Permite crear, editar y eliminar categorías del sistema
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <!-- Campo oculto para almacenar el ID del producto -->
        <asp:TextBox ID="TBId" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>

        <!-- Campo para ingresar el código del producto -->
        <asp:Label ID="Label1" runat="server" Text="Ingrese el código:"></asp:Label>
        <asp:TextBox ID="TBCode" runat="server" CssClass="form-control"></asp:TextBox>

        <!-- Campo para ingresar la descripción del producto -->
        <asp:Label ID="Label2" runat="server" Text="Ingrese la descripción:"></asp:Label>
        <asp:TextBox ID="TBDescription" runat="server" CssClass="form-control"></asp:TextBox>

        <!-- Campo para ingresar la cantidad del producto -->
        <asp:Label ID="Label3" runat="server" Text="Ingrese la cantidad:"></asp:Label>
        <asp:TextBox ID="TBQuantity" runat="server" CssClass="form-control"></asp:TextBox>

        <!-- Campo para ingresar el precio del producto -->
        <asp:Label ID="Label4" runat="server" Text="Ingrese el precio:"></asp:Label>
        <asp:TextBox ID="TBPrice" runat="server" CssClass="form-control"></asp:TextBox>

        <!-- Campo para seleccionar la categoría del producto -->
        <asp:Label ID="Label5" runat="server" Text="Seleccione la categoría:"></asp:Label>
        <asp:DropDownList ID="DDLCategories" runat="server" CssClass="form-select"></asp:DropDownList>

        <!-- Campo para seleccionar el proveedor del producto -->
        <asp:Label ID="Label6" runat="server" Text="Seleccione el proveedor:"></asp:Label>
        <asp:DropDownList ID="DDLProviders" runat="server" CssClass="form-select"></asp:DropDownList>
    </div>

    <!-- Botones para guardar o actualizar productos -->
    <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-success" Text="Guardar" OnClick="BtnSave_Click" />
    <asp:Button ID="BtnUpdate" runat="server" CssClass="btn btn-primary" Text="Actualizar" OnClick="BtnUpdate_Click" />

    <!-- Etiqueta para mostrar mensajes de éxito o error -->
    <asp:Label ID="LblMsj" runat="server" Text="" CssClass="text-danger"></asp:Label>

    <!-- GridView para mostrar los productos registrados -->
    <asp:GridView ID="GVProducts" runat="server" CssClass="table table-hover" 
                  OnRowDataBound="GVProducts_RowDataBound" 
                  OnSelectedIndexChanged="GVProducts_SelectedIndexChanged"
                  AutoGenerateColumns="False">
        <Columns>
            <!-- Columnas enlazadas a los campos del producto -->
            <asp:BoundField DataField="prod_codigo" HeaderText="Código" />
            <asp:BoundField DataField="prod_descripcion" HeaderText="Nombre" />
            <asp:BoundField DataField="prod_cantidad" HeaderText="Cantidad" />
            <asp:BoundField DataField="prod_precio" HeaderText="Precio" />
            <asp:BoundField DataField="prod_categoria" HeaderText="Categoría" />
            <asp:BoundField DataField="prod_proveedor" HeaderText="Proveedor" />
            <!-- Columna para seleccionar un producto -->
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>