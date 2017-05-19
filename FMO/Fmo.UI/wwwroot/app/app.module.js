angular.module('FMOApp', ['ngMaterial', 'ui.router', 'pascalprecht.translate',
                          'ngMessages', 'manageAccess',
                          'home', 'referencedata'
])
.run(['localization', '$translate', '$window', function (localization, $translate, $window) {
   
    var language = $window.navigator.language;
    localization.setLocale(language);
    $translate.use(language);
   
}])
.provider('localization', function LocalizationProvider() {
    return {
        updateLocale: function () { },
        $get: function () {
            var updateLocale = this.updateLocale;
            return {
                setLocale: updateLocale
            };
        }
    }
})
    .config(['localizationProvider', '$translateProvider', function (localizationProvider, $translateProvider) {
        localizationProvider.updateLocale = function (lang) {
            LangResourcesLoader($translateProvider, lang);
            $translateProvider.preferredLanguage(lang);
            $translateProvider.useSanitizeValueStrategy('escape');
        };
    }]);


function LangResourcesLoader($translateProvider, lang) {
    switch (lang) {
        case 'en-US':
            if (window.Translation_Route_log_EN)
                $translateProvider.translations(lang, Translation_Route_log_EN);
            else
                $translateProvider.translations(lang, new Object());
            break;

    }
}

