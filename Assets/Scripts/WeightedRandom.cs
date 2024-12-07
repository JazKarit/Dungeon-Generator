
using System.Collections.Generic;
using UnityEngine;

public class WeightedRandom
{

    // Unity-specific method (if in Unity, use UnityEngine.Random instead)
    public static int GetWeightedRandomIndex(float[] weights)
    {
        float totalWeight = 0;
        foreach (var weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = UnityEngine.Random.Range(0, totalWeight);  // Unity's Random.Range is inclusive of the lower bound and exclusive of the upper bound

        float accumulatedWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            accumulatedWeight += weights[i];
            if (randomValue < accumulatedWeight)
            {
                return i;
            }
        }
        Debug.Log(randomValue);
        Debug.Log(accumulatedWeight);
        Debug.Log("fali");
        return -1;  // Should never reach here
    }
}