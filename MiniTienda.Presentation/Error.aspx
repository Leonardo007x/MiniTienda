<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="MiniTienda.Presentation.Error" %>

<%--
    Error.aspx
    Página de error genérica
    Muestra un mensaje amigable al usuario cuando ocurre un error
    
    Autor: Leonardo
    Fecha: 15/05/2025
    Versión: 1.0 (Sprint 3)
--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error - Mini Tienda</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <style>
        /* Estilos para la página de error */
        body {
            background-color: #f8f9fa;
            padding-top: 50px;
        }
        
        /* Contenedor principal del mensaje de error */
        .error-container {
            max-width: 600px;
            margin: 100px auto;
            padding: 30px;
            background-color: white;
            border-radius: 5px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            text-align: center;
        }
        
        /* Estilos para el ícono de error */
        .error-icon {
            font-size: 64px;
            color: #dc3545;
            margin-bottom: 20px;
        }
        
        /* Estilos para el botón */
        .btn-home {
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="error-container">
                <div class="error-icon">
                    <i class="fas fa-exclamation-circle"></i>
                </div>
                
                <h2>Lo sentimos, ha ocurrido un error</h2>
                
                <div class="error-message my-4">
                    <asp:Label ID="LblMensajeError" runat="server" Text="Ha ocurrido un error inesperado en la aplicación."></asp:Label>
                    <div class="mt-2 text-muted">
                        <small><asp:Label ID="LblDetalleError" runat="server" Visible="false"></asp:Label></small>
                    </div>
                </div>
                
                <div class="error-code text-muted">
                    <asp:Label ID="LblCodigoError" runat="server" Text=""></asp:Label>
                </div>
                
                <div class="mt-4">
                    <asp:Button ID="BtnVolver" runat="server" Text="Volver al Inicio" CssClass="btn btn-primary btn-home" OnClick="BtnVolver_Click" />
                </div>
            </div>
        </div>
    </form>
    
    <!-- Font Awesome para iconos -->
    <script src="https://kit.fontawesome.com/a076d05399.js"></script>
    
    <!-- Scripts de Bootstrap y jQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</body>
</html> 