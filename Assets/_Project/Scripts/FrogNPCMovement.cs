using System.Collections;
using UnityEngine;

public class FrogNPCMovement : MonoBehaviour
{
    private enum MovePattern
    {
        Horizontal,
        Vertical,
        FourWayBox
    }

    [SerializeField] private MovePattern pattern = MovePattern.Horizontal;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float travelDistance = 3f;
    [SerializeField] private float waitTime = 1.2f;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 startOrigin;
    private Vector2[] waypoints;
    private int currentTargetIndex = 0;
    private bool isWaiting = false;
    private Vector2 lastFacingDirection = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = GetComponentInParent<Rigidbody2D>();
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Start()
    {
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            startOrigin = rb.position;
        }
        else
        {
            startOrigin = transform.position;
        }

        SetupRoute();
        UpdateAnimator(false, Vector2.zero, lastFacingDirection);
    }

    private void SetupRoute()
    {
        switch (pattern)
        {
            case MovePattern.Horizontal:
                waypoints = new Vector2[]
                {
                    startOrigin + (Vector2.right * travelDistance),
                    startOrigin,
                    startOrigin + (Vector2.left * travelDistance),
                    startOrigin
                };
                break;

            case MovePattern.Vertical:
                waypoints = new Vector2[]
                {
                    startOrigin + (Vector2.up * travelDistance),
                    startOrigin,
                    startOrigin + (Vector2.down * travelDistance),
                    startOrigin
                };
                break;

            case MovePattern.FourWayBox:
                waypoints = new Vector2[]
                {
                    startOrigin + (Vector2.right * travelDistance),
                    startOrigin + (Vector2.right * travelDistance) + (Vector2.up * travelDistance),
                    startOrigin + (Vector2.up * travelDistance),
                    startOrigin
                };
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isWaiting || waypoints == null || waypoints.Length == 0) return;

        Vector2 currentPos = rb != null ? rb.position : (Vector2)transform.position;
        Vector2 targetPos = waypoints[currentTargetIndex];
        Vector2 diff = targetPos - currentPos;

        if (diff.sqrMagnitude <= 0.0025f)
        {
            StartCoroutine(WaitAtTarget(targetPos));
            return;
        }

        Vector2 dir = diff.normalized;
        Vector2 snapDir = GetDominantDirection(dir);
        lastFacingDirection = snapDir;

        Vector2 nextPos = Vector2.MoveTowards(currentPos, targetPos, moveSpeed * Time.fixedDeltaTime);

        if (rb != null)
        {
            rb.MovePosition(nextPos);
        }
        else
        {
            transform.position = nextPos;
        }

        UpdateAnimator(true, snapDir, lastFacingDirection);
    }

    private Vector2 GetDominantDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            return dir.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return dir.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    private IEnumerator WaitAtTarget(Vector2 snapPosition)
    {
        isWaiting = true;

        if (rb != null)
        {
            rb.position = snapPosition;
            rb.velocity = Vector2.zero;
        }
        else
        {
            transform.position = snapPosition;
        }

        UpdateAnimator(false, Vector2.zero, lastFacingDirection);

        yield return new WaitForSeconds(waitTime);

        currentTargetIndex = (currentTargetIndex + 1) % waypoints.Length;
        isWaiting = false;
    }

    private void UpdateAnimator(bool isWalking, Vector2 currentDir, Vector2 lastDir)
    {
        if (animator == null) return;

        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            animator.SetFloat("inputX", currentDir.x);
            animator.SetFloat("inputY", currentDir.y);
        }

        animator.SetFloat("lastInputX", lastDir.x);
        animator.SetFloat("lastInputY", lastDir.y);
    }
}