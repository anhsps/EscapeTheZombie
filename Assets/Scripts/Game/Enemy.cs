using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform groundCheck;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        checkWall = GetComponentInChildren<CheckWall>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (!CheckGround() || checkWall.isWall) Flip();
    }

    private void Move()
    {
        //animator.SetTrigger("Run");
        float moveDir = facingLeft ? -1 : 1;
        transform.Translate(Vector2.right * moveDir * speed * Time.deltaTime);
    }

    private bool CheckGround()
    {
        groundCheck.position = new Vector2(groundCheck.position.x, col.bounds.min.y);
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, layerGround);
    }
}
