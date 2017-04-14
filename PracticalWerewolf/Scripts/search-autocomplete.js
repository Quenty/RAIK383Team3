$(function () {
    var searchForm = $(".search-form");
    var searchDropdown = $(".search-dropdown");

    searchForm.on('input', function () {
        var text = searchForm.val();
        if (text.length > 0) {
            console.log("Searching!", text)
            $.ajax({
                url: ("/Search/PartialResults?query=" + encodeURI(text)),
                data: null,
                success: (function (data) {
                    console.log("Got results!");
                    searchDropdown.addClass("open");
                    if (data.trim().length > 0) {
                        $(".autocomplete-list").html(data);
                    } else {
                        $(".autocomplete-list").html("<li class=\"disabled\"><a href=\"#\" class=\"disabled\">no results</a></li>");
                    }
                }),
                dataType: 'html'
            });
        } else {
            $(".autocomplete-list").html("");
            searchDropdown.removeClass("open");
        }
    })
});