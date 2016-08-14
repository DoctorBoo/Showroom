// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
"use strict";
var camera = null;

function onBodyLoad() {
	document.addEventListener("deviceready", onDeviceReady, false);
	navigator.vibrate(500);
	$('#path').text('onDeviceReady1');
}

(function () {
	console.log('doc ready!');
    var vibrationTime = 500;

    document.addEventListener('deviceready', onDeviceReady.bind(this), false);

    $('#scan').on('click', onScan);
    function onDeviceReady() {

    	navigator.vibrate(vibrationTime);
    	console.log('onDeviceReady');

    	// Handle the Cordova pause and resume events
    	document.addEventListener('pause', onPause.bind(this), false);
    	document.addEventListener('resume', onResume.bind(this), false);

    	// TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
    	camera = navigator.camera;
    	if (camera) {
    		console.log(camera);
    	}

    };
    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };

    function onScan() {
    	console.log('scan click');
    	navigator.vibrate(vibrationTime);
    	var cameraOptions = {
    		quality: 50,
    		destinationType: navigator.camera.DestinationType.DATA_URL
    	};

    	try {    		
    		navigator.device.capture.captureImage(captureSuccess, captureError);
    	} catch (e) {
    		console.log('No camerapicture available.');
    		$('#path').text(e.message);
    	}
    	try {
    		navigator.camera.getPicture(onSuccess, captureError, cameraOptions);    		
    	} catch (e) {
    		console.log('No camerapicture available.');
    		$('#path').text(e.message);
    	}
    }

    var captureSuccess = function (mediaFiles) {
    	navigator.vibrate(vibrationTime);
    	var i, path, len;
    	for (i = 0, len = mediaFiles.length; i < len; i += 1) {
    		navigator.vibrate(vibrationTime);
    		path = mediaFiles[i].fullPath;
    		// do something interesting with the file
    		console.log(path);
    		$('#path').text(path);
    		var image = document.getElementById('myImage');
    		image.src = path;
    		image.style.display = 'block';
	    }
    };

    var captureError = function (error) {
    	//navigator.notification.alert('Error code: ' + error.code, null, 'Capture Error');
    };
    function onSuccess(imageData) {
    	var image = document.getElementById('myImage');
    	image.src = imageData;
    	var path = image.src;
    	image.style.display = 'block';
    	console.log(path);
    	$('#path').text(path);
    }   

} )();