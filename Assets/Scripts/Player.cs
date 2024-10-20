using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static public Player instance;

    public string currentMapName;
    public string currentStartPointID;

    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    public LayerMask noPassLayer;
    private Animator animator;

    private Vector2 lastMoveInput;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            rb = GetComponent<Rigidbody2D>();
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        animator.SetFloat("DirX", moveInput.x);
        animator.SetFloat("DirY", moveInput.y);

        if (moveInput.magnitude > 0)
        {
            animator.SetBool("Walking", true);
            lastMoveInput = moveInput;
        }
        else
        {
            animator.SetBool("Walking", false);
            animator.SetFloat("DirX", lastMoveInput.x);
            animator.SetFloat("DirY", lastMoveInput.y);
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = rb.position + moveVelocity * Time.fixedDeltaTime;

        RaycastHit2D hit = Physics2D.Raycast(rb.position, moveVelocity.normalized, moveVelocity.magnitude * Time.fixedDeltaTime, noPassLayer);

        if (hit.collider == null)
        {
            rb.MovePosition(targetPosition);
        }
    }
}