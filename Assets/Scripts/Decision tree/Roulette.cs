using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Roulette
{
    public string Execute(Dictionary<string, int> actions)
    {
        var totalWeight = 0;
        foreach (var action in actions)
        {
            totalWeight += action.Value;
        }


        var random = Random.Range(0, totalWeight + 1);
        foreach (var action in actions)
        {
            random -= action.Value;
            if (random < 0)
            {
                return action.Key;
            }
        }

        return default;
    }
}
