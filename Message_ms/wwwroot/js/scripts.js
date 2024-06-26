document.addEventListener('DOMContentLoaded', function () {
    const alertForm = document.getElementById('alert-form');
    const searchForm = document.getElementById('search-form');
    const alertsList = document.getElementById('alerts-list');
    const searchResult = document.getElementById('search-result');

    // Función para crear o actualizar una alerta
    alertForm.addEventListener('submit', async function (event) {
        event.preventDefault();

        const formData = new FormData(alertForm);
        const date = formData.get('date');
        const level = formData.get('level');
        const message = formData.get('message');
        const userId = formData.get('userId');
        const eventId = formData.get('eventId');
        const alertId = formData.get('alert-id');

        const newAlert = {
            date: date,
            level: parseInt(level), // Convertir a entero
            message: message,
            userId: userId,
            eventId: eventId
        };

        let url = '/api/alert';
        let method = 'POST';

        if (alertId) {
            url += `/${alertId}`;
            method = 'PUT';
        }

        try {
            const response = await fetch(url, {
                method: method,
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newAlert)
            });

            if (!response.ok) {
                throw new Error('Error al guardar la alerta.');
            }

            const savedAlert = await response.json();
            displayAlert(savedAlert);
            alertForm.reset();
        } catch (error) {
            console.error('Error:', error);
        }
    });

    // Función para buscar una alerta por ID
    searchForm.addEventListener('submit', async function (event) {
        event.preventDefault();

        const searchId = document.getElementById('search-id').value;

        try {
            const response = await fetch(`/api/alert/${searchId}`);
            if (!response.ok) {
                throw new Error('Alerta no encontrada.');
            }

            const alert = await response.json();
            searchResult.innerHTML = ''; // Limpiar resultado anterior
            displayAlert(alert);
        } catch (error) {
            console.error('Error:', error);
        }
    });

    // Función para mostrar una alerta en la lista
    function displayAlert(alert) {
        const alertItem = document.createElement('li');
        alertItem.innerHTML = `
            <strong>ID:</strong> ${alert.id}<br>
            <strong>Fecha:</strong> ${new Date(alert.date).toLocaleString()}<br>
            <strong>Nivel:</strong> ${alert.level}<br>
            <strong>Mensaje:</strong> ${alert.message}<br>
            <strong>ID de Usuario:</strong> ${alert.userId}<br>
            <strong>ID de Evento:</strong> ${alert.eventId}<br>
            <button onclick="editAlert('${alert.id}')">Editar</button>
            <button onclick="deleteAlert('${alert.id}')">Eliminar</button>
        `;
        alertsList.appendChild(alertItem);
    }

    // Función para eliminar una alerta
    async function deleteAlert(id) {
        try {
            const response = await fetch(`/api/alert/${id}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                throw new Error('Error al eliminar la alerta.');
            }

            const result = await response.json();
            alertsList.innerHTML = ''; // Limpiar la lista
            result.message && alertsList.appendChild(document.createTextNode(result.message));
        } catch (error) {
            console.error('Error:', error);
        }
    }

    // Función para editar una alerta
    async function editAlert(id) {
        // Implementar la lógica para editar una alerta
        try {
            const response = await fetch(`/api/alert/${id}`);
            if (!response.ok) {
                throw new Error('Alerta no encontrada.');
            }

            const alert = await response.json();

            // Llenar el formulario con los datos de la alerta a editar
            document.getElementById('alert-id').value = alert.id;
            document.getElementById('date').value = alert.date; // Ajustar según el formato esperado
            document.getElementById('level').value = alert.level.toString();
            document.getElementById('message').value = alert.message;
            document.getElementById('userId').value = alert.userId;
            document.getElementById('eventId').value = alert.eventId;
        } catch (error) {
            console.error('Error:', error);
        }
    }

    // Cargar la lista de alertas al cargar la página
    async function loadAlerts() {
        try {
            const response = await fetch('/api/alert');
            if (!response.ok) {
                throw new Error('Error al cargar las alertas.');
            }

            const alerts = await response.json();
            alertsList.innerHTML = ''; // Limpiar la lista antes de llenarla

            alerts.forEach(alert => {
                displayAlert(alert);
            });
        } catch (error) {
            console.error('Error:', error);
        }
    }

    loadAlerts(); // Cargar las alertas al iniciar la página
});
