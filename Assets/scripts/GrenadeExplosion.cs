using UnityEngine;
using Unity.Netcode;

public class GrenadeExplosion : NetworkBehaviour
{
    public int damage = 20;
    public float explodeDelay = 2f;
    public float explodeRadius = 15f;
    public float explosionForce = 15f;
    public GameObject explodeEffect;

    private float collDelay = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnableCollider", collDelay);
        Invoke("Explode", explodeDelay);
    }

    private void EnableCollider()
    {
        GetComponent<SphereCollider>().isTrigger = false;
    }

    private void Explode()
    {
        if (IsServer)
        {
            Collider[] collidersHit = Physics.OverlapSphere(transform.position, explodeRadius);

            // foreach (Collider collider in collidersHit)
            // {
            //     Rigidbody rigid = collider.GetComponent<Rigidbody>();
            //     if (rigid != null)
            //         rigid.AddExplosionForce(explosionForce, transform.position, explodeRadius, 1f, ForceMode.Impulse);

            //     Character character = collider.GetComponent<Character>();
            //     if (character != null)
            //         character.TakeDamage(damage);
            // }

            
            // Notify clients about the explosion
            ExplodeClientRpc();

            // Destroy the grenade on the server
            Destroy(gameObject);
        }
    }

    [ClientRpc]
    private void ExplodeClientRpc()
    {
        GameObject effects = Instantiate(explodeEffect, transform.position, Quaternion.identity);
        Destroy(effects,2.0f);
    }
}
