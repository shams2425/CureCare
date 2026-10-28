document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("adminRegisterForm");
    const button = document.getElementById("adminRegisterButton");

    if (!form || !button) {
        return;
    }

    form.addEventListener("submit", function () {
        if (form.checkValidity()) {
            button.disabled = true;
            button.textContent = "Submitting...";
        }
    });
});