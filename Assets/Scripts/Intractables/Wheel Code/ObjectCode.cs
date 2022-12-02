using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCode : MonoBehaviour
{
    [SerializeField] private int[] result, correctCombination;
    [SerializeField] private ObjectOpenClose openClose;

    private void Start()
    {
        result = new int[] { 0, 0, 0 };
        correctCombination = new int[] { 2, 3, 4 };
        Rotate.Rotated += CheckResults;
    }

    private void CheckResults(string wheelName, int number)
    {
        switch (wheelName)
        {
            case "wheel1":
                result[0] = number;
                break;
            case "wheel2":
                result[1] = number;
                break;
            case "wheel3":
                result[2] = number;
                break;
        }
        if(result[0] == correctCombination[0] && result[1] == correctCombination[1] && result[2] == correctCombination[2])
        {
            openClose.isUnlocked = true;
            openClose.Open();
        }
    }

    private void OnDestroy()
    {
        Rotate.Rotated -= CheckResults;
    }
}
