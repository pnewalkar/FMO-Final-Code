angular.module('FMOApp', ['ngMaterial', 'ui.router', 'pascalprecht.translate',
                          'ngMessages', 'manageAccess',
                          'home', 'referencedata'
])
.run(['localization', '$translate', function (localization, $translate) {
   
    var language = window.navigator.language;
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
            console.log('changing lang' + lang);
            LangResourcesLoader($translateProvider, lang);
            $translateProvider.preferredLanguage(lang);
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

