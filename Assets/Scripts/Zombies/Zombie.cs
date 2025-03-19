using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


enum ZombieState
{
    None,
    Move,
    Eat,
    Die
}

public class Zombie : MonoBehaviour, IPausable, IPoolObject
{
    [Header("Settings")]
    public float moveSpeedType1 = 0.5f;
    public float moveSpeedType2 = 0.5f;
    public int liveHP = 181;
    public int dyingHP = 89;
    [SerializeField] int currentHP;
    public int damagePerAttack = 20;
    public float attachInterval = 0.2f;
    [SerializeField] private GameObject zombieHeadPrefab;
    [Header("Auto Decaying HP Settings")]
    public int decayHPPerTick = 5;
    public float decayInterval = 0.2f;
    private float moveSpeed;
    private int moveType;
    private bool canMove;
    private bool lostHead;
    [Header("Debugging")]
    [SerializeField] private ZombieState zombieState = ZombieState.Move;
    private Plant attackingPlant;
    private Rigidbody2D rbd;
    private Animator anim;
    private float pauseAnimSpeed;
    private ZombieState pauseZombieState;

    public virtual GameObject Prefab { get => PrefabConfig.Instance.ZombiePrefab; }

    private void Awake()
    {
        rbd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveType = Random.Range(0, 2);
        anim.SetInteger("moveType", moveType);
        if(moveType == 0)
        {
            moveSpeed = moveSpeedType1;
        }
        else
        {
            moveSpeed = moveSpeedType2;
        }
        anim.SetFloat("cycleOffset", Random.value);
        currentHP = liveHP + dyingHP;
        lostHead = false;
    }

    private void OnEnable()
    {
        PauseManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        PauseManager.Instance.Unregister(this);
    }

    private void FixedUpdate()
    {
        if (canMove && zombieState == ZombieState.Move)
        {
            rbd.MovePosition(rbd.position + Vector2.left * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void AddMovement()
    {
        canMove = true;
    }

    public void StopMovement()
    {
        canMove = false;
    }

    private IEnumerator AttackPlant()
    {
        while(true)
        {
            if (zombieState == ZombieState.None)
            {
                yield return null;
                continue;
            }
            yield return new PausableWaitForSeconds(attachInterval);
            if (attackingPlant != null)
            {
                attackingPlant.ReceiveDamage(damagePerAttack);
            }
            else break;
        }
    }

    private IEnumerator AutoDecayDyingHP()
    {
        while (true)
        {
            if(zombieState == ZombieState.None)
            {
                yield return null;
                continue;
            }
            currentHP -= decayHPPerTick;
            if(currentHP <= 0)
            {
                Die();
                break;
            }
            yield return new PausableWaitForSeconds(decayInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (zombieState == ZombieState.None || zombieState == ZombieState.Die) return;
        if(collision.CompareTag("Plant"))
        {
            anim.SetBool("isEating", true);
            zombieState = ZombieState.Eat;
            attackingPlant = collision.GetComponent<Plant>();
            StartCoroutine(AttackPlant());
        } else if (collision.CompareTag("LeftEnd"))
        {
            GameEventManager.Instance.TriggerZombieGetIn(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (zombieState == ZombieState.None || zombieState == ZombieState.Die) return;
        if (collision.CompareTag("Plant"))
        {
            anim.SetBool("isEating", false);
            zombieState = ZombieState.Move;
            attackingPlant = null;
        }
    }

    public void AttackPlantEffect()
    {
        if (attackingPlant == null) return;
        AudioManager.Instance.PlaySound(AudioConfig.GetRandomChomp(), transform.position);
        attackingPlant.GotHitEffect();
    }

    public void ReceiveDamage(int damage)
    {
        currentHP -= damage;
        anim.SetTrigger("gotHit");
        if (currentHP <= 0)
        {
            Die();
        }
        else if (currentHP <= dyingHP && !lostHead)
        {
            lostHead = true;
            LoseHead();
            StartCoroutine(AutoDecayDyingHP());
        }
    }

    private void LoseHead()
    {
        anim.SetTrigger("lostHead");
        GameObject zombieHead = Instantiate(zombieHeadPrefab, transform.position, Quaternion.identity);
        Destroy(zombieHead, 3.0f);
    }

    private void Die()
    {
        if (zombieState == ZombieState.Die) return;
        zombieState = ZombieState.Die;
        anim.SetTrigger("die");
        attackingPlant = null;
        GetComponent<BoxCollider2D>().enabled = false;
        GameEventManager.Instance.TriggerZombieDie(this);
        PausableTask.DelayedCall(5000, AddToPool);
        // DOVirtual.DelayedCall(5.0f, AddToPool);
        // Destroy(gameObject, 5f);
    }

    public void OnPause()
    {
        pauseAnimSpeed = anim.speed;
        anim.speed = 0;
        pauseZombieState = zombieState;
        zombieState = ZombieState.None;
    }

    public void OnResume()
    {
        anim.speed = pauseAnimSpeed;
        zombieState = pauseZombieState;
    }

    public void ResetState()
    {
        anim.Play("New State");
        zombieState = ZombieState.Move;
        currentHP = liveHP + dyingHP;
        lostHead = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void AddToPool()
    {
        StopAllCoroutines();
        PoolManager.Instance.AddGameObject(this);
    }

    private void OnDestroy()
    {
        PauseManager.Instance.Unregister(this);
    }
}
