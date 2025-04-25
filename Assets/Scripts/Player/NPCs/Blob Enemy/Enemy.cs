using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    #region Variables
    public EnemyBaseState currentState;

    //State Variables
    public PatrolState patrolState;
    public PlayerDetectedState playerDetectedState;
    public ChargeState chargeState;
    public MeleeAttackState meleeAttackState;
    public DamagedState damagedState;
    public DeathState deathState;

    public Animator anim;
    public Rigidbody2D rb;
    public Transform ledgeDetector;
    public LayerMask groundLayer, playerLayer, damageableLayer;
    //public LayerMask obstacleLayer;

    public int facingDirection = 1;
    

    public float stateTime;

    public StatsSO stats;
    public float currentHealth;

    public GameObject alert;

    [Header("Item Drop Variables")]
    public List<GameObject> itemDrops = new List<GameObject>();
    public float dropForce;
    public float torque;

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        patrolState = new PatrolState(this, "patrol");
        playerDetectedState = new PlayerDetectedState(this, "playerDetected");
        chargeState = new ChargeState(this, "charge");
        meleeAttackState = new MeleeAttackState(this, "meleeAttack");
        damagedState = new DamagedState(this, "isAttacking");
        deathState = new DeathState(this, "death");

        currentState = patrolState;
        currentState.Enter();
        Debug.Log("Enemy state: " + currentState);
    }

    private void Start()
    {
        currentHealth = stats.maxHealth;
    }
    private void Update()
    {

        currentState.LogicUpdate();

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
   
    void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }
    #endregion

    #region Checks
    public bool CheckForObstacles()
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, stats.cliffCheckDistance, groundLayer);
        //RaycastHit2D hitObstacle = Physics2D.Raycast(ledgeDetector.position, Vector2.right, stats.obstacleDistance, obstacleLayer);

        //for if: || hitObstacle.collider == true (just incase I put an obstacle) 
        // obstacle layer, obstacle distance: .1 
        if (hit.collider == null)
        {
            return true;
        } else
        {
            return false;
        }
    }
    
    public bool CheckForPlayer()
    {
       RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ? Vector2.right : Vector2.left, stats.playerDetectDistance, playerLayer);
        
        if (hitPlayer.collider == true)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public bool CheckForMeleeTarget()
    {
        RaycastHit2D hitMeleeTarget = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ? Vector2.right : Vector2.left, stats.meleeDetectDistance, playerLayer);

        if (hitMeleeTarget.collider == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Other Functions

    public void SwitchState(EnemyBaseState newState)
    {
        Debug.Log($"Switching from {currentState.GetType().Name} to {newState.GetType().Name}");
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
        stateTime = Time.time;
    }

    public void Damage(float damageAmount) { }

    public void Damage(float damageAmount, float KBForce, Vector2 KBAngle)
    {
        damagedState.KBForce = KBForce;
        damagedState.KBAngle = KBAngle;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            SwitchState(deathState);
        } else
        {
            SwitchState(damagedState);
        }
    }
    public void Rotate()
    {
        transform.Rotate(0, 180, 0);
        facingDirection = -facingDirection;
    }

    public void Instantiate (GameObject prefab, float force, float torque)
    {
        Rigidbody2D itemRb = Instantiate(prefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        Vector2 dropVelocity = new Vector2(Random.Range(.5f, -.5f), 1) * dropForce;
        itemRb.AddForce(dropVelocity, ForceMode2D.Impulse);
        itemRb.AddTorque(torque, ForceMode2D.Impulse);
    }

    public void AnimationFinishedTrigger()
    {
        currentState.AnimationFinishedTrigger();
    }

    public void AnimationAttackTrigger()
    {
        currentState.AnimationAttackTrigger();
    }

    #endregion

    public void Damage2(int damage)
    {
        currentHealth -= damage;
        gameObject.GetComponent<Animation>().Play("RedFlash");
    }
}
