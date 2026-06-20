using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private Dictionary<T, float> allElements = new Dictionary<T, float>();

    public int Count => allElements.Count;

    public void Enqueue(T element, float cost)
    {
        if (!allElements.ContainsKey(element))
            allElements.Add(element, cost);
        else
            allElements[element] = cost;
    }

    public T Dequeue()
    {
        T minElement = default;
        float minCost = Mathf.Infinity;

        foreach (var item in allElements)
        {
            if (item.Value < minCost)
            {
                minElement = item.Key;
                minCost = item.Value;
            }
        }

        if (minElement != null)
            allElements.Remove(minElement);

        return minElement;
    }
}
