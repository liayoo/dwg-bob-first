package com.ylyoo.iaplugin;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.indooratlas.android.sdk.*;
//import com.indooratlas.android.sdk.IALocation;
//import com.indooratlas.android.sdk.IALocationListener;
//import com.indooratlas.android.sdk.IALocationManager;
//import com.indooratlas.android.sdk.IALocationRequest;

import com.indooratlas.android.sdk.resources.IALocationListenerSupport;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;



public class IA_Plugin extends UnityPlayerActivity {

    private Intent LaunchIntent;

//    public void Launch() {
//        Log.d("Debug============", "Call-------Launch");
//        Intent myIntent = new Intent(activity, IA_Plugin.class);
//        activity.startActivity(myIntent);
//        startActivity(LaunchIntent);
//    }

    private static IA_Plugin _instance;
    private IALocationManager mLocationManager;
    private long mFastestInterval = -1L;
    private float mShortestDisplacement = -1f;

    private IALocationListener mLocationListener = new IALocationListenerSupport() {
        @Override
        public void onLocationChanged(IALocation location) {
            Log.d("Debug============", "Call-------onLocationChanged");
            Log.d("Debug============", "-----------"+location.getLongitude()+", "+location.getLatitude());
        }

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
    };


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        Log.d("Debug============", "Call-------onCreate");
        super.onCreate(savedInstanceState);
        mLocationManager = IALocationManager.create(this);
        Log.d("Debug============", "Call-------IALocationManager.create");
        //LaunchIntent = getPackageManager().getLaunchIntentForPackage(this.getPackageName());
        requestUpdates();
    }

    @Override
    protected void onDestroy() {
        Log.d("Debug============", "Call-------onDestroy");
        super.onDestroy();
        mLocationManager.destroy();
    }

    @Override
    protected void onResume() {
        super.onResume();
        try {
            mLocationManager.requestLocationUpdates(IALocationRequest.create(),mLocationListener);
        }
        catch (Exception e) {
            Log.d("Debug============","onResume---"+e.getMessage());
        }
    }

//    @Override
//    public void onLocationChanged(IALocation location) {
//        Log.d("Debug============", "Call-------onLocationChanged");
//        Log.d("Debug============", "-----------"+location.getLongitude()+", "+location.getLatitude());
//    }
//
//    @Override
//    public void onStatusChanged(String provider, int status, Bundle extras) {
//
//    }

    private void requestUpdates() {
        Log.d("Debug============", "Call-------requestUpdates");
        IALocationRequest request = IALocationRequest.create();
        request.setFastestInterval(mFastestInterval);
        request.setSmallestDisplacement(mShortestDisplacement);
        mLocationManager.requestLocationUpdates(request, mLocationListener);
    }

    // For communicating with Unity
    public void updateText() {
        Log.d("Debug============", "Call-------updateText");
        UnityPlayer.UnitySendMessage("Canvas","getTextFromAndroid","Hello!");
    }

}
