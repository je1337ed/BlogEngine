<%@ Page Language="C#" AutoEventWireup="true" Inherits="SimpleRoutingTest.RoutablePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<base href="<%= BaseUrl %>" />
    <title>Simple ASP.NET Routing Test (in subfolder; no Master Page)</title>
	<style type="text/css">
		@import "Css/TestStyle.css";
	</style>
    <script src="Javascript/TestScript.js" type="text/javascript"></script>
</head>
<body onload="TestMethod( document.getElementById( 'outputDiv' ) )">
    <form id="form1" runat="server">
		<div>
			<div><h1>"Detail" Route (in subfolder; no Master Page)</h1></div>

			<div>Category: <%= RouteValue( "category" ) %></div>
			<div>ID: <%= RouteValue( "id" ) %></div>

			<hr />

			<div>
				Some sample URLs to try:
				<blockquote>
					<div>"Simple" route: <%= ActionLink( GetVirtualPath( new { value = 444 } ) ) %></div>
					<div>"Search" route: <%= ActionLink( GetVirtualPath( "search", new { category = 14 } ) ) %></div>
					<div>"Detail" route: <%= ActionLink( GetVirtualPath( "details", new { category = 14, id = 42 } ) ) %></div>
					<div>"Detail" route (no Master Page): <%= ActionLink( GetVirtualPath( "noMaster", new { category = 14, id = 42 } ) ) %></div>
					<div>"Detail" route (in subfolder; no Master Page): <%= ActionLink( GetVirtualPath( "subNoMaster", new { category = 14, id = 42 } ) ) %></div>
				</blockquote>
			</div>
		</div>
		<div id="outputDiv" class="testClass">This text will be overwritten by TestMethod()</div>
    </form>
</body>
</html>