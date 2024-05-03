using System;
using System.Collections;
using UnityEngine;

public class UnitWarrior : Unit
{
    public Arms arms;
    protected Unit_StateMachine _stateMachine;

    public float moveSpeed { get; private set; }
    [SerializeField] private float baseMoveSpeed;

    public int damage { get; private set; }
    [SerializeField] private int baseDamage;

    public float attackRate { get; private set; }
    [SerializeField] private float baseAttackRate;

    private Unit attackedUnit;

    private SpriteRenderer spriteRenderer;


    public int multiplier = 1;

    [SerializeField] private GameObject particleDeath;

    public override void Init(UnitTower tower)
    {
        base.Init(tower);

        colorTeam = tower.colorTeam;

        if (tower.transform.localScale.x < 0)
            multiplier = -1;




        spriteRenderer = GetComponent<SpriteRenderer>();

        _stateMachine = new Unit_StateMachine(this);

        transform.localScale = new Vector3(
            transform.localScale.x * multiplier,
            transform.localScale.y, transform.localScale.z);


        damage = Convert.ToInt32(baseDamage + baseDamage * tower.buffs.damageBuff);
        moveSpeed = baseMoveSpeed * multiplier + baseMoveSpeed * tower.buffs.speedBuff;
        attackRate = baseAttackRate - baseAttackRate * tower.buffs.attackRateBuff;

        health = health + Convert.ToInt32(health * tower.buffs.healthBuff);

        arms.Init(this);
        Walk();
    }

   

    private void Update()
    {

        _stateMachine.DoAction();

    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        StartCoroutine(GetDamageAnimation());
    }

    IEnumerator GetDamageAnimation()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = Color.white;
    }
    private void Attack()
    {
        _stateMachine.ChangeState(_stateMachine.stateAttack);
    }

    private void Walk()
    {
        _stateMachine.ChangeState(_stateMachine.stateWalk);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Unit") && other.GetComponent<Unit>().colorTeam != colorTeam)
        {
            attackedUnit = other.GetComponent<Unit>();
            Attack();
        }

       
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit") && collision.GetComponent<Unit>().colorTeam != colorTeam)
        {
            attackedUnit = collision.GetComponent<Unit>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Unit") && other.GetComponent<Unit>().colorTeam != colorTeam)
        {
            Walk();
        }
    }



    public void CheckEnemy()
    {
        if (attackedUnit == null || attackedUnit.isDead)
        {
            
            Walk();
        }
    }

    protected override void CheckDeath()
    {
        if (health <= 0)
        {
            SpawnDeathEffect();
            base.CheckDeath();
            tower.IncreaseKillsOpponent();
        }
       
    }

    private void SpawnDeathEffect()
    {
        Instantiate(particleDeath, transform.position, Quaternion.identity);
    }
}
