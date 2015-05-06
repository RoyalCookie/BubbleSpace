function changeSize() {
    var width = parseInt($("#Width").val());
    var height = parseInt($("#Height").val());
}

var scrollBarIsSet = false;
$(window).resize(function () {
    if (!scrollBarIsSet && document.documentElement.clientWidth < 768) {
        setScrollBar();
        scrollBarIsSet = true;
    }
    else {
    }
});

$(function () {
    setScrollBar();
});

function setScrollBar() {
    /* 
        If the user is running any version of IE the perfect-scrollbar wont show. 
        Fallback is default scrollbar. -Andri

        IE check script snippet courtesy of:
        http://stackoverflow.com/questions/19999388/jquery-check-if-user-is-using-ie
    */
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
        // DO NOTHING
    }
    else {
        $('#list-view').perfectScrollbar();
        Ps.initialize(document.getElementById('list-view'));
    }

    return false;
}