<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Config_login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    u
                </td>
                <td>
                    <input id="txt_login" type="text" tabindex="1" onkeydown="if((event.keyCode||event.which) == 13){Login();}"/>
                </td>
            </tr>
            <tr>
                <td>
                    p
                </td>
                <td>
                    <input id="txt_password" type="password" tabindex="2" onkeydown="if((event.keyCode||event.which) == 13){Login();}"/>
                </td>
            </tr>
        </table>
        <input type="button" onclick="Login();" value="Log in" tabindex="3"/>
    </div>
    </form>
    <script type="text/javascript" src="../scripts/json2.js"></script>
    <script type="text/javascript" src="../scripts/jquery-1.3.2.min.js"></script>
    <script language="javascript" type="text/javascript">
        //path, methodName, parameters, succeededCallback and failedCallback are all used
        //userContext, useHttpGet and timeout are just defaulting for now, until/if we need them in the future
        function ServiceInvoke(path, methodName, useHttpGet, parameters, succeededCallback, failedCallback, userContext, timeout) {
            if (typeof parameters !== "string") {
                parameters = JSON.stringify(parameters);
            }

            $.ajax({
                type: "POST",
                url: path + "/" + methodName,
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function(e) { failedCallback(e); },
                success: function(e) { succeededCallback(e); }
            });
        }

        function OnLoginSuccess(response) {
            if (response.d == "Success") {
                window.location = "MeaPanel.aspx";
            }
            else {
                alert(response.d);
            }
        }

        function OnLoginFailed(result) {
            alert("Failed to authenticate your login.");
        }

        function Login() {
            var LoginID = $("#txt_login").val();
            var Password = $("#txt_password").val();
            ServiceInvoke("../AdminService.asmx", "ProcessUserLogin", false,  { "LoginID": LoginID, "Password": Password },
             OnLoginSuccess, OnLoginFailed, "User Context", 1000000);
        }
    </script>
</body>
</html>
