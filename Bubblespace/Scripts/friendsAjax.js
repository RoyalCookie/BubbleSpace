

$(function () {

    // For now this is merely test data to work out css.
    // TODO Replace with proper ajax.

    var friendslist = $("#list-view-items");
    friendslist.empty();
    for (var i = 1; i < 41; i++) {
        friendslist.append("<li class='list-item'><img src='/Content/Assets/placeholder.png'/>Placeholder #" + i + "</li>");
    }
});