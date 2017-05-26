angular.module('FMOApp')
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
        '50': 'f4f4f3',
        '100': 'f5f5f5',
        '200': 'eeeeee',
        '300': 'e0e0e0',
        '400': 'bdbdbd',
        '500': 'da202a',
        '600': '757575',
        '700': '616161',
        '800': 'c62828',
        '900': '212121',
        'A100': '#ffffff',
        'A200': '#000000',
        'A400': '#303030',
        'A700': '#616161',
        'contrastDefaultColor': 'light',    
        

        'contrastDarkColors': [
                                '50',
                                '100',
                                '200',
                                '300',
                                '400'
        ],
        
        'contrastLightColors': [
                                 '500',
                                 '600',
                                 '700',
                                 '800',
                                 '900',
                                 'A100',
                                 'A200',
                                 'A400',
                                 'A700'
        ]
    });
    
  
    /*FMO Accent Palette*/
    
    $mdThemingProvider.definePalette('royalMailAccent', {
        '50': '000000',
        '100': '000000',
        '200': '0892cb',
        '300': '000000',
        '400': '000000',
        '500': '000000',
        '600': '000000',
        '700': '000000',
        '800': '000000',
        '900': '000000',
        'A100': '#000000',
        'A200': '#000000',
        'A400': '#000000',
        'A700': '#FFFFFF',
        'contrastDefaultColor': 'light'
    });
    
    
    $mdThemingProvider.theme('default')
            .primaryPalette('royalMailPrimary')
            .accentPalette('royalMailAccent');
})


