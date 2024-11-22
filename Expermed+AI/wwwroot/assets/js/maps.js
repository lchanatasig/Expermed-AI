document.addEventListener('DOMContentLoaded', function () {
    const apiKey = 'a20088f849c74d5c895d7546a733315b';
    let map, marker;
    let isMapVisible = false;

    // Función para inicializar el mapa
    function inicializarMapa() {
        map = L.map('map').setView([37.7749, -122.4194], 13);
        L.tileLayer(`https://maps.geoapify.com/v1/tile/carto/{z}/{x}/{y}.png?apiKey=${apiKey}`, {
            attribution: '© OpenStreetMap contributors',
            maxZoom: 19,
        }).addTo(map);

        // Selección manual de ubicación en el mapa
        map.on('click', async function (e) {
            const lat = e.latlng.lat;
            const lon = e.latlng.lng;

            const response = await fetch(`https://api.geoapify.com/v1/geocode/reverse?lat=${lat}&lon=${lon}&apiKey=${apiKey}`);
            const data = await response.json();

            if (data.features.length > 0) {
                const direccion = data.features[0].properties.formatted;
                document.getElementById('direccionInput').value = direccion;

                if (marker) {
                    map.removeLayer(marker);
                }
                marker = L.marker([lat, lon]).addTo(map);
                marker.bindPopup(`<b>Dirección seleccionada:</b> ${direccion}`).openPopup();
            }
        });
    }

    // Función para buscar dirección
    async function buscarDireccion(direccion) {
        const url = `https://api.geoapify.com/v1/geocode/search?text=${direccion}&apiKey=${apiKey}`;
        const response = await fetch(url);
        const data = await response.json();

        if (data.features.length > 0) {
            const location = data.features[0].geometry.coordinates;
            const lat = location[1];
            const lon = location[0];

            map.setView([lat, lon], 13);
            if (marker) {
                map.removeLayer(marker);
            }
            marker = L.marker([lat, lon]).addTo(map);
            marker.bindPopup(`<b>Dirección:</b> ${data.features[0].properties.formatted}`).openPopup();
        }
    }

    // Función para obtener sugerencias de autocompletado
    async function obtenerSugerencias(direccion) {
        const response = await fetch(`https://api.geoapify.com/v1/geocode/autocomplete?text=${direccion}&apiKey=${apiKey}`);
        const data = await response.json();

        const suggestionsContainer = document.getElementById('suggestions');
        suggestionsContainer.innerHTML = '';
        if (data.features.length > 0) {
            suggestionsContainer.style.display = 'block';
            data.features.forEach(feature => {
                const suggestionItem = document.createElement('div');
                suggestionItem.className = 'suggestion-item';
                suggestionItem.textContent = feature.properties.formatted;
                suggestionItem.onclick = function () {
                    document.getElementById('direccionInput').value = feature.properties.formatted;
                    suggestionsContainer.style.display = 'none';
                    buscarDireccion(feature.properties.formatted);
                };
                suggestionsContainer.appendChild(suggestionItem);
            });
        } else {
            suggestionsContainer.style.display = 'none';
        }
    }

    // Mostrar mapa al hacer clic en el ícono
    document.getElementById('locationIcon').addEventListener('click', function () {
        const mapElement = document.getElementById('map');
        if (!isMapVisible) {
            if (!map) {
                inicializarMapa();
            }
            mapElement.style.display = 'block';
            isMapVisible = true;
        } else {
            mapElement.style.display = 'none';
            isMapVisible = false;
        }
    });

    // Evento de entrada para la búsqueda de dirección
    document.getElementById('direccionInput').addEventListener('input', function () {
        const direccion = this.value;
        if (direccion.length > 3) {
            obtenerSugerencias(direccion);
        } else {
            document.getElementById('suggestions').style.display = 'none';
        }
    });

    // Cerrar sugerencias si se hace clic fuera
    document.addEventListener('click', function (event) {
        const suggestionsContainer = document.getElementById('suggestions');
        if (!suggestionsContainer.contains(event.target) && event.target.id !== 'direccionInput') {
            suggestionsContainer.style.display = 'none';
        }
    });
});
