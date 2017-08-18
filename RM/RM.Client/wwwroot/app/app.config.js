angular.module('RMApp')
.config(function ($mdThemingProvider, $httpProvider) {
    //initialize get if not there
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }

    // Answer edited to include suggestions from comments
    // because previous version of code introduced browser-related errors

    //disable IE ajax request caching
    //  $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
    // extra
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';

    /*FMO Primary Palette*/

    $mdThemingProvider.definePalette('royalMailPrimary', {
        '50': 'fbe4e5',
        '100': 'f4bcbf',
        '200': 'ed9095',
        '300': 'e5636a',
        '400': 'e0414a',
        '500': 'da202a',
        '600': 'd61c25',
        '700': 'd0181f',
        '800': 'cb1319',
        '900': 'c20b0f',
        'A100': 'ffeeee',
        'A200': 'ffbbbc',
        'A400': 'ff8889',
        'A700': 'ff6e70',
        'contrastDefaultColor': 'light',
        'contrastDarkColors': [
            '50',
            '100',
            '200',
            '300',
            'A100',
            'A200',
            'A400',
            'A700'
        ],
        'contrastLightColors': [
            '400',
            '500',
            '600',
            '700',
            '800',
            '900'
        ]
    });


    /*FMO Accent Palette*/

    $mdThemingProvider.definePalette('royalMailAccent', {
        '50': 'ececec',
        '100': 'd0d0d0',
        '200': '84c9e5',
        '300': '909090',
        '400': '797979',
        '500': '616161',
        '600': '595959',
        '700': '4f4f4f',
        '800': '454545',
        '900': '333333',
        'A100': 'd5ebff',
        'A200': 'a2d2ff',
        'A400': '6fb9ff',
        'A700': '56adff',
        'contrastDefaultColor': 'light',
        'contrastDarkColors': [
            '50',
            '100',
            '200',
            '300',
            'A100',
            'A200'
        ],
        'contrastLightColors': [
            '400',
            '500',
            '600',
            '700',
            '800',
            '900',
            'A400',
            'A700'
        ]
    });


    $mdThemingProvider.theme('default')
            .primaryPalette('royalMailPrimary')
            .accentPalette('royalMailAccent');
})

.run(['localization', '$translate', '$window', 'referencedataApiService', function (localization, $translate, $window, referencedataApiService) {
    var language = $window.navigator.language;
    referencedataApiService.readJson().then(function (response) {
        localization.setJson(response);
        $translate.use(language);
    })
}])
.provider('localization', function LocalizationProvider() {
    return {
        updateLangJson: function () { },
        $get: function () {
            var updateLangJson = this.updateLangJson;
            return {
                setJson: updateLangJson
            };
        }
    }
})
    .config(['localizationProvider', '$translateProvider', '$httpProvider', function (localizationProvider, $translateProvider, $httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
        $httpProvider.interceptors.push('errorInterceptorService');
        
        localizationProvider.updateLangJson = function (Json) {
            var language = window.navigator.language;
            $translateProvider.translations(language, Json);
            $translateProvider.preferredLanguage(language);
            $translateProvider.useSanitizeValueStrategy('escape');       
        }
      
    }]);

