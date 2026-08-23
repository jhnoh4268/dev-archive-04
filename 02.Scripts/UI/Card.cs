using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardDB card;
    public bool isSelected = false;

    [SerializeField] TextMeshProUGUI descrption, title;
    [SerializeField] Image IconImage;
    [SerializeField] Image OutlineImage;

    [SerializeField] Color[] colors;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isSelected) return;
        SetAnimation(1.2f, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isSelected) return;
        SetAnimation(1.0f, 0.1f);
    }

    public void Initalize(CardDB cardDB)
    {
        card = cardDB; 
        transform.localScale = Vector3.one;
        isSelected = false;

        title.text = card.id;
        descrption.text = string.Format(card.description, cardDB.baseDamage);

        IconImage.sprite = MANAGER.DB.GetSprite(card.name);
        //OutlineImage.color = card.state == CardState.Active ? colors[0] : colors[1];
    }

    public void SetAnimation(float scale, float time)
    {
        if(isSelected) return;
        transform.DOScale(scale, time).SetUpdate(UpdateType.Normal, true);
    }
}
