using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.Common;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class MonsterViz : MonoBehaviour {

    // The first person camera being used to render the passthrough camera image
    public Camera firstPersonCamera;

    // The prefab for tracking and visualizing detected planes
    public GameObject detectedPlanePrefab;

    // A gameobject parenting UI for displaying the "searching for planes" snackbar.
    public GameObject searchingForPlaneUI;

    // The rotation in degrees need to apply to model it is placed.
    private const float modelRotation = 180f;

    // A list to hold all planes ARCore is tracking in the current frame. This object is used across
    // the application to avoid per-frame allocations.
    private List<DetectedPlane> allPlanes = new List<DetectedPlane>();

    // True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
    private bool isQuitting = false;

    private void Update()
    {
        UpdateApplicationLifeCycle();

        // Hide snackbar when displaying at least one tracked plane
        Session.GetTrackables<DetectedPlane>(allPlanes);
        bool showSearchingUI = true;

        for (int i = 0; i < allPlanes.Count; i++)
        {
            if (allPlanes[i].TrackingState == TrackingState.Tracking)
            {
                showSearchingUI = false;
                break;
            }
        }

        searchingForPlaneUI.SetActive(showSearchingUI);

        // If the player did not touch the screen, we are done with this Update
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) && Vector3.Dot(firstPersonCamera.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else
            {
                // Choose the model to be instantiated
                // Instantiatemodel at the hit pose.
                GameObject monster = Instantiate(MonsterCollector.sharedInstance.monsterToLookAt, hit.Pose.position, hit.Pose.rotation);

                monster.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                monster.transform.Rotate(0, modelRotation, 0, Space.Self);

                // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical world evolves.
                Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

                // Make model a child of the anchor.
                monster.transform.parent = anchor.transform;
            }
        }

    }

    private void UpdateApplicationLifeCycle()
    {
        //Go back to Camp when the back button is pressed
        if (Input.GetKey(KeyCode.Escape))
        {
            GameController.sharedInstance.ChangeScene("Ranch");
        }

        // Only allow the screen to sleep when not tracking
        if (Session.Status != SessionStatus.Tracking)
        {
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (isQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            ShowAndroidToastMessage("Camera permission is needed to run this application");
            isQuitting = true;
            Invoke("DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            ShowAndroidToastMessage("An error has occurred, please restart the app.");
            isQuitting = true;
            Invoke("DoQuit", 0.5f);
        }

    }

    private void DoQuit()
    {
        Application.Quit();
    }

    // Show an Android toast message
    private void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, 0);
                toastObject.Call("show");
            }));
        }
    }



}
