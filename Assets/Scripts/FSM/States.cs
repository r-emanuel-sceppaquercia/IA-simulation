using System.Collections.Generic;

public class States<T>
{
    private Dictionary<T, States<T>> dic = new Dictionary<T, States<T>>();

    public virtual void Awake() { }

    public virtual void Execute() { }

    public virtual void Sleep() { }

    public void AddTransition(T input, States<T> state)
    {
        dic.TryAdd(input, state);
    }

    public void RemoveTransition(T input)
    {
        if (dic.ContainsKey(input))
            dic.Remove(input);
    }

    public States<T> GetState(T input)
    {
        if (dic.ContainsKey(input))
            return dic[input];

        return default;
    }
}
