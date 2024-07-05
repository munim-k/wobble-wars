using UnityEngine;
using Unity.Netcode;

public class Grenade_Spawner : NetworkBehaviour
{
    public GameObject Player;
    public Animator animator;
    public GameObject grenadePrefab;
    public float grenadeForce = 2f;
    public GameObject GranadeModel;

    private bool hasSpawnedGrenade = false;
    private AnimatorStateInfo animatorStateInfo;

    void Update()
    {
        if (!IsOwner)
            return;

        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName("Throwing"))
        {
            if (!hasSpawnedGrenade && GranadeModel.activeSelf && animatorStateInfo.normalizedTime >= 0.6f)
            {
                hasSpawnedGrenade = true;
                SpawnGrenadeServerRpc(Player.transform.forward);
            }
        }
        else
        {
            hasSpawnedGrenade = false;
        }
    }

    [ServerRpc]
    void SpawnGrenadeServerRpc(Vector3 throwDirection, ServerRpcParams rpcParams = default)
    {
        GameObject grenadeInstance = Instantiate(grenadePrefab, transform.position, grenadePrefab.transform.rotation);
        NetworkObject networkObject = grenadeInstance.GetComponent<NetworkObject>();
        networkObject.Spawn();
        ApplyForceToGrenadeClientRpc(grenadeInstance.GetComponent<NetworkObject>().NetworkObjectId, throwDirection);
    }

    [ClientRpc]
    void ApplyForceToGrenadeClientRpc(ulong grenadeNetworkObjectId, Vector3 throwDirection)
    {
        NetworkObject networkObject = NetworkManager.SpawnManager.SpawnedObjects[grenadeNetworkObjectId];
        if (networkObject != null && networkObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(throwDirection * grenadeForce, ForceMode.Impulse);
        }
        GranadeModel.SetActive(false);
    }
}
