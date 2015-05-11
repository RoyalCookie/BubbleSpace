function friendsTab(){
    var friendslist = $("#list-view-items");
    friendslist.empty();
    $.post("/User/Friends", function (data) {
        for (var i = 0; i < data[0].length; i++) {
            friendslist.append("<li class='list-item'><img src='/Content/Assets/" + data[1][i] + ".png'/>" + data[0][i] + "</li>");
        }
    })
}

function eventsTab() {
    var eventsList = $("#list-view-items");
    eventsList.empty();
    $.post("/Event/Events", function (events) {
        console.log(events);
        for (var i = 0; i < events[0].length; i++) {
            eventsList.append("<li class='list-item'><img src='/Content/Assets/" + events[1][i] + ".png'/>" + events[0][i] + "</li>");
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
                  + posts[0][i] + " <p class='post-text'>" + posts[1][i]
              + "</p></li>");
            mainView.append(
                  "<input class='post-content-like fa fa-thumbs-up' type='button' value='like'>"
                + "<input class='post-content-burst' type='button' value='burst'>"
                + "<input class='post-content-comment' type='button' value='comment'>"
            );
        }
    })
}

$(function () {
    friendsTab();
    newsFeed();
});