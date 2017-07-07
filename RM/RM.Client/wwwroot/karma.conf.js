/// <reference path="app/access-link/access-link.module.js" />
/// <reference path="app/access-link/tests/access-link.controller.spec.js" />
// Karma configuration
// Generated on Fri Jun 23 2017 08:54:26 GMT+0100 (GMT Daylight Time)

module.exports = function(config) {
  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: '',

    plugins: [
         'karma-jasmine',
         'karma-coverage',
         'karma-phantomjs-launcher'
    ],
    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: ['jasmine'],


    // list of files / patterns to load in the browser
    files: [
          'bower_components/angular/angular.js',
          'bower_components/angular-mocks/angular-mocks.js',
          './app.module.js',
          './app/**/*.js',
          './app/**/**/*.spec.js',
    ],


    // list of files to exclude
    exclude: [
        //"./app/delivery-point/tests/delivery-point.controller.spec.js",
        //"./app/delivery-point/tests/delivery-point.context.controller.spec.js",
        //"./app/delivery-point/tests/delivery-point.factory.spec.js",
        //"./app/delivery-point/tests/delivery-point.service.spec.js",
        // "./app/access-link/tests/access-link.controller.spec.js",
        //"./app/access-link/tests/access-link.factory.spec.js",
        //"./app/access-link/tests/access-link.service.spec.js",
        //"./app/print-map/tests/print-map.factory.spec.js",
        //"./app/side-nav/tests/side-nav.controller.spec.js"
    ],


    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
        './app/**/*.js': 'coverage'
    },


    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['progress', 'coverage'],

    coverageReporter: {
        type: 'html',
        dir: 'coverage/'
    },

    // web server port
    port: 9876,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_INFO,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: true,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['PhantomJS'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: false,

    // Concurrency level
    // how many browser should be started simultaneous
    concurrency: Infinity
  })
}
