using UnityEngine;

public class CheckEyeTrackingRequirements : MonoBehaviour
{
    void Start()
    {
        var eyeTrackingRaysCount = GetComponents<EyeTrackingRay>().Length;

        if(eyeTrackingRaysCount > 1)
        {
            Debug.LogError("Multiple eye tracking rays are currently not supported. Make sure only one is assigned to either left or right eye interactor.");
        }

        if(!OVRManager.eyeTrackedFoveatedRenderingEnabled)
        {
            Debug.LogWarning("Eye tracking permission not enabled or not supported on this device.");
        }
    }
}


