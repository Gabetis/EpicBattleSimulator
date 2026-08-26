using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardInfo
{
    [SerializeField] public List<CardLevel> CardsLevelCom;
}
[System.Serializable]
public class CardLevel
{
    public CardLevel(string name, int x, int y)
    {
        this.Cards = name;
        this.column = x;
        this.row = y;
    }
    [SerializeField] public int row;
    [SerializeField] public int column;
    [SerializeField] public string Cards;
}

[System.Serializable]
[CreateAssetMenuAttribute(fileName = "LevelManager", menuName = "Data/Scriptable/Level Manager")]
public class MMSLevelManager : ScriptableObject
{
    public MMSCardInfoSystem MMSCardInfoSystem;
    public MMSMergeSystem MMSMergeSystem;
    public BoardInfo[] LevelInfos;

    public BoardInfo BaseBoard; 

    [Sirenix.OdinInspector.Button]
    public void GenerateLevels(int numberOfLevel = 1000,float difficultyValue = 1)
    {
        LevelInfos = new BoardInfo[numberOfLevel];
        LevelInfos[0] = BaseBoard;


        int oldDamge = 0;
        for (int i = 1; i < LevelInfos.Length; i++)
        {
            //float x = i - 1;
            float step = 0;// (5 + Mathf.Min((0.02f * i), 15));
            if (i <= 10)
            {
                step = 10;
            }
            else if (i % 10 == 0)
            {
                step = 15;
            }
            else
            {
                step = 10;
            }

            //float step = 3;
            //if (i == 1)
            //{
            //    step = 5;
            //}

            //LevelInfos[i] = SetLevelInfo(i,difficultyValue);
            LevelInfos[i] = SetLevelInfo(LevelInfos[i-1], Mathf.RoundToInt(step*0.5f), Mathf.RoundToInt(step * 0.5f));


            if (i % 10 == 0)
            {
                //Debug.Log("TOTAL Damge: level " + (int)(i / 10) + " ___ " + totalDamage(LevelInfos[i]));
                int n_oldDamge = totalDamage(LevelInfos[i]);
                Debug.Log("total health" + totalHealth(LevelInfos[i])/2f);
                Debug.Log("totalDamage: level " + (int)(i) + " ___ " + n_oldDamge/2f + "____" + (n_oldDamge- totalDamage(LevelInfos[i-1])));
                oldDamge = n_oldDamge;
            }

        }


        //sinh boss
        //for (int i = 1; i <= 5; i++)
        //{
        //    LevelInfos[i * 10].CardsLevelCom = new List<CardLevel>();

        //    LevelInfos[i * 10].CardsLevelCom.Add(new CardLevel("Boss_" + i, 0, 1));

        //    LevelInfos[i * 10].CardsLevelCom.Add(new CardLevel("Boss_" + i, 0, 3));


        //    //AddCard("Boss_"+i, LevelInfos[i * 10]);
        //    //AddCard("Boss_" + i, LevelInfos[i * 10]);
        //}
    }


    BoardInfo SetLevelInfo(BoardInfo baseBoard, int stepArchive, int stepWarrior)
    {
        BoardInfo board = new BoardInfo();
        board.CardsLevelCom = new List<CardLevel>();
        foreach (CardLevel cardLevel in baseBoard.CardsLevelCom)
        {
            CardLevel c = new CardLevel(cardLevel.Cards, cardLevel.column, cardLevel.row);
            board.CardsLevelCom.Add(c);
        }

        board.CardsLevelCom = MergeBoard(board.CardsLevelCom);

        for (int i = 0; i < stepArchive; i++)
        {
            if (!AddCard("Archery_1", board))
            {
                for (int cardi = 1; cardi < 11; cardi++)
                {
                    if (AutoLevelUp("Archery_" + i, "Archery_" + (i+1), board))
                    {
                        break;
                    }
                }
            }
            board.CardsLevelCom = MergeBoard(board.CardsLevelCom);
        }

        for (int i = 0; i < stepWarrior; i++)
        {
            if (!AddCard("Warrior_1", board))
            {
                for (int cardi = 1; cardi < 13; cardi ++)
                {
                    if (AutoLevelUp("Warrior_" + i, "Warrior_" + (i + 1), board))
                    {
                        break;
                    }
                }
            }
            board.CardsLevelCom = MergeBoard(board.CardsLevelCom);
        }

        board.CardsLevelCom = MergeBoard(board.CardsLevelCom);
        return board;
    }


    List<CardLevel> MergeBoard(List<CardLevel> listCard)
    {

        List<CardLevel> tempListCard = new List<CardLevel>();
        foreach (CardLevel card in listCard)
        {
            tempListCard.Add(new CardLevel(card.Cards, card.column, card.row));
        }

        bool hasMerge = false;
        for (int index = 0; index < tempListCard.Count; index++)
        {
            CardLevel currentCard = tempListCard[index];
            for (int j = index+1; j < tempListCard.Count; j++)
            {
                CardLevel nextCard = tempListCard[j];
                if (nextCard.Cards.Equals(currentCard.Cards) && MMSMergeSystem.Merge(currentCard.Cards, currentCard.Cards)!=null)
                {
                    tempListCard.Remove(nextCard);
                    currentCard.Cards = MMSMergeSystem.Merge(currentCard.Cards, currentCard.Cards);
                    hasMerge = true;
                    break;
                }
            }
        }

        if (hasMerge)
        {
            return MergeBoard(tempListCard);
        }
        return tempListCard;
    }

    bool AutoLevelUp(string cardName, string cardNameUp, BoardInfo board)
    {

        foreach (CardLevel card in board.CardsLevelCom)
        {
            if (card.Cards.Equals(cardName))
            {
                card.Cards = cardNameUp;
                return true;
            }
        }
        return false;
    }


    bool AddCard(string cardName, BoardInfo board)
    {

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j< 5; j++)
            {
                bool hasFound = false;
                foreach (CardLevel card in board.CardsLevelCom)
                {
                    if (card.column == i && card.row == j)
                    {
                        hasFound = true;
                        break;
                    }
                }

                if (!hasFound)
                {
                    board.CardsLevelCom.Add(new CardLevel(cardName, i, j));
                    return true;
                }
            }
        }
        return false;
    }



    

    BoardInfo SetLevelInfo(int level, float difficultyValue)
    {

        BoardInfo boardInfo = new BoardInfo();
        boardInfo.CardsLevelCom = new List<CardLevel>();
        string[,] matrix = new string[3, 5];

        int boardDamage = 8;

        for (int i = 0; i < level; i++)
        {
            boardDamage += (int)(Random.Range(4, 7) * Mathf.Clamp((i / 5)*difficultyValue, 2f, 10000f));
        }

        int archeryDamage = 0;

        switch (Random.Range(0, 4))
        {
            case 0:
                archeryDamage = (int)(boardDamage * 0.3);
                break;
            case 1:
                archeryDamage = (int)(boardDamage * 0.4) ;
                break;
            case 2:
                archeryDamage = (int)(boardDamage * 0.5) ;
                break;
            case 3:
                archeryDamage = (int)(boardDamage * 0.35);
                break;
            case 4:
                archeryDamage = (int)(boardDamage * 0.6);
                break;
        }

        SetArcheryOnBoard(archeryDamage, matrix);

        int warriorDamage = (int)(boardDamage - archeryDamage) ;

        SetWarriorOnBoard(warriorDamage, matrix);

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if (matrix[x, y] != null)
                {
                    CardLevel cardLevel = new CardLevel(matrix[x, y], x, y);
                    boardInfo.CardsLevelCom.Add(cardLevel);
                }
            }
        }

        return boardInfo;
    }

    void SetArcheryOnBoard(int damage, string[,] matrix)
    {
        int[] cardDamages = new int[5];
        string[] cardName = new string[5];

        while (damage > 3)
        {
            for (int i = 0; i < cardName.Length; i++)
            {
                if (cardName[i] == null)
                {
                    cardName[i] = "Archery_1";
                    cardDamages[i] = cardDamage(cardName[i]);
                    damage -= cardDamages[i];
                    if(damage < 3)
                    {
                        break;
                    }
                }
                else
                {
                    if(MMSMergeSystem.Merge(cardName[i], cardName[i]) != null)
                    {
                        string store = cardName[i];
                        cardName[i] = MMSMergeSystem.Merge(cardName[i], cardName[i]);
                        if (damage < cardDamage(cardName[i]))
                        {
                            damage -= cardDamages[i];
                            cardName[i] = store;
                            break;
                        }
                        cardDamages[i] = cardDamage(cardName[i]);
                    }
                    
                    damage -= cardDamages[i];
                    if (damage <= 3)
                    {
                        break;
                    }
                }
            }
        }

        int merge = Merge(cardName, Random.Range(3, 7));

        for (int i =0;i< cardName.Length; i++)
        {
            if(cardName[i] != null)
            {
                SetRow(cardName[i], 0, matrix);
            }
        }
    }

    void SetWarriorOnBoard(int damage, string[,] matrix)
    {
        int[] cardDamages = new int[10];
        string[] cardName = new string[10];

        while (damage >= 5)
        {
            for (int i = 0; i < cardName.Length; i++)
            {
                if (cardName[i] == null)
                {
                    cardName[i] = "Warrior_1";
                    cardDamages[i] = cardDamage(cardName[i]);

                    damage -= cardDamages[i];
                    if (damage < 5)
                    {
                        break;
                    }
                }
                else
                {
                    if (MMSMergeSystem.Merge(cardName[i], cardName[i]) != null)
                    {
                        string store = cardName[i];
                        cardName[i] = MMSMergeSystem.Merge(cardName[i], cardName[i]);
                        if (damage < cardDamage(cardName[i]))
                        {
                            damage -= cardDamages[i];
                            cardName[i] = store;
                            break;
                        }
                        cardDamages[i] = cardDamage(cardName[i]);
                    }

                    damage -= cardDamages[i];
                    
                }
            }
        }

        int merge = Merge(cardName,Random.Range(3,7));

        int countPos = 0;

        for (int i = 0; i < cardName.Length; i++)
        {
            if (cardName[i] != null)
            {
                countPos++;
                if(countPos > 5)
                {
                    SetRow(cardName[i], 1, matrix);
                }
                else
                {
                    SetRow(cardName[i], 2, matrix);
                }
                
            }
        }
    }

    int Merge(string[] cards,int mergeCount)
    {
        int count = 0;

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                for (int j = 0; j < cards.Length; j++)
                {
                    if (cards[j] != null)
                    {
                        if (i != j && MMSMergeSystem.Merge(cards[i], cards[j]) != null)
                        {
                            count++;
                            cards[i] = MMSMergeSystem.Merge(cards[i], cards[j]);
                            cards[j] = null;
                        }
                    }
                }
            }
        }

        if (count != 0 && count > mergeCount)
            count = Merge(cards,mergeCount);

        return count;
    }

    int cardDamage(string name)
    {
        var cardClone = Resources.Load(MMSCardInfoSystem.GetCardAddress(name)) as GameObject;
        MMSCard card = cardClone.GetComponent<MMSCard>();
        return (int)card.Damage;
    }

    int totalHealth(BoardInfo beforeUpdateBoardInfo)
    {
        int value = 0;
        for(int i =0; i< beforeUpdateBoardInfo.CardsLevelCom.Count; i++)
        {
            var cardClone = Resources.Load(MMSCardInfoSystem.GetCardAddress( beforeUpdateBoardInfo.CardsLevelCom[i].Cards)) as GameObject;
            MMSCard card = cardClone.GetComponent<MMSCard>();
            value += (int)card.MaxHealth;
        }
        return value;
    }

    int totalDamage(BoardInfo beforeUpdateBoardInfo)
    {
        int value = 0;
        for (int i = 0; i < beforeUpdateBoardInfo.CardsLevelCom.Count; i++)
        {
            var cardClone = Resources.Load(MMSCardInfoSystem.GetCardAddress(beforeUpdateBoardInfo.CardsLevelCom[i].Cards)) as GameObject;
            MMSCard card = cardClone.GetComponent<MMSCard>();
            value += (int)card.Damage;
        }
        Debug.Log(value);
        return value;
    }

    void SetRow(string cardName, int column, string[,] matrix)
    {
        matrix[column, CheckRow(column, matrix)] = cardName;
    }

    int CheckRow(int column, string[,] matrix)
    {
        int row = 2;
        if (matrix[column, row] == null)
        {
            return row;
        }
        row = 3;
        if (matrix[column, row] == null)
        {
            return row;
        }
        row = 1;
        if (matrix[column, row] == null)
        {
            return row;
        }
        row = 4;
        if (matrix[column, row] == null)
        {
            return row;
        }
        row = 0;
        if (matrix[column, row] == null)
        {
            return row;
        }
        return row;
    }
}
/*  int archerBuyCoin = 10;
        int warriorBuyCoin = 10;

        while (comCoin > 0)
        {
            if (comCoin >= archerBuyCoin)
            {
                comCoin -= archerBuyCoin;

                boardHeatlth += 8;
                boardDamage += 4;

                archerBuyCoin = (int)(archerBuyCoin * 1.5);
            }
            else if (comCoin >= warriorBuyCoin)
            {
                comCoin -= warriorBuyCoin;

                boardHeatlth += 20;
                boardDamage += 3;

                warriorBuyCoin = (int)(warriorBuyCoin * 1.5);
            }
            else
            {
                break;
            }
        } */