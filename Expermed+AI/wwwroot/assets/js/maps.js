document.addEventListener('DOMContentLoaded', function () {
	const apiKey = 'a20088f849c74d5c895d7546a733315b';
	let map, marker;
	let userLocation = null;
	let isMapVisible = false; // Estado para controlar la visibilidad del mapa

	// Función para inicializar el mapa
	function inicializarMapa() {
		map = L.map('map').setView([37.7749, -122.4194], 13); // Crear el mapa y establecer coordenadas iniciales
		L.tileLayer(`https://maps.geoapify.com/v1/tile/carto/{z}/{x}/{y}.png?apiKey=${apiKey}`, {
			attribution: '© OpenStreetMap contributors',
			maxZoom: 19,
		}).addTo(map);

		// Habilitar selección manual de ubicación en el mapa
		map.on('click', async function (e) {
			const lat = e.latlng.lat; // Latitud del punto clicado
			const lon = e.latlng.lng; // Longitud del punto clicado
			// Buscar la dirección de la ubicación seleccionada
			const response = await fetch(`https://api.geoapify.com/v1/geocode/reverse?lat=${lat}&lon=${lon}&apiKey=${apiKey}`);
			const data = await response.json();

			if (data.features.length > 0) {
				const direccion = data.features[0].properties.formatted; // Obtener la dirección formateada
				// Establecer el valor del input con la dirección seleccionada
				document.getElementById('addressInput').value = direccion;

				// Si ya existe un marcador, lo eliminamos
				if (marker) {
					map.removeLayer(marker);
				}
				// Añadir un nuevo marcador en la ubicación seleccionada
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
				inicializarMapa(); // Inicializar el mapa si no está ya inicializado
			}
			mapElement.style.display = 'block'; // Mostrar el mapa
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
