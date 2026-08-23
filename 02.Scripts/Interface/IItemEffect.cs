using UnityEngine;

public interface IItemEffect
{
    void Initlize();
    void OnPickUp(GameObject owner);
}
