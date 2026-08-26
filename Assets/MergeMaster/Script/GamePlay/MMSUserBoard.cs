using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MMSUserBoard : MMSBoard
{

    public Color colorSliderHealth;
    //  public GameObject MergeEffect;
    public GameObject SmokeEffect;

    public MMSDragAndDrop DragAndDrop;

    public override void OnBeginGame()
    {
        DragAndDrop.gameObject.SetActive(true);
        Coin = long.Parse(PlayerPrefs.GetString("UserCurrentCoin"));
    }

    public override void ResetBoard()
    {
        DragAndDrop.gameObject.SetActive(false);
        base.ResetBoard();
    }

    public override void Attack(MMSBoard board)
    {
        DragAndDrop.gameObject.SetActive(false);
        base.Attack(board);
    }

    public override void BeginDrag(MMSCard card)
    {
        base.BeginDrag(card);
        card.OnWillUpgrade(card);
        DragAndDrop.TurnOnSuggestLine(GetAllCardsTransformSameName(card.Name));
    }

    public override void AddCard(MMSCard card, int x, int y)
    {
        base.AddCard(card, x, y);
        if (card != null)
        {
            if (card.CardColor != null)
                card.CardColor.SetTexture(true);
            card.CreateSliderHealth(colorSliderHealth);
            card.Animator.Play("Jumping Down");
        }
    }

    MMSCard targetCard;
    public override void Drag(MMSCard card)
    {
        base.Drag(card);
        // Check Positio

        if (targetCard != null)
        {
            targetCard.OnWillUpgrade(null);
            targetCard = null;
        }

        Vector2 pos = CheckCardPosition(card);
        int x = (int)pos.x, y = (int)pos.y;
        if ((Cards[x, y] == null))
        {
            DragAndDrop.SetChooseLine(true, x, y);
        }
        else if ((Cards[x, y] != null && MergeCheck(card, Cards[x, y])))
        {
            DragAndDrop.SetChooseLine(true, x, y);
            targetCard = Cards[x, y];
            targetCard.OnWillUpgrade(card);
        }
        else
        {
            DragAndDrop.SetChooseLine(false, x, y);
        }

    }

    public override void Drop(MMSCard card)
    {
        if (targetCard != null)
        {
            targetCard.OnWillUpgrade(null);
            targetCard = null;
        }
        if (card == null)
            return;

        DragAndDrop.TurnOffSuggestLine();
        DragAndDrop.SetChooseLine(false, 0, 0);
        Vector2 pos = CheckCardPosition(card);
        int x = (int)pos.x, y = (int)pos.y;
        if ((Cards[x, y] == null))
        {
            card.OnWillUpgrade(null);
            AddCard(card, x, y);
            GameObject _SmokeEffect = Instantiate(SmokeEffect, new Vector3(card.transform.position.x, 0.05f, card.transform.position.z), SmokeEffect.transform.rotation);
            Destroy(_SmokeEffect, 1.5f);
        }
        else if ((Cards[x, y] != null && MergeCheck(card, Cards[x, y])))
        {
            MMSCard newCard = MMSCardSpawner.Instance.SpawnCard(Merge(card, Cards[x, y]));

            OnUnlockNewCard(Merge(card, Cards[x, y]));

            AddCard(newCard, x, y);
            StartCoroutine(EnumMergeEffect(newCard));
            Destroy(card.gameObject);

            //  GameObject _MergeEffect = Instantiate(MergeEffect, newCard.transform.position, transform.rotation);

            //  Destroy(_MergeEffect, 1f);
        }
        else
        {
            card.OnWillUpgrade(null);
            AddCard(card, card.x, card.y);
            GameObject _SmokeEffect = Instantiate(SmokeEffect, new Vector3(card.transform.position.x, 0.05f, card.transform.position.z), SmokeEffect.transform.rotation);
            Destroy(_SmokeEffect, 1.5f);
        }


    }

    public override void AddCard(string cardName)
    {
        if (CheckCartCount() >= 15)
        {
            if (PlayerPrefs.GetInt("DeleteTut", 0) < 3)
            {
                DTNViewManagement.GetView<MMSDeleteTutScene>().Show();
            }
            return;
        }

        MMSCard card = MMSCardSpawner.Instance.SpawnCard(cardName);
        for (int x = 0; x < m; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (Cards[x, y] == null)
                {
                    AddCard(card, x, y);
                    return;
                }
            }
        }
    }

    IEnumerator EnumMergeEffect(MMSCard card)
    {
        card.gameObject.transform.DOScale(Vector3.one * 1.25f, 0.35f);
        yield return new WaitForSeconds(0.35f);
        card.gameObject.transform.DOScale(Vector3.one, 0.35f);
    }
}
