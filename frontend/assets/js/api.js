// API Helper Functions
class APIClient {
    static async request(endpoint, options = {}) {
        const headers = {
            'Content-Type': 'application/json',
            ...options.headers,
        };

        const token = getToken();
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        const url = `${API_BASE_URL}${endpoint}`;
        
        try {
            const response = await fetch(url, {
                ...options,
                headers,
            });

            if (response.status === 401) {
                clearAuth();
                window.location.href = 'index.html';
                return null;
            }

            if (!response.ok) {
                const error = await response.json();
                throw new Error(error.message || `HTTP ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    }

    static get(endpoint) {
        return this.request(endpoint, { method: 'GET' });
    }

    static post(endpoint, data) {
        return this.request(endpoint, { method: 'POST', body: JSON.stringify(data) });
    }

    static put(endpoint, data) {
        return this.request(endpoint, { method: 'PUT', body: JSON.stringify(data) });
    }

    static delete(endpoint) {
        return this.request(endpoint, { method: 'DELETE' });
    }

    static async uploadFile(endpoint, file) {
        const formData = new FormData();
        formData.append('file', file);

        const headers = {};
        const token = getToken();
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'POST',
            headers,
            body: formData,
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || `HTTP ${response.status}`);
        }

        return await response.json();
    }
}

// Auth API
const AuthAPI = {
    login: (email, password) => APIClient.post('/auth/login', { email, password }),
    logout: () => {
        clearAuth();
        window.location.href = 'index.html';
    },
};

// Audio Files API
const AudioAPI = {
    list: () => APIClient.get('/audio'),
    get: (id) => APIClient.get(`/audio/${id}`),
    create: (data) => APIClient.post('/audio', data),
    update: (id, data) => APIClient.put(`/audio/${id}`, data),
    delete: (id) => APIClient.delete(`/audio/${id}`),
    upload: (file) => APIClient.uploadFile('/audio/upload', file),
};

// Schedules API
const ScheduleAPI = {
    list: () => APIClient.get('/schedules'),
    get: (id) => APIClient.get(`/schedules/${id}`),
    create: (data) => APIClient.post('/schedules', data),
    update: (id, data) => APIClient.put(`/schedules/${id}`, data),
    delete: (id) => APIClient.delete(`/schedules/${id}`),
};

// Buses API
const BusAPI = {
    list: () => APIClient.get('/buses'),
    get: (id) => APIClient.get(`/buses/${id}`),
    create: (data) => APIClient.post('/buses', data),
    update: (id, data) => APIClient.put(`/buses/${id}`, data),
    delete: (id) => APIClient.delete(`/buses/${id}`),
};

// Routes API
const RouteAPI = {
    list: () => APIClient.get('/routes'),
    get: (id) => APIClient.get(`/routes/${id}`),
    create: (data) => APIClient.post('/routes', data),
    update: (id, data) => APIClient.put(`/routes/${id}`, data),
    delete: (id) => APIClient.delete(`/routes/${id}`),
};

// Emergency API
const EmergencyAPI = {
    broadcast: (data) => APIClient.post('/emergency/broadcast', data),
    history: () => APIClient.get('/emergency/history'),
};

// Dashboard API
const DashboardAPI = {
    stats: () => APIClient.get('/dashboard/stats'),
};

// Show toast notification
function showToast(message, type = 'success') {
    const toast = document.createElement('div');
    const bgColor = type === 'success' ? 'bg-green-500' : type === 'error' ? 'bg-red-500' : 'bg-blue-500';
    
    toast.className = `fixed bottom-4 right-4 ${bgColor} text-white px-6 py-3 rounded-lg shadow-lg z-50`;
    toast.textContent = message;
    document.body.appendChild(toast);

    setTimeout(() => {
        toast.remove();
    }, 3000);
}

// Show error message
function showError(message) {
    showToast(message, 'error');
}

// Show success message
function showSuccess(message) {
    showToast(message, 'success');
}
