using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSBoss : MMSWarrior
{


    public override void AttackAnimation()
    {
        Animator.SetBool("Attack", true);
        Animator.SetBool("Move", false);
    }

    public override void DeadAnimation()
    {
        Animator.Play("Die");
        this.enabled = false;
    }

    public override void MoveAnimation()
    {
        Animator.SetBool("Move", true);
    }

    public override void WinAnimation()
    {
        Animator.SetBool("Attack", false);
        Animator.SetBool("Move", false);
        Animator.Play("Win");
    }

}
