using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float flySpeed = 10f;
    public float flyIncrement = 2f;  // How much to increase height each time Shift is pressed
    public float maxFlyHeight = 20f; // Maximum flying height

    private Rigidbody rb;
    private bool isFlying = false;
    private float currentFlyHeight = 0f;

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

        // Update the animator's flying state
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.SetBool("isFlying", isFlying);
        }
        else
        {
            Debug.LogWarning("No Animator component found on the player object.");
        }
    }

    private void HandleMovement(Vector3 movement)
    {
        movement = transform.TransformDirection(movement);  // Transform movement vector from local to world space

        if (movement != Vector3.zero) // Check if there is any movement input
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, Time.deltaTime * 5f);  // Adjust rotation speed if needed
        }

        if (!isFlying)
        {
            rb.MovePosition(rb.position + movement * walkSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 flyMovement = movement * flySpeed * Time.deltaTime + Vector3.up * (currentFlyHeight * Time.deltaTime);
            rb.MovePosition(rb.position + flyMovement);
        }
    }

    private void HandleFlyingToggle()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            isFlying = !isFlying;
            if (isFlying && currentFlyHeight < maxFlyHeight)
            {
                currentFlyHeight += flyIncrement;
                currentFlyHeight = Mathf.Min(currentFlyHeight, maxFlyHeight);
            }
            else if (!isFlying)
            {
                currentFlyHeight = 0f;  // Reset height when landing
            }
        }
    }
}
