using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.Common;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class BattleViz : MonoBehaviour {

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

    // Setting this to true will prevent multiple instances from being spawned
    private bool hasAppeared = false;

    public GameObject CrosshairGenerator;

    // These are the models to instantiate and fight

    public GameObject arena;
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    private Monster chosenMonster;
    private GameObject chosenMonsterPrefab;

    private void Start()
    {
        chosenMonster = MonsterCollector.sharedInstance.monsterToFight;
        chosenMonsterPrefab = MonsterCollector.sharedInstance.monsterPrefabsList[chosenMonster.PrefabName];
    }

    private void Update()
    {
        UpdateApplicationLifeCycle();

        // If the arena has already been placed, stop here
        if (hasAppeared)
        {
            return;
        }

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

                //Place the arena where the player touched

                Vector3 arenaPos = new Vector3(hit.Pose.position.x, hit.Pose.position.y - 0.3f, hit.Pose.position.z);

                arena.transform.position = arenaPos;
                arena.SetActive(true);

                // Choose the model to be instantiated
                // Instantiatemodel at the hit pose.
                GameObject monster = Instantiate(chosenMonsterPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation, playerSpawnPoint);

                // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical world evolves.
                Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

                // Make model a child of the anchor.
                arena.transform.parent = anchor.transform;
                hasAppeared = true;
                CrosshairGenerator.SetActive(false);
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
