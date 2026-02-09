let viewMap;
let viewMarkerGroup;
let viewMarker;

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
    if (viewMap) {
        viewMap.remove();
        viewMap = null;
    }
    return true;
}

export function setupMap(elementId, lat, lon) {
    if (!clearExistingMap(elementId)) return;

    viewMap = L.map(elementId).setView([lat, lon], 15);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap'
    }).addTo(viewMap);
    viewMarkerGroup = L.layerGroup().addTo(viewMap);
    viewMarker = L.marker([lat, lon], {icon: redMarkerIcon}).addTo(viewMap);
}

export function addMarker (elementId, services){
    if (viewMap) {
        services.forEach(service => {
            if (service.latitude && service.longitude) {
                const newMarker = L.marker([service.latitude, service.longitude]);

                newMarker.bindTooltip(service.name, {
                    permanent: false,
                    direction: 'top'
                });
                newMarker.addTo(viewMarkerGroup);
            }
        });
    }
}