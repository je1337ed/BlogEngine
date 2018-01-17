(function() {
    var cache = {};
    this.tmpl = function tmpl(str, data) {
        // Figure out if we're getting a template, or if we need to
        // load the template - and be sure to cache the result.
        var fn = !/\W/.test(str) ?
      cache[str] = cache[str] ||
        tmpl(document.getElementById(str).innerHTML) :
        // Generate a reusable function that will serve as a template
        // generator (and which will be cached).
      new Function("obj",
        "var p=[],print=function(){p.push.apply(p,arguments);};" +

        // Introduce the data as local variables using with(){}
        "with(obj){p.push('" +

        // Convert the template into pure JavaScript
        str
          .replace(/[\r\t\n]/g, " ")
          .split("<#").join("\t")
          .replace(/((^|%>)[^\t]*)'/g, "$1\r")
          .replace(/\t=(.*?)#>/g, "',$1,'")
          .split("\t").join("');")
          .split("#>").join("p.push('")
          .split("\r").join("\\'")
      + "');}return p.join('');");
        // Provide some basic currying to the user
        return data ? fn(data) : fn;
    };
})();

function OnTemplateLoadedAndBind(Src, Id, Destination, Template, Data) {
    var newScript = document.createElement('script');
    newScript.type = 'text/html';
    newScript.text = Template;
    var headID = document.getElementsByTagName("head")[0];
    newScript.id = Id;
    headID.appendChild(newScript);
    var string = document.getElementById(newScript.id).innerHTML;
    $('#' + Destination).html(string);
}

function LoadTemplateFromURLAndBindObject(src, Id, Destination, ObjectId, callBackFunction) {
    $.get(src, function(Template) {
    callBackFunction(src, Id, Destination, Template, ObjectId);
    });
}

function LoadTemplateFromUrlAndBind(src, Id, Destination, Data, callBackFunction) {
    $.get(src, function(Template) {
        callBackFunction(src, Id, Destination, Template, Data);
    });
}

function OnInProgressDraftTemplateLoaded(Src, Id, Destination, Template) {
    var newScript = document.createElement('script');
    newScript.type = 'text/html';
    newScript.text = Template;
    var headID = document.getElementsByTagName("head")[0];
    newScript.id = Id;
    headID.appendChild(newScript);
    var string = document.getElementById(newScript.id).innerHTML;
    $('#' + Destination).html(string);
}

function OnEditInProgressPostDraftTemplateLoaded(Src, Id, Destination, Template, ObjectId) {
    var newScript = document.createElement('script');
    newScript.type = 'text/html';
    newScript.text = Template;
    var headID = document.getElementsByTagName("head")[0];
    newScript.id = Id;
    headID.appendChild(newScript);
    var string = document.getElementById(newScript.id).innerHTML;
    $('#' + Destination).html(string);

    for (var ResultCount = 0; ResultCount < InProgressPosts.length; ResultCount++) {
        if (InProgressPosts[ResultCount].Id == ObjectId) {
            $("#Title").val(InProgressPosts[ResultCount].Title);
            $("#Body").val(InProgressPosts[ResultCount].Body);
        }
    }
}

function OnEditBlogDraftTemplateLoaded(Src, Id, Destination, Template, ObjectId) {
    var newScript = document.createElement('script');
    newScript.type = 'text/html';
    newScript.text = Template;
    var headID = document.getElementsByTagName("head")[0];
    newScript.id = Id;
    headID.appendChild(newScript);
    var string = document.getElementById(newScript.id).innerHTML;
    $('#' + Destination).html(string);
    
    for (var ResultCount = 0; ResultCount < Posts.length; ResultCount++) {
        if (Posts[ResultCount].Id == ObjectId) {
            $("#Title").val(Posts[ResultCount].Title);
            $("#Description").val(Posts[ResultCount].Description);
            $("#Body").val(Posts[ResultCount].Body);
        }
    } 
}

function OnBlogDraftTemplateLoaded(Src, Id, Destination, Template) {
    var newScript = document.createElement('script');
    newScript.type = 'text/html';
    newScript.text = Template;
    var headID = document.getElementsByTagName("head")[0];
    newScript.id = Id;
    headID.appendChild(newScript);
    var string = document.getElementById(newScript.id).innerHTML;
    $('#' + Destination).html(string);
}

function OnInProgressCommentTemplateLoaded(Src, Id, Destination, Template, Data) {
    var newScript = document.createElement('script');
    newScript.type = 'text/html';
    newScript.text = Template;
    var headID = document.getElementsByTagName("head")[0];
    newScript.id = Id;
    headID.appendChild(newScript);
    var string = document.getElementById(newScript.id).innerHTML;
    InProgressComments = Data;
    $('#' + Destination).html(tmpl(newScript.id, InProgressComments));
}

function OnInProgressPostTemplateLoaded(Src, Id, Destination, Template, Data) {
    var newScript = document.createElement('script');
    newScript.type = 'text/html';
    newScript.text = Template;
    var headID = document.getElementsByTagName("head")[0];
    newScript.id = Id;
    headID.appendChild(newScript);
    var string = document.getElementById(newScript.id).innerHTML;
    InProgressPosts = Data;
    $('#' + Destination).html(tmpl(newScript.id, InProgressPosts));
}

function OnBlogPostTemplateLoaded(Src, Id, Destination, Template, Data) {
    var newScript = document.createElement('script');
    newScript.type = 'text/html';
    newScript.text = Template;
    var headID = document.getElementsByTagName("head")[0];
    newScript.id = Id;
    headID.appendChild(newScript);
    var string = document.getElementById(newScript.id).innerHTML;
    Posts = Data;
    $('#' + Destination).html(tmpl(newScript.id, Posts));
}

function OnBlogCommentTemplateLoaded(Src, Id, Destination, Template, Data) {
    var newScript = document.createElement('script');
    newScript.type = 'text/html';
    newScript.text = Template;
    var headID = document.getElementsByTagName("head")[0];
    newScript.id = Id;
    headID.appendChild(newScript);
    var string = document.getElementById(newScript.id).innerHTML;
    BlogPostComments = Data;
    $('#' + Destination).html(tmpl(newScript.id, BlogPostComments));
}

function OnTemplateLoaded(Src, Id, Destination, Template) {
    var newScript = document.createElement('script');
    newScript.type = 'text/html';
    newScript.text = Template;
    var headID = document.getElementsByTagName("head")[0];
    newScript.id = Id;
    headID.appendChild(newScript);
    var string = document.getElementById(newScript.id).innerHTML;
    $('#' + Destination).html(string);
}

function LoadTemplateFromURL(src, Id, Destination, callBackFunction) {
    $.get(src, function(Template) {
        callBackFunction(src, Id, Destination, Template);
    });
}