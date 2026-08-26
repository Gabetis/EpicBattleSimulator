using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenuAttribute(fileName = "CardInfoSystem", menuName = "Data/Scriptable/Card Info System")]
public class MMSCardInfoSystem : ScriptableObject
{
    public List<MMSCardInfo> CardInfos;

    Hashtable CardInfoTable = new Hashtable();

    void CreateHashTable()
    {
        for (int i = 0; i < CardInfos.Count; i++)
        {
            CardInfoTable.Add(CardInfos[i].Name , CardInfos[i]);
        }
    }

    public string GetCardAddress(string name)
    {
        if(CardInfoTable.Count <= 0)
        {
            CreateHashTable();
        }

        MMSCardInfo info = (MMSCardInfo)CardInfoTable[name];
        return info.Address; 
    }

    public MMSCardInfo GetCardInfo(string name)
    {
        if (CardInfoTable.Count <= 0)
        {
            CreateHashTable();
        }
        MMSCardInfo info = (MMSCardInfo)CardInfoTable[name];
        return info;
    }

    public int GetStrongestWarrior()
    {
        List<MMSCard> cards = GetWarriorList();
        int strongest = 1;
        for (int i = 0; i < cards.Count; i++)
        {
            if (PlayerPrefs.GetInt(cards[i].Name + "IsUnlock") == 0)
            {
                break;
            }
            strongest = i+1;
        }
        return strongest;
    }

    public int GetStrongestBoss()
    {
        List<MMSCard> cards = GetWarriorList();
        int strongest = 1;
        for (int i = 0; i < cards.Count; i++)
        {
            if (PlayerPrefs.GetInt(cards[i].Name + "IsUnlock") == 0)
            {
                break;
            }
            strongest = i+1;
        }
        return strongest;
    }

    public int GetStrongestArchery()
    {
        List<MMSCard> cards = GetArcheryList();
        int strongest = 1;
        for (int i = 0; i < cards.Count; i++)
        {
            if (PlayerPrefs.GetInt(cards[i].Name + "IsUnlock") == 0)
            {
                break;
            }
            strongest = i + 1;
        }
        return strongest;
    }

    [Sirenix.OdinInspector.Button]
    public void SetWarrior(int baseDamage = 3,int baseHealth=20)
    {
        List<MMSCard> cards = GetWarriorList();
        Debug.Log(cards.Count);
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Damage = ReturnValue(baseDamage, i);
            cards[i].MaxHealth = ReturnValue(baseHealth, i);
            cards[i].Health = 0;
        }
    }



    List<MMSCard> GetWarriorList()
    {
        List<MMSCard> cards = new List<MMSCard>();

        for (int i = 0; i < CardInfos.Count; i++)
        {
            if (CardInfos[i].IsWarrior)
            {
                var cardClone = Resources.Load(CardInfos[i].Address) as GameObject;
                MMSCard card = cardClone.GetComponent<MMSCard>();
                if (card.GetType() != typeof(MMSBoss))
                {
                    cards.Add(card);
                }
            }
        }

        return cards;
    }

    [Range(1,10)]
    public float StrengthRatio;

    int ReturnValue(int value,int count)
    {
        int returnValue = value;
        if(count != 0)
        {
            count--;
            returnValue = ReturnValue((int)(returnValue * StrengthRatio),count);
        }              
        return returnValue;
    }

    [Sirenix.OdinInspector.Button]
    public void SetArchery(int baseDamage = 4, int baseHealth = 8)
    {
        List<MMSCard> cards = GetArcheryList();

        Debug.Log(cards.Count);

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Damage = ReturnValue(baseDamage, i);
            cards[i].MaxHealth = ReturnValue(baseHealth, i);
            cards[i].Health = 0;
        }
    }

    List<MMSCard> GetArcheryList()
    {
        List<MMSCard> cards = new List<MMSCard>();
        
        for (int i = 0; i < CardInfos.Count; i++)
        {
            if (!CardInfos[i].IsWarrior)
            {
                var cardClone = Resources.Load(CardInfos[i].Address) as GameObject;
                MMSCard card = cardClone.GetComponent<MMSCard>();
                cards.Add(card);
            }
        }

        return cards;
    }
}
