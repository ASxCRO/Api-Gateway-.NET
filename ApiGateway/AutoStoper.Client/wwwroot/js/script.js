class Lokacija {
    constructor(lat, lng) {
        this.lat = lat;
        this.lng = lng;
    }
}

class Ruta {
    constructor(polaziste, odrediste, distanca, vrijeme) {
        this.polaziste = polaziste;
        this.odrediste = odrediste;
        this.distanca = distanca;
        this.vrijeme = vrijeme
    }
}

var _lokacijaPolaziste;
var _lokacijaOdrediste;
var _ruta;
var dotNetObj;
var mapPolaziste;
var mapOdrediste;
var mapRoute;

var GLOBAL = {};
GLOBAL.DotNetReference = null;
GLOBAL.SetDotnetReference = function (pDotNetReference) {
    GLOBAL.DotNetReference = pDotNetReference;
};
window.getRoute = () => {
    GLOBAL.DotNetReference.invokeMethodAsync('ShowMoreInfo');
};

(function () {

})();



function SpremiLokacijuPolaziste(lokacija) {
    _lokacijaPolaziste = lokacija;
}

function SpremiLokacijuOdrediste(lokacija) {
    _lokacijaOdrediste = lokacija;
}

function SpremiRutu(ruta) {
    _ruta = ruta;
}

function GetLokacijaPolaziste() {
    return _lokacijaPolaziste;
}

function GetLokacijaOdrediste() {
    return _lokacijaOdrediste;
}

function GetRutu() {
    return _ruta;
}



function inicijalizirajMapuPolaziste() {

    if (mapPolaziste != undefined) {
        mapPolaziste.off();
        mapPolaziste.remove(); 
    }

    mapPolaziste = L.map('mapPolaziste').setView([44.60, 16.70], 5);

    mapPolaziste.on('click', function (e) {
        let lokacija = new Lokacija(e.latlng.lat,e.latlng.lng);
        SpremiLokacijuPolaziste(lokacija);
        L.popup()
            .setLatLng(e.latlng)
            .setContent("Lokacija polazišta postavljena na " + e.latlng.toString())
            .openOn(mapPolaziste);
    });

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoiYXN1cGFuIiwiYSI6ImNrajhyMms5NjA4c2UyeXBldWxpaTEzN3kifQ.lk1KeaqGyg-Z0tJv-3J9FQ'
    }).addTo(mapPolaziste);
}

function inicijalizirajMapuOdrediste() {


    if (mapOdrediste != undefined) {
        mapOdrediste.off();
        mapOdrediste.remove();  
    }

    mapOdrediste = L.map('mapOdrediste').setView([44.60, 16.70], 5);

    mapOdrediste.on('click', function (e) {
        let lokacija = new Lokacija(e.latlng.lat, e.latlng.lng);
        SpremiLokacijuOdrediste(lokacija);
        L.popup()
            .setLatLng(e.latlng)
            .setContent("Lokacija odredišta postavljena na " + e.latlng.toString())
            .openOn(mapOdrediste);
    });

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoiYXN1cGFuIiwiYSI6ImNrajhyMms5NjA4c2UyeXBldWxpaTEzN3kifQ.lk1KeaqGyg-Z0tJv-3J9FQ'
    }).addTo(mapOdrediste);
}

function inicijalizirajMapuRuta() {
    if (mapRoute != undefined) {
        mapRoute.off();
        mapRoute.remove(); 
    }

    mapRoute = L.map('mapRoute').setView([44.60, 16.70], 5);

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoiYXN1cGFuIiwiYSI6ImNrajhyMms5NjA4c2UyeXBldWxpaTEzN3kifQ.lk1KeaqGyg-Z0tJv-3J9FQ'
    }).addTo(mapRoute);

    L.Routing.control({
        waypoints: [
            L.latLng(_lokacijaPolaziste.lat, _lokacijaPolaziste.lng),
            L.latLng(_lokacijaOdrediste.lat, _lokacijaOdrediste.lng)
        ]
    })
    .on('routeselected', function (e) {
        var route = e.route;
        var ruta = new Ruta(route.name.split(',')[0], route.name.split(',')[1].trim(), route.summary.totalDistance, route.summary.totalTime);
        SpremiRutu(ruta);
        getRoute();
    })
    .addTo(mapRoute);
}

function dohvatiVoznjeURadiusu(sveLokacije, lokacija) {
    console.log(sveLokacije);
    console.log(lokacija);

    var inRange = [], latlng_a = new L.LatLng(lokacija.lat, lokacija.lng), latlng_b;

    sveLokacije.forEach(function (location) {
        if (location != undefined) {

            latlng_b = new L.LatLng(location.lat, location.lng);
            if (latlng_a.distanceTo(latlng_b) < 50000) {
                inRange.push(location);
            }
        }
    });

    return inRange;
}