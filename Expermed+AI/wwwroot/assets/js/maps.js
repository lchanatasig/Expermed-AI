document.addEventListener('DOMContentLoaded', function () {
    const apiKey = 'a20088f849c74d5c895d7546a733315b'; // Reemplaza con tu clave de API
    let map, marker;
    let userLocation = null;
    let isMapVisible = false;

    // Función para inicializar el mapa
    function inicializarMapa() {
        map = L.map('map').setView([37.7749, -122.4194], 13);
        L.tileLayer(`https://maps.geoapify.com/v1/tile/carto/{z}/{x}/{y}.png?apiKey=${apiKey}`, {
            attribution: '© OpenStreetMap contributors',
            maxZoom: 19,
        }).addTo(map);

        // Habilitar selección manual de ubicación en el mapa
        map.on('click', async function (e) {
            const lat = e.latlng.lat;
            const lon = e.latlng.lng;

            // Mostrar latitud y longitud
            document.getElementById('latInput').value = lat.toFixed(6);
            document.getElementById('lonInput').value = lon.toFixed(6);

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

    // Función para obtener la ubicación actual
    function obtenerUbicacionActual() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const lat = position.coords.latitude;
                    const lon = position.coords.longitude;
                    userLocation = [lon, lat];

                    // Mostrar latitud y longitud
                    document.getElementById('latInput').value = lat.toFixed(6);
                    document.getElementById('lonInput').value = lon.toFixed(6);

                    map.setView([lat, lon], 13);
                    if (marker) {
                        map.removeLayer(marker);
                    }
                    marker = L.marker([lat, lon]).addTo(map);
                    marker.bindPopup('Ubicación actual').openPopup();
                },
                (error) => {
                    console.error('Error al obtener la ubicación:', error);
                }
            );
        } else {
            alert('Geolocalización no es soportada por este navegador.');
        }
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

            // Mostrar latitud y longitud
            document.getElementById('latInput').value = lat.toFixed(6);
            document.getElementById('lonInput').value = lon.toFixed(6);

            map.setView([lat, lon], 13);
            if (marker) {
                map.removeLayer(marker);
            }
            marker = L.marker([lat, lon]).addTo(map);
            marker.bindPopup(`<b>Dirección:</b> ${data.features[0].properties.formatted}`).openPopup();
        }
    }

    // Evento para alternar el mapa y obtener la ubicación al hacer clic en el ícono
    document.getElementById('locationIcon').addEventListener('click', function () {
        const mapElement = document.getElementById('map');
        if (!isMapVisible) {
            if (!map) {
                inicializarMapa();
            }
            mapElement.style.display = 'block';
            isMapVisible = true;

            const direccion = document.getElementById('direccionInput').value.trim();
            if (direccion === '') {
                obtenerUbicacionActual(); // Obtener ubicación actual si el input está vacío
            } else {
                buscarDireccion(direccion); // Buscar la dirección si hay un valor en el input
            }
        } else {
            mapElement.style.display = 'none'; // Ocultar el mapa
            isMapVisible = false;
        }
    });

    // Evento para mostrar el menú de compartir
    document.getElementById('shareLocationBtn').addEventListener('click', function (event) {
        const lat = document.getElementById('latInput').value;
        const lon = document.getElementById('lonInput').value;

        // Muestra el menú de compartir en la posición del ratón
        const shareMenu = document.getElementById('shareMenu');
        shareMenu.style.display = 'block';
        shareMenu.style.left = `${event.pageX}px`;
        shareMenu.style.top = `${event.pageY}px`;
    });

    // Ocultar el menú de compartir si se hace clic fuera de él
    document.addEventListener('click', function (event) {
        const shareMenu = document.getElementById('shareMenu');
        if (!shareMenu.contains(event.target) && event.target.id !== 'shareLocationBtn') {
            shareMenu.style.display = 'none';
        }
    });

    // Compartir como enlace
    document.getElementById('shareLinkBtn').addEventListener('click', function () {
        const lat = document.getElementById('latInput').value;
        const lon = document.getElementById('lonInput').value;
        const googleMapsUrl = `https://www.google.com/maps/?q=${lat},${lon}`;

        window.open(googleMapsUrl, '_blank');
    });
    // Enviar por mensaje (WhatsApp)
    document.getElementById('shareMessageBtn').addEventListener('click', function () {
        const lat = document.getElementById('latInput').value;
        const lon = document.getElementById('lonInput').value;
        const message = `Aquí está mi ubicación: https://www.google.com/maps/?q=${lat},${lon}`;
        const url = `whatsapp://send?text=${encodeURIComponent(message)}`;
        window.open(url);
    });

    // Enviar por correo electrónico (Outlook)
    document.getElementById('shareEmailBtn').addEventListener('click', function () {
        const lat = document.getElementById('latInput').value;
        const lon = document.getElementById('lonInput').value;
        const subject = encodeURIComponent('Mi ubicación');
        const body = encodeURIComponent(`Aquí está mi ubicación: https://www.google.com/maps/?q=${lat},${lon}`);

        // Usar el esquema de Outlook
        const outlookLink = `ms-outlook://compose?subject=${subject}&body=${body}`;
        window.location.href = outlookLink; // Intentar abrir la app de Outlook
    });
    // Función para obtener sugerencias de autocompletado
    async function obtenerSugerencias(direccion) {
        const bias = userLocation ? `${userLocation[0]},${userLocation[1]}` : undefined;
        const response = await fetch(`https://api.geoapify.com/v1/geocode/autocomplete?text=${direccion}${bias ? '&bias=proximity:' + bias : ''}&apiKey=${apiKey}`);
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
