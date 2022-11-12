using System.Collections.Generic;

public class SpawnRateHandler<T>
{
    private Dictionary<T,  int> content = new Dictionary<T, int>();
    private int totalRate = 0;

    public void Register(T key, int spawnRate)
    {
        if(IsRegistered(key) || spawnRate == 0)
        {
            return;
        }

        content.Add(key, spawnRate);
        totalRate += spawnRate;
    }

    public int this[T element]
    {
        get => content[element];
        set
        {
            int dif = content[element] - value;
            totalRate -= dif;
            content[element] = value;
        }
    }

    public bool IsRegistered(T key) => content.ContainsKey(key);

    public bool TryGetByRate(int rate, out T spawn)
    {
        if(rate > totalRate)
        {
            spawn = default(T);
            return false;
        }

        int count = 0;
        
        foreach (T element in content.Keys)
        {
            int itemWeight = content[element];
            count += itemWeight;
            if (rate <= count)
            {
                spawn = element;
                return true;
            }
        }

        spawn = default(T);
        return false;
    }

    public T GetRandom()
    {
        int count = 0;
        int randomSpawn = UnityEngine.Random.Range(0, totalRate + 1); 
        foreach (T element in content.Keys)
        {
            int itemWeight = content[element];
            count += itemWeight;
            if (randomSpawn <= count)
            {
                return element;
            }
        }
        return default(T);
    }

    public int RegistriesAmount => content.Count;

    public int TotalRate => totalRate;

    public bool Contains(T element) => content.ContainsKey(element);
}
