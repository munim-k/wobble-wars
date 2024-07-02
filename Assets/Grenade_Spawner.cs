using UnityEngine;

public class Grenade_Spawner : MonoBehaviour
{
    public GameObject Player;
    public Animator animator;
    public float animationtime;
    public GameObject grenade;
    public float grenadeForce = 2f;
    AnimatorStateInfo animatorStateInfo;
    void Update()
    {
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(animatorStateInfo.IsName("Throwing"))
        {
            animationtime = animatorStateInfo.normalizedTime;

             if(animationtime>=0.6f)
            {
                 SpawnGrenade();
                 animationtime = 0;
            }
        }
    }

    void SpawnGrenade()
    {
        GameObject grenadeInstance =Instantiate(grenade,transform.position, grenade.transform.rotation);
        grenadeInstance.GetComponent<Rigidbody>().AddForce(Player.transform.forward * grenadeForce, ForceMode.Impulse);
        gameObject.SetActive(false);
    }

}
