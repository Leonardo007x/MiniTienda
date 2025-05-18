<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="MiniTienda.Presentation.Dashboard" %>

<%--
    Dashboard.aspx
    Página de panel de control del sistema
    Muestra estadísticas y accesos rápidos para el usuario
    
    Fecha: 15/05/2025
--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Panel de Control - Mini Tienda</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" />
    <style>
        /* Estilos para la página de dashboard */
        body {
            background-color: #f8f9fa;
        }
        
        /* Contenedor principal */
        .dashboard-container {
            margin-top: 30px;
        }
        
        /* Estilos para la cabecera */
        .welcome-header {
            margin-bottom: 30px;
            padding-bottom: 20px;
            border-bottom: 1px solid #eee;
        }
        
        /* Estilos para las tarjetas de estadísticas */
        .stat-card {
            text-align: center;
            margin-bottom: 20px;
            border-radius: 5px;
            padding: 20px;
            color: white;
            transition: transform 0.3s;
        }
        
        .stat-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 20px rgba(0,0,0,0.1);
        }
        
        .stat-card .number {
            font-size: 60px;
            font-weight: bold;
            margin: 10px 0;
        }
        
        .stat-card .title {
            font-size: 18px;
            text-transform: uppercase;
        }
        
        .stat-card .btn {
            margin-top: 15px;
            color: white;
            border-color: white;
        }
        
        /* Colores de las tarjetas */
        .card-categorias {
            background-color: #17a2b8; /* Azul */
        }
        
        .card-productos {
            background-color: #28a745; /* Verde */
        }
        
        .card-proveedores {
            background-color: #ffc107; /* Amarillo */
        }
        
        .card-usuarios {
            background-color: #dc3545; /* Rojo */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
            <div class="container">
                <a class="navbar-brand" href="#">Mini Tienda</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav mr-auto">
                        <li class="nav-item">
                            <a class="nav-link" href="Index.aspx"><i class="fas fa-home"></i> Inicio</a>
                        </li>
                    </ul>
                    <div class="user-info d-flex align-items-center">
                        <div class="dropdown">
                            <a class="nav-link dropdown-toggle text-white" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <i class="fas fa-user-circle"></i> <asp:Label ID="LblUsuarioActual" runat="server" Text="Usuario"></asp:Label>
                            </a>
                            <div class="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
                                <asp:LinkButton ID="BtnCerrarSesion" runat="server" CssClass="dropdown-item" OnClick="BtnCerrarSesion_Click">
                                    <i class="fas fa-sign-out-alt"></i> Cerrar Sesión
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </nav>

        <div class="container dashboard-container">
            <!-- Cabecera de bienvenida -->
            <div class="welcome-header">
                <h2 class="text-center">Bienvenido a Mini Tienda</h2>
                <p class="text-center text-muted">Hola, <asp:Label ID="LblUsuario" runat="server" Text="prueba@example.com"></asp:Label>!</p>
                <p class="text-center">Bienvenido al sistema de gestión de Mini Tienda. Desde aquí podrás administrar todos los aspectos de tu negocio.</p>
            </div>
            
            <!-- Tarjetas de estadísticas -->
            <div class="row">
                <!-- Tarjeta de Categorías -->
                <div class="col-md-3">
                    <div class="stat-card card-categorias">
                        <div class="title">Categorías</div>
                        <div class="number">
                            <asp:Label ID="LblCategorias" runat="server" Text="5"></asp:Label>
                        </div>
                        <asp:Button ID="BtnVerCategorias" runat="server" Text="Ver Detalles" CssClass="btn btn-outline-light" OnClick="BtnVerCategorias_Click" />
                    </div>
                </div>
                
                <!-- Tarjeta de Productos -->
                <div class="col-md-3">
                    <div class="stat-card card-productos">
                        <div class="title">Productos</div>
                        <div class="number">
                            <asp:Label ID="LblProductos" runat="server" Text="25"></asp:Label>
                        </div>
                        <asp:Button ID="BtnVerProductos" runat="server" Text="Ver Detalles" CssClass="btn btn-outline-light" OnClick="BtnVerProductos_Click" />
                    </div>
                </div>
                
                <!-- Tarjeta de Proveedores -->
                <div class="col-md-3">
                    <div class="stat-card card-proveedores">
                        <div class="title">Proveedores</div>
                        <div class="number">
                            <asp:Label ID="LblProveedores" runat="server" Text="10"></asp:Label>
                        </div>
                        <asp:Button ID="BtnVerProveedores" runat="server" Text="Ver Detalles" CssClass="btn btn-outline-light" OnClick="BtnVerProveedores_Click" />
                    </div>
                </div>
                
                <!-- Tarjeta de Usuarios -->
                <div class="col-md-3">
                    <div class="stat-card card-usuarios">
                        <div class="title">Usuarios</div>
                        <div class="number">
                            <asp:Label ID="LblUsuarios" runat="server" Text="15"></asp:Label>
                        </div>
                        <asp:Button ID="BtnVerUsuarios" runat="server" Text="Ver Detalles" CssClass="btn btn-outline-light" OnClick="BtnVerUsuarios_Click" />
                    </div>
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