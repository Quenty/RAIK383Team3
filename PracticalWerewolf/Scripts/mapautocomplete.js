// This example displays an address form, using the autocomplete feature
// of the Google Places API to help users fill in the information.

// This example requires the Places library. Include the libraries=places
// parameter when you first load the API. For example:
// <script src="https://maps.googleapis.com/maps/api/js?key=YOUR_API_KEY&libraries=places">

function AddressAutocompleter(element) {
    var self = this;

    console.log("Made address auto competer");
    this.element = element
    this.inputElement = element.getElementsByClassName("map-autocomplete-input")[0]
    if (this.inputElement == null)
    {
        console.error("No input object found!");
    }

    this.autocomplete = new google.maps.places.Autocomplete(this.inputElement, { types: ['geocode'] });

    this.autocomplete.addListener('place_changed', function() {
        self.fillInAddress()
    });
    this.enableFields();

    google.maps.event.addDomListener(this.inputElement, 'keydown', function (e) {
        if (e.keyCode == 13) {
            e.preventDefault(); // Prevent submitting form on selecting address
        }
    });
}

AddressAutocompleter.prototype.getFields = function() {
    return this.element.getElementsByClassName("map-autocomplete-field");
}

AddressAutocompleter.prototype.disableFields = function() {
    var fields = this.getFields();
    for (var i = 0; i < fields.length; i++) {
        var element = fields[i];
        element.disabled = true;
    }
}

AddressAutocompleter.prototype.enableFields = function() {
    var fields = this.getFields();
    for (var i = 0; i < fields.length; i++) {
        var element = fields[i];
        element.disabled = false;
    }
}


AddressAutocompleter.prototype.fillInAddress = function () {
    var place = this.autocomplete.getPlace();
    if (place) {
        var types = {}
        var fields = this.getFields();
        for (var i = 0; i < fields.length; i++) {
            var element = fields[i];

            var componentData = element.getAttribute("data-map-autocomplete-field");
            var componentDataType = element.getAttribute("data-map-autocomplete-datatype") || "short_name";

            types[componentData] = {
                data: componentData,
                dataType: componentDataType,
                element: element
            };

            element.disabled = false;
        }

        for (var i = 0; i < place.address_components.length; i++) {
            var addressComponent = place.address_components[i];
            var addressType = addressComponent.types[0];

            if (types[addressType]) {
                var type = types[addressType]
                var element = type.element
                element.value = addressComponent[type.dataType];
            }
        }
    }
    else
    {
        console.write("No place loaded from autocomplete! Ran out of tokens?")
        this.disableFields();
    }
}

// Bias the autocomplete object to the user's geographical location,
// as supplied by the browser's 'navigator.geolocation' object.
AddressAutocompleter.prototype.geolocate = function() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function(position) {
            var geolocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: position.coords.accuracy
            });
            autocomplete.setBounds(circle.getBounds());
        });
    }
}

function initAutocompleteCallback() {
    // Called by Google's library after it is initialized
    var components = document.getElementsByClassName("map-autocomplete-element");
    console.log(components);
    for (var i = 0; i < components.length; i++)
    {
        new AddressAutocompleter(components[i]);
    }
}