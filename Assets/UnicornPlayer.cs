using UnityEngine;

public class PlayerController : MonoBehaviour
{
     public float walkSpeed = 5f;
    public float flySpeed = 10f;
    public float rotationSpeed = 2f; // Slower rotation speed for more gradual turning
    public float flyIncrement = 0.05f;
    public float maxFlyHeight = 20f;
    public float timeToReachMaxHeight = 20f; // Time to reach maximum height for smoother ascent

    private Rigidbody rb;
    private bool isFlying = false;
    private float currentFlyHeight = 0f;
    private float timeFlying = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogError("Rigidbody component is missing from the player object.");
        }
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        HandleMovement(movement);
        HandleFlyingToggle();

        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            bool isMoving = movement.magnitude > 0;
            animator.SetBool("isRunning", isMoving && !isFlying);
            animator.SetBool("isFlying", isFlying);
            if (!isMoving && !isFlying)
                animator.Play("IdleA");
        }
        else
        {
            Debug.LogWarning("No Animator component found on the player object.");
        }
    }

    private void HandleMovement(Vector3 movement)
    {
        movement = transform.TransformDirection(movement);
        if (movement != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }

        Vector3 newPosition = rb.position + movement * (isFlying ? flySpeed : walkSpeed) * Time.deltaTime;
        newPosition.y += currentFlyHeight;
        rb.MovePosition(newPosition);
    }


    private void HandleFlyingToggle()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (!isFlying) // Start flying
            {
                isFlying = true;
                timeFlying = 0f; // Reset flying timer
            }

            timeFlying += Time.deltaTime;
            currentFlyHeight = Mathf.Lerp(0f, maxFlyHeight, timeFlying / timeToReachMaxHeight);
            currentFlyHeight = Mathf.Min(currentFlyHeight, maxFlyHeight);
        }
        else if (isFlying)
        {
            // Gradually decrease height when not holding shift, slower than before
            timeFlying -= Time.deltaTime * 0.1f; // Decrease timeFlying more slowly
            timeFlying = Mathf.Max(timeFlying, 0f);
            currentFlyHeight = Mathf.Lerp(0f, maxFlyHeight, timeFlying / timeToReachMaxHeight);

            if (timeFlying == 0f)
            {
                isFlying = false; // Stop flying once height has decreased to the minimum
            }
        }
    }
}
