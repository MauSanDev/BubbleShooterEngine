public class SpawnRateHandler<T> : StackCounter<T>
{
    private int totalRate = 0;

    protected override void OnStack(T element, int amount = 1)
    {
        totalRate += amount;
    }

    protected override void OnPop(T element, int amount = 1)
    {
        totalRate -= amount;
    }


    public bool TryGetByRate(int rate, out T spawn)
    {
        if(rate > totalRate)
        {
            spawn = default(T);
            return false;
        }

        int count = 0;
        
        foreach (T element in Keys)
        {
            int itemWeight = this[element];
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
        foreach (T element in Keys)
        {
            int itemWeight = this[element];
            count += itemWeight;
            if (randomSpawn <= count)
            {
                return element;
            }
        }
        return default(T);
    }


    public int TotalRate => totalRate;

    public bool Contains(T element) => ContainsKey(element);
}
