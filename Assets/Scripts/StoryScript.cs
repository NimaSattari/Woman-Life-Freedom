using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScript : MonoBehaviour
{
    [SerializeField]
    GameObject move, pickup, open, drop, throwIt,
    change, lighter, invisiblaWall1, invisiblaWall2, invisiblaWall3,
    invisiblaWall4, whereisLight;

    private void Start()
    {
        StartCoroutine(TurnOnObjectAtTime(move, 3f));
        StartCoroutine(TurnOffObjectAtTime(move, 7f));
        StartCoroutine(TurnOnObjectAtTime(whereisLight, 10f));
        StartCoroutine(TurnOffObjectAtTime(whereisLight, 15f));
        StartCoroutine(TurnOnObjectAtTime(lighter, 16f));
        StartCoroutine(TurnOffObjectAtTime(lighter, 20f));

        StartCoroutine(TurnOffObjectAtTime(invisiblaWall1, 22f));
        StartCoroutine(TurnOffObjectAtTime(invisiblaWall2, 22f));
        StartCoroutine(TurnOffObjectAtTime(invisiblaWall3, 22f));
        StartCoroutine(TurnOffObjectAtTime(invisiblaWall4, 22f));
    }

    IEnumerator TurnOffObjectAtTime(GameObject game, float timer)
    {
        yield return new WaitForSeconds(timer);
        game.SetActive(false);
    }
    IEnumerator TurnOnObjectAtTime(GameObject game, float timer)
    {
        yield return new WaitForSeconds(timer);
        game.SetActive(true);
    }
}
