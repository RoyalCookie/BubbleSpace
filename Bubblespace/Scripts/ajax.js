/*
    TODO: This page requires javascript alert on noscript!
*/







// Main Functions
function friendsTab() {
    var friendslist = $("#list-view-items");
    friendslist.empty();
    addSearchFeature();
    $.post("/User/GetFriends", function (data) {
        for (var i = 0; i < data[0].length; i++) {
            friendslist.append(     
                  "<li class='list-item'>"
                + "<img src='/Images/Users/" + data[1][i] + "'/>"
                + "<a onclick='friendMain(\"" + data[2][i] + "\"); return false;' id='user-name'>" + data[0][i] + "</a></li>"
            );
        }
    })
}

function groupsTab() {
    var groupList = $("#list-view-items");
    groupList.empty();
    addSearchFeature();
    groupList.append("<li><a onclick='createGroupMain(); return false;' title='Create Group' class='create-button btn btn-default btn-sm'><span class='glyphicon glyphicon-plus'></span></a></li>");
    $.post("/Group/GetAllGroups", function (data) {
        for (var i = 0; i < data[0].length; i++) {
            groupList.append(
                  "<li class='list-item'>"
                + "<img src='/Images/Groups/" + data[1][i] + "'/>"
                + "<a onclick='groupMain(\"" + data[2][i] + "\"); return false;' id='group-name'>" + data[0][i] + "</a></li>"
            );
        }
    })
}

function createGroupMain() {
    var headView = $("#head-view");
    headView.empty();
    headView.append("<h1>Create Group</h1>")
    var mainView = $("#main-view");
    mainView.empty();
    mainView.append(
                 "<form method='post' action='/Group/Create' enctype='multipart/form-data'>"
               + "<label for='group-name'>Group Name</label>"
               + "<input type='text' class='form-control' id='groupName' name='group-name'>"
               + "<br>"
               + "<label for='group-description'>Group Description</label>"
               + "<textarea class='form-control' name='group-description'></textarea>"
               + "<input type='file' id='image-upload' name='contentImage' accept='image/*'>"
               + "<input type='submit' class='btn btn-default btn-create' value='Create'>"
               + "</form>"
            );

    // FileStyle: styles the file submit button.
    $(":file").filestyle({ input: false });
    $(":file").filestyle({ iconName: "glyphicon-inbox" });
    $(":file").filestyle('size', 'xs');
}

function createEventMain() {
    var headView = $("#head-view");
    headView.empty();
    headView.append("<h1>Create Event</h1>")
    var mainView = $("#main-view");
    mainView.empty();
    mainView.append(
                 "<form method='post' action='/Event/Create' enctype='multipart/form-data'>"
               + "<label for='event-name'>Event Name</label>"
               + "<input type='text' class='form-control' id='eventName' name='event-name'>"
               + "<br>"
               + "<label for='event-description'>Event Description</label>"
               + "<textarea class='form-control' name='event-description'></textarea>"
               + "<input type='file' id='image-upload' name='contentImage' accept='image/*'>"
               + "<input type='submit' class='btn btn-default btn-create' value='Create'>"
               + "</form>"
            );
    // FileStyle: styles the file submit button.
    $(":file").filestyle({ input: false });
    $(":file").filestyle({ iconName: "glyphicon-inbox" });
    $(":file").filestyle('size', 'xs');
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
       headView.append("<img class='profile-header-image' src='/Images/Groups/" + info[2] + "'/>");
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
                 + "<img class='post-profile-image' src='/Images/Users/" + info["profileImage"] + "' />"
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
    eventsList.append("<li><a onclick='createEventMain(); return false;' title='Create Event' class='create-button btn btn-default btn-sm'><span class='glyphicon glyphicon-plus'></span></a></li>");
    $.post("/Event/Events", function (events) {
        for (var i = 0; i < events[0].length; i++) {
            eventsList.append(
                  "<li class='list-item'>"
                + "<img src='/Images/Events/" + events[1][i] + "'/>"
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
        headView.append("<img class='profile-header-image' src='/Images/Events/" + data[2] + "'/>");
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
                  + "<img class='post-profile-image' src='/Images/Users/" + posts[2][i] + "' />"
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
            headView.append("<img class='profile-header-image' src='/Images/Users/" + data[1] + "'/>");
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
            headView.append("<img class='profile-header-image' src='/Images/Users/" + data["profileImage"] + "'/>");
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


// Start Of Chat Section


// Helper Functions

function appendMessageToView(view, time, sender, message) {
    view.append(
        "<li>"
        + "<p class='post-text'> Time:" + time + " - " + sender + ": " + message
        + "</p></li>"
    );
}

function updateOrCreateLastInsertId(id) {
    var lastMessageId = document.getElementById("lastMessageId");
    if (lastMessageId.length === 0) {
        mainView.append(
                "<input type=\"hidden\" name=\"lastMessageId\" id=\"lastMessageId\" value=\"" + id + "\">"
            );
        alert("lastMessageId Not Found");
    } else {
        lastMessageId.value = id;
        alert("lastMessageId Found");
    }
}

function sendMessage(chatId) {
    var message = document.getElementById("messageBox").value;

    console.log(chatId);
    console.log(message);
    var view = $("#main-view");
    $.ajax({
        method: "POST",
        url: "/Chat/Send",
        data: { chatId: chatId, message: message }
    }).success(function (message) {
        appendMessageToView(view, message["timeStamp"], message["sender"], message["message"]);
    });
}

// Main Functions

// User can get any chats by changing the buttons id for the chat

function chatTab() {
    var chatlist = $("#list-view-items");
   chatlist.empty();
    //addSearchFeature();
    $.post("/Chat/GetUserChats", function (data) {
        console.log(data);
        for (var i = 0; i < data["chatId"].length; i++) {
            chatlist.append(
                  "<li class='list-item'>"
                + "<a onclick='chatHead(\"" + data["chatId"][i] + "\"); return false;'>" + data["chatName"][i] + "</a></li>"
            );
        }
    })
}

function chatHead(id) {
    var chatUsers = $("#head-view");
    chatUsers.empty();

    $.ajax({
        method: "POST",
        url: "/Chat/GetChatUsers",
        data: { chatId: id }
    }).success(function (users) {
        console.log(users);
        for (var i = users["profileImage"].length - 1; i >= 0; i--) {
            chatUsers.append(
                    "<div class=\"col-md-3\">"
                  + "<img class='post-profile-image' src='/Images/Users/" + users["profileImage"][i] + "' />"
                  + "<div class='post-user-name'>" + users["userName"][i] + "</div>"
                  + "</p></div>"
              );
        }
        chatMain(id);
    });

    
}

function chatMain(id) {
    $.ajax({
        method: "POST",
        url: "/Chat/GetAllMessagesFromChat",
        data: { chatId: id }
    })
    .success(function (message) {
        var mainView = $("#main-view");
        mainView.empty();
        mainView.append(
                "<div id=\"chatBox\"></div>"
            );
        for (var i = message["sender"].length - 1; i >= 0; i--) {
            appendMessageToView(mainView, message["timeStamp"][i], message["sender"][i],  message["message"][i]);
        }
        if (message["id"].length > 0) {
            mainView.append(
                "<input type=\"hidden\" name=\"lastMessageId\" id=\"lastMessageId\" value=\"" + message["id"][0] + "\">"
            );
        }
        
        mainView.append(
                "<input type=\"text\" name=\"messageBox\" id=\"messageBox\">"
                +"<button type=\"button\" onClick=\"sendMessage(" + id + ")\">Click Me!</button>"
            );
        
    });
}

function addSearchFeature() {
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