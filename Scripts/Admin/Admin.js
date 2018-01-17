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
        error: function(result) { failedCallback(result); },
        success: function(result) { succeededCallback(result.d); }
    });
}

function OnFailure(result) {
    alert('failure' + result);
}

function OnActivateSuccess(result) {
    alert('success' + result);
}

function OnDeactivateSuccess(result) {
    alert('success' + result);
}

/* In Progress Posts start */
function DeactivateInProgressPost(InProgressPostId) {
    ServiceInvoke("../AdminService.asmx", "DeactivateInProgressPost", false, { "InProgressPostId": InProgressPostId },
    OnDeactivateSuccess, OnFailure, "User Context", 1000000);
}

function ActivateInProgressPost(InProgressPostId) {
    ServiceInvoke("../AdminService.asmx", "ActivateInProgressPost", false, { "InProgressPostId": InProgressPostId },
    OnActivateSuccess, OnFailure, "User Context", 1000000);
}

function SubmitEditInProgresssPost(InProgressPostId) {
    for (var ResultCount = 0; ResultCount < InProgressPosts.length; ResultCount++) {
        if (InProgressPosts[ResultCount].Id == InProgressPostId) {
            var InProgressPost = {};
            InProgressPost.Id = InProgressPostId;
            InProgressPost.Title = $("#Title").val();
            InProgressPost.Body = $("#Body").val();
            InProgressPost.DatePublished = "";
            InProgressPost.DateModified = "";
            InProgressPost.Active = false;
            // Create a data transfer object (DTO) with the proper structure.
            var DTO = { 'Post': InProgressPost };
            ServiceInvoke("../adminservice.asmx", "EditInProgressPost", true, DTO, OnInsertInProgressPostSuccess,
        OnFailure, "User Context", 1000000);
        }
    }
}

function SubmitInProgressDraft() {
    if (EditInProgressPost != null) {
        if (EditInProgressPost.Id) {
            SubmitEditInProgresssPost(EditInProgressPost.Id);
            return;
        }
    }
     // Initialize the object, before adding data to it.
    //  { } is declarative shorthand for new Object().
    var InProgressPost = {};
    InProgressPost.Id = 0;
    InProgressPost.Title = $("#Title").val();
    InProgressPost.Body = $("#Body").val();
    InProgressPost.DatePublished = "";
    InProgressPost.DateModified = "";
    InProgressPost.Active = false;
    // Create a data transfer object (DTO) with the proper structure.
    var DTO = { 'Post': InProgressPost };
    ServiceInvoke("../adminservice.asmx", "InsertInProgressDraft", true, DTO, OnInsertInProgressPostSuccess,
        OnFailure, "User Context", 1000000);
}

function GetInProgressDrafts() {
    var DTO = "{}";
    ServiceInvoke("../adminservice.asmx", "GetInProgressDrafts", true, DTO, PopulateInProgressDrafts,
                          OnFailure, "User Context", 1000000);
}

function GetApprovedInProgressPosts() {
    var DTO = "{}";
    ServiceInvoke("../adminservice.asmx", "GetApprovedInProgressPosts", true, DTO, PopulateInProgressPosts,
                          OnFailure, "User Context", 1000000);
}
/* In Progress Posts end */

/* In Progress Comments start */
//DeactivateInProgressComment(<#=DeactivateInProgressComment[ResultCount].Id#>)
//ActivateInProgressComment(<#=ActivateInProgressComment[ResultCount].Id#>)

function DeactivateInProgressComment(InProgressCommentId) {
    ServiceInvoke("../AdminService.asmx", "DeactivateInProgressComment", false, { "InProgressCommentId": InProgressCommentId },
    OnDeactivateSuccess, OnFailure, "User Context", 1000000);
}

function ActivateInProgressComment(InProgressCommentId) {
    ServiceInvoke("../AdminService.asmx", "ActivateInProgressComment", false, { "InProgressCommentId": InProgressCommentId },
    OnActivateSuccess, OnFailure, "User Context", 1000000);
}

function GetUnApprovedInProgressComments() {
    var DTO = "{}";
    ServiceInvoke("../adminservice.asmx", "GetUnApprovedInProgressComments", true, DTO, PopulateUnApprovedInProgressComments,
                          OnFailure, "User Context", 1000000);
}

function GetApprovedInProgressComments() {
    var DTO = "{}";
    ServiceInvoke("../adminservice.asmx", "GetApprovedInProgressComments", true, DTO, PopulateApprovedInProgressComments,
                          OnFailure, "User Context", 1000000);
}
/* In Progress Comments end */

/* Blog Posts start */
//DeactivateBlogPost(<#=Posts[ResultCount].Id#>)
//ActivateBlogPost(<#=Posts[ResultCount].Id#>)
function DeactivateBlogPost(BlogPostId) {
    ServiceInvoke("../AdminService.asmx", "DeactivateBlogPost", false, { "BlogPostId": BlogPostId },
    OnDeactivateSuccess, OnFailure, "User Context", 1000000);
}

function ActivateBlogPost(BlogPostId) {
    ServiceInvoke("../AdminService.asmx", "ActivateBlogPost", false, { "BlogPostId": BlogPostId },
    OnActivateSuccess, OnFailure, "User Context", 1000000);
}

function GetBlogPostDrafts() {
    var DTO = "{}";
    ServiceInvoke("../adminservice.asmx", "GetBlogPostDrafts", true, DTO, PopulateBlogPostDrafts,
                          OnFailure, "User Context", 1000000);
}

function GetApprovedBlogPosts() {
    var DTO = "{}";
    ServiceInvoke("../adminservice.asmx", "GetApprovedBlogPosts", true, DTO, PopulateBlogPosts,
        OnFailure, "User Context", 1000000);
}

function SubmitEditBlogPost(PostId) {
    for (var ResultCount = 0; ResultCount < Posts.length; ResultCount++) {
        if (Posts[ResultCount].Id == PostId) {
            var BlogPost = {};
            BlogPost.Id = PostId;
            BlogPost.Title = $("#Title").val(); //Posts[ResultCount].Title
            BlogPost.Description = $("#Description").val(); //Posts[ResultCount].Description
            BlogPost.Body = $("#Body").val(); //Posts[ResultCount].Body
            BlogPost.Link = ""; //Posts[ResultCount].Link
            BlogPost.DatePublished = ""; //Posts[ResultCount].DatePublished
            BlogPost.DateModified = ""; //Posts[ResultCount].DateModified
            BlogPost.Active = false;
            // Create a data transfer object (DTO) with the proper structure.
            var DTO = { 'Post': BlogPost };
            //InsertBlogPost(string Title, string Description, string Body, string Link)
            ServiceInvoke("../adminservice.asmx", "EditBlogPost", true, DTO, OnInsertPostSuccess,
        OnFailure, "User Context", 1000000);

        }
    }
}

function SubmitBlogPostDraft() {
    // Initialize the object, before adding data to it.
    //  { } is declarative shorthand for new Object().
    if (EditPost != null) {
        if (EditPost.Id) {
            SubmitEditBlogPost(EditPost.Id);
            return;
        }
    }
    var BlogPost = {};
    BlogPost.Id = 0;
    BlogPost.Title = $("#Title").val();
    BlogPost.Description = $("#Description").val();
    BlogPost.Body = $("#Body").val();
    BlogPost.Link = "";
    BlogPost.DatePublished = "";
    BlogPost.DateModified = "";
    BlogPost.Active = false;
    // Create a data transfer object (DTO) with the proper structure.
    var DTO = { 'Post': BlogPost };
    //InsertBlogPost(string Title, string Description, string Body, string Link)
    ServiceInvoke("../adminservice.asmx", "InsertBlogPostDraft", true, DTO, OnInsertPostSuccess,
        OnFailure, "User Context", 1000000);
}

/* Blog Posts end */

/* Blog Post Comments start */
//DeactivateBlogPostComment(<#=DeactivateBlogPostComments[ResultCount].Id#>)
//ActivateBlogPostComment(<#=ActivateBlogPostComments[ResultCount].Id#>)
function DeactivateBlogPostComment(BlogPostCommentId) {
    ServiceInvoke("../AdminService.asmx", "DeactivateBlogPostComment", false, { "BlogPostCommentId": BlogPostCommentId },
    OnDeactivateSuccess, OnFailure, "User Context", 1000000);
}

function ActivateBlogPostComment(BlogPostCommentId) {
    ServiceInvoke("../AdminService.asmx", "ActivateBlogPostComment", false, { "BlogPostCommentId": BlogPostCommentId },
    OnActivateSuccess, OnFailure, "User Context", 1000000);
}

function GetApprovedBlogComments() {
    var DTO = "{}";
    ServiceInvoke("../adminservice.asmx", "GetApprovedBlogComments", true, DTO, PopulateApprovedBlogComments,
                          OnFailure, "User Context", 1000000);
}

function GetUnApprovedBlogComments() {
    var DTO = "{}";
    ServiceInvoke("../adminservice.asmx", "GetUnApprovedBlogComments", true, DTO, PopulateUnapprovedBlogComments,
                          OnFailure, "User Context", 1000000);
}
/* Blog Post Comments end */