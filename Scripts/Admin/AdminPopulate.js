var Posts;
var BlogPostComments;
var InProgressPosts;
var InProgressComments;

var EditPost = null;
var EditInProgressPost = null;

/* blog post start */
function AddBlogPostDraftFormFinished(Id) {
    var string = document.getElementById(Id).innerHTML;
    $('#MainContentWrapper').html(string);
}

function PopulateBlogPostDraftForm() {
    $('#MainContentWrapper').html($('#AddPostDraft').html());
}

function AddInProgressDraftForm() {
    var src = "../Templates/InProgressPostForm.htm";
    var Id = "PostDraftTemplate";
    LoadTemplateFromURL(src, Id, 'MainContentWrapper', OnInProgressDraftTemplateLoaded);
}

function OnInsertPostSuccess(result) {
    alert('success' + result);
    $('#MainContentWrapper').html("");
}

function AddBlogPostDraftForm() {
    var src = "../Templates/BlogPostForm.htm";
    var Id = "PostDraftTemplate";
    LoadTemplateFromURL(src, Id, 'MainContentWrapper', OnBlogDraftTemplateLoaded);
}

function AddEditInProgressPostDraftForm(InProgressPostId) {
    var src = "../Templates/InProgressPostForm.htm";
    var Id = "InProgressPostDraftTemplate";
    for (var ResultCount = 0; ResultCount < InProgressPosts.length; ResultCount++) {
        if (InProgressPosts[ResultCount].Id == InProgressPostId) {
            EditInProgressPost = InProgressPosts[ResultCount];
        }
    }
    //LoadTemplateFromURLAndBindObject(src, Id, Destination, Data, ObjectId, callBackFunction) {
    LoadTemplateFromURLAndBindObject(src, Id, 'MainContentWrapper', InProgressPostId, OnEditInProgressPostDraftTemplateLoaded);
}

function AddEditBlogPostDraftForm(PostId) {
    var src = "../Templates/BlogPostForm.htm";
    var Id = "PostDraftTemplate";
    for (var ResultCount = 0; ResultCount < Posts.length; ResultCount++) {
        if (Posts[ResultCount].Id == PostId) {
            EditPost = Posts[ResultCount];
        }
    } 
    //LoadTemplateFromURLAndBindObject(src, Id, Destination, Data, ObjectId, callBackFunction) {
    LoadTemplateFromURLAndBindObject(src, Id, 'MainContentWrapper', PostId, OnEditBlogDraftTemplateLoaded);
}

function AddBlogPostDraftForm() {
    var src = "../Templates/BlogPostForm.htm";
    var Id = "PostDraftTemplate";
    LoadTemplateFromURL(src, Id, 'MainContentWrapper', OnBlogDraftTemplateLoaded);
}

 function PopulateBlogPosts(BlogPosts) {
    if (BlogPosts) {
        if (BlogPosts.length > 0) {
            LoadTemplateFromUrlAndBind('../Templates/BlogPostList.htm', 'PostList', 'MainContentWrapper',
        BlogPosts, OnBlogPostTemplateLoaded);
        }
        else { $('#MainContentWrapper').html(""); }
    }
}

function PopulateBlogPostDrafts(BlogPosts) {
    if (BlogPosts) {
        if (BlogPosts.length > 0) {
            LoadTemplateFromUrlAndBind('../Templates/BlogPostList.htm', 'PostList', 'MainContentWrapper',
        BlogPosts, OnBlogPostTemplateLoaded);
        }
        else { $('#MainContentWrapper').html(""); }
    }
}
/* blog post end */

/* blog post comment start */
function PopulateUnapprovedBlogComments(BlogPostComments) {
    if (BlogPostComments) {
        if (BlogPostComments.length > 0) {
            LoadTemplateFromUrlAndBind('../Templates/BlogPostCommentList.htm', 'PostCommentList', 'MainContentWrapper',
        BlogPostComments, OnBlogCommentTemplateLoaded);
        }
        else { $('#MainContentWrapper').html(""); }
    }
}

function PopulateApprovedBlogComments(BlogPostComments) {
    if (BlogPostComments) {
        if (BlogPostComments.length > 0) {
            LoadTemplateFromUrlAndBind('../Templates/BlogPostCommentList.htm', 'PostCommentList', 'MainContentWrapper',
        BlogPostComments, OnBlogCommentTemplateLoaded);
        }
        else { $('#MainContentWrapper').html(""); }
    }
}
/* blog post comment end */


/* In Progress post start */
function OnInsertInProgressPostSuccess(result) {
    alert('success' + result);
    $('#MainContentWrapper').html(""); 
}

function PopulateInProgressDrafts(InProgressPosts) {
    if (InProgressPosts) {
        if (InProgressPosts.length > 0) {
            LoadTemplateFromUrlAndBind('../Templates/InProgressPostList.htm', 'InProgressPostList', 'MainContentWrapper',
        InProgressPosts, OnInProgressPostTemplateLoaded);
        }
        else { $('#MainContentWrapper').html(""); }
    }
}

function PopulateInProgressPosts(InProgressPosts) {
    if (InProgressPosts) {
        if (InProgressPosts.length > 0) {
            LoadTemplateFromUrlAndBind('../Templates/InProgressPostList.htm', 'InProgressPostList', 'MainContentWrapper',
        InProgressPosts, OnInProgressPostTemplateLoaded);
        }
        else { $('#MainContentWrapper').html(""); }
    }
}
/* In Progress post end */


/* In Progress comments start */
function PopulateUnApprovedInProgressComments(InProgressComments) {
    if (InProgressComments) {
        if (InProgressComments.length > 0) {
            LoadTemplateFromUrlAndBind('../Templates/InProgressComment.htm', 'InProgressCommentList', 'MainContentWrapper',
        InProgressComments, OnInProgressCommentTemplateLoaded);
        }
        else { $('#MainContentWrapper').html(""); }
    }
    else { $('#MainContentWrapper').html(""); }
}

function PopulateApprovedInProgressComments(InProgressComments) {
    if (InProgressComments) {
        if (InProgressComments.length > 0) {
            LoadTemplateFromUrlAndBind('../Templates/InProgressComment.htm', 'InProgressCommentList', 'MainContentWrapper',
        InProgressComments, OnInProgressCommentTemplateLoaded);
        }
        else { $('#MainContentWrapper').html(""); }
    }
    else { $('#MainContentWrapper').html(""); }
}
/* In Progress comments end */

/* assorted start */
function AddTemplate(ElementId, Template, Data) {
    $('#' + ElementId).html(tmpl(Template, Data));
}
/* assorted end */