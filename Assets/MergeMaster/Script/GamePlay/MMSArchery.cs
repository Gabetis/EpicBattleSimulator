using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MMSArchery : MMSCard
{
    public Animator animator_person;
    public MMSBullet bullet;
    public Transform FirePos;
    protected MMSCard target;
    public GameObject ExplosionEffect;
    Coroutine AttackCoroutine;

    public MMSArchery()
    {

    }

    public override int BeAttack(MMSCard card)
    {
        return base.BeAttack(card);
    }

    public override void AttackTarget(MMSCard card)
    {
        base.AttackTarget(card);
        target = card;
    }

    public virtual void ShotBullet()
    {
        GameObject _bullet = DTNPoolingGameManager.Instance.GenerateObject(bullet.gameObject, transform);
        _bullet.transform.localScale = bullet.transform.localScale;
        _bullet.transform.position = FirePos.position;
        _bullet.transform.rotation = FirePos.rotation;
        _bullet.GetComponent<MMSBullet>().CallBack = (MMSCard card, Transform pos) =>
        {
            BulletCallBack(card, pos);
        };
        _bullet.GetComponent<MMSBullet>().AttackTarget(target, this);
    }


    protected override IEnumerator EnumAttack(MMSCard card)
    {
        while (this.Health > 0 && card.Health > 0)
        {
            yield return StartCoroutine(EnumMoveToTarget(card));
            
            AttackAnimation();

            yield return null;
        }

        if (card.Health <= 0 && this.Health > 0)
        {
            if (OnFinishAttack != null)
            {
                OnFinishAttack(this);
            }
        }
    }

    public void BulletCallBack(MMSCard card, Transform pos)
    {
        if (card.Health > 0)
        {
            int coin = card.BeAttack(this);

            if (OnEarnCoin != null)
                OnEarnCoin(this, coin);

            // Effect coin fly
            CoinFlyEffect(card, coin);
            // Effect nổ
            // ExplosionPowEffect(pos);

            if (card.OnGetBoard().Name == "User" && MMSGameController.Instance.isVibrate)
                Vibration.VibratePop();
        }
    }

    private void ExplosionPowEffect(Transform pos)
    {
        if (ExplosionEffect == null) return;

        GameObject _explosion = Instantiate(ExplosionEffect, pos.transform.position, transform.rotation);
        _explosion.transform.localScale = Vector3.one * 0.5f;
        Destroy(_explosion, 1f);
    }

    protected override IEnumerator EnumMoveToTarget(MMSCard card)
    {
        yield return StartCoroutine(EnumLookToTarger(card));
    }

    IEnumerator EnumLookToTarger(MMSCard card)
    {
        float angle = Vector3.Angle((card.transform.position - transform.position).normalized, transform.GetChild(0).forward);
        if(angle >= 5f)
        {
            int index = 0;
            while (index < 10)
            {
                yield return null;
                transform.GetChild(0).forward = Vector3.Lerp(transform.GetChild(0).forward, (card.transform.position - transform.position).normalized, MoveSpeed * Time.deltaTime * 5f);
                index++;
            }
            transform.GetChild(0).forward = (card.transform.position - transform.position).normalized;
        }
        else
        {
            yield return null;
        }
    }

    public override void Dead()
    {
        base.Dead();
    }

    public override void AttackAnimation()
    {
        if (this.Health <= 0)
            return;
        Animator.SetBool("Attack", true);
        Animator.SetBool("Move", false);
        if (animator_person != null)
        {
            animator_person.SetBool("Attack", true);
        }
    }

    public override void DeadAnimation()
    {
        StopAllCoroutines();
        Animator.SetBool("Attack", false);
        Animator.SetBool("Move", false);
        Animator.Play("Die");
        if (animator_person != null)
        {
            animator_person.Play("Die"); ;
        }
        this.enabled = false;
    }

    public override void MoveAnimation()
    {
        Animator.SetBool("Move", true);
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
        if (animator_person != null)
        {
            animator_person.SetBool("Attack", false);
            animator_person.SetBool("Move", false);
            animator_person.Play("Win");
        }
    }


}
