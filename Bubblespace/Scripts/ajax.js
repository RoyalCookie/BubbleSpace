/*
    TODO: This page requires javascript alert on noscript!
    TODO: Show user posted to group on the newsfeed!
*/

$(document).ready(function () {
    refresh();
});

// This function refreshes the main view to it's intro state.
// Displays the friends tab and logged in user news feed including the post form.
function refresh() {
    friendsTab();
    newsFeed();
    newPost("newsFeed");
}

// This applies the onkeyup event handler to the search input at the top of the list view.
// The event fires an ajax post call with a string to the server that returns search results.
// Results cover all users, groups and events in the database.
$(document).ready(function () {
    document.getElementById("search-bar").onkeyup = function (event) {

        // We get the search string and check if it contains a value.
        var searchString = $("#search-bar").val();
        if (!searchString == "") {
            $.ajax({
                method: "POST",
                url: "/Search/GetResults",
                data: { search_string: searchString }
            })
            .success(function (results) {
                // Results:
                // [users/groups/events][name/image/id][indexer]

                // We get the search result container and empty it.
                var searchList = $("#list-view-items");
                searchList.empty();

                // We populate it with user results.
                searchList.append("<li class='search-header'>Users</li>")
                if (results[0][0].length == 0) {
                    searchList.append("<li class='list-item'>Nothing Found</li>")
                }
                else {
                    console.log(results);
                    for (var i = 0; i < results[0][0].length; i++) {
                        searchList.append(
                              "<li class='list-item'>"
                            +     "<img src='/Images/Users/" + results[0][1][i] + "'/>"
                            +     "<a onclick='friendMain(\"" + results[0][2][i] + "\"); return false;' class='list-name'>"
                            +         results[0][0][i]
                            +     "</a>"
                            + "</li>"
                            + "<div class='friendrequest-icon-container' id='friendrequest-icon-" + results[0][2][i] + "'>"
                            +     "<img onclick='followUser(\"" + results[0][2][i] + "\")' title='Add Friend' class='add-friend-img' src='/Content/Assets/addFriend.png'/>"
                            + "</div>"
                        );
                    }
                }

                // We populate it with group results.
                searchList.append("<li class='search-header'>Groups</li>")
                if (results[1][0].length == 0) {
                    searchList.append("<li class='list-item'>Nothing Found</li>")
                }
                else {
                    for (var i = 0; i < results[1][0].length; i++) {
                        searchList.append(
                              "<li class='list-item'>"
                            +     "<img src='/Images/Groups/" + results[1][1][i] + "'/>"
                            +     "<a onclick='groupMain(\"" + results[1][2][i] + "\"); return false;' class='list-name'>" + results[1][0][i] + "</a>"
                            + "</li>"
                        );
                    }
                }

                // We populate it with event results.
                searchList.append("<li class='search-header'>Events</li>")
                if (results[2][0].length == 0) {
                    searchList.append("<li class='list-item'>Nothing Found</li>")
                }
                else {
                    for (var i = 0; i < results[2][0].length; i++) {
                        searchList.append(
                              "<li class='list-item'>"
                            +     "<img src='/Images/Events/" + results[2][1][i] + "'/>"
                            +     "<a onclick='eventMain(\"" + results[2][2][i] + "\"); return false;' class='list-name'>" + results[2][0][i] + "</a>"
                            + "</li>"
                        );
                    }
                }
            });
        }
        else {
            // If the search input did not contain anything we display an empty list.
            var searchList = $("#list-view-items");
            searchList.empty();
        }
    }
});


// Tab Content: 
// These functions populate the list view with ajax return content.

// Friends Tab
function friendsTab() {
    // We get the friends list container and empty it.
    var friendslist = $("#list-view-items");
    friendslist.empty();

    // We request a list of friends from the server.
    $.post("/User/GetFriends", function (results) {
        // Results:
        // [usernames/images/userIds][indexer]
        for (var i = 0; i < results[0].length; i++) {
            friendslist.append(
                  "<li class='list-item'>"
                +     "<img src='/Images/Users/" + results[1][i] + "'/>"
                +     "<a onclick='friendMain(\"" + results[2][i] + "\"); return false;' class='list-name'>" + results[0][i] + "</a>"
                + "</li>"
            );
        }
    })
}


// Groups Tab
function groupsTab() {
    var groupList = $("#list-view-items");
    groupList.empty();
    groupList.append("<li><a onclick='createGroupMain(); return false;' title='Create Group' class='create-button btn btn-default btn-sm'><span class='glyphicon glyphicon-plus'></span></a></li>");
    $.post("/Group/GetAllGroups", function (results) {
        for (var i = 0; i < results[0].length; i++) {
            groupList.append(
                  "<li class='list-item'>"
                + "<img src='/Images/Groups/" + results[1][i] + "'/>"
                + "<a onclick='groupMain(\"" + results[2][i] + "\"); return false;' id='group-name'>" + results[0][i] + "</a>"
                + "</li>"
            );
        }
    })
}

// Events Tab
function eventsTab() {
    var eventsList = $("#list-view-items");
    eventsList.empty();
    eventsList.append("<li><a onclick='createEventMain(); return false;' title='Create Event' class='create-button btn btn-default btn-sm'><span class='glyphicon glyphicon-plus'></span></a></li>");
    $.post("/Event/Events", function (results) {
        for (var i = 0; i < results[0].length; i++) {
            eventsList.append(
                  "<li class='list-item'>"
                + "<img src='/Images/Events/" + results[1][i] + "'/>"
                +     "<div class='post-user-name'>"
                +         "<a onclick='eventMain(\"" + results[2][i] + "\"); return false;'>" + results[0][i] + "</a>"
                +     "</div>"
                + "</li>"
            );
        }
    })
}

// Chat Tab
function chatTab() {
    var chatlist = $("#list-view-items");
    chatlist.empty();
    $.post("/Chat/GetUserChats", function (results) {
        for (var i = 0; i < results["chatId"].length; i++) {
            chatlist.append(
                  "<li class='list-item'>"
                +     "<a onclick='chatHead(\"" + results["chatId"][i] + "\"); return false;'>" + results["chatName"][i] + "</a>"
                + "</li>"
            );
        }
    })
}

// Main View Content:
// These functions populate the main view with ajax return content.
// We also populate the head view with basica content relative to the main view.

// Friend Main
// This takes in a user ID and returns a main view with that users content.
function friendMain(id) {
    $.ajax({
        method: "POST",
        url: "/User/GetUserInformation",
        data: { userId: id }
    })
   .success(function (results) {
       // We get the main view and empty it.
       var mainView = $("#main-view");
       mainView.empty();

       // We populate the view with the newest posts on top.
       for (var i = results["posts"].length - 1; i >= 0; i--) {
           // The post info:
           mainView.append(
                   "<li class='feed-post'>"
                 + "<img class='post-profile-image' src='/Images/Users/" + results["profileImage"] + "' />"
                 +     "<div class='post-user-name'>"
                 +         "<a onclick='friendMain(\"" + results["Id"] + "\"); return false;'>" + results["userName"] + "</a>"
                 +     "</div>"
                 + "<p class='post-text'>" + results["posts"][i] + "</p>"
                 + "</li>"
             );
           // The post like / burst / comment feature.
           mainView.append(
                 "<div class='post-feedback'><i class='fa fa-thumbs-up'></i>"
               + "<i class='fa fa-thumb-tack'></i>"
               + "<i class='fa fa-comment'></i></div>"
           );
       }       
    });

    // Here we populate the head view with the users information.
    $.ajax({
        method: "POST",
        url: "/User/GetUserInformation",
        data: { userId: id }
    })
    .success(function (results) {
        headView.empty();
        headView.append(
                "<img class='profile-header-image' src='/Images/Users/" + results["profileImage"] + "'/>"
            +   "<h1 class='profile-header'>" + results["userName"] + "</h1>"
        );
    });
}

// Group Main
// This takes in a group ID and returns a main view with that groups content.
function groupMain(id) {
    $.ajax({
        method: "POST",
        url: "/Group/GetGroupById",
        data: { groupId: id }
    })
   .success(function (info) {
       var headView = $("#head-view");
       headView.append(
             "<img class='profile-header-image' src='/Images/Groups/" + info[2] + "'/>"
           + "<h1 class='profile-header'>" + info[0] + "</h1>"
           + "<p class='profile-description'>" + info[1] + "</p>"
           );
       var mainView = $("#main-view");
       mainView.empty();

       // Friendly reminders.
       console.log("TODO: DISPLAY GROUP POSTS. @groupMain()");
       console.log("TODO: ADD ID TO FORM");
       // We append the appropriate version of the new post form to the head view.
       newPost("groupPage");
   });
}

// Event Main
// This takes in an event ID and returns a main view with that events content.
function eventMain(id) {
    $.ajax({
        method: "POST",
        url: "/Event/GetEventById",
        data: { eventId: id }
    })
    .success(function (results) {
        var headView = $("#head-view");
        headView.empty();

        headView.append(
                "<img class='profile-header-image' src='/Images/Events/" + results[2] + "'/>"
            +   "<h1 class='profile-header'>" + results[0] + "</h1><br>"
            +   "<p class='profile-description'>" + results[1] + "</p>"
            +   "<p class='profile-description-time'>From: &nbsp&nbsp" + results[3].substring(0, 10) + "</p>"
            +   "<p class='profile-description-time'>To: &nbsp&nbsp&nbsp&nbsp&nbsp " + results[4].substring(0, 10) + "</p>"
        );

        // Main view is left empty.
        $("#main-view").empty();
    });
}

function chatMain(id) {
    $.ajax({
        method: "POST",
        url: "/Chat/GetAllMessagesFromChat",
        data: { chatId: id }
    })
    .success(function (results) {

        var mainView = $("#main-view");
        var chatBox = $("#chatBox");

        mainView.empty();
        mainView.append("<div id=\"chatBox\"></div>");
        
        // We populate the chat with names and messages.
        for (var i = 0; i < results["sender"].length; i++) {
            //Friendly Reminder
            console.log("TODO: ADD TIMESTAMP TO CHAT? @chatMain()");
            mainView.append(
                  "<li>"
                +     "<p class='post-text'> " + results["sender"][i] + ": " + results["message"][i] + "</p>"
                + "</li>"
            );
        }

        // We store the id of the newest message, this is usefull for looking up and appending newer messages later.
        if (results["id"].length > 0) {
            chatBox.append("<input type=\"hidden\" name=\"lastMessageId\" id=\"lastMessageId\" value=\"" + results["id"][0] + "\">");
        }

        // The input box.
        mainView.append(
                "<input type=\"text\" name=\"messageBox\" id=\"messageBox\">"
            +   "<button type=\"button\" onClick=\"sendMessage(" + id + ")\">Click Me!</button>"
        );
    });
}

// Here we have Create views for groups and events.

// Create Group
function createGroupMain() {
    // We get the head view container and display a header message.
    var headView = $("#head-view");
    headView.empty();
    headView.append("<h1>Create Group</h1>")

    // We append the form.
    var mainView = $("#main-view");
    mainView.empty();
    mainView.append(
                 "<form method='post' action='/Group/Create' enctype='multipart/form-data'>"
               +      "<label for='group-name'>Group Name</label>"
               +      "<input type='text' class='form-control' id='groupName' name='group-name'><br>"
               +      "<label for='group-description'>Group Description</label>"
               +      "<textarea class='form-control' name='group-description'></textarea>"
               +      "<input type='file' id='image-upload' name='contentImage' accept='image/*'>"
               +      "<input type='submit' class='btn btn-default btn-create' value='Create'>"
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
                + "<div id=\"post-id-" + posts[4][i] + "\">" + posts[5][i] + "</div>"
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
        var oldCount = $("#post-id-" + id).val();
        console.log(oldCount);
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
    else if (type == "groupPage") {
        console.log("here");
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

