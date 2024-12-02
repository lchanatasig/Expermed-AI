const addressInput = document.getElementById('addressInput');
const suggestionsContainer = document.getElementById('suggestions');
const mapContainer = document.getElementById('map');

const apiKey = 'fc26fc0381bb4bdcb433a1ba63e0dfe3'; // Reemplaza con tu API Key de Geoapify

let userLocation = null; // Guardará la ubicación del usuario

// Obtiene la ubicación del usuario
navigator.geolocation.getCurrentPosition(
    (position) => {
        const { latitude, longitude } = position.coords;
        userLocation = { latitude, longitude };
        console.log('Ubicación del usuario:', userLocation);
    },
    (error) => {
        console.error('No se pudo obtener la ubicación:', error);
        alert('Por favor habilita la ubicación para obtener mejores sugerencias.');
    }
);

// Manejador de eventos para el campo de texto
addressInput.addEventListener('input', async () => {
    const query = addressInput.value.trim();

    if (query.length < 3) {
        suggestionsContainer.style.display = 'none';
        return;
    }

    // Llamada a la API de autocompletado con el contexto de ubicación del usuario
    try {
        let url = `https://api.geoapify.com/v1/geocode/autocomplete?text=${encodeURIComponent(query)}&apiKey=${apiKey}`;
        if (userLocation) {
            url += `&bias=proximity:${userLocation.longitude},${userLocation.latitude}`;
        }

        const response = await fetch(url);
        const data = await response.json();

        // Limpia el contenedor de sugerencias
        suggestionsContainer.innerHTML = '';

        if (data.features && data.features.length > 0) {
            suggestionsContainer.style.display = 'block';

            data.features.forEach((feature) => {
                const suggestionItem = document.createElement('div');
                suggestionItem.classList.add('suggestion-item');
                suggestionItem.textContent = feature.properties.formatted;

                // Al hacer clic en una sugerencia, actualizamos el input y mostramos el mapa
                suggestionItem.addEventListener('click', () => {
                    addressInput.value = feature.properties.formatted;
                    suggestionsContainer.style.display = 'none';
                    showMap(feature.geometry.coordinates);
                });

                suggestionsContainer.appendChild(suggestionItem);
            });
        } else {
            suggestionsContainer.style.display = 'none';
        }
    } catch (error) {
        console.error('Error al obtener sugerencias:', error);
    }
});

// Función para mostrar el mapa
function showMap([lon, lat]) {
    mapContainer.style.display = 'block';
    mapContainer.innerHTML = ''; // Limpia el contenedor del mapa

    const map = L.map('map').setView([lat, lon], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data © <a href="https://openstreetmap.org">OpenStreetMap</a> contributors'
    }).addTo(map);

    L.marker([lat, lon]).addTo(map)
        .bindPopup(`<b>${addressInput.value}</b>`)
        .openPopup();
}
