using UnityEngine;

public class UnitWalk : UnitState
{
    private float moveSpeed;
    private Rigidbody2D rb;

    private Transform parent;

    public override void InitState(UnitWarrior unit)
    {
        base.InitState(unit);
        moveSpeed = unit.moveSpeed;
        parent = unit.transform.parent;
        rb = parent.GetComponent<Rigidbody2D>();

        anim.SetBool("isWalk", true);
    }
    public override void UpdateAction()
    {
        base.UpdateAction();
        rb.velocityX = moveSpeed;
    }

    public override void Exit()
    {
        anim.SetBool("isWalk", false);
        rb.velocityX = 0;
    }
}
