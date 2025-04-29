using UnityEngine;

public class PortalCameraSync : MonoBehaviour
{
    [SerializeField] private  Transform playerCamera;
    [SerializeField] private  Transform portalA;
    [SerializeField] private Transform portalB;  
    void LateUpdate()
    {
        // Get player's relative rotation to portal A
        Quaternion relativeRot = Quaternion.Inverse(portalA.rotation) * playerCamera.rotation;

        // Flip it (180° rotation as if stepping through portal)
        Quaternion flippedRot = Quaternion.Euler(0f, 180f, 0f) * relativeRot;

        // Final desired camera rotation at Portal B
        Quaternion targetRot = portalB.rotation * flippedRot;

        transform.SetPositionAndRotation(portalB.position, targetRot);
    }
}
