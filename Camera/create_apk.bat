rem keytool -genkey -v -keystore barcodereader-mobileapps.keystore -alias barcodereader -keyalg RSA -keysize 2048 -validity 10000

pause

jarsigner -verbose -sigalg SHA1withRSA -digestalg SHA1 -keystore barcodereader-mobileapps.keystore CordovaApp-release-unsigned.apk barcodereader 

pause

zipalign -v 4 CordovaApp-release-unsigned.apk CordovaApp-first-release.apk

pause