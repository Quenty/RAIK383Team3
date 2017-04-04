$(document).ready(function () {
    function initMap() {
        var uluru = { lat: 40.8, lng: -96.7 };
        //var location = {lat: document.getElementById('#Lat'), lng: document.getElementById('#Long')}
        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 4,
            center: uluru
        });
        var marker = new google.maps.Marker({
            position: uluru,
            map: map,
            clickable: true
        });

        var update_timeout = null;

        google.maps.event.addListener(map, 'click', function (event) {
            update_timeout = setTimeout(function () {
                $('#Lat').val(event.latLng.lat());
                $('#Long').val(event.latLng.lng());
                var newPoint = new google.maps.LatLng(event.latLng.lat(), event.latLng.lng());
                if (marker) {
                    marker.setPosition(newPoint);
                }
            }, 200);
        });

        google.maps.event.addListener(map, 'dblclick', function (event) {
            clearTimeout(update_timeout);
        });
    }
});

