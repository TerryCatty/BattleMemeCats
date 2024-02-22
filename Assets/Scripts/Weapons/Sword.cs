using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Sword : Arms
{
    [SerializeField] protected Vector3 endPos;
    [SerializeField] protected Vector3 startPos;



    protected override void AttackAnimation()
    {
        startPos = transform.localPosition;
        endPos = startPos;
        endPos = new Vector3(endPos.x + armsKeeper.multiplier, endPos.y, endPos.z);
       StartCoroutine(AttackAnimate());
    }

    IEnumerator AttackAnimate()
    {
        animationIsEnd = false;
        transform.DOLocalMove(endPos, timeAttack);
        yield return new WaitForSeconds(timeAttack);
        isHitting = false;
        armsKeeper.CheckEnemy();
        transform.DOLocalMove(startPos, timeAttack);
        animationIsEnd = true;
        yield return new WaitForSeconds(timeAttack);
        armsKeeper.CheckEnemy();
    }
}
