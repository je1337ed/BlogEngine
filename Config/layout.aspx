<%@ Page Language="C#" AutoEventWireup="true" CodeFile="layout.aspx.cs" Inherits="Config_layout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../styles/admin.css" />
    <style media="screen" type="text/css">
        </style>
</head>
<body>
    <div id="MainHeader">
        Skynet Software Site Admin Panel
    </div>
    <div class="MainWrapperMask MainWrapper">
        <div class="Main">
            <div id="MainContentWrapper" class="MainContentWrapper">
            </div>
            <div class="MainMenuWrapper">
                <!-- Column 2 start -->
                <div class="MenuSection">
                    <div class="MenuWidget">
                        <div class="MenuHeader">
                            Posts</div>
                        <div class="MenuItems">
                            <a href="#" onclick="AddBlogPostDraftForm();">Start Draft</a><br />
                            <a href="#" onclick="GetBlogPostDrafts();">View Drafts</a><br />
                            <a href="#" onclick="GetApprovedBlogPosts();">View Posts</a><br />
                            <a href="#" onclick="GetUnApprovedBlogComments();">Approve Comments</a><br />
                            <a href="#" onclick="GetApprovedBlogComments();">View Comments</a><br />
                        </div>
                    </div>
                    <div class="MenuWidget">
                        <div class="MenuHeader">
                            In Progress</div>
                        <div class="MenuItems">
                            <a href="#" onclick="AddInProgressDraftForm();">Start Draft</a><br />
                            <a href="#" onclick="GetInProgressDrafts();">View Drafts</a><br />
                            <a href="#" onclick="GetApprovedInProgressPosts();">View In Progress Posts</a><br />
                            <a href="#" onclick="GetUnApprovedInProgressComments();">Approve Comments</a><br />
                            <a href="#" onclick="GetApprovedInProgressComments();">View Comments</a><br />
                        </div>
                    </div>
                    <div class="MenuWidget">
                        <div class="MenuHeader">
                            Alerts</div>
                        <div class="MenuItems">
                            <a href="#" onclick="return false;">Add Alert</a><br />
                        </div>
                    </div>
                </div>
                <!-- Column 2 end -->
            </div>
        </div>
    </div>
    <div id="MainFooter">
        Skynet's BlogNet, Copyright 2009
    </div>
    <script type="text/javascript" src="../scripts/json2.js"></script>
    <script type="text/javascript" src="../scripts/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="../scripts/admin/admin.js"></script>
    <script type="text/javascript" src="../scripts/admin/adminPopulate.js"></script>
    <script type="text/javascript" src="../scripts/admin/LoadTemplates.js"></script>
</body>
</html>
