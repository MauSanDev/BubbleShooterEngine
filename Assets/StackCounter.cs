using System.Collections.Generic;
using UnityEngine;

public class StackCounter<T> : Dictionary<T, int>
{
    public void Stack(T element, int amount = 1)
    {
        if(!ContainsKey(element))
        {
            Add(element, amount);
            return;
        }
        
        this[element] += amount;
    }

    public int GetAmount(T type)
    {
        if (!ContainsKey(type))
        {
            return 0;
        }

        return this[type];
    }

    public void Pop(T element, int amount = 1)
    {
        if(!ContainsKey(element))
        {
            return;
        }
        
        this[element] = Mathf.Max(0, this[element] - amount);
    }

    public int TotalStacks
    {
        get
        {
            int count = 0;
            foreach (T key in Keys)
            {
                count += this[key];
            }

            return count;
        }
    }
}
