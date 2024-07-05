using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : NetworkBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 700.0f;

    private Rigidbody rb;
    private Animator animator;
    
    public GameObject GrenadeSpawner;
    public VariableJoystick variableJoystick;
    public Button BombButton;
    public Button KickButton;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        variableJoystick = GameObject.FindWithTag("joystick").GetComponent<VariableJoystick>();
        BombButton = GameObject.FindWithTag("BombButton").GetComponentInChildren<Button>();
//        KickButton = GameObject.FindGameObjectWithTag("KickButton").GetComponentInChildren<Button>();
        // Ensure Rigidbody settings for proper movement
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        BombButton.onClick.AddListener(BombClick);
//        KickButton.onClick.AddListener(KickClick);
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;
        Vector3 movement = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        movement = Vector3.ClampMagnitude(movement, 1.0f); // Normalize to prevent faster diagonal movement

        // Calculate the actual speed for the blend tree
        float actualSpeed = movement.magnitude * speed;

        // Update animation based on speed
        animator.SetFloat("Speed", actualSpeed);

        // If there is movement, rotate the character
        if (movement.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move the player
        Vector3 move = movement * speed * Time.deltaTime;
        rb.MovePosition(transform.position + move);
    }
    public void BallKickAnimation()
    {
        animator.SetTrigger("Kick");
    }
    void BombClick()
    {
        ThrowBomb();
    }
    public void ThrowBomb()
    { 
        if(!IsOwner) return;
        GrenadeSpawner.SetActive(true);
        animator.SetTrigger("Throw");
       // animator.ResetTrigger("Throw");
    }
    void KickClick()
    {
        Kick();
    }
    public void Kick()
    {
        if(!IsOwner) return;
        animator.SetTrigger("Kick");
    }
}