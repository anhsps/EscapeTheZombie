using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private float horiInput, verInput;
    private float moveInput;
    private Vector3 moveDir;
    private Vector3 groundCheck;
    //private CheckWall checkWall;
    [SerializeField] private float moveSP = 7f;
    [SerializeField] private float jumpSP = 12f;

    [Header("KnockBack")]
    [SerializeField] private float kbForce = 3f;
    [SerializeField] private float kbDuration = 0.4f;
    [HideInInspector] public bool kbFromRight;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        checkWall = GetComponentInChildren<CheckWall>();
        //Test();
    }

    // Update is called once per frame
    void Update()
    {
        if (done) return;

        if (!IsOnScreen(transform.position)) StartCoroutine(DelayLose());

        horiInput = Input.GetAxisRaw("Horizontal");
        verInput = Input.GetAxisRaw("Vertical");
        moveInput = (moveDir.x != 0) ? moveDir.x : horiInput;

        rb.velocity = new Vector2(moveInput * moveSP, rb.velocity.y);

        if (IsGrounded() && (verInput > 0 || moveDir.y > 0))
            rb.velocity = new Vector2(rb.velocity.x, jumpSP);

        if (checkWall.isWall) rb.velocity = new Vector2(0, rb.velocity.y);

        animator.SetBool("Run", moveInput != 0);

        if (moveInput > 0 && facingLeft || moveInput < 0 && !facingLeft)
            Flip();
    }

    public void SetMoveDir(Vector3 dir) => moveDir = dir;

    private bool IsGrounded()
    {
        groundCheck = new Vector2(transform.position.x, col.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(groundCheck, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    /*private bool CheckWall()
    => Physics2D.OverlapPoint(wallCheck.position, LayerMask.GetMask("Ground", "Wall"));*/

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, groundCheck);
    }*/

    private IEnumerator Winner()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Win");
        GameManager19.Instance.GameWin();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            SoundManager19.Instance.PlaySound(5);
            //GameManager19.Instance.IncreaseScore(1);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if (collision.gameObject.CompareTag("Finish"))
        {
                collision.GetComponent<Collider2D>().enabled = false;
                //animator.Rebind();
                rb.velocity = Vector2.zero;
                StartCoroutine(Winner());
                done = true;
        }*/

        /*if (collision.gameObject.CompareTag("Trap"))
        {
            collision.GetComponent<Collider2D>().enabled = false;
            kbFromRight = !facingLeft;
            StartCoroutine(DelayLose());
        }*/
    }

    public IEnumerator DelayLose()
    {
        HandlerDie(true, 4);

        float x = kbForce * (kbFromRight ? -1 : 1);
        yield return KnockBack(rb, kbDuration, x, kbForce / 3);

        yield return new WaitForSeconds(1f);
        GameManager19.Instance.GameLose();
    }

    private bool IsOnScreen(Vector3 pos)
    {
        // convert pos: world space -> viewpost space
        Vector3 screenPos = Camera.main.WorldToViewportPoint(pos);
        // check pos vs range [0,1] trong Viewpost space
        return screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1;
    }
}
