using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_First : MonoBehaviour
{
    public static Player_First instance;

    public float speed;
    private Rigidbody2D rigid;
    private Vector2 moveVelocity;
    private Vector2 lastMoveInput;

    GameObject scanObject;

    public LayerMask noPassLayer;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private WaitForFixedUpdate wait = new WaitForFixedUpdate();

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        if (Input.GetButtonDown("Jump") && scanObject != null && GameManager_First.instance != null)
        {
            GameManager_First.instance.Action(scanObject);
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = rigid.position + moveVelocity * Time.fixedDeltaTime;

        RaycastHit2D hit = Physics2D.Raycast(rigid.position, moveVelocity.normalized, moveVelocity.magnitude * Time.fixedDeltaTime, noPassLayer);
        if (hit.collider == null)
        {
            rigid.MovePosition(targetPosition);
        }

        RaycastHit2D npcHit = Physics2D.Raycast(rigid.position, lastMoveInput.normalized, 10f, LayerMask.GetMask("Object"));
        scanObject = npcHit.collider != null ? npcHit.collider.gameObject : null;
    }

    public void OffInvincible()
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}