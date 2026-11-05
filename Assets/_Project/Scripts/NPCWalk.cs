using System.Collections;
using UnityEngine;

public class NPCWalk : MonoBehaviour
{
    private enum MovePattern
    {
        Horizontal,
        Vertical,
        FourWayBox
    }

    [Header("Movement Settings")]
    [SerializeField] private MovePattern pattern = MovePattern.Horizontal;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float travelDistance = 3f;
    [SerializeField] private float waitTime = 5f; // Jeda diam 5 detik

    [Header("Sprite Force Settings")]
    [Tooltip("CENTANG jika gambar aset sprite bawaan kamu menghadap KANAN. HILANGKAN CENTANG jika menghadap KIRI.")]
    [SerializeField] private bool defaultSpriteFacesRight = true;

    [Tooltip("CENTANG jika Animator Blend Tree kamu SUDAH PUNYA animasi Kiri dan Kanan sendiri (supaya tidak di-flip paksa oleh script).")]
    [SerializeField] private bool useAnimatorForLeftRight = false;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 startOrigin;
    private Vector2[] waypoints;
    private int currentTargetIndex = 0;
    private bool isWaiting = false;
    private Vector2 lastFacingDirection = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = GetComponentInParent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        
        // Langsung arahkan sprite ke waypoint pertama di awal game
        if (waypoints != null && waypoints.Length > 0)
        {
            Vector2 initialDir = (waypoints[0] - startOrigin).normalized;
            Vector2 initialSnap = GetDominantDirection(initialDir);
            lastFacingDirection = initialSnap;
            ForceFlipSprite(initialSnap.x);
        }

        UpdateAnimator(false, Vector2.zero, lastFacingDirection);
    }

    private void SetupRoute()
    {
        switch (pattern)
        {
            case MovePattern.Horizontal:
                waypoints = new Vector2[]
                {
                    startOrigin + (Vector2.left * travelDistance), // Jalan ke Kiri dulu
                    startOrigin + (Vector2.right * travelDistance) // Lalu ke Kanan
                };
                break;

            case MovePattern.Vertical:
                waypoints = new Vector2[]
                {
                    startOrigin + (Vector2.up * travelDistance),
                    startOrigin + (Vector2.down * travelDistance)
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

        // PAKSA SPRITE FLIP SESUAI ARAH GERAK SAA T INI
        ForceFlipSprite(snapDir.x);

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

    private void ForceFlipSprite(float dirX)
    {
        if (spriteRenderer == null || useAnimatorForLeftRight) return;

        // Gerak ke KANAN (dirX > 0)
        if (dirX > 0.01f)
        {
            spriteRenderer.flipX = !defaultSpriteFacesRight;
        }
        // Gerak ke KIRI (dirX < 0)
        else if (dirX < -0.01f)
        {
            spriteRenderer.flipX = defaultSpriteFacesRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isWaiting)
        {
            StopAllCoroutines();
            StartCoroutine(TurnAroundOnCollision());
        }
    }

    private IEnumerator TurnAroundOnCollision()
    {
        isWaiting = true;

        if (rb != null) rb.velocity = Vector2.zero;

        UpdateAnimator(false, Vector2.zero, lastFacingDirection);

        yield return new WaitForSeconds(waitTime);

        currentTargetIndex = (currentTargetIndex + 1) % waypoints.Length;
        isWaiting = false;
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
        
        // Paksa ubah pandangan ke arah target berikutnya SEBELUM melangkah
        Vector2 nextTargetPos = waypoints[currentTargetIndex];
        Vector2 nextDir = (nextTargetPos - snapPosition).normalized;
        Vector2 nextSnap = GetDominantDirection(nextDir);
        ForceFlipSprite(nextSnap.x);

        isWaiting = false;
    }

    private void UpdateAnimator(bool isWalking, Vector2 currentDir, Vector2 lastDir)
    {
        if (animator == null) return;

        animator.SetBool("isWalking", isWalking);

        // Kirim nilai asli InputX (-1 untuk Kiri, 1 untuk Kanan)
        if (isWalking)
        {
            animator.SetFloat("InputX", currentDir.x);
            animator.SetFloat("InputY", currentDir.y);
        }

        animator.SetFloat("LastInputX", lastDir.x);
        animator.SetFloat("LastInputY", lastDir.y);
    }
}