// Audio Management Logic
document.addEventListener('DOMContentLoaded', async function() {
    requireAuth();

    const uploadNewBtn = document.getElementById('uploadNewBtn');
    const uploadZone = document.getElementById('uploadZone');
    const closeUploadBtn = document.querySelector('[id="closeModalBtn"]');
    const audioFileInput = document.getElementById('audioFileInput');
    const searchInput = document.getElementById('searchInput');
    const categoryFilter = document.getElementById('categoryFilter');
    const statusFilter = document.getElementById('statusFilter');
    const logoutBtn = document.getElementById('logoutBtn');

    // Display current user
    const user = getCurrentUser();
    if (user) {
        document.getElementById('adminName').textContent = user.name || 'Admin User';
    }

    // Toggle upload zone
    if (uploadNewBtn) {
        uploadNewBtn.addEventListener('click', () => {
            uploadZone.classList.toggle('hidden');
        });
    }

    // Handle file uploads
    if (audioFileInput) {
        audioFileInput.addEventListener('change', handleFileUpload);
    }

    // Drag and drop
    if (uploadZone) {
        uploadZone.addEventListener('dragover', (e) => {
            e.preventDefault();
            uploadZone.classList.add('drag-over');
        });

        uploadZone.addEventListener('dragleave', () => {
            uploadZone.classList.remove('drag-over');
        });

        uploadZone.addEventListener('drop', async (e) => {
            e.preventDefault();
            uploadZone.classList.remove('drag-over');
            const files = e.dataTransfer.files;
            await uploadFiles(files);
        });
    }

    // Search and filter
    if (searchInput) {
        searchInput.addEventListener('input', filterAudioFiles);
    }
    if (categoryFilter) {
        categoryFilter.addEventListener('change', filterAudioFiles);
    }
    if (statusFilter) {
        statusFilter.addEventListener('change', filterAudioFiles);
    }

    // Logout
    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            if (confirm('Are you sure you want to logout?')) {
                AuthAPI.logout();
            }
        });
    }

    // Load audio files
    await loadAudioFiles();
});

async function handleFileUpload(e) {
    const files = e.target.files;
    await uploadFiles(files);
}

async function uploadFiles(files) {
    if (files.length === 0) return;

    const uploadProgress = document.getElementById('uploadProgress');
    const progressBar = document.getElementById('progressBar');
    const progressText = document.getElementById('progressText');

    for (const file of files) {
        if (!file.name.match(/\.(mp3|wav)$/i)) {
            showError(`${file.name} is not a valid audio file (MP3/WAV)`);
            continue;
        }

        if (file.size > 50 * 1024 * 1024) {
            showError(`${file.name} exceeds 50MB limit`);
            continue;
        }

        try {
            uploadProgress.classList.remove('hidden');

            // Simulate upload progress
            let progress = 0;
            const interval = setInterval(() => {
                progress += Math.random() * 30;
                if (progress > 90) progress = 90;
                progressBar.style.width = progress + '%';
                progressText.textContent = Math.round(progress);
            }, 200);

            const response = await AudioAPI.upload(file);

            clearInterval(interval);
            progressBar.style.width = '100%';
            progressText.textContent = '100';

            setTimeout(() => {
                uploadProgress.classList.add('hidden');
                showSuccess(`${file.name} uploaded successfully`);
                loadAudioFiles(); // Refresh list
            }, 500);
        } catch (error) {
            uploadProgress.classList.add('hidden');
            showError(`Failed to upload ${file.name}: ${error.message}`);
        }
    }
}

async function loadAudioFiles() {
    try {
        // For demo, use mock data
        const audioFiles = [
            { id: 1, name: 'Route 5 Announcement', duration: '0:15', category: 'Route', status: 'Active' },
            { id: 2, name: 'Emergency Alert Siren', duration: '0:20', category: 'Emergency', status: 'Active' },
            { id: 3, name: 'Festival Message - Diwali', duration: '0:45', category: 'Festival', status: 'Active' },
            { id: 4, name: 'Commercial - Local Store', duration: '0:30', category: 'Advertisement', status: 'Inactive' },
        ];

        displayAudioFiles(audioFiles);
    } catch (error) {
        console.error('Failed to load audio files:', error);
        showError('Failed to load audio files');
    }
}

function displayAudioFiles(files) {
    const tableBody = document.getElementById('audioTableBody');
    const emptyState = document.getElementById('emptyState');

    if (files.length === 0) {
        tableBody.innerHTML = '';
        emptyState.classList.remove('hidden');
        return;
    }

    emptyState.classList.add('hidden');
    tableBody.innerHTML = files.map(file => `
        <tr class="hover:bg-gray-50">
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="text-sm font-medium text-gray-900">${escapeHtml(file.name)}</span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="text-sm text-gray-600">${file.duration}</span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="px-3 py-1 inline-flex text-xs leading-5 font-semibold rounded-full ${getCategoryBadgeColor(file.category)}">
                    ${file.category}
                </span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
                <span class="px-3 py-1 inline-flex text-xs leading-5 font-semibold rounded-full ${file.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'}">
                    ${file.status}
                </span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-sm space-x-2">
                <button class="text-blue-600 hover:text-blue-900 font-medium" onclick="playAudio(${file.id})">
                    ▶ Play
                </button>
                <button class="text-red-600 hover:text-red-900 font-medium" onclick="deleteAudio(${file.id})">
                    Delete
                </button>
            </td>
        </tr>
    `).join('');
}

function filterAudioFiles() {
    // This will be implemented when backend is ready
    loadAudioFiles();
}

function getCategoryBadgeColor(category) {
    const colors = {
        'Route': 'bg-blue-100 text-blue-800',
        'Emergency': 'bg-red-100 text-red-800',
        'Festival': 'bg-purple-100 text-purple-800',
        'Advertisement': 'bg-yellow-100 text-yellow-800',
    };
    return colors[category] || 'bg-gray-100 text-gray-800';
}

function playAudio(id) {
    showSuccess('Playing audio...');
    // Would be implemented with HTML5 audio element in production
}

function deleteAudio(id) {
    if (confirm('Are you sure you want to delete this audio file?')) {
        showSuccess('Audio file deleted successfully');
        loadAudioFiles();
    }
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}
