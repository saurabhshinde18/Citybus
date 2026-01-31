// Authentication and Login Logic
document.addEventListener('DOMContentLoaded', function() {
    // Check if already logged in
    if (isAuthenticated()) {
        window.location.href = 'dashboard.html';
        return;
    }

    const loginForm = document.getElementById('loginForm');
    const emailInput = document.getElementById('email');
    const passwordInput = document.getElementById('password');
    const errorAlert = document.getElementById('errorAlert');
    const errorMessage = document.getElementById('errorMessage');
    const loadingAlert = document.getElementById('loadingAlert');

    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();

            const email = emailInput.value.trim();
            const password = passwordInput.value;

            // Validation
            if (!email || !password) {
                showError('Please enter email and password');
                return;
            }

            // Show loading state
            loadingAlert.classList.remove('hidden');
            errorAlert.classList.add('hidden');

            try {
                const response = await AuthAPI.login(email, password);
                
                if (response.success) {
                    setToken(response.data.token);
                    setCurrentUser(response.data.user);
                    showSuccess('Login successful!');
                    
                    // Redirect to dashboard
                    setTimeout(() => {
                        window.location.href = 'dashboard.html';
                    }, 500);
                } else {
                    showError(response.message || 'Login failed');
                }
            } catch (error) {
                console.error('Login error:', error);
                errorMessage.textContent = error.message || 'An error occurred. Please try again.';
                errorAlert.classList.remove('hidden');
            } finally {
                loadingAlert.classList.add('hidden');
            }
        });
    }

    // Demo credentials setup
    const emailField = document.querySelector('input[type="email"]');
    if (emailField) {
        emailField.value = 'admin@citybus.gov';
        passwordInput.value = 'Password@123';
    }
});
