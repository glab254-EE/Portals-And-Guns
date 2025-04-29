using UnityEngine;

public class PortalHandler : MonoBehaviour
{
    [SerializeField] private Camera CameraA;
    [SerializeField] private Camera CameraB;
    [SerializeField] private Camera plrcamera;
    [SerializeField] private  Transform portalA;
    [SerializeField] private Transform portalB; 
    [SerializeField] private Material MatA;
    [SerializeField] private Material MatB;
    void Start()
    {
        plrcamera = Camera.main;
        CameraA.targetTexture?.Release();
        CameraA.targetTexture = new RenderTexture(Screen.width,Screen.height,24);
        MatA.mainTexture = CameraA.targetTexture;

        CameraB.targetTexture?.Release();
        CameraB.targetTexture = new RenderTexture(Screen.width,Screen.height,24);
        MatB.mainTexture = CameraB.targetTexture;

    }
    void LateUpdate()
    {
        // Get player's relative rotation to portal A
        Quaternion relativeRot = Quaternion.Inverse(portalA.rotation) * plrcamera.transform.rotation;



        // Flip it (180° rotation as if stepping through portal)
        Quaternion flippedRot = Quaternion.Euler(0f, 180f, 0f) * relativeRot;

        // Final desired camera rotation at Portal B
        Quaternion targetRot = portalB.rotation * flippedRot;

        transform.SetPositionAndRotation(portalB.position, targetRot);
    }
}
