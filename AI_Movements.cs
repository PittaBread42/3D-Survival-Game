using UnityEngine;

public class AI_Movement : MonoBehaviour
{
    Animator animator;

    public float moveSpeed = 1.5f;
    public float rotationSpeed = 4f;
    public AnimationCurve hopCurve;
    public float hopHeight = 0.5f;

    public float raycastHeight = 1.0f;
    public float groundOffset = 0.0f;
    public LayerMask groundLayer;


    float hopTimer;
    public float hopDuration = 0.6f; // match your hop animation length


    float walkTime;
    float walkCounter;

    float waitTime;
    float waitCounter;

    bool isWalking;
    float targetAngle;

    void Start()
    {
        animator = GetComponent<Animator>();

        walkTime = Random.Range(3f, 6f);
        waitTime = Random.Range(5f, 7f);

        walkCounter = walkTime;
        waitCounter = waitTime;

        ChooseDirection();
    }

    void Update()
    {

        if (isWalking)
        {
            animator.SetFloat("Speed", 1f);

            walkCounter -= Time.deltaTime;

            // Smooth rotation
            Quaternion targetRot = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            // Move forward
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            // HOP MOTION
            hopTimer += Time.deltaTime;
            float t = (hopTimer % hopDuration) / hopDuration;

            float hopOffset = hopCurve.Evaluate(t) * hopHeight;

            transform.position = new Vector3(
            transform.position.x,
            hopOffset,
            transform.position.z
            );

            Ray ray = new Ray(transform.position + Vector3.up * raycastHeight, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 5f, groundLayer))
            {
                Vector3 pos = transform.position;
                pos.y = hit.point.y + groundOffset;
                transform.position = pos;
            }


            if (walkCounter <= 0)
            {
                isWalking = false;
                animator.SetFloat("Speed", 0f);
                waitCounter = waitTime;

                hopTimer = 0f; // reset hop cycle
            }

        }
        else
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }

    void ChooseDirection()
    {
        // Pick a random angle (0–360)
        targetAngle = Random.Range(0f, 360f);

        isWalking = true;
        walkCounter = walkTime;
    }
}

