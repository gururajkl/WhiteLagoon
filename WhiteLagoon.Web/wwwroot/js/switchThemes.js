function SwitchTheme() {
    // Root HTML tag.
    var htmlTag = document.documentElement;

    // Get the current theme from the local storage.
    var currentTheme = localStorage.getItem("theme");

    // Toggle between dark and light themes.
    if (currentTheme === "dark") {
        htmlTag.removeAttribute("data-bs-theme");
        localStorage.setItem("theme", "light");
        toggleIcons("moonIcon", "sunIcon");
    } else {
        htmlTag.setAttribute("data-bs-theme", "dark");
        localStorage.setItem("theme", "dark");
        toggleIcons("sunIcon", "moonIcon");
    }
}

// Switch the display style based on the values.
function toggleIcons(showIconId, hideIconId) {
    document.getElementById(showIconId).style.display = "inline";
    document.getElementById(hideIconId).style.display = "none";
}

// Set initial theme and icon on page load
document.addEventListener("DOMContentLoaded", function () {
    // Get the current theme from the localStorage.
    var currentTheme = localStorage.getItem("theme");

    if (currentTheme === "dark") {
        document.documentElement.setAttribute("data-bs-theme", "dark");
        toggleIcons("sunIcon", "moonIcon");
    } else {
        toggleIcons("moonIcon", "sunIcon");
    }
});
