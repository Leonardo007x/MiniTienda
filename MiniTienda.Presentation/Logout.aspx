<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="MiniTienda.Presentation.Logout" %>

<%--
    Logout.aspx
    Página para cerrar sesión del sistema
    Esta página no contiene elementos visuales ya que su único propósito es cerrar la sesión y redirigir al usuario
    
    Autor: Elkin
    Fecha: 12/05/2025
--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cerrando sesión - Mini Tienda</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container text-center mt-5">
            <h2>Cerrando sesión...</h2>
            <p>Por favor espere mientras se cierra su sesión.</p>
            <div class="spinner-border text-primary" role="status">
                <span class="sr-only">Cerrando sesión...</span>
            </div>
        </div>
    </form>

    <!-- Scripts de Bootstrap y jQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</body>
</html> 