<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminSetup.aspx.cs" Inherits="MiniTienda.Presentation.AdminSetup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Configuración de Administrador - MiniTienda</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body>
    <div class="container">
        <div class="row">
            <div class="col-md-6 mx-auto mt-5">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h4>Configuración de Administrador</h4>
                    </div>
                    <div class="card-body">
                        <form id="formAdmin" runat="server" autocomplete="off">
                            <div class="alert alert-warning">
                                <strong>Advertencia:</strong> Esta página debe ser accesible únicamente para 
                                administradores del sistema. Permite crear o actualizar usuarios administradores.
                            </div>
                            
                            <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-info d-block" Visible="false"></asp:Label>
                            
                            <div class="form-group mb-3">
                                <label for="txtEmail">Correo Electrónico:</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="admin@minitienda.com"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="El correo es obligatorio" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                            
                            <div class="form-group mb-3">
                                <label for="txtPassword">Contraseña:</label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="La contraseña es obligatoria" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                            
                            <div class="form-group mt-4 d-grid">
                                <asp:Button ID="btnCrearAdmin" runat="server" Text="Crear/Actualizar Administrador" 
                                    CssClass="btn btn-primary btn-block" OnClick="btnCrearAdmin_Click" />
                            </div>
                            
                            <div class="mt-3 text-center">
                                <a href="Default.aspx" class="btn btn-outline-secondary btn-sm">Volver al inicio de sesión</a>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="Scripts/jquery-3.6.0.min.js"></script>
    <script src="Scripts/bootstrap.bundle.min.js"></script>
</body>
</html> 