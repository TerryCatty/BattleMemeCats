using System.Collections;
using UnityEngine;

public class Arms : MonoBehaviour
{
    [SerializeField] protected float timeAttack;
    [SerializeField] protected int damage;
    protected bool isAttack = false;
    protected bool isHitting = false;

    protected UnitWarrior armsKeeper;

    public bool animationIsEnd;

    protected Animator anim;
    public void Init(UnitWarrior unit)
    {
        armsKeeper = unit;
        animationIsEnd = true;
        timeAttack = unit.attackRate / 2;
        anim = GetComponent<Animator>();
    }
    public void Attack()
    {
        isAttack = true;
        isHitting = true;
        AttackAnimation();
    }

    protected virtual void AttackAnimation()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHitting)
        {
            if (collision.gameObject.tag == "Unit" && collision.gameObject.GetComponent<Unit>().colorTeam != armsKeeper.colorTeam)
            {
                collision.gameObject.GetComponent<Unit>().GetDamage(damage);
            }
        }
    }

}
