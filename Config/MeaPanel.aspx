<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MeaPanel.aspx.cs" Inherits="Admin_MeaPanel" ValidateRequest="false"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Panel ID="pnlAddPost" runat="server">
    <table>
    <tr>
    <td>Title:</td>
    <td><asp:TextBox ID="tbTitle" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
    <td>Description:</td>
    <td><asp:TextBox ID="tbDescription" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
    <td>Link:</td>
    <td><asp:TextBox ID="tbLink" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
    <td>PubDate:</td>
    <td><asp:TextBox ID="tbPubDate" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
    <td colspan="4">
    <td><asp:TextBox ID="tbBody" TextMode="MultiLine" Width="400px" Height="200px" MaxLength="5000" runat="server"></asp:TextBox></td>
    </tr>
    </table>
    <asp:Button ID="btnAdd" runat="server" Text="Add Post" onclick="btnAdd_Click" />
    </asp:Panel>
    </div>
    </form>
</body>
</html>
