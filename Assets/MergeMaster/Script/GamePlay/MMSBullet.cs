using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MMSBullet : MonoBehaviour
{
    public Action<MMSCard, Transform> CallBack;
    MMSCard OpponentCard;
    Transform target;

    public void AttackTarget(MMSCard opponentCard, MMSArchery card)
    {
        this.OpponentCard = opponentCard;
        if (opponentCard != null)
            this.target = opponentCard.transform;
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 temp = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.position = Vector3.MoveTowards(transform.position, temp, 18f * Time.deltaTime);
        transform.forward = temp - transform.position;

        if (Vector3.Distance(transform.position, temp) < 0.4f)
        {
            target = null;

            if (CallBack != null)
                CallBack(OpponentCard, transform);

            DTNPoolingGameManager.Instance.DestroyObject(this.gameObject);
        }
    }
}
