/*
    TODO: This page requires javascript alert on noscript!
*/

function friendsTab() {
    var friendslist = $("#list-view-items");
    friendslist.empty();
    $.post("/User/Friends", function (data) {
        console.log(data);
        for (var i = 0; i < data[0].length; i++) {
            
            friendslist.append(
                  "<li class='list-item'>"
                + "<img src='/Content/Assets/" + data[1][i] + ".png'/>"
                + "<a onclick='friendMain(\""+ data[2][i] +"\"); return false;' id='user-name' data-val='" + data[2][i] + "'>" + data[0][i] + "</a></li>"
            );
        }
    })
}

function friendMain(id) {
    console.log(id);

    $.ajax({
        method: "POST",
        url: "/User/GetUserInformation",
        data: { userId: id }
    })
   .success(function( info ) {
       var mainView = $("#main-view");
       mainView.empty();
       console.log(info);
       for (var i = info["posts"].length - 1; i >= 0; i--) {
           mainView.append(
                   "<li class='feed-post'>"
                 + "<img class='post-profile-image' src='/Content/Assets/" + info["profileImage"] + ".png' />"
                 + "<div class='post-user-name'>" + info["userName"] + "</div>"
                 + "<p class='post-text'>" + info["posts"][i]
                 + "</p></li>"
             );
           mainView.append(
                 "<i class='fa fa-thumbs-up'></i>"
               + "<i class='fa fa-thumb-tack'></i>"
               + "<i class='fa fa-comment'></i>"
           );
       }
   });
}

function eventsTab() {
    var eventsList = $("#list-view-items");
    eventsList.empty();
    $.post("/Event/Events", function (events) {
        for (var i = 0; i < events[0].length; i++) {
            eventsList.append(
                  "<li class='list-item'>"
                + "<img src='/Content/Assets/" + events[1][i] + ".png'/>"
                + events[0][i] + "</li>"
            );
        }
    })
}

function newsFeed() {
    var mainView = $("#main-view");
    mainView.empty();
    $.post("/Post/GetAllPosts", function (posts) {
        console.log(posts);
        for (var i = posts[0].length - 1; i >= 0; i--) {
            mainView.append(
                    "<li class='feed-post'>"
                  + "<img class='post-profile-image' src='/Content/Assets/" + posts[2][i] + ".png' />"
                  + "<div class='post-user-name'>" + posts[0][i] + "</div>"
                  + "<p class='post-text'>" + posts[1][i]
                  + "</p></li>"
              );
            mainView.append(
                  "<i class='fa fa-thumbs-up'></i>"
                + "<i class='fa fa-thumb-tack'></i>"
                + "<i class='fa fa-comment'></i>"
            );
        }
    })
    mainView.perfectScrollbar();
    Ps.initialize(document.getElementById('main-view'));
}

function newPost(type) {
    var headView = $("#head-view");
    headView.empty();
    headView.append(
        "<form class='new-post' method='post' action='/Post/Create' enctype='multipart/form-data'>"
            + "<textarea id='content_text' name='content_text' rows='3' cols='40'></textarea><br />"
            + "<input type='submit' class='btn btn-default' value='Post' />"
            + "<input type='file' data-iconName='glyphicon-inbox' name='contentImage' accept='image/*'>"
        + "</form>"
    );
    // FileStyle: styles the file submit button.
    $(":file").filestyle({ input: false });
    $(":file").filestyle({ iconName: "glyphicon-inbox" });
    $(":file").filestyle('size', 'xs');

}

$(document).ready(function () {
    friendsTab();
    newsFeed();
    newPost("newsFeed");
});