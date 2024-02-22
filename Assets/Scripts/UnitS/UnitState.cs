using UnityEngine;

public class UnitState : MonoBehaviour
{
    public bool isActive = false;
    public Unit unit;

    protected Animator anim;

    public virtual void InitState(UnitWarrior unit)
    {
        this.unit = unit; 
        anim = unit.GetComponent<Animator>();
        isActive = true;
    }
    public virtual void UpdateAction()
    {
        if (isActive == false) return;
    }

    public virtual void Exit()
    {
        isActive = false;
    }
}
