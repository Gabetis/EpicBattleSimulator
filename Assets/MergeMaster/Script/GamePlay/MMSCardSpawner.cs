using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSCardSpawner : MonoBehaviour
{
    public static MMSCardSpawner Instance;
    public MMSCardInfoSystem MMSCardInfoSystem;
    public int Level;

    private void Awake()
    {
        Instance = this;
    }

    public MMSCard SpawnCard(string name)
    {
        if(name == null)
            return null;

        var card = Resources.Load(MMSCardInfoSystem.GetCardAddress(name)) as GameObject;

        if(card != null)
        {
            GameObject newCard = Instantiate(card, transform);
            return newCard.GetComponent<MMSCard>();
        }

        return null;
    }
}
