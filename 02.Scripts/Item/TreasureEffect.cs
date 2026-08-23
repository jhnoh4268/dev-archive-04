using UnityEngine;

public class TreasureEffect : MonoBehaviour, IItemEffect
{
    //public ParticleSystemRenderer particle;
    //public Material[] materials;

    int value = 0;
    private bool isPickUp = false;

    public void Initlize()
    {
        isPickUp = false;
        int randomValue = Random.Range(0, 3);
        value = randomValue;
    }

    public void OnPickUp(GameObject owner)
    {
        if(isPickUp) return;
        isPickUp = true;
        BaseCanvas.instance.SelectTreasure(value);
    }
}
