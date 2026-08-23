using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class DBManager : MonoBehaviour
{
    public LevelDesign levelDesign;
    public PartDB Monster;
    public PartDB Item;

    public List<CardDB> ActiveCards = new List<CardDB>();
    public List<CardDB> PassiveCards = new List<CardDB>();
    public List<CardDB> NoneCards = new List<CardDB>();

    private SpriteAtlas atlas;
    private void Awake()
    {
        levelDesign = Resources.Load<LevelDesign>("DB/LevelDesign");
        Monster = GetDB("Monster");
        Item = GetDB("Item");

        atlas = Resources.Load<SpriteAtlas>("Atlas");

        ActiveCards = new List<CardDB>(Resources.LoadAll<CardDB>("DB/Card/Actives"));
        PassiveCards = new List<CardDB>(Resources.LoadAll<CardDB>("DB/Card/Passives"));
        NoneCards = new List<CardDB>(Resources.LoadAll<CardDB>("DB/Card/None"));
    }
    
    public Sprite GetSprite(string temp)
    {
        return atlas.GetSprite(temp);
    }

    public List<CardDB>GetRandomCardSet(bool AllActive = false)
    {
        List<CardDB> result = new();
    
        List<CardDB> activeCandidates = new();
        List<CardDB> passiveCandidates = new();

        foreach(var card in ActiveCards)
        {
            if(CanBeSelected(card)) activeCandidates.Add(card);
        }

        foreach(var card in PassiveCards)
        {
            if(CanBeSelected(card)) passiveCandidates.Add(card);
        }

        if(AllActive)
        {
            int activeCount = Mathf.Min(3, activeCandidates.Count);

            while(result.Count < activeCount)
            {
                CardDB pick = activeCandidates[Random.Range(0, activeCandidates.Count)];
                if(!result.Contains(pick)) result.Add(pick);
            }
            return result.OrderBy(x => Random.value).ToList();
        }

        if(activeCandidates.Count > 0) 
        { 
            result.Add(activeCandidates[Random.Range(0, activeCandidates.Count)]);
        }

        if(passiveCandidates.Count > 0)
        {
            result.Add(passiveCandidates[Random.Range(0, passiveCandidates.Count)]);
        }

        List<CardDB> candidates = new();
        candidates.AddRange(activeCandidates);
        candidates.AddRange(passiveCandidates);

        candidates.RemoveAll(x => result.Contains(x));

        int count = Mathf.Min(3, candidates.Count);

        while(result.Count < count)
        {
            CardDB pick = candidates[Random.Range(0, candidates.Count)];
            if(!result.Contains(pick)) result.Add(pick);
        }

        return result.OrderBy(x => Random.value).ToList();
    }

    private bool CanBeSelected(CardDB card)
    {
        var session = MANAGER.Session;

        if(session.selectedCard.TryGetValue(card.id, out SelectCard selected))
        {
            return selected.level < 5;
        }

        int active = session.selectedCard.Values.Count(x => x.db.state == CardState.Active);
        int passive = session.selectedCard.Values.Count(x => x.db.state == CardState.Passive);

        if(card.state == CardState.Active && active > 5) return false;
        if(card.state == CardState.Passive && active > 5) return false;

        return true;
    }

    private PartDB GetDB(string path)
    {
        return Resources.Load<PartDB>("DB/Part/" + path);
    }
}
