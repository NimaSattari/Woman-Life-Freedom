using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoryScript : MonoBehaviour
{
    [SerializeField] public
    GameObject move, line1, line2, pick, throwIt, lighter, whereisKeyLight, toomaj, phone, music, mommy, letter;

    [SerializeField] GameObject[] beforePhoneObjects;
    [SerializeField] GameObject[] afterPhoneObjects;

    #region Singleton
    [SerializeField] public static StoryScript instance;
    private void OnEnable()
    {
        if (StoryScript.instance == null)
        {
            StoryScript.instance = this;
        }
        else
        {
            if (StoryScript.instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
    #endregion

    private void Start()
    {
        StartCoroutine(TurnOnObjectAtTime(move, 2));
        StartCoroutine(TurnOffObjectAtTime(move, 4));
        StartCoroutine(TurnOnObjectAtTime(lighter, 5));
        StartCoroutine(TurnOffObjectAtTime(lighter, 8));
        StartCoroutine(TurnOnObjectAtTime(line1, 9));
        StartCoroutine(TurnOffObjectAtTime(line1, 14));
        StartCoroutine(TurnOnObjectAtTime(line2, 14));
        StartCoroutine(TurnOffObjectAtTime(line2, 19));
        StartCoroutine(TurnOnObjectAtTime(pick, 20));
        StartCoroutine(TurnOffObjectAtTime(pick, 23));
        StartCoroutine(TurnOnObjectAtTime(throwIt, 24));
        StartCoroutine(TurnOffObjectAtTime(throwIt, 27));
        StartCoroutine(TurnOnObjectAtTime(whereisKeyLight, 28));
        StartCoroutine(TurnOffObjectAtTime(whereisKeyLight, 31));

        StartCoroutine(TurnOnObjectAtTime(phone, 31));
    }

    public IEnumerator TurnOffObjectAtTime(GameObject game, float timer)
    {
        yield return new WaitForSeconds(timer);
        game.SetActive(false);
    }
    public IEnumerator TurnOnObjectAtTime(GameObject game, float timer)
    {
        yield return new WaitForSeconds(timer);
        game.SetActive(true);
        if(game == phone)
        {
            music.GetComponent<AudioSource>().Stop();
        }
    }

    public void AfterPhone1()
    {
        foreach (GameObject gameObject in beforePhoneObjects)
        {
            gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in afterPhoneObjects)
        {
            gameObject.SetActive(true);
        }
        StartCoroutine(TurnOnObjectAtTime(mommy, 3));
        StartCoroutine(TurnOffObjectAtTime(mommy, 15));
    }
    public void AfterLetter()
    {
        StartCoroutine(TurnOnObjectAtTime(letter, 3f));
        StartCoroutine(TurnOffObjectAtTime(letter, 10f));
    }
    public void Toomaj()
    {
        StartCoroutine(TurnOnObjectAtTime(toomaj, 1f));
        StartCoroutine(TurnOffObjectAtTime(toomaj, 10f));
    }
}
