// Dashboard Logic
document.addEventListener('DOMContentLoaded', async function() {
    requireAuth();

    const adminNameEl = document.getElementById('adminName');
    const logoutBtn = document.getElementById('logoutBtn');

    // Display current user
    const user = getCurrentUser();
    if (user && adminNameEl) {
        adminNameEl.textContent = user.name || 'Admin User';
    }

    // Logout handler
    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            if (confirm('Are you sure you want to logout?')) {
                AuthAPI.logout();
            }
        });
    }

    // Load dashboard statistics
    await loadDashboardStats();
});

async function loadDashboardStats() {
    try {
        // For demo purposes, we'll use mock data
        // In production, replace with: const stats = await DashboardAPI.stats();
        
        const stats = {
            totalAudioFiles: 8,
            activeSchedules: 5,
            totalBuses: 12,
            upcomingAnnouncements: 3,
        };

        document.getElementById('totalAudioFiles').textContent = stats.totalAudioFiles;
        document.getElementById('activeSchedules').textContent = stats.activeSchedules;
        document.getElementById('totalBuses').textContent = stats.totalBuses;
        document.getElementById('upcomingAnnouncements').textContent = stats.upcomingAnnouncements;
    } catch (error) {
        console.error('Failed to load dashboard stats:', error);
    }
}
