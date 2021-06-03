
function inicijalizirajMapuPolaziste() {
    var mapPolaziste = L.map('mapPolaziste').setView([44.60, 16.70], 5);

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoiYXN1cGFuIiwiYSI6ImNrajhyMms5NjA4c2UyeXBldWxpaTEzN3kifQ.lk1KeaqGyg-Z0tJv-3J9FQ'
    }).addTo(mapPolaziste);

    L.Routing.control({
        waypoints: [
            L.latLng(45.331208, 17.676212),
            L.latLng(45.373629, 17.662172)
        ]
    }).addTo(mapPolaziste);
}

function inicijalizirajMapuOdrediste() {
    var mapOdrediste = L.map('mapOdrediste').setView([44.60, 16.70], 5);

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoiYXN1cGFuIiwiYSI6ImNrajhyMms5NjA4c2UyeXBldWxpaTEzN3kifQ.lk1KeaqGyg-Z0tJv-3J9FQ'
    }).addTo(mapOdrediste);
}