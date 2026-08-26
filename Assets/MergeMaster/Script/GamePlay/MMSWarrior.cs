using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSWarrior : MMSCard
{
    public float AttackRange = 2f;
    public Transform ModelTrans;
    Vector3 velocity;
    MMSCard cardTarget;

    public override int BeAttack(MMSCard card)
    {
        return base.BeAttack(card);
    }

    public override void AttackTarget(MMSCard card)
    {
        base.AttackTarget(card);
        cardTarget = card;
    }
    protected override IEnumerator EnumMoveToTarget(MMSCard card)
    {
        float oppenentAttackRange = 0;
        if (card is MMSWarrior)
        {
            oppenentAttackRange = ((MMSWarrior)card).AttackRange;
        }
        float attackRange = AttackRange + oppenentAttackRange;
        Vector3 targetPos = card.transform.position + new Vector3((y - 2) * (x - 1), 0, 0).normalized * attackRange;

        if ((card.transform.position - transform.position).magnitude > attackRange)
        {
           StartCoroutine(EnumLookToTarger(targetPos));
        }
        else
        {
            yield break;
        }        
            
        while ((card.transform.position - transform.position).magnitude > attackRange)
        {
            MoveAnimation();
            yield return null;

            if (card.Health <= 0|| this.Health <= 0)
                break;

            targetPos = card.transform.position + new Vector3((y - 2) * (x - 1), 0, 0).normalized * attackRange;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed * Time.deltaTime);
        }

        yield return StartCoroutine(EnumLookToTarger(card.transform.position));
    }

    protected override IEnumerator EnumAttack(MMSCard card)
    {
        while (this.Health > 0 && card.Health > 0)
        {
            yield return StartCoroutine(EnumMoveToTarget(card));

            if (this.Health > 0)
            {
                if (card.Health > 0)
                {
                    AttackAnimation();
                }
                else
                {
                    break;
                }
            }

            yield return new WaitForSeconds(AttackSpeed);
        }

        if (card.Health <= 0 && this.Health > 0)
        {
            if (OnFinishAttack != null)
            {
                OnFinishAttack(this);
            }
        }
    }

    public bool HitCallBack()
    {
        if (cardTarget == null) return false;
        if (cardTarget.Health > 0)
        {
            int coin = cardTarget.BeAttack(this);

            if (OnEarnCoin != null)
                OnEarnCoin(this, coin);

            CoinFlyEffect(cardTarget, coin);

            if (cardTarget.OnGetBoard().Name == "User" && MMSGameController.Instance.isVibrate)
                Vibration.VibratePop();

            return true;
        }
        return false;
    }

    IEnumerator EnumLookToTarger(Vector3 target)
    {
        int index = 0;
        while (index < 10)
        {
            yield return null;
            ModelTrans.forward = Vector3.Lerp(ModelTrans.forward, (target - transform.position).normalized, 7.5f * Time.deltaTime);
            index++;
        }
        ModelTrans.forward = (target - transform.position).normalized;
    }

    public override void AttackAnimation()
    {
        if (this.Health <= 0)
            return;
        Animator.SetBool("Attack", true);
        Animator.SetBool("Move", false);
    }

    public override void DeadAnimation()
    {
        StopAllCoroutines();
        Animator.SetBool("Attack", false);
        Animator.SetBool("Move", false);
        Animator.Play("Die");
        this.enabled = false;
    }

    public override void MoveAnimation()
    {
        if (this.Health <= 0)
            return;
        Animator.SetBool("Move", true);
        Animator.SetBool("Attack", false);
    }

    public override void WinAnimation()
    {
        if (this.enabled == false)
        {
            DeadAnimation();
            return;
        }

        Animator.SetBool("Attack", false);
        Animator.SetBool("Move", false);
        Animator.Play("Win");
    }

}
