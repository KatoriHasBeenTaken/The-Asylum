using UnityEngine;

/// <summary>
/// L?c camera (bobbing) + nghiêng (tilt) cho FPS.
/// G?n script vào Main?Camera, kéo CharacterController c?a player vào ô Controller.
/// </summary>
public class CameraBobbing : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;          // gán CharacterController c?a player

    [Header("Bobbing ? lên/xu?ng")]
    public float walkBobSpeed = 8f;
    public float walkBobAmount = 0.05f;
    public float runBobSpeed = 13f;
    public float runBobAmount = 0.08f;
    public float crouchBobAmount = 0.02f;

    [Header("Tilt ? nghiêng trái/ph?i")]
    public float tiltAmount = 2f;    // ð? nghiêng t?i ða (ð?)
    public float tiltSpeed = 6f;    // t?c ð? nghiêng

    Vector3 localOrigin;         // v? trí g?c
    Quaternion rotOrigin;        // rotation g?c
    float bobTimer;

    void Awake()
    {
        localOrigin = transform.localPosition;
        rotOrigin = transform.localRotation;

        // T? t?m CharacterController n?u quên gán
        if (controller == null)
        {
            controller = GetComponentInParent<CharacterController>();
            if (controller == null)
                Debug.LogWarning("CameraBobbing: Không t?m th?y CharacterController!");
        }
    }

    
    void LateUpdate()
    {
        if (controller == null) return;

        Vector3 velocity = controller.velocity;
        bool isGround = controller.isGrounded;
        bool isMoving = velocity.magnitude > 0.1f && isGround;

        /* 1. BOBBING (l?c Y) */
        if (isMoving)
        {
            // Ch?n t?c ð? & biên ð? d?a trên t?c ch?y
            float bobSpeed = (velocity.z > controller.height) ? runBobSpeed : walkBobSpeed;
            float bobAmount = (velocity.z > controller.height) ? runBobAmount : walkBobAmount;
            if (controller.height < 1.5f) bobAmount = crouchBobAmount;

            bobTimer += Time.deltaTime * bobSpeed;
            float offsetY = Mathf.Sin(bobTimer) * bobAmount;

            transform.localPosition = new Vector3(localOrigin.x,
                                                  localOrigin.y + offsetY,
                                                  localOrigin.z);
        }
        else
        {
            bobTimer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition,
                                                    localOrigin,
                                                    Time.deltaTime * walkBobSpeed);
        }

        /* 2. TILT (nghiêng Z) */
        float targetTilt = -velocity.x * tiltAmount;            // âm = nghiêng ph?i, dýõng = nghiêng trái
        Quaternion tiltRot = Quaternion.Euler(0f, 0f, targetTilt);

        transform.localRotation = Quaternion.Slerp(transform.localRotation,
                                                   rotOrigin * tiltRot,
                                                   Time.deltaTime * tiltSpeed);
    }
}
