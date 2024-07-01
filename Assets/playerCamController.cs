using Unity.Netcode;
using UnityEngine;

public class PlayerCamController : NetworkBehaviour
{
    public GameObject playerCameraPrefab; // Assign Cinemachine Virtual Camera prefab here

    private void Start()
    {
        if (IsLocalPlayer)
        {
            GameObject cameraInstance = Instantiate(playerCameraPrefab);
            Cinemachine.CinemachineFreeLook freeLookCamera = cameraInstance.GetComponent<Cinemachine.CinemachineFreeLook>();
            freeLookCamera.Follow = transform;
           // freeLookCamera.LookAt = transform;
        }
    }
}
