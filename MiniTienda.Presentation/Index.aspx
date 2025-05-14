<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MiniTienda.Presentation.Index" %>

<%--
    Index.aspx
    Página de inicio del sistema que muestra un dashboard con estadísticas
    Proporciona acceso rápido a las principales secciones del sistema
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Bienvenidos"</asp:Label><br />
    <!-- Panel de bienvenida -->
    <div class="row">
        <div class="col-md-12 mb-4">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h4 class="mb-0">Bienvenido a Mini Tienda</h4>
                </div>
                <div class="card-body">
                    <h5>Hola, <asp:Label ID="lblUsername" runat="server" Text="Usuario"></asp:Label>!</h5>
                    <p>Bienvenido al sistema de gestión de Mini Tienda. Desde aquí podrás administrar todos los aspectos de tu negocio.</p>
                </div>
            </div>
        </div>
    </div>

    <!-- Tarjetas de estadísticas -->
    <div class="row">
        <div class="col-md-3 mb-4">
            <div class="card bg-primary text-white">
                <div class="card-body">
                    <h5 class="card-title">Categorías</h5>
                    <p class="card-text display-4 text-center"><asp:Label ID="lblCategories" runat="server" Text="0"></asp:Label></p>
                    <a href="WFCategories.aspx" class="btn btn-light btn-sm">Ver Detalles</a>
                </div>
            </div>
        </div>
        <div class="col-md-3 mb-4">
            <div class="card bg-success text-white">
                <div class="card-body">
                    <h5 class="card-title">Productos</h5>
                    <p class="card-text display-4 text-center"><asp:Label ID="lblProducts" runat="server" Text="0"></asp:Label></p>
                    <a href="WFProducts.aspx" class="btn btn-light btn-sm">Ver Detalles</a>
                </div>
            </div>
        </div>
        <div class="col-md-3 mb-4">
            <div class="card bg-warning text-white">
                <div class="card-body">
                    <h5 class="card-title">Proveedores</h5>
                    <p class="card-text display-4 text-center"><asp:Label ID="lblProviders" runat="server" Text="0"></asp:Label></p>
                    <a href="WFProviders.aspx" class="btn btn-light btn-sm">Ver Detalles</a>
                </div>
            </div>
        </div>
        <div class="col-md-3 mb-4">
            <div class="card bg-danger text-white">
                <div class="card-body">
                    <h5 class="card-title">Usuarios</h5>
                    <p class="card-text display-4 text-center"><asp:Label ID="lblUsers" runat="server" Text="0"></asp:Label></p>
                    <a href="WFUsers.aspx" class="btn btn-light btn-sm">Ver Detalles</a>
                </div>
            </div>
        </div>
    </div>

    <!-- Panel de estado del sistema -->
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header bg-secondary text-white">
                    <h5 class="mb-0">Estado del Sistema</h5>
                </div>
                <div class="card-body">
                    <p><strong>Fecha y Hora del Servidor:</strong> <asp:Label ID="lblDateTime" runat="server" Text=""></asp:Label></p>
                    <p><strong>Versión del Sistema:</strong> 1.0.0</p>
                    <p><strong>Último Acceso:</strong> <asp:Label ID="lblLastAccess" runat="server" Text=""></asp:Label></p>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 