/*
    TODO: This page requires javascript alert on noscript!
*/
function friendsTab() {
    var friendslist = $("#list-view-items");
    friendslist.empty();
    addSearchFeature();
    $.post("/User/GetFriends", function (data) {
        for (var i = 0; i < data[0].length; i++) {
            friendslist.append(     
                  "<li class='list-item'>"
                + "<img src='/Content/Assets/" + data[1][i] + ".png'/>"
                + "<a onclick='friendMain(\"" + data[2][i] + "\"); return false;' id='user-name'>" + data[0][i] + "</a></li>"
            );
        }
    })
}

function groupsTab() {
    var groupList = $("#list-view-items");
    groupList.empty();
    addSearchFeature();
    groupList.append("<li><a onclick='createGroup(); return false;' title='Create Group' class='create-button btn btn-default btn-sm'><span class='glyphicon glyphicon-plus'></span></a></li>");
    $.post("/Group/GetAllGroups", function (data) {
        for (var i = 0; i < data[0].length; i++) {
            groupList.append(
                  "<li class='list-item'>"
                + "<img src='/Content/Assets/" + data[1][i] + ".png'/>"
                + "<a onclick='groupMain(\"" + data[2][i] + "\"); return false;' id='group-name'>" + data[0][i] + "</a></li>"
            );
        }
    })
}

function createGroup() {
    var headView = $("#head-view");
    headView.empty();
    headView.append("<h1>Create Group</h1>")
    var mainView = $("#main-view");
    mainView.empty();
    mainView.append(
                 "<form>"
               + "<label for='group-name'>Group Name</label>"
               + "<input type='text' class='form-control' name='group-name'>"
               + "<br>"
               + "<label for='group-description'>Group Description</label>"
               + "<textarea class='form-control' name='group-description'></textarea>"
               + "<input type='file' id='image-upload' name='contentImage' accept='image/*'>"
               + "<button type='button' onclick='createGroup(); return false;' id='create-submit' class='btn btn-default'>Submit</button>"    
               + "</form>"
            );

    // FileStyle: styles the file submit button.
    $(":file").filestyle({ input: false });
    $(":file").filestyle({ iconName: "glyphicon-inbox" });
    $(":file").filestyle('size', 'xs');
}

function createGroup() {

}

function groupMain(id) {
    $.ajax({
        method: "POST",
        url: "/Group/GetGroupById",
        data: { groupId: id }
    })
   .success(function (info) {
       console.log(info);
       newPost("groupPage");
       var headView = $("#head-view");       
       headView.append("<img class='profile-header-image' src='/Content/Assets/" + info[2] + ".png'/>");
       headView.append("<h1 class='profile-header'>" + info[0] + "</h1>");
       headView.append("<p class='profile-description'>" + info[1] + "</p>");
       var mainView = $("#main-view");
       mainView.empty();

   });
}

function friendMain(id) {
    $.ajax({
        method: "POST",
        url: "/User/GetUserInformation",
        data: { userId: id }
    })
   .success(function( info ) {
       var mainView = $("#main-view");
       mainView.empty();
       for (var i = info["posts"].length - 1; i >= 0; i--) {
           mainView.append(
                   "<li class='feed-post'>"
                 + "<img class='post-profile-image' src='/Content/Assets/" + info["profileImage"] + ".png' />"
                 + "<div class='post-user-name'><a onclick='friendMain(\"" + info["Id"] + "\"); return false;'>" + info["userName"] + "</a></div>"
                 + "<p class='post-text'>" + info["posts"][i]
                 + "</p></li>"
             );
           mainView.append(
                 "<i class='fa fa-thumbs-up'></i>"
               + "<i class='fa fa-thumb-tack'></i>"
               + "<i class='fa fa-comment'></i>"
           );
       }
       newPost("friendPage", info["Id"]);
   });
}

function eventsTab() {
    var eventsList = $("#list-view-items");
    eventsList.empty();
    addSearchFeature();
    $.post("/Event/Events", function (events) {
        for (var i = 0; i < events[0].length; i++) {
            eventsList.append(
                  "<li class='list-item'>"
                + "<img src='/Content/Assets/" + events[1][i] + ".png'/>"
                + "<div class='post-user-name'><a onclick='eventMain(\"" + events[2][i] + "\"); return false;'>" + events[0][i] + "</a></div>"
            );
        }
    })
}

function eventMain(id) {
    $.ajax({
        method: "POST",
        url: "/Event/GetEventById",
        data: { eventId: id }
    })
    .success(function (data) {
        var headView = $("#head-view");
        headView.empty();
        headView.append("<img class='profile-header-image' src='/Content/Assets/" + data[2] + ".png'/>");
        headView.append("<h1 class='profile-header'>" + data[0] + "</h1><br>");
        headView.append("<p class='profile-description'>" + data[1] + "</p>");
        headView.append("<p class='profile-description-time'>From: &nbsp&nbsp" + data[3].substring(0, 10) + "</p>");
        headView.append("<p class='profile-description-time'>To: &nbsp&nbsp&nbsp&nbsp&nbsp " + data[4].substring(0, 10) + "</p>");
        $("#main-view").empty();
    });
}

function newsFeed() {
    var mainView = $("#main-view");
    mainView.empty();
    $.post("/Post/GetAllUserPosts", function (posts) {
        for (var i = posts[0].length - 1; i >= 0; i--) {
            mainView.append(
                    "<li class='feed-post'>"
                  + "<img class='post-profile-image' src='/Content/Assets/" + posts[2][i] + ".png' />"
                  + "<div class='post-user-name'><a onclick='friendMain(\"" + posts[3][i] + "\"); return false;'>" + posts[0][i] + "</a></div>"
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

function newPost(type, id) {
    var headView = $("#head-view");
    
    if (type == "newsFeed") {
        headView.empty();
        headView.append(
        "<form class='new-post' method='post' action='/Post/Create' enctype='multipart/form-data'>"
            + "<textarea id='content_text' class='form-control' name='content_text' rows='3' cols='40'></textarea><br />"
            + "<input type='submit' class='btn btn-default' value='Post' />"
            + "<input type='file' data-iconName='glyphicon-inbox' name='contentImage' accept='image/*'>"
            + "</form>"
        );
        // FileStyle: styles the file submit button.
        $(":file").filestyle({ input: false });
        $(":file").filestyle({ iconName: "glyphicon-inbox" });
        $(":file").filestyle('size', 'xs');

        $.post("/User/GetLoggedInUserInfo", function (data) {
            headView.append("<img class='profile-header-image' src='/Content/Assets/" + data[1] + ".png'/>");
            headView.append("<h1 class='profile-header'>" + data[0] + "</h1>");
        });
    }
    else if (type == "friendPage") {
        $.ajax({
            method: "POST",
            url: "/User/GetUserInformation",
            data: { userId: id }
        })
        .success(function (data) {
            headView.empty();
            headView.append("<img class='profile-header-image' src='/Content/Assets/" + data["profileImage"] + ".png'/>");
            headView.append("<h1 class='profile-header'>" + data["userName"] + "</h1>");
        });
    }
    else if (type == "groupPage") {
        headView.empty();
        headView.append(
       "<form class='new-post' method='post' action='/Post/Create' enctype='multipart/form-data'>"
           + "<textarea id='content_text' class='form-control' name='content_text' rows='3' cols='40'></textarea><br />"
           + "<input type='submit' class='btn btn-default' value='Post' />"
           + "<input type='file' data-iconName='glyphicon-inbox' name='contentImage' accept='image/*'>"
           + "</form>"
       );
        // FileStyle: styles the file submit button.
        $(":file").filestyle({ input: false });
        $(":file").filestyle({ iconName: "glyphicon-inbox" });
        $(":file").filestyle('size', 'xs');
    }
}

function chatTab() {
    var friendslist = $("#list-view-items");
    friendslist.empty();
    //addSearchFeature();
    $.post("/Chat/GetUserChats", function (data) {
        console.log(data);
        for (var i = 0; i < data["chatId"].length; i++) {
            friendslist.append(
                  "<li class='list-item'>"
                + "<a onclick='chatMain(\"" + data["chatId"][i] + "\"); return false;'>" + data["chatName"][i] + "</a></li>"
            );
        }
    })
}

function chatHead(id) {
    
}

function chatMain(id) {
    $.ajax({
        method: "POST",
        url: "/User/GetUserInformation",
        data: { userId: id }
    })
    .success(function (info) {
        var mainView = $("#main-view");
        mainView.empty();
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

function addSearchFeature(type) {
    var list = $("#list-view-items");
    list.empty();
    list.append(
          "<li>"
        + "<input type='text' id='search-bar' class='form-control' placeholder='search..'/>"
        + "</li>"
    );
    document.getElementById("search-bar").onkeyup = function (event) {
        console.log($("#search-bar").val());
    }
}

function refresh() {    
    friendsTab();
    newsFeed();
    newPost("newsFeed");
}

$(document).ready(function () {
    refresh();
});