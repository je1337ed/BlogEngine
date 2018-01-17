<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MeaPanelRevamped.aspx.cs" Inherits="Config_MeaPanelRevamped" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="AddBlogPostDiv">
    <table>
    <tr>
    <td>Title:</td>
    <td><input type="text" id="Title"/></td>
    </tr>
    <tr>
    <td>Description:</td>
    <td><input type="text" id="Description"/></td>
    </tr>
    <tr>
    <td>Link:</td>
    <td><input type="text" id="Link"/></td>
    </tr>
    <tr>
    <td>PubDate:</td>
    <td><input type="text" id="PubDate"/></td>
    </tr>
    <tr>
    <td colspan="4">
    <input type="text" id="Body" size="5000" /></td>
    </tr>
    </table>
    <input type="button" value="Add Post" />
    </div>
</div>
    </form>
</body>
    <script type="text/javascript" src="../scripts/json2.js"></script>
    <script type="text/javascript" src="../scripts/jquery-1.3.2.min.js"></script>
    <script type="text/javascript">
        function ServiceInvoke(path, methodName, useHttpGet, parameters, succeededCallback, 
        failedCallback, userContext, timeout) {
            if (typeof parameters !== "string") {
                parameters = JSON.stringify(parameters);
            }

            $.ajax({
                type: "POST",
                url: path + "/" + methodName,
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function(result) { failedCallback(result); },
                success: function(result) { succeededCallback(result.d); }
            });
        }

        function OnLoginSuccess(response) {
            if (response == "Success") {
                alert(response);
            }
            else {
                alert(response);
            }
        }

        function OnLoginFailed(result) {
            alert("Failed to authenticate your login.");
        }

        function Login() {
            var LoginID = $("#txt_login").val();
            var Password = $("#txt_password").val();
            ServiceInvoke("../AdminService.asmx", "ProcessUserLogin", false, { "LoginID": LoginID, "Password": Password },
             OnLoginSuccess, OnLoginFailed, "User Context", 1000000);
        }
    </script>
</html>
