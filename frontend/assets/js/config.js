// Configuration and constants
const API_BASE_URL = 'http://localhost:5000/api';
const TOKEN_KEY = 'authToken';
const USER_KEY = 'currentUser';

// Helper to get token from localStorage
function getToken() {
    return localStorage.getItem(TOKEN_KEY);
}

// Helper to set token in localStorage
function setToken(token) {
    localStorage.setItem(TOKEN_KEY, token);
}

// Helper to get current user
function getCurrentUser() {
    const user = localStorage.getItem(USER_KEY);
    return user ? JSON.parse(user) : null;
}

// Helper to set current user
function setCurrentUser(user) {
    localStorage.setItem(USER_KEY, JSON.stringify(user));
}

// Helper to clear authentication
function clearAuth() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
}

// Check if user is authenticated
function isAuthenticated() {
    return !!getToken();
}

// Redirect to login if not authenticated
function requireAuth() {
    if (!isAuthenticated()) {
        window.location.href = 'index.html';
    }
}
