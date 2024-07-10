using Microlight.MicroBar;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : NetworkBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 700.0f;

    private MicroBar microBar;
    private Rigidbody rb;
    private Animator animator;
    
    public GameObject GrenadeSpawner;
    public VariableJoystick variableJoystick;
    public Button BombButton;
    private float sprintHoldCounter;

    [SerializeField] private float MaxKickHoldTime = 1.5f;

    [SerializeField] private float SprintTime = 2.0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        variableJoystick = GameObject.FindWithTag("joystick").GetComponent<VariableJoystick>();
        BombButton = GameObject.FindWithTag("BombButton").GetComponentInChildren<Button>();       
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        BombButton.onClick.AddListener(BombClick);       
        microBar = gameObject.GetComponentInChildren<MicroBar>();
        InitializeMicrobar();
    }
    public void InitializeMicrobar()
    {
        microBar.Initialize(MaxKickHoldTime);
        microBar.UpdateBar(0.0f,true,UpdateAnim.Heal);
    }
    public void UpdateMicrobar(float value)
    {
        microBar.UpdateBar(value,true,UpdateAnim.Heal);
    }
    
    void FixedUpdate()
    {
        if (!IsOwner) return;
        if(Mathf.Sqrt(math.pow(variableJoystick.Horizontal,2) + math.pow(variableJoystick.Vertical,2))>0.8f)
        {
            sprintHoldCounter += Time.deltaTime;
            if(sprintHoldCounter > SprintTime)
            {
              variableJoystick.SetColor(Color.green);
              speed = 10.0f;
            }
        }
        else
        {
            variableJoystick.SetColor(Color.white);
            sprintHoldCounter = 0;
            speed = 5.0f;
        }
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
    }
}