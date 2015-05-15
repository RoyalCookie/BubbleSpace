/*
TODO: This page requires javascript alert on noscript!
TODO: Show user posted to group on the newsfeed!
TODO: Check authentication before loading the main view!
*/


/*
    Script Index:
    1. Tab Content
        a. friends
        b. groups
        c. events
        d. chats
        e. settings
    2. Main View Content
        a. news feed
        b. friend
        c. group
        d. event
        e. chat
    3. Create
        a. group
        b. event
    4. Setting Pages
        a. profile image
    5. Minor Functional Calls
        a. like
        b. follow
        c. unfollow
    6. Helper Functions
        a. style the file picker
        b. new post form
*/


var chatInterval;

$(document).ready(function () {
    $("#list-view-search").append("<input type='text' id='search-bar' class='form-control' placeholder='search..' />");
    $("#right-view").append('<div id="main-view" class="col-xs-8"><ul></ul></div>');
    refresh();
    // Replacing the default scrollbar look with perfect scrollbar.
    var mainView = $("#main-view");
    mainView.perfectScrollbar();
    Ps.initialize(document.getElementById('main-view'));
});

// This function refreshes the main view to it's intro state.
// Displays the friends tab and logged in user news feed including the post form.
function refresh() {
    friendsTab();
    newsFeed();
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
                    for (var i = 0; i < results[0][0].length; i++) {
                        searchList.append(
                              "<li class='list-item'>"
                            + "<img src='/Images/Users/" + results[0][1][i] + "'/>"
                            + "<a onclick='friendMain(\"" + results[0][2][i] + "\"); return false;' class='list-name'>"
                            + results[0][0][i]
                            + "</a>"
                            + "</li>"
                            + "<div class='friendrequest-icon-container' id='friendrequest-icon-" + results[0][2][i] + "'>"
                            + "<img onclick='followUser(\"" + results[0][2][i] + "\")' title='Add Friend' class='add-friend-img' src='/Content/Assets/addFriend.png'/>"
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
                            + "<img src='/Images/Groups/" + results[1][1][i] + "'/>"
                            + "<a onclick='groupMain(\"" + results[1][2][i] + "\"); return false;' class='list-name'>" + results[1][0][i] + "</a>"
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
                            + "<img src='/Images/Events/" + results[2][1][i] + "'/>"
                            + "<a onclick='eventMain(\"" + results[2][2][i] + "\"); return false;' class='list-name'>" + results[2][0][i] + "</a>"
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
                + "<img src='/Images/Users/" + results[1][i] + "'/>"
                + "<a onclick='friendMain(\"" + results[2][i] + "\"); return false;' class='list-name'>" + results[0][i] + "</a>"
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
                + "<div class='post-user-name'>"
                + "<a onclick='eventMain(\"" + results[2][i] + "\"); return false;'>" + results[0][i] + "</a>"
                + "</div>"
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
                + "<a onclick='chatHead(\"" + results["chatId"][i] + "\"); return false;'>" + results["chatName"][i] + "</a>"
                + "</li>"
            );
        }
    })
}

// Settings Tab
function settingsTab() {
    // We get the friends list container and empty it.
    var settingsList = $("#list-view-items");
    settingsList.empty();
    settingsList.append(
            "<li class='list-item'>"
        +   "<a onclick='profilePictureMain(); return false;'>Profile Image</a>"
        +   "</li>"
    );


}

// Main View Content:
// These functions populate the main view with ajax return content.
// We also populate the head view with basica content relative to the main view.

//This displays the newsfeed, it is the default starting position of the page.
function newsFeed() {
    clearInterval(chatInterval);
    chatInterval = null;

    // Get the view container and empty it.
    var mainView = $("#main-view");
    mainView.empty();

    $.post("/Post/GetAllUserPosts", function (results) {
        // We populate the news feed with relevant posts to the logged in user.
        for (var i = results[0].length - 1; i >= 0; i--) {

            // If the post has 10 bursts (dislikes) the post gets a red border.
            // If the post has 10 likes the post gets a green border.
            // If both are above 10 the higher one wins, if they are the same red wins.
            var post_class;
            var likes = results[5][i];
            var dislikes = results[6][i];

            if (dislikes >= 10) {
                post_class = "burst-feed-post";
            }
            else {
                post_class = "feed-post";
            }

            var image = "";
            if (results[7][i]) {
                image = "<img class = 'post-image' src='/Images/Posts/" + results[7][i] + "' />";
            }

            // The post itself.
            mainView.append(
                    "<li class=\"" + post_class + "\">"
                    + "<img class='post-profile-image' src='/Images/Users/" + results[2][i] + "' />"
                    + "<div class='post-user-name'>"
                    + "<a onclick='friendMain(\"" + results[3][i] + "\"); return false;'>" + results[0][i] + "</a>"
                    + "</div>"
                    + image
                    + "<p class='post-text'>" + results[1][i] + "</p>"
                    + "</li>"
                );

            // Feedback to the post.
            mainView.append(
                  "<div class='post-feedback'>"
                + "<div class='like-count' id=\"like-post-id-" + results[4][i] + "\">" + results[5][i] + "</div>"
                + "<div class='burst-count' id=\"burst-post-id-" + results[4][i] + "\">" + results[6][i] + "</div>"
                + "<i onclick=\"likePost(" + results[4][i] + "); return false;\" class='fa fa-thumbs-up'></i>"
                + "<i onclick=\"burstPost(" + results[4][i] + "); return false;\" class='fa fa-thumb-tack'></i>"
                + "</div>"
            );
        }
    })

    // Here we populate the head view with the users information.
    $.post("/User/GetLoggedInUserInfo", function (results) {
        var headView = $("#head-view");
        headView.empty();
        headView.append(
              "<img class='profile-header-image' src='/Images/Users/" + results[1] + "'/>"
            + "<h1 class='profile-header'>" + results[0] + "</h1>"
        );

        // We append the appropriate version of the new post form to the head view.
        newPost("newsFeed");
    });
}

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
       for (var i = results["postBody"].length - 1; i >= 0; i--) {

           // If the post has 10 bursts (dislikes) the post gets a red border.
           // If the post has 10 likes the post gets a green border.
           // If both are above 10 the higher one wins, if they are the same red wins.
           var post_class;
           var likes = results["postLikeCount"][i];
           var dislikes = results["postBurstcount"][i];

           if (dislikes >= 10) {
               post_class = "burst-feed-post";
           }
           else {
               post_class = "feed-post";
           }

           var image = "";
           if (results["postImage"][i]) {
               image = "<img class = 'post-image' src='/Images/Posts/" + results["postImage"][i] + "' />";
           }

           // The post info:
           mainView.append(
                    "<li class=\"" + post_class + "\">"
                 + "<img class='post-profile-image' src='/Images/Users/" + results["profileImage"] + "' />"
                 + "<div class='post-user-name'>"
                 + "<a onclick='friendMain(\"" + results["Id"] + "\"); return false;'>" + results["userName"] + "</a>"
                  + "</div>"
                  + image
                 + "<p class='post-text'>" + results["postBody"][i] + "</p>"
                 + "</li>"
             );
           // Feedback to the post.
           mainView.append(
                 "<div class='post-feedback'>"
               + "<div class='like-count' id=\"like-post-id-" + results["postId"][i] + "\">" + results["postLikeCount"][i] + "</div>"
               + "<div class='burst-count' id=\"burst-post-id-" + results["postId"][i] + "\">" + results["postBurstcount"][i] + "</div>"
               + "<i onclick=\"likePost(" + results["postId"][i] + "); return false;\" class='fa fa-thumbs-up'></i>"
               + "<i onclick=\"burstPost(" + results["postId"][i] + "); return false;\" class='fa fa-thumb-tack'></i>"
               + "</div>"
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
        var headView = $("#head-view");
        headView.empty();
        headView.append(
              "<img class='profile-header-image' src='/Images/Users/" + results["profileImage"] + "'/>"
            + "<h1 class='profile-header'>" + results["userName"] + "</h1>"
            + "<a title='Start Chat!' onClick='createChat(\"" + results["Id"] + "\")'> <img class='profile-header-image' src='/Images/System/startChat.png'/></a> "
        );
    });
}

function createChat(friendId) {
    $.ajax({
        method: "POST",
        url: "/Chat/Create",
        data: { friendId: friendId }
    }).success(function (results) {
        if (results != undefined) {
            chatTab();
            chatHead(results["chatId"]);
        }
    });
}

// Group Main
// This takes in a group ID and returns a main view with that groups content.
function groupMain(id) {
    clearInterval(chatInterval);
    chatInterval = null;
    $.ajax({
        method: "POST",
        url: "/Group/GetGroupById",
        data: { groupId: id }
    })
   .success(function (info) {
       var headView = $("#head-view");
       headView.empty();
       headView.append(
             "<img class='profile-header-image' src='/Images/Groups/" + info[2] + "'/>"
           + "<h1 class='profile-header'>" + info[0] + "</h1>"
           + "<p class='profile-description'>" + info[1] + "</p>"
           );
       var mainView = $("#main-view");
       mainView.empty();

       $.ajax({
           method: "POST",
           url: "/Group/GetGroupPosts",
           data: { groupId: id }
       })
       .success(function (results) {

           // We populate the news feed with relevant posts to the logged in user.
           for (var i = results[0].length - 1; i >= 0; i--) {

               // If the post has 10 bursts (dislikes) the post gets a red border.
               // If the post has 10 likes the post gets a green border.
               // If both are above 10 the higher one wins, if they are the same red wins.
               var post_class;
               var likes = results[5][i];
               var dislikes = results[6][i];

               if (dislikes >= 10) {
                   post_class = "burst-feed-post";
               }
               else {
                   post_class = "feed-post";
               }

               var image = "";
               if (results[7][i]) {
                   image = "<img class='post-image' src='/Images/Posts/" + results[7][i] + "' />";
               }

               // The post itself.
               mainView.append(
                       "<li class=\"" + post_class + "\">"
                     + "<img class='post-profile-image' src='/Images/Users/" + results[2][i] + "' />"
                     + "<div class='post-user-name'>"
                     + "<a onclick='friendMain(\"" + results[3][i] + "\"); return false;'>" + results[0][i] + "</a>"
                     + "</div>"
                     + image
                     + "<p class='post-text'>" + results[1][i] + "</p>"
                     + "</li>"
                 );

               // Feedback to the post.
               mainView.append(
                     "<div class='post-feedback'>"
                   + "<div class='like-count' id=\"like-post-id-" + results[4][i] + "\">" + results[5][i] + "</div>"
                   + "<div class='burst-count' id=\"burst-post-id-" + results[4][i] + "\">" + results[6][i] + "</div>"
                   + "<i onclick=\"likePost(" + results[4][i] + "); return false;\" class='fa fa-thumbs-up'></i>"
                   + "<i onclick=\"burstPost(" + results[4][i] + "); return false;\" class='fa fa-thumb-tack'></i>"
                   + "</div>"
               );
           }

       });

       // We append the appropriate version of the new post form to the head view.
       newPost("groupPage", id);
   });
}

// Event Main
// This takes in an event ID and returns a main view with that events content.
function eventMain(id) {
    clearInterval(chatInterval);
    chatInterval = null;
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
            + "<h1 class='profile-header'>" + results[0] + "</h1><br>"
            + "<p class='profile-description'>" + results[1] + "</p>"
            + "<p class='profile-description-time'>From: &nbsp&nbsp" + results[3].substring(0, 10) + "</p>"
            + "<p class='profile-description-time'>To: &nbsp&nbsp&nbsp&nbsp&nbsp " + results[4].substring(0, 10) + "</p>"
        );

        // Main view is left empty.
        $("#main-view").empty();
    });
}



// Here we have Create views for groups and events.

// Create Group.
function createGroupMain() {
    clearInterval(chatInterval);
    chatInterval = null;
    // We get the head view container and display a header message.
    var headView = $("#head-view");
    headView.empty();
    headView.append("<h1>Create Group</h1>")

    // We append the form.
    var mainView = $("#main-view");
    mainView.empty();
    mainView.append(
                 "<form method='post' action='/Group/Create' enctype='multipart/form-data'>"
               + "<label for='group-name'>Group Name</label>"
               + "<input type='text' class='form-control' id='groupName' name='group-name'><br>"
               + "<label for='group-description'>Group Description</label>"
               + "<textarea class='form-control' name='group-description'></textarea>"
               + "<input type='file' id='image-upload' name='contentImage' accept='image/*'>"
               + "<input type='submit' class='btn btn-default btn-create' value='Create'>"
               + "</form>"
            );

    // FileStyle: styles the file submit button.
    styleTheFilePicker();
}

// Create Event.
function createEventMain() {
    clearInterval(chatInterval);
    chatInterval = null;
    // We get the head view container and display a header message.
    var headView = $("#head-view");
    headView.empty();
    headView.append("<h1>Create Event</h1>")

    // We append the form.
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
    styleTheFilePicker();

    // Adding jquery datepicker control to the date inputs.
    $("#datepickerFrom").datepicker();
    $("#datepickerTo").datepicker();
}


// Setting pages:

function profilePictureMain() {
    // We get the head view container and display a header message.
    var headView = $("#head-view");
    headView.empty();
    headView.append("<h1>Profile Image Setting</h1>")

    // We append the form.
    var mainView = $("#main-view");
    mainView.empty();
    mainView.append(
                 "<form method='post' action='/User/UpdateProfileImage' enctype='multipart/form-data'>"
               + "<label for='contentImage'>Upload a new profile image</label>"
               + "<input type='file' id='image-upload' name='contentImage' accept='image/*'>"
               + "<input type='submit' class='btn btn-default btn-create' value='Update'>"
               + "</form>"
            );
}

// Minor functional calls.

// This adds a like from the logged in user to the post with the given id.
// Success updates the number of likes. One like per user!
function likePost(id) {
    $.ajax({
        method: "POST",
        url: "/Post/LikePost",
        data: { postId: id }
    })
    .success(function (data) {
        $("#like-post-id-" + id).empty().append(data);
    });
}

// WORK IN PROGRESS
// This adds a burst from the logged in user to the post with the given id.
// Success updates the number of burst. One burst per user!
function burstPost(id) {
    $.ajax({
        method: "POST",
        url: "/Post/BurstPost",
        data: { postId: id }
    })
    .success(function (data) {
        $("#burst-post-id-" + id).empty().append(data);
    });
}

// This allows the logged in user to follow the user with the given id.
function followUser(id) {
    $.ajax({
        method: "POST",
        url: "/User/FriendRequest",
        data: { user_id: id }
    })
    .success(function (result) {
        // Result is a boolean, indicates wether the request was successful or not.
        if (result) {
            $("#friendrequest-icon-" + id).empty().append("<img onclick='unfollowUser(\"" + id + "\")' title='Add Friend' class='add-friend-img' src='/Content/Assets/removeFriend.png'/>");
        }
    });
}

//This allows the user to unfollow the user with the given id.
function unfollowUser(id) {
    $.ajax({
        method: "POST",
        url: "/User/FriendRemove",
        data: { user_id: id }
    })
    .success(function (result) {
        // Result is a boolean, indicates wether the request was successful or not.
        if (result) {
            $("#friendrequest-icon-" + id).empty().append("<img onclick='followUser(\"" + id + "\")' title='Add Friend' class='add-friend-img' src='/Content/Assets/addFriend.png'/>");
        }
    });
}

// Helper functions:

// This applies filestile to the file input forms.
function styleTheFilePicker() {
    $(":file").filestyle({ input: false });
    $(":file").filestyle({ iconName: "glyphicon-inbox" });
    $(":file").filestyle('size', 'xs');
}

// This takes in a type and id (for groups) and appends the appropriate post submission form to the head view container.
function newPost(type, id) {
    var headView = $("#head-view");

    // If we are dealing with the news feed.
    if (type == "newsFeed") {
        headView.append(
        "<form class='new-post' method='post' action='/Post/Create' enctype='multipart/form-data'>"
            + "<textarea id='content_text' class='form-control' name='content_text' rows='3' cols='40'></textarea><br />"
            + "<input type='submit' class='btn btn-default' value='Post' />"
            + "<input type='file' data-iconName='glyphicon-inbox' name='contentImage' accept='image/*'>"
            + "</form>"
        );
        // FileStyle: styles the file submit button.
        styleTheFilePicker();
    }
        // If we are dealing with the group page.
    else if (type == "groupPage") {
        headView.append(
             "<form class='new-post' method='post' action='/Post/Create' enctype='multipart/form-data'>"
           + "<textarea id='content_text' class='form-control' name='content_text' rows='3' cols='40'></textarea><br />"
           + "<input type='submit' class='btn btn-default' value='Post' />"
           + "<input type='hidden' name='group-id' value=\"" + id + "\" />"
           + "<input type='file' data-iconName='glyphicon-inbox' name='contentImage' accept='image/*'>"
           + "</form>"
        );

        // FileStyle: styles the file submit button.
        styleTheFilePicker();
    }
}




/*
    @JANUS -- Vantar að commenta þetta almennilega!
*/

// Send Message
// This function sends message and on success takes the info about the sent message and adds it to the chat
function sendMessage(chatId) {
    var message = document.getElementById("messageBox").value;
    $("#messageBox").val('');
    var view = $("#chatBox");
    $.ajax({
        method: "POST",
        url: "/Chat/Send",
        data: { chatId: chatId, message: message }
    }).success(function (message) {
        view.append(
        "<li>"
        + "<p class='post-text'> " + message["sender"] + ": " + message["message"]
        + "</p></li>");
        if ($("#lastMessageId") === 0) {
                chatBox.append("<input type=\"hidden\" id=\"lastMessageId\" value=\"" + results["highestId"] + "\">");
        } else {
            $("#lastMessageId").val(message["id"]);
            $("#chatBox").scrollTop(1E10);
        }

    });
}

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
                  + "<div class='post-user-name'>" + users["userName"][i] + "</div>"
                  + "</p></div>"
              );
        }
        chatUsers.append("<button onClick=\"renameChat()\" >Rename Chat</button>");
        // Add User Button
        //chatUsers.append("<a onclick='renameChat()'><img class='post-profile-image' src='/Images/System/addUser.png'></a>");
        chatMain(id);
    });
}

// Chat Main
// This takes in a chat ID and returns a main view with that chats content.
function chatMain(id) {
    $.ajax({
        method: "POST",
        url: "/Chat/GetAllMessagesFromChat",
        data: { chatId: id }
    })
    .success(function (results) {

        var mainView = $("#main-view");

        mainView.empty();
        mainView.append("<div id=\"chatBox\">"
                        + "<input type=\"hidden\" id=\"chatId\" value=\"" + id + "\""
                        + "</div>");
        var chatBox = $("#chatBox");

        // We populate the chat with names and messages.
        for (var i = 0; i < results["sender"].length; i++) {
            if (chatBox === 0) {
            }
            chatBox.append(
                  "<li>"
                + "<p class='post-text'> " + results["sender"][i] + ": " + results["message"][i] + "</p>"
                + "</li>"
            );
        }

        // We store the id of the newest message, this is usefull for looking up and appending newer messages later.
        if (results["id"].length > 0) {
            chatBox.append("<input type=\"hidden\" id=\"lastMessageId\" value=\"" + results["highestId"] + "\">");
        } else {
            chatBox.append("<input type=\"hidden\" id=\"lastMessageId\" value=\"0\">");
        }

        // The input box.
        mainView.append(
                "<input type=\"text\" name=\"messageBox\" id=\"messageBox\">"
            + "<button type=\"button\" onClick=\"sendMessage(" + id + ")\">Send</button>"
        );

        chatInterval = setInterval(chatUpdate, 1000);
        $("#chatBox").scrollTop(1E10);
    });
}

function renameChat() {

    var chatId = $("#chatId").val();
    var newName = prompt("New name: ");
    
    if (newName.length != 0) {
        $.ajax({
            method: "POST",
            url: "/Chat/Rename",
            data: { chatId: chatId, newName : newName }
        }).success(function (retObj) {
            chatTab();
            chatHead(chatId);
        });
    }
}

function chatUpdate() {
    // If the interval is null this won't run.
    if (chatInterval) {
        var chatId = $("#chatId").val();
        var lastId = $("#lastMessageId").val();

        var chatBox = $("#chatBox");

        $.ajax({
            method: "POST",
            url: "/Chat/GetChatUpdates",
            data: { chatId: chatId, lastId: lastId }
        }).success(function (results) {
            for (var i = 0; i < results["sender"].length; i++) {
                if (chatBox === 0) {
                }
                chatBox.append(
                        "<li>"
                    + "<p class='post-text'> " + results["sender"][i] + ": " + results["message"][i] + "</p>"
                    + "</li>"
                );
            }
            if (results["id"].length > 0) {
                var temp = $("#lastMessageId");
                if (temp === 0) {
                    chatBox.append("<input type=\"hidden\" id=\"lastMessageId\" value=\"" + results["id"][0] + "\">");
                } else {
                    temp.val(results["id"][0]);
                }
                $("#chatBox").scrollTop(1E10);
            }
        });
    }
}
