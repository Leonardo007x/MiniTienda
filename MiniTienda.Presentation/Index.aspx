<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MiniTienda.Presentation.Index" %>

<%--
    Index.aspx
    Página de inicio del sistema 
    Muestra una bienvenida al usuario autenticado
    
   
    Fecha: 15/05/2025
--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inicio - Mini Tienda</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <style>
        /* Estilos para la página de inicio */
        body {
            background-color: #f8f9fa;
            padding-top: 50px;
        }
        
        /* Contenedor principal */
        .welcome-container {
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
            background-color: white;
            border-radius: 5px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        }
        
        /* Estilos para la cabecera */
        .welcome-header {
            text-align: center;
            margin-bottom: 30px;
            padding-bottom: 20px;
            border-bottom: 1px solid #eee;
        }
        
        /* Estilos para el botón de cerrar sesión */
        .btn-logout {
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="welcome-container">
                <div class="welcome-header">
                    <h2>Mini Tienda</h2>
                    <p class="text-muted">Panel de administración</p>
                </div>
                
                <!-- Mensaje de bienvenida -->
                <div class="alert alert-success">
                    <h4>¡Bienvenido, <asp:Label ID="lblUsername" runat="server" Text="Usuario"></asp:Label>!</h4>
                    <p>Has iniciado sesión correctamente en el sistema.</p>
                </div>
                
                <div class="row mt-4">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-header bg-info text-white">
                                <h5 class="mb-0">Acciones rápidas</h5>
                            </div>
                            <div class="card-body">
                                <ul class="list-group">
                                    <li class="list-group-item"><a href="Dashboard.aspx">Ver Dashboard</a></li>
                                    <li class="list-group-item"><a href="WFUsers.aspx">Gestión de usuarios</a></li>
                                    <li class="list-group-item"><a href="WFProducts.aspx">Gestión de productos</a></li>
                                    <li class="list-group-item"><a href="WFSales.aspx">Gestión de ventas</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                
                <!-- Botón para cerrar sesión -->
                <div class="text-center mt-4">
                    <asp:Button ID="BtnCerrarSesion" runat="server" Text="Cerrar Sesión" 
                        CssClass="btn btn-danger btn-logout" OnClick="BtnCerrarSesion_Click" />
                </div>
            </div>
        </div>
    </form>
    
    <!-- Scripts de Bootstrap y jQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</body>
</html> 