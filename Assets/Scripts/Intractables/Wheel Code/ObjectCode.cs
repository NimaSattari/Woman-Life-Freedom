using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCode : MonoBehaviour
{
    [SerializeField] private int[] result, correctCombination;
    [SerializeField] private ObjectOpenClose openClose;

    private void Start()
    {
        result = new int[] { 5, 5, 5 };
        correctCombination = new int[] { 1, 2, 3 };
        Rotate.Rotated += CheckResults;
    }

    private void CheckResults(string wheelName, int number)
    {
        int numb = Random.Range(0, 2);
        if(numb == 0)
        {
            SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.codeshuffle1);
        }
        else
        {
            SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.codeshuffle2);
        }
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
            SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.codesolve);
        }
    }

    private void OnDestroy()
    {
        Rotate.Rotated -= CheckResults;
    }
}
