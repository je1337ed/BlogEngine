<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin"
    MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .userName
        {
            color: Black;
            text-decoration: underline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divUsers" style="float: left; height: 100%; width: 250px;">
        &nbsp;
    </div>
    <div>
        <table>
            <tr>
                <td>
                    Login (Username)
                </td>
                <td>
                    First Name
                </td>
                <td>
                    Last Name
                </td>
                <td id="tdUserType">
                    User Type
                </td>
            </tr>
            <tr>
                <td>
                    <input id="tbLogin" type="text" />
                </td>
                <td>
                    <input id="tbFirstName" type="text" />
                </td>
                <td>
                    <input id="tbLastName" type="text" />
                </td>
                <td>
                    <select id="ddlUserType">
                        <option value="U" selected="selected">User</option>
                        <option value="A">Admin</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    Password
                </td>
                <td>
                    Password (Confirm)
                </td>
            </tr>
            <tr>
                <td>
                    <input id="tbPassword" type="text" />
                </td>
                <td>
                    <input id="tbPasswordConfirm" type="text" />
                </td>
            </tr>
            <tr>
                <td>
                    Address 1
                </td>
                <td>
                    Address 2
                </td>
            </tr>
            <tr>
                <td>
                    <input id="tbAddress1" type="text" />
                </td>
                <td>
                    <input id="tbAddress2" type="text" />
                </td>
            </tr>
            <tr>
                <td>
                    City
                </td>
                <td>
                    State
                </td>
                <td>
                    Zip
                </td>
            </tr>
            <tr>
                <td>
                    <input id="tbCity" type="text" />
                </td>
                <td>
                    <select id="ddlState">
                        <option value="FL">FL</option>
                        <option value="NC">NC</option>
                    </select>
                </td>
                <td>
                    <input id="tbZip" type="text" />
                </td>
            </tr>
            <tr>
                <td>
                    Email Address
                </td>
                <td>
                    Phone Number
                </td>
                <td>
                    Fax Number
                </td>
            </tr>
            <tr>
                <td>
                    <input id="tbEmail" type="text" />
                </td>
                <td>
                    <input id="tbPhoneNumber" type="text" />
                </td>
                <td>
                    <input id="tbFax" type="text" />
                </td>
            </tr>
            <tr>
                <td>
                    <label id="lblActive" for="moreinfo">
                        Active
                        <input id="cbActive" type="checkbox" checked="checked" />
                    </label>
                </td>
            </tr>
            <tr>
                <td>
                    <input id="btnSubmit" type="button" value="Add Account" onclick="AddUser();" />
                </td>
            </tr>
        </table>
        <input type="hidden" id="hfUserID" />
    </div>

    <script type="text/javascript" src="scripts/json2.js"></script>

    <script type="text/javascript" src="scripts/jquery-1.3.2.min.js"></script>

    <script type="text/javascript">
        var UserArr;
        var SelectedUser;

        function GetVal(ID) {
            //            var Value = '';
            //            var Element = $get(ID);
            //            if (Element) {
            //                if (Element.value !== '') {
            //                    Value = Element.value;
            //                }
            //            }
            return $("#" + ID).val(); // Value;
        }

        function GetDdlValue(ID) {
            //            var Value = '';
            //            var Element = $get(ID);
            //            if (Element) {
            //                if (Element.value !== '') {
            //                    Value = Element[Element.selectedIndex].value;
            //                }
            //            }
            return $("#" + ID).val(); //Value;
        }

        function AddUser() {
            //$('#tbPasswordConfirm').value
            //$('#tbAdjustingCompany').value
            var Active;
            if ($('#cbActive').value == 'on') { Active = "True"; }
            else { Active = "False"; }
            //$('#tbAddressType').value
            var User = { 'type': 'AtlasObjects.ClaimsAdminSystemUser', 'UserID': 0, 'SystemUserID': 0,
                'AdjustingCompanyContactTypeAssocID': 0,
                'UserType': GetDdlValue('ddlUserType'), 'UserName': GetVal('tbLogin'),
                'LoginName': GetVal('tbLogin'), 'LoginPassword': GetVal('tbPassword'),
                'FirstName': GetVal('tbFirstName'), 'LastName': GetVal('tbLastName'),
                'EmailAddress': GetVal('tbEmail'), 'AddressType': '1',
                'Address1': GetVal('tbAddress1'), 'Address2': GetVal('tbAddress2'),
                'City': GetVal('tbCity'), 'State': GetDdlValue('ddlState'), 'Zip': GetVal('tbZip'),
                'Phone': GetVal('tbPhoneNumber'), 'Fax': GetVal('tbFax'),
                'IsActive': Active, 'LastLoginDate': '01/01/1900'
            };

            ServiceInvoke("ClaimsAdminService.asmx", "AddUser", false, User, OnAddUserSuccess, OnFailure, "User Context", 1000000);
        }

        function UpdateUser() {
            var Active;
            var User = UserArr[SelectedUser];
            if ($('#cbActive').value == 'on') { Active = "True"; }
            else { Active = "False"; }
            var User = { 'type': 'AtlasObjects.ClaimsAdminSystemUser', 'UserID': User.UserID, 'SystemUserID': User.SystemUserID,
                'AdjustingCompanyContactTypeAssocID': 0,
                'UserType': GetDdlValue('ddlUserType'), 'UserName': GetVal('tbLogin'),
                'LoginName': GetVal('tbLogin'), 'LoginPassword': GetVal('tbPassword'),
                'FirstName': GetVal('tbFirstName'), 'LastName': GetVal('tbLastName'),
                'EmailAddress': GetVal('tbEmail'), 'AddressType': "1",
                'Address1': GetVal('tbAddress1'), 'Address2': GetVal('tbAddress2'),
                'City': GetVal('tbCity'), 'State': GetDdlValue('ddlState'), 'Zip': GetVal('tbZip'),
                'Phone': GetVal('tbPhoneNumber'), 'Fax': GetVal('tbFax'),
                'IsActive': Active, 'LastLoginDate': '01/01/1900'
            };

            ServiceInvoke("ClaimsAdminService.asmx", "UpdateUser", false, User, OnUpdateUserSuccess, OnFailure, "User Context", 1000000);
        }

        function SetUpNewUser() {
            SelectedUser = null;
            var StringEmpty = "";
            $('#ddlUserType').val("U");
            $('#ddlUserType').disabled = false;
            $('#tbEmail').val(StringEmpty);
            $('#tbLogin').val(StringEmpty);
            $('#tbPassword').val(StringEmpty);
            $('#tbPasswordConfirm').val(StringEmpty);
            $('#tbFirstName').val(StringEmpty);
            $('#tbLastName').val(StringEmpty);
            $('#tbAddress1').val(StringEmpty);
            $('#tbAddress2').val(StringEmpty);
            $('#tbCity').val(StringEmpty);
            $('#ddlState').attr("selectedIndex", 0);
            $('#tbZip').val(StringEmpty);
            $('#tbPhoneNumber').val(StringEmpty);
            $('#tbFax').val(StringEmpty);
            $('#cbActive').val(StringEmpty);
            $('#btnSubmit').val("Add Account");
            $('#btnSubmit')[0].onclick = AddUser;
        }

        function UserSelected(index) {
            SelectedUser = index;
            var User = UserArr[index];
            SetDdlValue($('#ddlUserType'), User.UserType);
            if (User.UserType === "A") { $('#ddlUserType').attr("disabled", "disabled"); }
            else { $('#ddlUserType').removeAttr("disabled"); }
            $('#tbEmail').val(User.EmailAddress);
            $('#tbLogin').val(User.LoginName);
            $('#tbPassword').val(User.LoginPassword);
            $('#tbPasswordConfirm').val(User.LoginPassword);
            $('#tbFirstName').val(User.FirstName);
            $('#tbLastName').val(User.LastName);
            $('#tbAddress1').val(User.Address1);
            $('#tbAddress2').val(User.Address2);
            $('#tbCity').val(UserArr[index].City);
            SetDdlValue($('#ddlState'), UserArr[index].State);
            $('#tbZip').val(UserArr[index].Zip);
            $('#tbPhoneNumber').val(UserArr[index].Phone);
            $('#tbFax').val(UserArr[index].Fax);
            $('#cbActive').val(UserArr[index].IsActive);
            $('#btnSubmit').val("Update Account");
            $('#btnSubmit')[0].onclick = UpdateUser;
        }

        function SetDdlValue(Element, Value) {
            Element.val(Value);
            //            for (i = 0; i < Element.length; i++) {
            //                if (Value == Element.options[i].value) {
            //                    Element.options[i].selected = true;
            //                    return;
            //                }
            //            }
        }


        function OnAddUserSuccess(result) {
            if (result.d) {
                //alert('Update user was successful!' + result);
                //alert('direct ' + result.UserName);
                TempHtml = "<a href='#' class='userName' onclick='UserSelected(" + UserArr.length + ")'>" +
                            result.d.UserName + "</a><br />";
                UserArr[UserArr.length] = result.d;
                divUsers.innerHTML += TempHtml;
            }
            else {
                //alert('we got nothing, boss!');
            }
        }

        function OnUpdateUserSuccess(result) {
            if (result.length > 0) {
                //alert('Update user was successful!' + result);
                //alert('direct ' + result.UserName);
                //alert('not direct ' + result[ResultCount].UserName);
            }
            else {
                //alert('we got nothing, boss!');
            }
        }

        function OnFailure(error) {
            alert(error.get_message());
        }

        //function AppInit(sender, args) {
        //    Sys.Net.WebServiceProxy.invoke("ClaimsAdminService.asmx", "GetUsersByContactTypeAssocID", false, {},
        //    OnInitSuccess, OnFailure, "User Context", 1000000);
        //}
        //Sys.Application.add_init(AppInit);

        //path, methodName, parameters, succeededCallback and failedCallback are all used
        //userContext, useHttpGet and timeout are just defaulting for now, until/if we need them in the future
        function ServiceInvoke(path, methodName, useHttpGet, parameters, succeededCallback, failedCallback, userContext, timeout) {
            //            var User = { 'type': 'AtlasObjects.ClaimsAdminSystemUser', 'UserID': 0, 'SystemUserID': 0,
            //                'AdjustingCompanyContactTypeAssocID': 0,
            //                'UserType': GetDdlValue('ddlUserType'), 'UserName': 'testusername',
            //                'LoginName': GetVal('tbLogin'), 'LoginPassword': GetVal('tbPassword'),
            //                'FirstName': GetVal('tbFirstName'), 'LastName': GetVal('tbLastName'),
            //                'EmailAddress': GetVal('tbEmail'), 'AddressType': "1",
            //                'Address1': GetVal('tbAddress1'), 'Address2': GetVal('tbAddress2'),
            //                'City': GetVal('tbCity'), 'State': GetDdlValue('ddlState'), 'Zip': GetVal('tbZip'),
            //                'Phone': GetVal('tbPhoneNumber'), 'Fax': GetVal('tbFax'),
            //                'IsActive': 'True', 'LastLoginDate': '01/01/1900'
            //            };
            if (typeof parameters !== "string") {
                var userText = JSON.stringify(parameters);
                parameters = "{'UserJSON':'" + userText + "'}";
            }

            $.ajax({
                type: "POST",
                url: path + "/" + methodName,
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function(e) { failedCallback(e.statusText); },
                success: function(e) { succeededCallback(e); }
            });
        }

        function Succeeded(response) {
            var Users = response.d;
            UserArr = Users;
            var divUsers = document.getElementById('divUsers');
            if (Users.length !== 0) {
                if (divUsers) {
                    var TempHtml;
                    if (Users.length !== 1) {
                        TempHtml = "<a href='#' class='userName' onclick='SetUpNewUser()'>New User</a><br />";
                    }
                    else {
                        //SetDdlValue($('#ddlUserType'), User.UserType);
                        TempHtml = '';
                        $('#ddlUserType')[0].style.visibility = "hidden";
                        $('#tdUserType')[0].style.visibility = "hidden";
                        $('#cbActive')[0].style.visibility = "hidden";
                        $('#lblActive')[0].style.visibility = "hidden";
                        UserSelected(0);
                    }
                    for (var ResultCount = 0; ResultCount < Users.length; ResultCount++) {
                        TempHtml += "<a href='#' class='userName' onclick='UserSelected(" + ResultCount + ")'>" +
                                            Users[ResultCount].UserName + "</a><br />";
                    }
                    divUsers.innerHTML = TempHtml;
                    TempHtml = null;
                }
            }
        }

        function Failed(e) {
            alert("Failure: " + e);
        }

        $(document).ready(function() {

            ServiceInvoke("ClaimsAdminService.asmx", "GetUsersByContactTypeAssocID", false, "{}",
            Succeeded, Failed, "userContext", 10000000);
        });
    </script>

</asp:Content>
