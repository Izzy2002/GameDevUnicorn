using UnityEngine;

public class UnicornController : MonoBehaviour
{
    public float walkSpeed = 2.0f;
    public float flySpeed = 5.0f;
    public float flightHeight = 10.0f;
    public float turnSpeed = 5.0f;
    private bool isFlying = false;

    private Animator animator;
    private Vector3 targetPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = transform.TransformDirection(move);
        move.Normalize();

        float speed = isFlying ? flySpeed : walkSpeed;
        targetPosition += move * speed * Time.fixedDeltaTime;

        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        if (move != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.fixedDeltaTime);
        }

        animator.SetBool("isFlying", isFlying);
        animator.SetFloat("Speed", move.magnitude);

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            isFlying = !isFlying;
            float newY = isFlying ? flightHeight : 0;
            targetPosition = new Vector3(targetPosition.x, newY, targetPosition.z);
        }
    }
}
