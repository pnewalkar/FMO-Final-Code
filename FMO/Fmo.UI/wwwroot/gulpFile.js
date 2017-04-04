var gulp = require('gulp');
var sass = require('gulp-sass');

//Styles-task
gulp.task('styles', function() {
    gulp.src(['./app/**/*.scss', '!./app/variables.scss'])
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./build/sass'));
});


//Watch-task
gulp.task('default',function() {
    gulp.watch('./app/**/*.scss',['styles']);
});