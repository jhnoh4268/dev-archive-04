using UnityEngine;

public class Item : MonoBehaviour
{
    public string  ItemId;
    private IFactory<Item> factory;
    private IItemEffect itemEffect;

    public void Initilze(string id)
    {
        ItemId = id;
        if(factory == null)
        {
            factory = new GenericPartFactory<Item>(MANAGER.DB.Item);
        }

        factory.Build(this, id);

        itemEffect = GetComponentInChildren<IItemEffect>();
        itemEffect.Initlize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            itemEffect.OnPickUp(other.gameObject);
            MANAGER.poolManager.m_Pool_Dictionary["Item"].Return(gameObject);
        }
    }
}
