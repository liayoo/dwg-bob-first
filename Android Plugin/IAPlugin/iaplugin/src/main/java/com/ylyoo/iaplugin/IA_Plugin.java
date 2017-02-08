package com.ylyoo.iaplugin;


import android.app.Fragment;
import android.os.Bundle;
//import android.support.v4.app.ActivityCompat;
//import android.support.v4.content.ContextCompat;
import android.util.Log;

import com.indooratlas.android.sdk.*;
import com.unity3d.player.UnityPlayer;

/*
 * IA_Plugin
 * Android plugin class for Unity that utilizes IndoorAtlas' SDK
 *
 * TODO: Make the plugin work with higher API levels.
 *       Currently tested with ...
 *
 *          - Nexus 6P (7.1.1): Unity app doesn't open.
 *              - on device: "Failure to initialize! Your hardware does not support this application, sorry!"
 *              - logcat: (Unity) "Unable to find main"
 *
 *          - Galaxy A6 (5.1.1): It works!!
 *
 *          - Vega (4.4): Unity app opens but IndoorAtlas' SDK doesn't work.
 *                 - can get status updates
 *                 - can't get user location updates).
 */

public class IA_Plugin extends Fragment implements IALocationListener {

    // Singleton instance.
    public static IA_Plugin instance;
    // Unity context.
    String gameObjectName;
    // Constants.
    public static final String TAG = "IA_Plugin_Fragment";
    private static final String CALLBACK_METHOD_NAME = "getTextFromAndroid";
    // IndoorAtlas.
    private static IALocationManager mLocationManager;
    private static long mFastestInterval = -1L;
    private static float mShortestDisplacement = -1f;
    // User location.
    public static String longitude;
    public static String latitude;


    /*
     * Unity calls this method when it creates a plugin instance with a game object name.
     */
    public static void start(String gameObjectName) {
        Log.d("Debug============", "Call-------start");
        instance = new IA_Plugin();
        instance.gameObjectName = gameObjectName;
        UnityPlayer.currentActivity.getFragmentManager().beginTransaction().add(instance,IA_Plugin.TAG).commit();
        Log.d("Debug============", "Exit-------start");
    }

    /*
     * Creates an IALocationManager instance upon creation of the plugin instance.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        Log.d("Debug============", "Call-------onCreate");
        super.onCreate(savedInstanceState);
        // Retain between configuration changes such as device rotation
        setRetainInstance(true);
        Log.d("Debug============", "Call-------IALocationManager.create");
        mLocationManager = IALocationManager.create(getActivity());
        Log.d("Debug============", "Exit-------onCreate");
    }

    /*
     * Called when user locaion has been changed.
     */
    @Override
    public void onLocationChanged(IALocation location) {
        Log.d("Debug============", "Call-------onLocationChanged");
        // update longitude and latitude
        longitude = ""+location.getLongitude();
        latitude = ""+location.getLatitude();
        Log.d("Debug============", "-----------Location: "+longitude+", "+latitude);
    }

    /*
     * Called when user (mobile's) status (wifi, calibration, etc.) has been changed.
     */
    @Override
    public void onStatusChanged(String provider, int status, Bundle extras) {
        Log.d("Debug============", "Call-------onStatusChanged");
        switch(status) {
            case IALocationManager.STATUS_AVAILABLE:
                Log.d("Debug============", "---------------STATUS_AVAILABLE");
                break;
            case IALocationManager.STATUS_LIMITED:
                Log.d("Debug============", "---------------STATUS_LIMITED");
                break;
            case IALocationManager.STATUS_CALIBRATION_CHANGED:
                Log.d("Debug============", "---------------STATUS_CALIBRATION_CHANGED");
                break;
            case IALocationManager.STATUS_TEMPORARILY_UNAVAILABLE:
                Log.d("Debug============", "---------------STATUS_TEMPORARILY_UNAVAILABLE");
                break;
            case IALocationManager.STATUS_OUT_OF_SERVICE:
                Log.d("Debug============", "---------------STATUS_OUT_OF_SERVICE");
        }
    }

    /*
     * getUpdateRequestFromUnity() calls this method.
     * Sets the fastest interval and the smallest displacement of updates and creates
     * an IALocationRequest to request location updates using IALocationManager.
     */
    private static void requestUpdates() {
        Log.d("Debug============", "Call-------requestUpdates");
        IALocationRequest request = IALocationRequest.create();
        Log.d("Debug============", "-----------IALocationRequest Created");

        if (mLocationManager == null) {
            Log.d("Debug============", "-----------location manager is null");
        }
        request.setFastestInterval(mFastestInterval);
        request.setSmallestDisplacement(mShortestDisplacement);
        mLocationManager.removeLocationUpdates(instance);
        Log.d("Debug============", "Call-------requestLocationUpdates");
        mLocationManager.requestLocationUpdates(request, instance);
        Log.d("Debug============", "Exit-------requestUpdates");
    }

    /*
     * Called when the instance is being destroyed.
     */
    @Override
    public void onDestroy() {
        Log.d("Debug============", "Call-------onDestroy");
        super.onDestroy();
        mLocationManager.destroy();
    }

    /*
     * Called when the IA_Plugin fragment is visible to the user and actively running.
     * Currently requests for location updates are made in here.
     */
    @Override
    public void onResume() {
        Log.d("Debug============", "Call-------onResume");
        super.onResume();
        try {
            mLocationManager.requestLocationUpdates(IALocationRequest.create(), instance);
        }
        catch (Exception e) {
            Log.d("Debug============","onResume---"+e.getMessage());
        }
    }


    /*
     * Unity calls this method to request location updates and
     * this method calls requestUpdates.
     * Currently location update starts even without explicitly calling this method in Unity.
     * (Can change that by removing requestUpdates call in onResume())
     */
    public static void getUpdateRequestFromUnity() {
        requestUpdates();
    }

    /*
     * For testing communication with Unity.
     * Finds a Unity GameObject with the name gameObjectName and
     * calls CALLBACK_METHOD_NAME attached to the GameObject with a string argument,
     * which is the third parameter of UnitySendMessage.
     */
    public void updateText() {
        Log.d("Debug============", "Call-------updateText");
        UnityPlayer.UnitySendMessage(gameObjectName, CALLBACK_METHOD_NAME, "Hello from Android!");
    }


}
