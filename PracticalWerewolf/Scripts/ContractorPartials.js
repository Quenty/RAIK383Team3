function loadFromHash() {
    var element = $("a[data-tab-name=" + window.location.hash.substring(1) + "]");
    if (element.length > 0) {
        element.click();
    }
    else
    {
        $("#ajax-nav-controls > li.active > a[data-ajax=true]").click();
    }
}

var currentTabName = "";
var originalTitle = document.title;

function updateActivePartial() {
    $("#ajax-nav-controls > li.active").removeClass("active");
    $(this.parentElement).addClass("active");

    var tabName = this.getAttribute("data-tab-name");
    currentTabName = tabName;

    var title = this.childNodes[0].nodeValue
    if (title)
    {
        document.title = title + " - " + originalTitle;
    }
    
    history.replaceState(null, tabName, document.URL);
    window.location.hash = tabName;
}



$(function () {
    $("a[data-ajax=true]").attr("data-ajax-begin", "updateActivePartial");
    loadFromHash();

    $(window).bind('hashchange', function () {
        if (window.location.hash != currentTabName)
        {
            loadFromHash();
        }

    });
});