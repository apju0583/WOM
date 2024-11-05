using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int hp;
    public int speed = 5;
    public int damage = 5;

    [SerializeField] int nextXMove;
    [SerializeField] int nextYMove;
    [SerializeField] float range;
    Rigidbody2D rigid;
    Animator anim;
    [SerializeField] GameObject target;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameManager.instance.player.gameObject;
        Invoke("Think", 2);
    }

    void Update() {
        range = Vector2.Distance(target.transform.position, transform.position);
    }

    void FixedUpdate() {
        if (range <= 7) {
            anim.SetBool("Walking", true);

            Vector2 targetPos = target.transform.position;
            Vector2 pos = transform.position;
            Vector2 dist = targetPos - pos;
            Vector2 dir = dist.normalized;
        
            pos += speed * Time.fixedDeltaTime * dir;

            anim.SetFloat("DirX", dir.x);
            anim.SetFloat("DirY", dir.y);
            transform.position = pos;
        }
        else {
            if (nextXMove == 0 && nextYMove == 0) {
                anim.SetBool("Walking", false);
            }
            else {
                anim.SetBool("Walking", true);
            }
            anim.SetFloat("DirX", nextXMove);
            anim.SetFloat("DirY", nextYMove);

            rigid.velocity = new Vector2(nextXMove, nextYMove);
        }
    }

    void Think() {
        nextXMove = Random.Range(-1, 2);
        nextYMove = Random.Range(-1, 2);

        Invoke("Stop", 2);
    }

    void Stop() {
        nextXMove = 0;
        nextYMove = 0;

        Invoke("Think", 2);
    }
}
