using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum MonsterType { Aggresive, Passive }

    public MonsterType type;
    public int hp;
    public int speed = 5;
    public int damage = 5;

    public Item dropItemA;
    public Item dropItemB;
    public float probabilityA;

    [SerializeField] int nextXMove;
    [SerializeField] int nextYMove;
    [SerializeField] float range;
    [SerializeField] bool isHit;
    [SerializeField] bool wasHit;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    [SerializeField] GameObject target;

    void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameManager.instance.player.gameObject;
        Invoke("Think", 2);
    }

    void Update() 
    {
        if (!isHit) 
        {
            range = Vector2.Distance(target.transform.position, transform.position);
        }
    }

    void FixedUpdate() 
    {
        if (type == MonsterType.Aggresive) 
        {
            if (range <= 7) 
            {
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

            else 
            {
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

        else if (type == MonsterType.Passive) 
        {
            if (range <= 7) 
            {
                if (wasHit) 
                {
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

                else 
                {
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

            else 
            {
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
    }

    void Think() 
    {
        nextXMove = Random.Range(-1, 2);
        nextYMove = Random.Range(-1, 2);
        Invoke("Stop", 2);
    }

    void Stop() 
    {
        nextXMove = 0;
        nextYMove = 0;
        Invoke("Think", 2);
    }

    public void GotDamage(int damage) 
    {
        isHit = true;
        wasHit = true;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        CancelInvoke();
        nextXMove = 0;
        nextYMove = 0;
        range = 100;

        hp -= damage;
        SoundManager.instance.SFXPlay(11);

        if (hp <= 0) {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            anim.SetBool("isDead", true);
            Die();
        }

        Invoke("OffInvincible", 3);
    }

    void OffInvincible() 
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        isHit = false;
        Invoke("Think", 2);
    }

    public void Die() 
    {
        SoundManager.instance.SFXPlay(12);
        Debug.Log("Enemy is Dead");
        DropRandomItem();
        gameObject.SetActive(false);
    }

    private void DropRandomItem()
    {
        Item droppedItem = (UnityEngine.Random.value <= probabilityA) ? dropItemA : dropItemB;

        if (droppedItem != null)
        {
            Inventory inventory = GameManager.instance.GetInventory().gameObject.GetComponent<Inventory>();
            inventory.AddItem(droppedItem);
            inventory.FreshSlot();
            GameManager.instance.GetItemBar().RefreshSlot();
            Debug.Log($"Dropped item: {droppedItem.name}");
        }
    }
}