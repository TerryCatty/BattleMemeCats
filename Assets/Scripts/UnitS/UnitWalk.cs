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
    }
    public override void UpdateAction()
    {
        base.UpdateAction();
        rb.velocityX = moveSpeed;
    }

    public override void Exit()
    {
        rb.velocityX = 0;
    }
}
