var apiLoaded = false;
function loadAPI() {
    if (typeof google === 'object' && typeof google.maps === 'object') {
        initMap();
    } else if (!apiLoaded) {
        apiLoaded = true;
        var script = document.createElement("script");
        script.type = "text/javascript";
        script.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyCZ7pBFhPlZvw9Hm2CDAL7gFoI7s5WsPpc&callback=initMap";
        document.body.appendChild(script);
    }
}
var map;

function initMap() {
    if (!map) {
        var lat = parseFloat(document.getElementById("Lat").value);
        var long = parseFloat(document.getElementById("Long").value);
        var location = { lat: lat, lng: long }
        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 4,
            center: location,
            scrollwheel: false
        });
        var marker = new google.maps.Marker({
            position: location,
            map: map,
            clickable: true
        });

        map.setZoom(10);
        map.panTo(marker.position);

        google.maps.event.trigger(map, 'resize')
    } else {
        $("#map").append(map.getDiv());
        google.maps.event.trigger(map, 'resize')
    }
}

function createMap() {
    var uluru = { lat: 40.8, lng: -96.7 };
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 4,
        center: uluru
    });
    var marker = new google.maps.Marker({
        position: uluru,
        map: map,
        clickable: true
    });

    $('#Lat').val(uluru.lat);
    $('#Long').val(uluru.lng);

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

