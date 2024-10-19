using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char : MonoBehaviour
{
    static public Char instance;

    public string currentMapName;
    public string currentStartPointID;

    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    public LayerMask noPassLayer;

    private void Start()
    {
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
