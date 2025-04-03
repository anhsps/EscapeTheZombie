using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    /*[HideInInspector] */public float girlNum, itemNum;
    private float girlMax, itemMax;
    private TimeDisplay timeDisplay;

    [Header("Move")]
    private float horiInput, verInput;
    private float moveInput;
    private Vector3 moveDir;
    private Vector3 groundCheck;
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
        timeDisplay = FindObjectOfType<TimeDisplay>(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (done) return;

        if (!IsOnScreen(transform.position)) StartCoroutine(DelayLose());

        if (timeDisplay.currentTime <= 0)
        {
            GameManager19.Instance.GameLose();
            done = true;
        }

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
        return Physics2D.OverlapCircle(groundCheck, 0.1f, layerGround);
    }

    private IEnumerator Winner()
    {
        animator.Rebind();
        rb.velocity = Vector2.zero;
        done = true;
        yield return new WaitForSeconds(1f);
        Debug.Log("Win");
        GameManager19.Instance.GameWin();
    }

    public IEnumerator DelayLose()
    {
        HandlerDie(true, 4);

        float x = kbForce * (kbFromRight ? -1 : 1);
        yield return KnockBack(rb, kbDuration, x, kbForce / 3);

        yield return new WaitForSeconds(1f);
        GameManager19.Instance.GameLose();
    }

    private void CheckWin()
    {
        if (girlNum >= girlMax && itemNum >= itemMax)
            StartCoroutine(Winner());
    }

    public void UpdateGirlItemCount()
    {
        girlMax = FindObjectsOfType<Girl>().Length;
        itemMax = GameObject.FindGameObjectsWithTag("Item").Length;
        Debug.Log("girl: " + girlMax);
        Debug.Log("item: " + itemMax);
    }

    public void CollectItem()
    {
        SoundManager19.Instance.PlaySound(5);
        itemNum++;
        CheckWin();
    }

    public void IncreaseGirl()
    {
        girlNum++;
        CheckWin();
    }

    private bool IsOnScreen(Vector3 pos)
    {
        // convert pos: world space -> viewpost space
        Vector3 screenPos = Camera.main.WorldToViewportPoint(pos);
        // check pos vs range [0,1] trong Viewpost space
        return screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1;
    }
}
