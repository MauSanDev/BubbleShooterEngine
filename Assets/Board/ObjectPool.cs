using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : class
{
    private T blueprint;
    private Transform container;
    private int maxAmount;

    private Queue<T> poolList = new Queue<T>();

    public ObjectPool(T blueprint, Transform container, int maxAmount)
    {
        this.blueprint = blueprint;
        this.container = container;
        this.maxAmount = maxAmount;
    }

    public List<T> InstantiateAmount(int amount)
    {
        List<T> toReturn = new List<T>();
        for (int i = 0; i < amount; i++)
        {
            toReturn.Add(Object.Instantiate(blueprint as Object, container) as T);
        }
        return toReturn;
    }


    public virtual T GetElement()
    {
        if(poolList.Count > 0)
        {
            return poolList.Dequeue();
        }
        else if (poolList.Count < maxAmount)
        {
            T creation = Object.Instantiate(blueprint as Object, container) as T;
            OnObjectCreated(creation);
            return creation;
        }

        return null;
    }

    protected virtual void OnObjectCreated(T obj) { }

    public void ReturnElement(T toReturn)
    {
        poolList.Enqueue(toReturn);
    }
}
