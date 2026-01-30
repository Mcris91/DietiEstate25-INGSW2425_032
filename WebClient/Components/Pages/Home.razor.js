let map;
let markerGroup;
let marker;
function clearExistingMap(elementId) {
    const container = document.getElementById(elementId);
    if (!container) return false;
    if (map) {
        map.remove();
        map = null;
    }
    return true;
}
export function setupMap(elementId, lat, lon){
    return new Promise((resolve, reject) => {
        if (!clearExistingMap(elementId)) {
            reject("Container non trovato");
            return;
        }

        if (lat != null && lon != null) {
            map = L.map(elementId).setView([lat, lon], 15);
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '© OpenStreetMap'
            }).addTo(map);

            markerGroup = L.layerGroup().addTo(map);
            marker = L.marker([lat, lon]).addTo(map);
            resolve();
        }
        else {
            let defaultLat = 41.9028, defaultLon = 12.4964;

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(
                    (position) => {
                        map = L.map(elementId).setView([position.coords.latitude, position.coords.longitude], 15);
                        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(map);
                        markerGroup = L.layerGroup().addTo(map);
                        marker = L.marker([position.coords.latitude, position.coords.longitude]).addTo(map);
                        resolve();
                    },
                    () => {
                        map = L.map(elementId).setView([defaultLat, defaultLon], 15);
                        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(map);
                        markerGroup = L.layerGroup().addTo(map);
                        marker = L.marker([defaultLat, defaultLon]).addTo(map);
                        resolve();
                    },
                    { timeout: 7000 }
                );
            } else {
                map = L.map(elementId).setView([defaultLat, defaultLon], 15);
                L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(map);
                markerGroup = L.layerGroup().addTo(map);
                marker = L.marker([defaultLat, defaultLon]).addTo(map);
                resolve();
            }
        }
    });
}

export async function searchAddress (query) {
    if (!map) return;
    try {
        const response = await fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${query}`);
        const data = await response.json();
        if (data && data.length > 0) {
            const lat = parseFloat(data[0].lat);
            const lon = parseFloat(data[0].lon);
            map.setView([lat, lon], 15);
            marker.setLatLng([lat, lon]); 
        } else {
            alert("Indirizzo non trovato");
        }
    } catch (exception) {
        console.error("Errore nella ricerca dell'indirizzo:");
    }
}

export function addMarker (elementId, lat, lon, title, catlistingId){
    if (map) {
        const newMarker = L.marker([lat, lon]);

        newMarker.bindTooltip(title, {
            permanent: false,
            direction: 'top'
        });

        newMarker.on('click', function() {
            window.location.href = `/ListingPage/${catlistingId}`;
        });

        newMarker.addTo(markerGroup);
    }
}

export async function removeAllMarkers(elementId){
    if (markerGroup) {
        markerGroup.clearLayers();
    }
}
    