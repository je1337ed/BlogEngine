<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="ContentTitleCell">
    </div>
    <div class="ContentCell">
        <h3>
            Contact Skynet Software</h3>
        <p>
            <asp:Panel ID="pnl1" runat="server">
                <table cellpadding="5" border="0" style="font-size: 13px; font-family: Tahoma,Verdana;">
                    <tbody>
                        <tr>
                            <td align="right">
                                Your Name :
                            </td>
                            <td>
                                <asp:TextBox ID="tbName" runat="server" Style="width: 200px;" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Your Email :
                            </td>
                            <td>
                                <asp:TextBox ID="tbEmail" runat="server" Style="width: 200px;" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Subject :
                            </td>
                            <td>
                                <asp:TextBox ID="tbSubject" runat="server" Style="width: 200px;" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Message :
                            </td>
                            <td>
                                <asp:TextBox ID="tbMessage" runat="server" TextMode="MultiLine" Columns="50" Rows="10"
                                    Style="padding: 3px; font-size: 13px; font-family: Tahoma,Verdana;" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="padding: 0px;" colspan="2">
                                <asp:Button ID="submitbutton" runat="server" Text="Submit" 
                                    onclick="submitbutton_Click" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </p>
    </div>
</asp:Content>
