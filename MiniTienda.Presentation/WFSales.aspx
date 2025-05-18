<%@ Page Title="Ventas" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WFSales.aspx.cs" Inherits="MiniTienda.Presentation.WFSales" %>

<%--
    WFSales.aspx
    Página para la gestión de ventas del sistema
    Permite registrar y consultar ventas
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h4><i class="fas fa-cash-register"></i> Gestión de Ventas</h4>
                    </div>
                    <div class="card-body">
                        <div class="alert alert-info">
                            <h5>Módulo en desarrollo</h5>
                            <p>El módulo de gestión de ventas está actualmente en desarrollo y estará disponible próximamente.</p>
                            <p>Por favor, regrese más tarde para acceder a esta funcionalidad.</p>
                        </div>
                        <div class="text-center mt-3">
                            <a href="Index.aspx" class="btn btn-primary">Volver al inicio</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 