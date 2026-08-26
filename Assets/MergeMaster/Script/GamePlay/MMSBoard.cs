using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MMSBoard : MonoBehaviour
{
    public string Name;
    public int m = 3, n = 5;
    public float RangeX = 4f;
    public float RangeY = 4f;
    public MMSMergeSystem MMSMergeSystem;
    public MMSCard[,] Cards = new MMSCard[3, 5];

    public long Coin;
    public long RoundCoin;
    public Action OnFinishAttack;
    public Action<long> OnSetCoin;
    public Action<string> OnUnlockNewCard;

    public int SpawnLevelWarrior;
    public int SpawnLevelArchery;
    public float MaxHealth = 0;
    // public List<MMSCard> CardsList;
    public List<string> RewardCards = new List<string>();

    public virtual void OnBeginGame()
    {

    }

    List<MMSCard> CardsList;
    public List<MMSCard> GetAllCards()
    {
        CardsList = new List<MMSCard>();

        for (int x = 0; x < m; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (Cards[x, y] != null && Cards[x, y].enabled == true)
                {
                    CardsList.Add(Cards[x, y]);
                }
            }
        }

        return CardsList;
    }

    public List<Transform> GetAllCardsTransformSameName(string CardName)
    {
        List<Transform> _cardsList = new List<Transform>();

        for (int x = 0; x < m; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (Cards[x, y] != null && Cards[x, y].enabled == true && Cards[x, y].Name == CardName)
                {
                    _cardsList.Add(Cards[x, y].transform);
                }
            }
        }

        return _cardsList;
    }
    List<CardLevelUser> CardsListUser;
    public List<CardLevelUser> GetUserSaveCardLevel()
    {
         CardsListUser = new List<CardLevelUser>();

        for (int x = 0; x < m; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (Cards[x, y] != null && Cards[x, y].enabled == true)
                {
                    CardLevelUser cardLevelUser = new CardLevelUser();
                    cardLevelUser.row = x;
                    cardLevelUser.column = y;
                    cardLevelUser.CardsName = Cards[x, y].name;
                    cardLevelUser.CardsName = cardLevelUser.CardsName.Replace("(Clone)", "");
                    CardsListUser.Add(cardLevelUser);
                }
            }
        }

        return CardsListUser;
    }
    List<MMSCard> cards;
    public float GetHealth()
    {
        cards = GetAllCards();
        float totalHealth = 0;

        for (int i = 0; i < cards.Count; i++)
        {
            totalHealth += cards[i].Health;
        }

        return totalHealth;
    }

    public void GetMaxHealth()
    {
        MaxHealth = GetHealth();
    }

    public void ApplyPosition(MMSCard card, int x, int y)
    {
        card.transform.localPosition = new Vector3((y - (float)(n - 1) / 2f) * RangeY, 0, (x - (float)(m - 1) / 2f) * RangeX);
        card.transform.forward = transform.forward;
        // This
    }

    public void ApplyPosition(Transform obj, int x, int y)
    {
        obj.transform.localPosition = new Vector3((y - (float)(n - 1) / 2f) * RangeY, 0, (x - (float)(m - 1) / 2f) * RangeX);
    }

    public virtual void LoadBoard(string[,] cardNames, MMSBoard board)
    {
        RoundCoin = 0;
        LoadMatrix(cardNames, board);
        GetMaxHealth();
        LoadRewardCards();
    }

    public void LoadRewardCards()
    {
        for (int i = 0; i < RewardCards.Count; i++)
        {
            AddCard(RewardCards[i]);
        }
        RewardCards = new List<string>();
    }

    public void AddRewardCard(string cardName)
    {
        RewardCards.Add(cardName);
    }

    public virtual void ResetBoard()
    {
        m = Cards.GetLength(0);
        n = Cards.GetLength(1);

        for (int x = 0; x < m; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (Cards[x, y] != null)
                {
                    Destroy(Cards[x, y].gameObject);
                }

                Cards[x, y] = null;
            }
        }

    }

    public virtual void LoadMatrix(string[,] cardNames, MMSBoard board)
    {
        m = cardNames.GetLength(0);
        n = cardNames.GetLength(1);

        ResetBoard();

        for (int x = 0; x < m; x++)
        {
            for (int y = 0; y < n; y++)
            {
                MMSCard card = MMSCardSpawner.Instance.SpawnCard(cardNames[x, y]);

                if (card != null)
                {
                    AddCard(card, x, y);
                }
            }
        }
    }

    public virtual void Attack(MMSBoard board)
    {
        List<MMSCard> cardsUser = GetAllCards();

        for (int i = 0; i < cardsUser.Count; i++)
        {
            cardsUser[i].OnFinishAttack = (MMSCard ca) =>
            {
                MMSCard target = FindTarget(ca, board);
                if (target != null)
                {
                    ca.AttackTarget(target);
                }
                else
                {
                    ca.WinAnimation();
                    if (OnFinishAttack != null)
                        OnFinishAttack();
                }

            };
            cardsUser[i].AttackTarget(FindTarget(cardsUser[i], board));
        }
    }

    MMSCard FindTarget(MMSCard card, MMSBoard board)
    {
        List<MMSCard> cardsOpponent = board.GetAllCards();
        MMSCard target = null;
        float min = 798798798f;
        for (int i = 0; i < cardsOpponent.Count; i++)
        {
            if ((card.gameObject.transform.position - cardsOpponent[i].transform.position).magnitude < min)
            {
                float abc = (card.gameObject.transform.position - cardsOpponent[i].transform.position).magnitude;
                if (abc <= min)
                {
                    target = cardsOpponent[i];
                    min = abc;
                }
            }
        }
        return target;
    }

    public virtual void BeginDrag(MMSCard card)
    {
        Cards[card.x, card.y] = null;
    }

    public virtual void Drag(MMSCard card)
    {
        // Check Position

    }

    public virtual void Drop(MMSCard card)
    {
        AddCard(card, card.x, card.y);
        // Check Position
    }


    public bool MergeCheck(MMSCard card1, MMSCard card2)
    {
        if (MMSMergeSystem.Merge(card1, card2) != null)
        {
            return true;
        }

        return false;
    }

    public string Merge(MMSCard card1, MMSCard card2)
    {
        // Debug.LogError("Merge!!");
        DTNSoundManagement.instance.Play("mergeSound"+UnityEngine.Random.Range(0,2));
        return MMSMergeSystem.Merge(card1, card2);
    }

    public virtual Vector2 CheckCardPosition(MMSCard card)
    {
        float xRange = ((float)(n - 1) / 2f) * RangeX;
        float yRange = ((float)(m - 1) / 2f) * RangeY;
        Vector3 cardLocalPos = new Vector3(Mathf.Clamp(card.transform.localPosition.x, -xRange, xRange), 0, Mathf.Clamp(card.transform.localPosition.z, -yRange, yRange));

        Vector2 returnValue = Vector2.zero;

        float minRange = 78885848548f;
        for (int x = 0; x < m; x++)
        {
            for (int y = 0; y < n; y++)
            {
                Vector3 temp = new Vector3((y - (float)(n - 1) / 2f) * RangeY, 0, (x - (float)(m - 1) / 2f) * RangeX);
                if ((temp - cardLocalPos).magnitude < minRange)
                {
                    minRange = (temp - cardLocalPos).magnitude;
                    returnValue = new Vector2(x, y);
                }
            }
        }


        return returnValue;
    }

    public virtual void AddCard(MMSCard card, int x, int y)
    {
        if (Cards[x, y] != null)
            Destroy(Cards[x, y].gameObject);

        Cards[x, y] = card;
        card.x = x;
        card.y = y;

        if (Cards[x, y] != null)
        {
            card.OnEarnCoin = (MMSCard ca, long _coin) =>
            {
                RoundCoin += _coin;
                Coin += 1;
                if (OnSetCoin != null)
                    OnSetCoin(Coin);
            };

            card.OnAttack = (MMSCard ca) =>
            {

            };

            card.OnDead = (MMSCard ca) =>
            {
                if (GetHealth() <= 0)
                {
                    if (OnFinishAttack != null)
                        OnFinishAttack();
                }
            };

            card.OnWillAttack = (MMSCard ca) =>
            {

            };

            card.OnFinishAttack = (MMSCard ca) =>
            {

            };

            card.OnGetBoard = new MMSCard.MyDelegate(ReturnBoard);

            Cards[x, y].Health = Cards[x, y].MaxHealth;
            Cards[x, y].transform.parent = this.transform;
            Cards[x, y].transform.localScale = Vector3.one;
            ApplyPosition(card, x, y);

        }
    }

    public MMSBoard ReturnBoard()
    {
        return this;
    }

    public virtual void AddCard(MMSCard card)
    {
        for (int x = 0; x < m; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (Cards[x, y] != null)
                {
                    AddCard(card, x, y);
                    return;
                }
            }
        }
    }

    public virtual void AddCard(string cardName)
    {
        if (CheckCartCount() >= 15)
            return;
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
    public int CheckCartCount()
    {
        int count = 0;
        for (int x = 0; x < m; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (Cards[x, y] != null)
                {
                    count++;
                }
            }
        }
        return count;
    }
}
