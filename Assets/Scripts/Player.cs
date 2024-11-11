using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;

    public string currentMapName;
    public string currentStartPointID;
    public float speed;
    private Rigidbody2D rigid;
    private Vector2 moveVelocity;
    private Vector2 lastMoveInput;

    GameObject scanObject;
    public LayerMask noPassLayer;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public Quest quest;
    public bool questStatus;
    public List<bool> clearStatus;

    public float attackCooldown = 0.5f;
    private float nextAttackTime = 0f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;

    public int maxHp = 4;
    public int hp;
    public string revivePoint = "RevivePoint";
    private WaitForFixedUpdate wait = new WaitForFixedUpdate();
    public Inventory inventory;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = GameObject.Find("Canvas/Inventory").GetComponent<Inventory>();

        if (inventory == null)
        {
            Debug.LogError("No Inventory");
        }
    }

    void Start()
    {
        hp = maxHp;
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

        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime && !animator.GetBool("IsAttacking")) //공격
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }

        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            GameManager.instance.Action(scanObject);
        }

        if (Input.GetKeyDown(KeyCode.J)) //퀘스트 창
        {
            if (!GameManager.instance.isAction)
            {
                GameManager.instance.ShowQuest();
            }

            else
            {
                GameManager.instance.HideQuest();
            }
        }

        // 인벤토리 창
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (GameManager.instance.GetInventoryShow())
            {
                GameManager.instance.SetInventoryShow(false);
                GameManager.instance.HideInventory();
            }

            else
            {
                GameManager.instance.SetInventoryShow(true);
                GameManager.instance.ShowInventory();
            }
        }

        // 지도 창
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (GameManager.instance.GetMapShow())
            {
                GameManager.instance.SetMapShow(false);
                GameManager.instance.HideMap();
            }

            else
            {
                GameManager.instance.SetMapShow(true);
                GameManager.instance.ShowMap();
            }
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

    private void Attack()
    {
        animator.SetTrigger("AttackTrigger");
        SoundManager.instance.SFXPlay(10);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit enemy: " + enemy.gameObject.name);
            Monster monster = enemy.gameObject.GetComponent<Monster>();
            if (monster != null)
            {
                monster.GotDamage(attackDamage);
            }
        }
    }

    public void ResetAttack()
    {
        animator.SetBool("IsAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            GotDamage(other.gameObject);
            GameManager.instance.SetHP(hp);
        }
    }

    public void GotDamage(GameObject enemy)
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        Monster monster = enemy.GetComponent<Monster>();
        if (monster != null)
        {
            hp -= monster.damage;
            SoundManager.instance.SFXPlay(11);
            StartCoroutine(KnockBack(enemy));
        }

        Invoke("OffInvincible", 3);

        if (hp <= 0)
        {
            Resurrect();
        }
    }

    private IEnumerator KnockBack(GameObject enemy)
    {
        yield return wait;
        Vector2 dir = transform.position - enemy.transform.position;
        rigid.AddForce(dir.normalized * 10, ForceMode2D.Impulse);
    }

    public void OffInvincible()
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void SetMaxHp(int hp) {
        maxHp = hp;
    }

    public void SetHp(int hp) {
        this.hp = hp;
    }

    public void RestoreHP(int hp)
    {
        this.hp += hp;
        if (this.hp > maxHp) {
            this.hp = maxHp;
        }
    }

    public void SetDamage(int damage) {
        attackDamage = damage;
    }

    private void Resurrect()
    {
        hp = maxHp;
        StartCoroutine(LoadReviveScene());
    }

    private IEnumerator LoadReviveScene()
    {
        LoadingSceneController.Instance.LoadScene("Mountain_Player_House");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Mountain_Player_House");

        GameObject reviveLocation = GameObject.Find(revivePoint);
        if (reviveLocation != null)
        {
            transform.position = reviveLocation.transform.position;
        }

        else
        {
            Debug.LogWarning("부활 지점 X");
        }
    }
}