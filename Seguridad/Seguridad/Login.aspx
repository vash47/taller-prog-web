﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Seguridad.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Usuario <input type="text" ID="txtNombreUsuario" runat="server"/>
    </div>
    <div>
        Contraseña <input type="password" ID="txtContraseña" runat="server"/>
    </div>
    <div>
        <%--<input type="submit" ID="txtLogin" value="Log in" runat="server"/>--%>
        <asp:Button ID="txtLogin" Text="Log in" runat="server" 
            onclick="txtLogin_Click" />
    </div>
    </form>
</body>
</html>
