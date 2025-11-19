// Custom Swagger UI JavaScript
window.onload = function() {
    // Remove existing favicon
    var existingFavicon = document.querySelector('link[rel="icon"]');
    if (existingFavicon) {
        existingFavicon.remove();
    }
    
    // Add custom favicon
    var favicon = document.createElement('link');
    favicon.rel = 'icon';
    favicon.type = 'image/x-icon';
    favicon.href = 'data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><text y=".9em" font-size="90">🏛️</text></svg>';
    document.head.appendChild(favicon);
    
    // Update page title
    document.title = '🏛️ GoVisit API - Government Appointments';
};