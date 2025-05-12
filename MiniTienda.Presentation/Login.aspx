<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MiniTienda.Presentation.Login" %>

<%--
    Login.aspx
    Página de inicio de sesión del sistema
    Muestra el formulario para autenticación de usuarios
--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Iniciar Sesión - Mini Tienda</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <style>
        /* Estilos para la página de login */
        body {
            background-color: #f8f9fa;
        }
        
        /* Contenedor principal del formulario de login */
        .login-container {
            max-width: 400px;
            margin: 100px auto;
            padding: 20px;
            background-color: white;
            border-radius: 5px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        }
        
        /* Estilos para la cabecera del formulario */
        .login-header {
            text-align: center;
            margin-bottom: 20px;
        }
        
        /* Estilos para el botón de login */
        .btn-login {
            width: 100%;
            margin-top: 10px;
        }
        
        /* Estilos para mensajes de error */
        .error-message {
            color: red;
            margin-top: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- Formulario de inicio de sesión -->
            <div class="login-container">
                <div class="login-header">
                    <h2>Mini Tienda</h2>
                    <p>Ingrese sus credenciales para acceder</p>
                </div>
                
                <!-- Campo para el correo electrónico -->
                <div class="form-group">
                    <asp:Label ID="lblUsername" runat="server" Text="Correo:" AssociatedControlID="txtUsername"></asp:Label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Ingrese su correo electrónico"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                        ControlToValidate="txtUsername" 
                        ErrorMessage="El correo es requerido" 
                        Display="Dynamic" 
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                </div>
                
                <!-- Campo para la contraseña -->
                <div class="form-group">
                    <asp:Label ID="lblPassword" runat="server" Text="Contraseña:" AssociatedControlID="txtPassword"></asp:Label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Ingrese su contraseña"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                        ControlToValidate="txtPassword" 
                        ErrorMessage="La contraseña es requerida" 
                        Display="Dynamic" 
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                </div>
                
                <!-- Botón para iniciar sesión -->
                <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" CssClass="btn btn-primary btn-login" OnClick="btnLogin_Click" />
                
                <!-- Área para mostrar mensajes de error -->
                <div class="text-center error-message">
                    <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
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