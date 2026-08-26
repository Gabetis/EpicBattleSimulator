using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MMSCard : MonoBehaviour
{
    public string Name;
    public string FullName;
    public int x, y;

    public int HitCount = 1;
    public int HitDamage = 0;
    public float Damage;
    public float MaxHealth;
    public float Health;
    public float MoveSpeed;
    public float AttackSpeed;
    public Animator Animator;
    public MMSCardColor CardColor;
    public Action<MMSCard> OnDead;
    public Action<MMSCard> OnAttack;
    public Action<MMSCard> OnFinishAttack;
    public Action<MMSCard> OnWillAttack;
    public Action<MMSCard> OnWillUpgrade;
    public Action<MMSCard, long> OnEarnCoin;
    public delegate MMSBoard MyDelegate();
    public MyDelegate OnGetBoard;
    public SliderHealthParent sliderHealthParent;
    private GameObject sliderHealth;
    public GameObject coinFloatingText;
    public GameObject floodFX;
    public AudioSource audioSource;
    public AudioClip soundDead;
    public Transform headTrans;
    public Outline Outline;
    private void Start()
    {
        HitDamage = (int)(Damage / HitCount);

        SetDelegates();
    }

    void SetDelegates() 
    {
        OnWillUpgrade = (MMSCard ca) =>
        {
            if (ca != null)
            {
                if (ca.Name == Name)
                    Outline.enabled = true;
            }
            else
            {
                Outline.enabled = false;
            }
        };
    }

    public bool Upgrade(MMSCard card)
    {
        return false;
    }

    public virtual void AttackTarget(MMSCard card)
    {
        if (OnFinishAttack != null && card == null)
        {
            OnFinishAttack(this);
            return;
        }

        if (this.Health > 0)
        {
            StopAllCoroutines();
            StartCoroutine(EnumAttack(card));
        }   
    }

    protected virtual IEnumerator EnumAttack(MMSCard card)
    {
        while (this.Health > 0 && card.Health > 0)
        {
            yield return StartCoroutine(EnumMoveToTarget(card));

            AttackAnimation();

            yield return new WaitForSeconds(AttackSpeed);

            if (card.gameObject.activeSelf == false && this.Health > 0)
            {
                if (OnFinishAttack != null)
                {
                    OnFinishAttack(this);
                    yield break;
                }
            }

            int coin = card.BeAttack(this);
            if (coin != 0)
            {
                if (OnEarnCoin != null)
                    OnEarnCoin(this, coin);
                CoinFlyEffect(card, coin);
                if (card.OnGetBoard().Name == "User" && MMSGameController.Instance.isVibrate)
                    Vibration.VibratePop();
            }
        }

        if (card.Health <= 0 && this.Health > 0)
        {
            if (OnFinishAttack != null)
            {
                OnFinishAttack(this);
            }
        }
    }


    public void CoinFlyEffect(MMSCard card, int coin)
    {
        if (card.OnGetBoard().Name == "User")
            return;

        GameObject _coinFloatingText = Instantiate(coinFloatingText, card.transform.position, coinFloatingText.transform.rotation);
        _coinFloatingText.GetComponent<CoinFloatingTextParent>().UpdateText(coin);
        Destroy(_coinFloatingText, 1f);
    }

    protected virtual IEnumerator EnumMoveToTarget(MMSCard card)
    {
        MoveAnimation();
        yield return new WaitForSeconds(2f);
    }

    public virtual int BeAttack(MMSCard card)
    {
        if (Health <= 0)
            return 0;
        
        Health -= card.HitDamage;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        // Effect
        EffectBlood();

        sliderHealthParent.SetValue(Health);

        CardColor.SetFlash();

        if (Health == 0)
        {
            Dead();
        }

        return (int)card.HitDamage / 3;
    }

    public void EffectBlood()
    {
        GameObject blood = Instantiate(floodFX, transform.position, Quaternion.identity);
        Destroy(blood, 0.25f);
    }

    public virtual void Dead()
    {
        Destroy(sliderHealth);
        if (OnDead != null)
            OnDead(this);
        PlaySoundDead();
        DeadAnimation();
        this.enabled = false;
    }

    private void PlaySoundDead()
    {
        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
        audioSource.clip = soundDead;
        if (audioSource.isPlaying)
            audioSource.Stop();
        audioSource.Play();
    }
    public abstract void MoveAnimation();


    public abstract void DeadAnimation();


    public abstract void AttackAnimation();

    public abstract void WinAnimation();

    public virtual void ResetAnimation()
    {
        Animator.Rebind();
        Animator.Update(0f);
    }

    bool isCreateSlider = false;
    public void CreateSliderHealth(Color color)
    {
        if (!isCreateSlider)
        {
            isCreateSlider = true;
            sliderHealth = Instantiate(sliderHealthParent.gameObject, headTrans.position, sliderHealthParent.gameObject.transform.rotation);
            sliderHealth.gameObject.transform.parent = this.gameObject.transform;
            sliderHealthParent = sliderHealth.GetComponent<SliderHealthParent>();
            sliderHealthParent.SetMaxValue(MaxHealth);
            sliderHealthParent.SetValue(Health);
            sliderHealthParent.SetFillImage(color);
        }
    }


}
