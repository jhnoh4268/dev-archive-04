using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagnetEffect : MonoBehaviour, IItemEffect
{
    private bool isPickUp = false;
    public void Initlize()
    {
        isPickUp = false;
    }

    public void OnPickUp(GameObject owner)
    {
        if(isPickUp) return;
        isPickUp = true;

        List<Orb> orbs = MANAGER.Session.Orbs;
        for(int i = 0; i < orbs.Count; i++)
        {
            orbs[i].StartFollow(Player.instance.transform);
        }
    }
}
