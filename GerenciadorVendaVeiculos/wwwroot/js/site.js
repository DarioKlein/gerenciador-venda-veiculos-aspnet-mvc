document.addEventListener('DOMContentLoaded', function () {
    lucide.createIcons();

    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebar-overlay');
    const toggle = document.getElementById('sidebar-toggle');

    function openSidebar() {
        sidebar.classList.remove('-translate-x-full');
        overlay.classList.remove('hidden');
        toggle.setAttribute('aria-expanded', 'true');
    }

    function closeSidebar() {
        sidebar.classList.add('-translate-x-full');
        overlay.classList.add('hidden');
        toggle.setAttribute('aria-expanded', 'false');
    }

    if (toggle && sidebar && overlay) {
        toggle.addEventListener('click', function () {
            const isOpen = !sidebar.classList.contains('-translate-x-full');
            isOpen ? closeSidebar() : openSidebar();
        });

        overlay.addEventListener('click', closeSidebar);
    }
});