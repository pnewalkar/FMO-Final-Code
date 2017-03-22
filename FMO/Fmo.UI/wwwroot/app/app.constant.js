(function (window) {
    window.__env = window.__env || {};

    // API url
    window.__env.apiUrl = 'http://localhost:62032';

    // Base url
    window.__env.baseUrl = '/';

    // Whether or not to enable debug mode
    // Setting this to false will disable console output
    window.__env.enableDebug = false;
}(this));

MyApp.constant("__env", window.__env);
