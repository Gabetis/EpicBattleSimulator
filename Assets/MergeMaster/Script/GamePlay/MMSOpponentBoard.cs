using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSOpponentBoard : MMSBoard
{
    public Color colorSliderHealth;
    public override void AddCard(MMSCard card, int x, int y)
    {
        base.AddCard(card, x, y);
        if (card != null)
        {
            if (card.CardColor != null)
                card.CardColor.SetTexture(false);
            card.CreateSliderHealth(colorSliderHealth);
        }
    }
}
