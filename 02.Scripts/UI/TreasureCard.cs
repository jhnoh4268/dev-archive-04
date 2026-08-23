using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureCard : MonoBehaviour
{
    public RectTransform rolletParent;
    public GameObject cardPrefab;
    public float cardHeight = 240.0f;
    private float rollDuration;
    private int loopCount;

    private List<RectTransform> cards = new();
    private Animator animator;

    public bool isFinished = false;

    [SerializeField] Treasure treasure;
    Dictionary<int, SelectCard> ActiveCards = new Dictionary<int, SelectCard>();
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Initlize(List<SelectCard> candidateCards)
    {
        isFinished = false;
        rollDuration = Random.Range(5.0f, 8.0f);
        loopCount = Random.Range(5, 10);

        foreach(Transform child in rolletParent)
        {
            Destroy(child.gameObject);
        }
        cards.Clear();

        ActiveCards.Clear();
        for(int i = 0; i < 20; i++)
        {
            int randomValue = Random.Range(0, candidateCards.Count);
            var pick = candidateCards[randomValue];
            ActiveCards.Add(i, pick);

            GameObject go = Instantiate(cardPrefab, rolletParent);
            go.SetActive(true);
            go.GetComponent<Image>().sprite = MANAGER.DB.GetSprite(pick.db.name);
            RectTransform rt = go.GetComponent<RectTransform>();

            rt.anchoredPosition = new Vector2(0, cardHeight * -i);
            cards.Add(rt);
        }

        rolletParent.anchoredPosition = Vector2.zero;

        int targetIndex = Random.Range(0, cards.Count);
        SelectCard card = ActiveCards[targetIndex];
        if(MANAGER.Session.selectedCard.ContainsKey(card.db.id))
        {
            MANAGER.Session.selectedCard[card.db.id].level++;
        }

        StartCoroutine(RollingCoroutine(targetIndex));
    }
    IEnumerator RollingCoroutine(int targetIndex)
    {

        int totalSteps = loopCount * cards.Count + targetIndex;
        float totalDistance = totalSteps * cardHeight;

        Vector2 startPos = rolletParent.anchoredPosition;
        Vector2 endPos = startPos - new Vector2(0, totalDistance);
        float elapsed = 0.0f;

        Vector2 prevPos = startPos;
        float threshold = 1.5f;

        while(elapsed < rollDuration) 
        {
            elapsed += Time.unscaledDeltaTime;
            float t = EaseOutQuart(elapsed / rollDuration);
            Vector2 currentPos = Vector2.Lerp(startPos, endPos, t);

            if((currentPos - prevPos).magnitude < threshold && (endPos - currentPos).magnitude < 10.0f)
            {
                rolletParent.anchoredPosition = endPos;
                break;
            }
            rolletParent.anchoredPosition = currentPos;
            prevPos = currentPos;

            for(int i = 0; i < cards.Count; i++)
            {
                if(cards[i].anchoredPosition.y + rolletParent.anchoredPosition.y < -cardHeight)
                {
                    float highestY = GetHightestCardY();
                    cards[i].anchoredPosition = new Vector2(0, highestY + cardHeight);
                }
            }
            yield return null;
        }

        isFinished = true;

        SelectCard card = ActiveCards[targetIndex];
        if(MANAGER.Session.selectedCard.ContainsKey(card.db.id))
        { 
            MANAGER.Session.RegisterSkill(card.db);
        }
        else
        {
            MANAGER.Session.HP += 25;
        }

        treasure.ConfirmCheck();
        animator.Play("Effect");
        // 최종 정렬
        RectTransform selectedCard = cards[targetIndex];
        rolletParent.anchoredPosition = endPos;
    }

    float GetHightestCardY()
    {
        float maxY = float.MinValue;
        foreach(var card in cards)
        {
            if(card.anchoredPosition.y > maxY)
                maxY = card.anchoredPosition.y;
        }
        return maxY;
    }

    float EaseOutQuart(float t)
    {
        // 감속
        return 1 - Mathf.Pow(1 - t, 4);
    }
}
