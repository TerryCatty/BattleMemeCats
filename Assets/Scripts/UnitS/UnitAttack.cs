using UnityEngine;

public class UnitAttack : UnitState
{
    private float attackRate;
    private float timer = 0;
    private Arms arms;

    private float defaultAnimSpeed;
    public override void InitState(UnitWarrior unit)
    {
        base.InitState(unit);
        attackRate = unit.attackRate;
        arms = unit.arms;

        defaultAnimSpeed = anim.speed;

        anim.speed = 1 / attackRate;
        anim.SetBool("isAttack", true);
    }
    public override void UpdateAction()
    {
        base.UpdateAction();

        timer -= Time.deltaTime;
        if (timer <= 0 && arms.animationIsEnd)
        {
            timer = attackRate;
            Attack();
        }

    }

    private void Attack()
    {
        arms.Attack();
    }

    public override void Exit()
    {
        anim.speed = defaultAnimSpeed;
        anim.SetBool("isAttack", false);
        base.Exit();
    }

}
