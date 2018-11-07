
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;

public class CrosshairGenerator : MonoBehaviour
{
    public GameObject targetPrefab;

    private List<DetectedPlane> m_NewPlanes = new List<DetectedPlane>();

    private void Start()
    {
        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i));
            }
        }
    }

    private void Update()
    {

        Debug.Log(Session.Status);

        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        Session.GetTrackables(m_NewPlanes, TrackableQueryFilter.New);

        for (int i = 0; i < m_NewPlanes.Count; i++)
        {
            GameObject target = Instantiate(targetPrefab, m_NewPlanes[i].CenterPose.position, Quaternion.identity, this.transform);
            target.transform.LookAt(Camera.main.transform.position);
        }

    }

}



