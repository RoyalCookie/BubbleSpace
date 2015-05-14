/*
    TODO: This page requires javascript alert on noscript!

    TODO: Show user posted to group on the newsfeed!
        
    MAYBE: replace datepicker with http://xdsoft.net/jqplugins/datetimepicker/
*/


// Main Functions
function friendsTab() {
    var friendslist = $("#list-view-items");
    friendslist.empty();
    $.post("/User/GetFriends", function (data) {
        for (var i = 0; i < data[0].length; i++) {
            friendslist.append(     
                  "<li class='list-item'>"
                + "<img src='/Images/Users/" + data[1][i] + "'/>"
                + "<a onclick='friendMain(\"" + data[2][i] + "\"); return false;' class='list-name'>" + data[0][i] + "</a></li>"
            );
        }
    })
}

function groupsTab() {
    var groupList = $("#list-view-items");
    groupList.empty();
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
               + "<p>From: <input type='text' id='datepickerFrom' name='start-time'></p>"
               + "<p>To: <input type='text' id='datepickerTo' name='end-time'></p>"
               + "<input type='submit' class='btn btn-default btn-create' value='Create'>"
               + "</form>"
            );
    // FileStyle: styles the file submit button.
    $(":file").filestyle({ input: false });
    $(":file").filestyle({ iconName: "glyphicon-inbox" });
    $(":file").filestyle('size', 'xs');
    $("#datepickerFrom").datepicker();
    $("#datepickerTo").datepicker();
}

function groupMain(id) {
    $.ajax({
        method: "POST",
        url: "/Group/GetGroupById",
        data: { groupId: id }
    })
   .success(function (info) {
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
                 + "<p class='post-text'>" + info["posts"][i] + "</p>"
                 + "</li>"
             );
           mainView.append(
                 "<div class='post-feedback'><i class='fa fa-thumbs-up'></i>"
               + "<i class='fa fa-thumb-tack'></i>"
               + "<i class='fa fa-comment'></i></div>"
           );
       }
       newPost("friendPage", info["Id"]);
   });
}

function eventsTab() {
    var eventsList = $("#list-view-items");
    eventsList.empty();
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
        console.log(posts);
        for (var i = posts[0].length - 1; i >= 0; i--) {
            mainView.append(
                    "<li class='feed-post'>"
                  + "<img class='post-profile-image' src='/Images/Users/" + posts[2][i] + "' />"
                  + "<div class='post-user-name'><a onclick='friendMain(\"" + posts[3][i] + "\"); return false;'>" + posts[0][i] + "</a></div>"
                  + "<p class='post-text'>" + posts[1][i]
                  + "</p></li>"
              );
            mainView.append(
                  "<div class='post-feedback'>"
                + posts[5][i]
                + "<i onclick=\"likePost(" + posts[4][i] + "); return false;\" class='fa fa-thumbs-up'></i>"
                + "<i class='fa fa-thumb-tack'></i>"
                + "<i class='fa fa-comment'></i></div>"
            );
        }
    })
    mainView.perfectScrollbar();
    Ps.initialize(document.getElementById('main-view'));
}

function likePost(id) {
    $.ajax({
        method: "POST",
        url: "/Post/LikePost",
        data: { postId: id }
    })
    .success(function (data) {

    });
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
        + "<p class='post-text'> " + sender + ": " + message
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
    var view = $("#chatBox");
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
    $.post("/Chat/GetUserChats", function (data) {
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

        var chatBox = $("#chatBox");
        if (chatBox === 0) {
            console.log("Chat is empty");
        } else {
            console.log("Chat aint empty");
        }
        for (var i =0; i < message["sender"].length; i++) {
            appendMessageToView(chatBox, message["timeStamp"][i], message["sender"][i],  message["message"][i]);
        }
        if (message["id"].length > 0) {
            chatBox.append(
                "<input type=\"hidden\" name=\"lastMessageId\" id=\"lastMessageId\" value=\"" + message["id"][0] + "\">"
            );
        }
        
        mainView.append(
                "<input type=\"text\" name=\"messageBox\" id=\"messageBox\">"
                +"<button type=\"button\" onClick=\"sendMessage(" + id + ")\">Click Me!</button>"
            );
    });
}

$(function() {
    document.getElementById("search-bar").onkeyup = function (event) {
        if (!$("#search-bar").val() == "") {
            $.ajax({
                method: "POST",
                url: "/Search/GetResults",
                data: { search_string: $("#search-bar").val() }
            })
            .success(function (results) {
                var searchList = $("#list-view-items");
                searchList.empty();
                searchList.append("<li>Users</li>")
                if (results[0][0].length == 0) {
                    searchList.append("<li class='list-item'>Nothing Found</li>")
                }
                else {
                    for (var i = 0; i < results[0][0].length; i++) {
                        searchList.append(
                              "<li class='list-item'>"
                            + "<img src='/Images/Users/" + results[0][1][i] + "'/>"
                            + "<a onclick='friendMain(\"" + results[0][2][i] + "\"); return false;' class='list-name'>" + results[0][0][i] + "</a></li>"
                            + "<div id='friendrequest-icon-" + results[0][2][i] + "'><img onclick='followUser(\""+ results[0][2][i] +"\")' title='Add Friend' class='add-friend-img' src='/Content/Assets/addFriend.png'/></div>"
                        );
                    }
                }

                searchList.append("<li>Groups</li>")
                if (results[1][0].length == 0) {
                    searchList.append("<li class='list-item'>Nothing Found</li>")
                }
                else {
                    for (var i = 0; i < results[1][0].length; i++) {
                        searchList.append(
                              "<li class='list-item'>"
                            + "<img src='/Images/Groups/" + results[1][1][i] + "'/>"
                            + "<a onclick='groupMain(\"" +results[1][2][i]+ "\"); return false;' class='list-name'>" + results[1][0][i] + "</a></li>"
                        );
                    }
                }

                searchList.append("<li>Events</li>")
                if (results[2][0].length == 0) {
                    searchList.append("<li class='list-item'>Nothing Found</li>")
                }
                else {
                    for (var i = 0; i < results[2][0].length; i++) {
                        searchList.append(
                              "<li class='list-item'>"
                            + "<img src='/Images/Events/" + results[2][1][i] + "'/>"
                            + "<a onclick='eventMain(\"" + results[2][2][i] + "\"); return false;' class='list-name'>" + results[2][0][i] + "</a></li>"
                        );
                    }
                }
            });
        }
    }
});
    
function followUser(id) {
    $.ajax({
        method: "POST",
        url: "/User/FriendRequest",
        data: { user_id: id }
    })
    .success(function (result) {
        if (result) {
            $("#friendrequest-icon-" + id).empty().append("<img onclick='unfollowUser(\"" + id + "\")' title='Add Friend' class='add-friend-img' src='/Content/Assets/removeFriend.png'/>");
        }
    });
}

function unfollowUser(id) {
    $.ajax({
        method: "POST",
        url: "/User/FriendRemove",
        data: { user_id: id }
    })
    .success(function (result) {
        if (result) {
            $("#friendrequest-icon-" + id).empty().append("<img onclick='followUser(\"" + id + "\")' title='Add Friend' class='add-friend-img' src='/Content/Assets/addFriend.png'/>");
        }
    });
}

function refresh() {    
    friendsTab();
    newsFeed();
    newPost("newsFeed");
}

$(document).ready(function () {
    refresh();
});