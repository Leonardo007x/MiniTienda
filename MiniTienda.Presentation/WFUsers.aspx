<%@ Page Title="Users" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WFUsers.aspx.cs" Inherits="MiniTienda.Presentation.WFUsers" %>

<%--
    WFUsers.aspx
    Página para la gestión de categorías de productos
    Permite crear, editar y eliminar categorías del sistema
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lbl1" runat="server" Text="Correo:"></asp:Label>
    <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>

    <asp:Label ID="Label2" runat="server" Text="Ingresa la contraseña"></asp:Label>
    <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>

    <asp:Label ID="lbl3" runat="server" Text="Estado"></asp:Label>
    <asp:DropDownList ID="DDLState" runat="server" CssClass="form-select">
        <asp:ListItem Text="--Seleccione--"></asp:ListItem>
        <asp:ListItem Value="1">Activo</asp:ListItem>
        <asp:ListItem Value="0">Inactivo</asp:ListItem>
    </asp:DropDownList>

    <br />

    <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-success" Text="Guardar" OnClick="BtnSave_Click" />
    <asp:Button ID="BtnUpdate" runat="server" CssClass="btn btn-primary" Text="Actualizar" OnClick="BtnUpdate_Click" />
    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>

    <br />

    <asp:GridView ID="GVUsers" runat="server" AutoGenerateColumns="False" 
                  OnSelectedIndexChanged="GVUsers_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="usu_correo" HeaderText="Correo" />
            <asp:BoundField DataField="usu_contrasena" HeaderText="Contraseña" />
            <asp:BoundField DataField="usu_area" HeaderText="Área" />
            <asp:BoundField DataField="usu_estado" HeaderText="Estado" />
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>