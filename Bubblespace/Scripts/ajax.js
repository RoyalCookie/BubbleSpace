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
        for (var i = 0; i < events[0].length; i++) {
            eventsList.append("<li class='list-item'><img src='/Content/Assets/" + events[1][i] + ".png'/>" + events[0][i] + "</li>");
        }
    })
}

$(function () {
    var mainView = $("#main-view");
    mainView.empty();
    $.post("/Post/NAME!", function (posts) {
        for (var i = 0; i < posts[0].length; i++) {
            mainView.append("<li>This is a post</li>");
        }
    })
});