$()

$(function () {

    // For now this is merely test data to work out css.
    // TODO Replace with proper ajax.

    var friendslist = $("#list-view-items");
    friendslist.empty();
    $.post("/User/Friends", function (users) {
        for (var i = 0; i < users.length; i++) {
            friendslist.append("<li class='list-item'><img src='/Content/Assets/" + users[i].profile_image + ".png'/>" + users[i].NickName + "</li>");
        }
    })
});