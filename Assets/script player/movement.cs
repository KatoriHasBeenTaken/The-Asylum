using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaDrain = 20f;
    public float staminaRegen = 10f;

    public Slider staminaSlider;

    private CharacterController controller;
    private bool isRunning;
    private float currentSpeed;

    private Vector3 velocity;
    private float gravity = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = stamina;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleStamina();
        UpdateStaminaUI();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        bool canRun = Input.GetKey(KeyCode.LeftShift) && stamina > 0f && z > 0f;
        currentSpeed = canRun ? runSpeed : walkSpeed;
        isRunning = canRun;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity.y = -1f;
        }
    }

    void HandleStamina()
    {
        if (isRunning)
        {
            stamina -= staminaDrain * Time.deltaTime;
        }
        else
        {
            stamina += staminaRegen * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    void UpdateStaminaUI()
    {
        if (staminaSlider != null)
            staminaSlider.value = stamina;
    }
}
