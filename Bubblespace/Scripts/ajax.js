function friendsTab(){
    var friendslist = $("#list-view-items");
    friendslist.empty();
    $.post("/User/Friends", function (data) {
        for (var i = 0; i < data[0].length; i++) {
            friendslist.append(
                  "<li class='list-item'>"
                + "<img src='/Content/Assets/" + data[1][i] + ".png'/>"
                + "<a onclick='friendMain(); return false;'>" + data[0][i] + "</a></li>"
            );
        }
    })
}

function friendMain() {
    var mainView = $("#main-view");
    mainView.empty();
    $.post("/Post/GetUserInformation", function (posts) {
        // TODO: Header
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
}

$(function () {
    friendsTab();
    newsFeed();
});