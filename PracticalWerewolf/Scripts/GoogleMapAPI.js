var apiLoaded = false;
function loadAPI() {
    //if (typeof google === 'object' && typeof google.maps === 'object') {
    if(typeof google === 'object' && typeof google.maps === 'object') {
        initMap();
    } else if(!apiLoaded) {
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
        var lat = parseInt(document.getElementById("Lat").value, 10);
        var long = parseInt(document.getElementById("Long").value, 10);
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

        google.maps.event.trigger(map, 'resize')
    } else {
        $("#map").append(map.getDiv());
        google.maps.event.trigger(map, 'resize')
    }
    
      
}

//function initMap() {
//    var uluru = { lat: 40.8, lng: -96.7 };
//    //var location = {lat: document.getElementById('#Lat'), lng: document.getElementById('#Long')}
//    var map = new google.maps.Map(document.getElementById('map'), {
//        zoom: 4,
//        center: uluru
//    });
//    var marker = new google.maps.Marker({
//        position: uluru,
//        map: map,
//        clickable: true
//    });

//    var update_timeout = null;

//    google.maps.event.addListener(map, 'click', function (event) {
//        update_timeout = setTimeout(function () {
//            $('#Lat').val(event.latLng.lat());
//            $('#Long').val(event.latLng.lng());
//            var newPoint = new google.maps.LatLng(event.latLng.lat(), event.latLng.lng());
//            if (marker) {
//                marker.setPosition(newPoint);
//            }
//        }, 200);
//    });
//}
//            google.maps.event.addListener(map, 'dblclick', function (event) {
//                clearTimeout(update_timeout);
//            });
//        }
////    });
////}



