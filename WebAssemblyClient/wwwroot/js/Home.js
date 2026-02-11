let map;
let markerGroup;
let marker;

var redMarkerIcon = L.icon({
    iconUrl: 'assets/icons/red-marker.png',
    iconSize: [38, 95],
    iconAnchor: [22, 94],
    popupAnchor: [-3, -76],
    shadowUrl: 'assets/icons/red-marker-shadow.png',
    shadowSize: [68, 95],
    shadowAnchor: [22, 94]
});
export function clearExistingMap(elementId) {
    const container = document.getElementById(elementId);
    if (!container) return false;
    if (map) {
        map.remove();
        map = null;
    }
    return true;
}
export function getUserLocation() {
    return new Promise((resolve) => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    resolve([ position.coords.latitude, position.coords.longitude ]);
                },
                () => resolve(null),
                { timeout: 5000 }
            );
        } else {
            resolve(null);
        }
    });
}

export function setupMap(elementId, lat, lon) {
    if (!clearExistingMap(elementId)) return;

    map = L.map(elementId).setView([lat, lon], 15);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap'
    }).addTo(map);
    markerGroup = L.layerGroup().addTo(map);
    marker = L.marker([lat, lon], {icon: redMarkerIcon}).addTo(map);
}

export async function searchAddress (address) {
    if (!map) return null;
    try {
        const response = await fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${address}`);
        const data = await response.json();
        if (data && data.length > 0) {
            const lat = parseFloat(data[0].lat);
            const lon = parseFloat(data[0].lon);
            map.setView([lat, lon], 15);
            marker.setLatLng([lat, lon]);
            return [lat, lon];
        } else {
            alert("Indirizzo non trovato");
            return null;
        }
    } catch (exception) {
        console.error("Errore nella ricerca dell'indirizzo:");
        return null;
    }
}

export function addMarker (elementId, lat, lon, title, listingId){
    if (map) {
        const newMarker = L.marker([lat, lon]);

        newMarker.bindTooltip(title, {
            permanent: false,
            direction: 'top'
        });

        newMarker.on('click', function() {
            window.location.href = `/View/${listingId}`;
        });

        newMarker.addTo(markerGroup);
    }
}

export async function removeAllMarkers(elementId){
    if (markerGroup) {
        markerGroup.clearLayers();
    }
}
    