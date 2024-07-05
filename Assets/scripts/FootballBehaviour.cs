using Unity.Netcode;
using UnityEngine;

public class NetworkedFootball : NetworkBehaviour
{
    public float collisionForceMultiplier = 1f;

    private Rigidbody rb;
    PlayerController playerController;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 forceDirection = collision.contacts[0].point - transform.position;
                forceDirection = -forceDirection.normalized;
                float playerVelocityMagnitude = playerRb.velocity.magnitude;
                ApplyForceServerRpc(forceDirection, playerVelocityMagnitude);
            }
        }
    }
    void StartKickAnimation()
    {
        playerController.BallKickAnimation();
    }

    [ServerRpc]
    void ApplyForceServerRpc(Vector3 direction, float playerVelocity)
    {
        ApplyForceClientRpc(direction, playerVelocity);
    }

    [ClientRpc]
    void ApplyForceClientRpc(Vector3 direction, float playerVelocity)
    {
        rb.AddForce(direction * playerVelocity * collisionForceMultiplier, ForceMode.Impulse);
      //  StartKickAnimation();
    }
}
