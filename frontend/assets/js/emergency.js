// Emergency Announcement Logic
document.addEventListener('DOMContentLoaded', async function() {
    requireAuth();

    const emergencyForm = document.getElementById('emergencyForm');
    const broadcastBtn = document.getElementById('broadcastBtn');
    const previewBtn = document.getElementById('previewBtn');
    const busTargetRadios = document.querySelectorAll('input[name="busTarget"]');
    const routeSelector = document.getElementById('routeSelector');
    const logoutBtn = document.getElementById('logoutBtn');

    // Display current user
    const user = getCurrentUser();
    if (user) {
        document.getElementById('adminName').textContent = user.name || 'Admin User';
    }

    // Load audio files for emergency
    await loadEmergencyAudioFiles();
    await loadRoutesForEmergency();
    await loadBroadcastHistory();

    // Bus target selection handler
    busTargetRadios.forEach(radio => {
        radio.addEventListener('change', (e) => {
            if (e.target.value === 'route') {
                routeSelector.classList.remove('hidden');
            } else {
                routeSelector.classList.add('hidden');
            }
        });
    });

    // Form submission
    if (emergencyForm) {
        emergencyForm.addEventListener('submit', handleEmergencyBroadcast);
    }

    // Preview audio
    if (previewBtn) {
        previewBtn.addEventListener('click', previewAudio);
    }

    // Logout
    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            if (confirm('Are you sure you want to logout?')) {
                AuthAPI.logout();
            }
        });
    }
});

async function loadEmergencyAudioFiles() {
    try {
        // Mock data for demo - only emergency category
        const emergencyAudios = [
            { id: 2, name: 'Emergency Alert Siren' },
            { id: 5, name: 'Medical Emergency Alert' },
            { id: 6, name: 'Road Accident Warning' },
        ];

        const select = document.getElementById('emergencyAudioSelect');
        select.innerHTML = '<option value="">Choose an emergency audio file...</option>' +
            emergencyAudios.map(f => `<option value="${f.id}">${f.name}</option>`).join('');
    } catch (error) {
        console.error('Failed to load emergency audio files:', error);
    }
}

async function loadRoutesForEmergency() {
    try {
        // Mock data for demo
        const routes = [
            { id: 1, name: 'Route 1' },
            { id: 2, name: 'Route 2' },
            { id: 3, name: 'Route 5' },
        ];

        const select = document.getElementById('targetRoute');
        select.innerHTML = '<option value="">Select route...</option>' +
            routes.map(r => `<option value="${r.id}">${r.name}</option>`).join('');
    } catch (error) {
        console.error('Failed to load routes:', error);
    }
}

async function handleEmergencyBroadcast(e) {
    e.preventDefault();

    const audioFileId = document.getElementById('emergencyAudioSelect').value;
    const priorityLevel = document.getElementById('priorityLevel').value;
    const busTarget = document.querySelector('input[name="busTarget"]:checked').value;
    const targetRoute = busTarget === 'route' ? document.getElementById('targetRoute').value : null;
    const confirmationMsg = document.getElementById('confirmationMessage').value;

    if (!audioFileId) {
        showError('Please select an audio file');
        return;
    }

    if (busTarget === 'route' && !targetRoute) {
        showError('Please select a route');
        return;
    }

    // Show confirmation
    const targetDesc = busTarget === 'all' ? 'ALL BUSES' : `Route ${targetRoute}`;
    if (!confirm(`⚠️ EMERGENCY BROADCAST CONFIRMATION\n\nTarget: ${targetDesc}\nPriority: ${priorityLevel}\n\nProceed with broadcast?`)) {
        return;
    }

    try {
        // Disable button during broadcast
        document.getElementById('broadcastBtn').disabled = true;
        document.getElementById('broadcastBtn').textContent = 'BROADCASTING...';

        // In production: await EmergencyAPI.broadcast(...)
        // Simulate broadcast
        await new Promise(resolve => setTimeout(resolve, 2000));

        showSuccess('🚨 Emergency broadcast sent to all buses!');
        document.getElementById('emergencyForm').reset();
        await loadBroadcastHistory();
    } catch (error) {
        showError(`Broadcast failed: ${error.message}`);
    } finally {
        document.getElementById('broadcastBtn').disabled = false;
        document.getElementById('broadcastBtn').textContent = 'BROADCAST NOW';
    }
}

function previewAudio() {
    const audioSelect = document.getElementById('emergencyAudioSelect');
    if (!audioSelect.value) {
        showError('Please select an audio file');
        return;
    }
    showSuccess('Playing preview audio...');
}

async function loadBroadcastHistory() {
    try {
        // Mock data for demo
        const history = [
            {
                id: 1,
                audioName: 'Emergency Alert Siren',
                timestamp: new Date(Date.now() - 3600000).toLocaleString(),
                target: 'All Buses',
                status: 'Completed',
            },
            {
                id: 2,
                audioName: 'Medical Emergency Alert',
                timestamp: new Date(Date.now() - 7200000).toLocaleString(),
                target: 'Route 5',
                status: 'Completed',
            },
        ];

        displayBroadcastHistory(history);
    } catch (error) {
        console.error('Failed to load broadcast history:', error);
    }
}

function displayBroadcastHistory(broadcasts) {
    const historyDiv = document.getElementById('broadcastHistory');
    const noBroadcasts = document.getElementById('noBroadcasts');

    if (broadcasts.length === 0) {
        historyDiv.innerHTML = '';
        noBroadcasts.classList.remove('hidden');
        return;
    }

    noBroadcasts.classList.add('hidden');
    historyDiv.innerHTML = broadcasts.map(broadcast => `
        <div class="border-l-4 border-red-500 pl-4 py-3 bg-red-50 rounded">
            <div class="flex items-start justify-between">
                <div>
                    <h4 class="font-bold text-gray-900">${broadcast.audioName}</h4>
                    <p class="text-sm text-gray-600">Target: ${broadcast.target}</p>
                </div>
                <span class="px-3 py-1 bg-green-100 text-green-800 text-xs font-semibold rounded-full">
                    ${broadcast.status}
                </span>
            </div>
            <p class="text-xs text-gray-500 mt-2">${broadcast.timestamp}</p>
        </div>
    `).join('');
}
