using System.Collections;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    public Card[] cards;
    public float DelayTime = 1.0f;
    private bool isSelected = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
 
    public void Initalize(bool AllActive = false)
    {
        animator.Play("Selector_Open");
        isSelected = false;

        var Cards = MANAGER.DB.GetRandomCardSet(AllActive);
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].Initalize(Cards[i]);
        }
    }

    public void SelectCard(int value)
    {
        if(isSelected) return;
        isSelected = true;

        for(int i = 0; i < cards.Length; i++)
        {
            if(i == value)
            {
                cards[i].SetAnimation(1.2f, 0.2f);
                MANAGER.Session.SelectedCard(cards[i].card);
            }
            else
            {
                cards[i].SetAnimation(0.0f, 0.2f);
            }
            cards[i].isSelected = true;
        }
        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSecondsRealtime(DelayTime);
        animator.Play("Selector_Close");
        Time.timeScale = 1;
    }
}
