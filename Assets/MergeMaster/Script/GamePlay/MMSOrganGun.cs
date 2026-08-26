using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSOrganGun : MMSArchery
{
    public Transform[] FireTrans;

    public override void ShotBullet()
    {
        for(int i = 0;i < FireTrans.Length; i++)
        {
            GameObject _bullet = DTNPoolingGameManager.Instance.GenerateObject(bullet.gameObject, transform);
            _bullet.transform.localScale = bullet.transform.localScale;
            _bullet.transform.position = FireTrans[i].position;
            _bullet.transform.rotation = FireTrans[i].rotation;
            _bullet.GetComponent<MMSBullet>().CallBack = (MMSCard card, Transform pos) =>
            {
                BulletCallBack(card, pos);
            };
            _bullet.GetComponent<MMSBullet>().AttackTarget(target, this);
        }
    }
}
