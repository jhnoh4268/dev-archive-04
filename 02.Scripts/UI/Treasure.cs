using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : MonoBehaviour
{
    //public Image chestPrite;

    public GameObject ConfirmBtn;
    
    [SerializeField]TreasureCard[] cards;
    int valueCount = 0;
    Animator animator;
    CanvasGroup canvasGroup;

    private void Start()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Initlize(int chestValue)
    {
        CheckCanvasGroup(true);

        animator.Play("Selector_Open");
        valueCount = chestValue;

        Debug.Log(chestValue);
        Debug.Log(cards.Length);

        switch(chestValue)
        {
            case 0:
                cards[0].gameObject.SetActive(true);
                cards[0].Initlize(GetList());
                break;
            case 1:
                for(int i = 0; i < 3; i++)
                {
                    cards[i].gameObject.SetActive(true);
                    cards[i].Initlize(GetList());
                }
                
                break;
            case 2:
                for(int i = 0; i < 5; i++)
                {
                    cards[i].gameObject.SetActive(true);
                    cards[i].Initlize(GetList());
                }
            break;
        }
    }

    public List<SelectCard> GetList()
    {
        List<SelectCard> list = new();
        foreach(var selected in MANAGER.Session.selectedCard)
        {
            if(selected.Value.level < 5)
                list.Add(selected.Value);
        }

        if(list.Count == 0)
        {
            
            for(int i = 0; i < MANAGER.DB.NoneCards.Count; i++)
            {
                list.Add(new SelectCard
                {
                    db = MANAGER.DB.NoneCards[i],
                    level = 0
                });
            }

        }

        return list;
    }

    public void ConfirmCheck()
    {
        switch(valueCount)
        {
            case 0:
            if(!cards[0].isFinished)
                return;
            break;
            case 1:
            for(int i = 0; i < 3; i++)
            {
                if(!cards[i].isFinished)
                    return;
            }
            break;
            case 2:
            for(int i = 0; i < 3; i++)
            {
                if(!cards[i].isFinished)
                    return;
            }
            break;
        }
        ConfirmBtn.transform.localScale = Vector3.one;
    }

    public void confirm()
    {
        Time.timeScale = 1.0f;
        for(int i = 0; i < cards.Length; i++) cards[i].gameObject.SetActive(false);
        ConfirmBtn.transform.localScale = Vector3.zero;
        CheckCanvasGroup(false);
        animator.Play("Selector_Close");
    }

    private void CheckCanvasGroup(bool B)
    {
        canvasGroup.interactable = B;
        canvasGroup.blocksRaycasts = B;
    }
}
