using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSNewCharacterScene : DTNView
{
    public MMSCardInfoSystem MMSCardInfoSystem;
    public string CardName;
    [SerializeField] Transform newCharacter;
    public override void InitView()
    {
    }
    public override void Show()
    {
        SetNewCardCharacter();
        base.Show();
        StartCoroutine(ShowNewCard());
    }
    public override void Hide()
    {
        base.Hide();
    }
    IEnumerator ShowNewCard()
    {
        yield return new WaitForSeconds(3f);
        DTNViewManagement.GetView<MMSNewCard>().Show();
        Hide();
    }
    private void SetNewCardCharacter()
    {
        foreach (var item in newCharacter.GetComponentsInChildren<MMSCard>())
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        if (CardName == null)
            return;

        var cardGameObject = Resources.Load(MMSCardInfoSystem.GetCardAddress(CardName)) as GameObject;

        if (cardGameObject == null)
            return;

        GameObject newCard = cardGameObject;
        GameObject character = Instantiate(newCard, newCharacter);
        character.layer = LayerMask.NameToLayer("NewCharacter");
        foreach (var item in character.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            item.gameObject.layer = LayerMask.NameToLayer("NewCharacter");
        }
        character.transform.localScale = new Vector3(3f, 3f, 3f);
        character.transform.rotation = Quaternion.Euler(0, 180, 0);
        this.GetComponentInChildren<Camera>().enabled = true;
    }
}
