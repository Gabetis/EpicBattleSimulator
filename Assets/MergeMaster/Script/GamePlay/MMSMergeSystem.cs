using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenuAttribute(fileName = "MergeSystem", menuName = "Data/Scriptable/Merge System")]
public class MMSMergeSystem : ScriptableObject
{
    public List<MMSMergeInfo> MergeInfos;

    Hashtable MergeInfoTable = new Hashtable();

    void CreateHashTable()
    {
        for(int i =0;i < MergeInfos.Count; i++)
        {
            MergeInfoTable.Add(MergeInfos[i].Card1Name + "+" + MergeInfos[i].Card2Name, MergeInfos[i].ReturnCard3Name);
        }
    }
    
    public string Merge(MMSCard card1, MMSCard card2)
    {
        if (MergeInfoTable.Count <= 0)
        {
            CreateHashTable();
        }

        string merge = (string)MergeInfoTable[(card1.Name + "+" + card2.Name)];

        if(merge == null)
        {
            return null;
        }

        return merge;
    }

    public string Merge(string card1Name, string card2Name)
    {
        if (MergeInfoTable.Count <= 0)
        {
            CreateHashTable();
        }

        string merge = (string)MergeInfoTable[(card1Name + "+" + card2Name)];

        if (merge == null)
        {
            //Debug.LogError("Not found merge card:" + (card1Name + "+" + card2Name));
            return null;
        }

        return merge;
    }
}
