// Bus Management Logic
document.addEventListener('DOMContentLoaded', async function() {
    requireAuth();

    const addBusBtn = document.getElementById('addBusBtn');
    const addRouteBtn = document.getElementById('addRouteBtn');
    const logoutBtn = document.getElementById('logoutBtn');

    // Display current user
    const user = getCurrentUser();
    if (user) {
        document.getElementById('adminName').textContent = user.name || 'Admin User';
    }

    // Button handlers
    if (addBusBtn) {
        addBusBtn.addEventListener('click', () => {
            showSuccess('Add bus modal will appear here in full implementation');
        });
    }

    if (addRouteBtn) {
        addRouteBtn.addEventListener('click', () => {
            showSuccess('Add route modal will appear here in full implementation');
        });
    }

    // Logout
    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            if (confirm('Are you sure you want to logout?')) {
                AuthAPI.logout();
            }
        });
    }

    // Load data
    await loadBuses();
    await loadRoutes();
});

async function loadBuses() {
    try {
        // Mock data for demo
        const buses = [
            { id: 1, number: 'B001', route: 'Route 1', status: 'Active', capacity: 45 },
            { id: 2, number: 'B002', route: 'Route 2', status: 'Active', capacity: 45 },
            { id: 3, number: 'B003', route: 'Route 5', status: 'Inactive', capacity: 50 },
            { id: 4, number: 'B004', route: 'Route 3', status: 'Active', capacity: 45 },
        ];

        displayBuses(buses);
    } catch (error) {
        console.error('Failed to load buses:', error);
        showError('Failed to load buses');
    }
}

function displayBuses(buses) {
    const tableBody = document.getElementById('busesTableBody');
    const emptyState = document.getElementById('busesEmptyState');

    if (buses.length === 0) {
        tableBody.innerHTML = '';
        emptyState.classList.remove('hidden');
        return;
    }

    emptyState.classList.add('hidden');
    tableBody.innerHTML = buses.map(bus => `
        <tr class="hover:bg-gray-50">
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="text-sm font-bold text-gray-900">${bus.number}</span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="text-sm text-gray-600">${bus.route}</span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="px-3 py-1 inline-flex text-xs leading-5 font-semibold rounded-full ${bus.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'}">
                    ${bus.status}
                </span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="text-sm text-gray-600">${bus.capacity} seats</span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-sm space-x-2">
                <button class="text-blue-600 hover:text-blue-900 font-medium" onclick="editBus(${bus.id})">
                    Edit
                </button>
                <button class="text-red-600 hover:text-red-900 font-medium" onclick="deleteBus(${bus.id})">
                    Delete
                </button>
            </td>
        </tr>
    `).join('');
}

async function loadRoutes() {
    try {
        // Mock data for demo
        const routes = [
            { id: 1, name: 'Route 1', startPoint: 'Central Station', endPoint: 'Airport', distance: '25 km' },
            { id: 2, name: 'Route 2', startPoint: 'Downtown', endPoint: 'University', distance: '15 km' },
            { id: 3, name: 'Route 3', startPoint: 'Shopping Mall', endPoint: 'Hospital', distance: '8 km' },
            { id: 4, name: 'Route 5', startPoint: 'Railway Station', endPoint: 'Bus Terminal', distance: '12 km' },
        ];

        displayRoutes(routes);
    } catch (error) {
        console.error('Failed to load routes:', error);
        showError('Failed to load routes');
    }
}

function displayRoutes(routes) {
    const tableBody = document.getElementById('routesTableBody');
    const emptyState = document.getElementById('routesEmptyState');

    if (routes.length === 0) {
        tableBody.innerHTML = '';
        emptyState.classList.remove('hidden');
        return;
    }

    emptyState.classList.add('hidden');
    tableBody.innerHTML = routes.map(route => `
        <tr class="hover:bg-gray-50">
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="text-sm font-bold text-gray-900">${route.name}</span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="text-sm text-gray-600">${route.startPoint}</span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="text-sm text-gray-600">${route.endPoint}</span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="text-sm text-gray-600">${route.distance}</span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-sm space-x-2">
                <button class="text-blue-600 hover:text-blue-900 font-medium" onclick="editRoute(${route.id})">
                    Edit
                </button>
                <button class="text-red-600 hover:text-red-900 font-medium" onclick="deleteRoute(${route.id})">
                    Delete
                </button>
            </td>
        </tr>
    `).join('');
}

function editBus(id) {
    showSuccess('Edit bus functionality coming soon');
}

function deleteBus(id) {
    if (confirm('Are you sure you want to delete this bus?')) {
        showSuccess('Bus deleted successfully');
        loadBuses();
    }
}

function editRoute(id) {
    showSuccess('Edit route functionality coming soon');
}

function deleteRoute(id) {
    if (confirm('Are you sure you want to delete this route?')) {
        showSuccess('Route deleted successfully');
        loadRoutes();
    }
}
