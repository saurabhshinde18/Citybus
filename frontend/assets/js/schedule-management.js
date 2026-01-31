// Schedule Management Logic
document.addEventListener('DOMContentLoaded', async function() {
    requireAuth();

    const createScheduleBtn = document.getElementById('createScheduleBtn');
    const scheduleModal = document.getElementById('scheduleModal');
    const closeModalBtn = document.getElementById('closeModalBtn');
    const cancelBtn = document.getElementById('cancelBtn');
    const scheduleForm = document.getElementById('scheduleForm');
    const logoutBtn = document.getElementById('logoutBtn');

    // Display current user
    const user = getCurrentUser();
    if (user) {
        document.getElementById('adminName').textContent = user.name || 'Admin User';
    }

    // Modal handlers
    if (createScheduleBtn) {
        createScheduleBtn.addEventListener('click', openScheduleModal);
    }

    if (closeModalBtn) {
        closeModalBtn.addEventListener('click', closeScheduleModal);
    }

    if (cancelBtn) {
        cancelBtn.addEventListener('click', closeScheduleModal);
    }

    // Close modal when clicking outside
    if (scheduleModal) {
        scheduleModal.addEventListener('click', (e) => {
            if (e.target === scheduleModal) {
                closeScheduleModal();
            }
        });
    }

    // Form submission
    if (scheduleForm) {
        scheduleForm.addEventListener('submit', handleScheduleSubmit);
    }

    // Logout
    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            if (confirm('Are you sure you want to logout?')) {
                AuthAPI.logout();
            }
        });
    }

    // Load initial data
    await loadAudioOptions();
    await loadBusOptions();
    await loadSchedules();
});

function openScheduleModal() {
    document.getElementById('scheduleModal').classList.add('active');
    document.getElementById('modalTitle').textContent = 'Create New Schedule';
}

function closeScheduleModal() {
    document.getElementById('scheduleModal').classList.remove('active');
    document.getElementById('scheduleForm').reset();
}

async function loadAudioOptions() {
    try {
        // Mock data for demo
        const audioFiles = [
            { id: 1, name: 'Route 5 Announcement' },
            { id: 2, name: 'Emergency Alert Siren' },
            { id: 3, name: 'Festival Message' },
        ];

        const select = document.getElementById('audioSelect');
        select.innerHTML = '<option value="">Select an audio file</option>' +
            audioFiles.map(f => `<option value="${f.id}">${f.name}</option>`).join('');
    } catch (error) {
        console.error('Failed to load audio options:', error);
    }
}

async function loadBusOptions() {
    try {
        // Mock data for demo
        const buses = [
            { id: 1, number: 'B001', name: 'Bus 001' },
            { id: 2, number: 'B002', name: 'Bus 002' },
            { id: 3, number: 'B003', name: 'Bus 003' },
        ];

        const select = document.getElementById('busSelect');
        select.innerHTML = '<option value="">Select a bus</option>' +
            buses.map(b => `<option value="${b.id}">${b.number} - Route ${b.id}</option>`).join('');
    } catch (error) {
        console.error('Failed to load bus options:', error);
    }
}

async function handleScheduleSubmit(e) {
    e.preventDefault();

    const formData = {
        audioFileId: document.getElementById('audioSelect').value,
        busId: document.getElementById('busSelect').value,
        startDate: document.getElementById('startDate').value,
        startTime: document.getElementById('startTime').value,
        repeatPattern: document.getElementById('repeatSelect').value,
        isActive: document.getElementById('isActive').checked,
    };

    if (!formData.audioFileId || !formData.busId || !formData.startDate || !formData.startTime) {
        showError('Please fill in all required fields');
        return;
    }

    try {
        // In production: const response = await ScheduleAPI.create(formData);
        showSuccess('Schedule created successfully!');
        closeScheduleModal();
        await loadSchedules();
    } catch (error) {
        showError(`Failed to create schedule: ${error.message}`);
    }
}

async function loadSchedules() {
    try {
        // Mock data for demo
        const schedules = [
            {
                id: 1,
                audioName: 'Route 5 Announcement',
                busNumber: 'B001',
                dateTime: '2026-01-31 08:00 AM',
                repeat: 'Daily',
                status: 'Active',
            },
            {
                id: 2,
                audioName: 'Festival Message',
                busNumber: 'B002',
                dateTime: '2026-02-01 10:00 AM',
                repeat: 'Weekly',
                status: 'Active',
            },
        ];

        displaySchedules(schedules);
    } catch (error) {
        console.error('Failed to load schedules:', error);
        showError('Failed to load schedules');
    }
}

function displaySchedules(schedules) {
    const schedulesList = document.getElementById('schedulesList');
    const emptyState = document.getElementById('emptyState');

    if (schedules.length === 0) {
        schedulesList.innerHTML = '';
        emptyState.classList.remove('hidden');
        return;
    }

    emptyState.classList.add('hidden');
    schedulesList.innerHTML = schedules.map(schedule => `
        <div class="bg-white rounded-lg border border-gray-200 p-6 hover:shadow-md transition">
            <div class="flex items-start justify-between mb-4">
                <div>
                    <h3 class="text-lg font-bold text-gray-900">${escapeHtml(schedule.audioName)}</h3>
                    <p class="text-sm text-gray-600 mt-1">Bus: ${schedule.busNumber}</p>
                </div>
                <span class="px-3 py-1 inline-flex text-xs leading-5 font-semibold rounded-full ${schedule.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'}">
                    ${schedule.status}
                </span>
            </div>

            <div class="grid grid-cols-2 gap-4 mb-4 text-sm">
                <div>
                    <p class="text-gray-600">Scheduled Time</p>
                    <p class="font-medium text-gray-900">${schedule.dateTime}</p>
                </div>
                <div>
                    <p class="text-gray-600">Repeat Pattern</p>
                    <p class="font-medium text-gray-900">${schedule.repeat}</p>
                </div>
            </div>

            <div class="flex space-x-3">
                <button class="flex-1 px-3 py-2 text-sm font-medium text-gray-700 border border-gray-300 rounded-lg hover:bg-gray-50" onclick="editSchedule(${schedule.id})">
                    Edit
                </button>
                <button class="flex-1 px-3 py-2 text-sm font-medium text-red-600 border border-red-300 rounded-lg hover:bg-red-50" onclick="deleteSchedule(${schedule.id})">
                    Delete
                </button>
            </div>
        </div>
    `).join('');
}

function editSchedule(id) {
    showSuccess('Edit functionality coming soon');
}

function deleteSchedule(id) {
    if (confirm('Are you sure you want to delete this schedule?')) {
        showSuccess('Schedule deleted successfully');
        loadSchedules();
    }
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}
