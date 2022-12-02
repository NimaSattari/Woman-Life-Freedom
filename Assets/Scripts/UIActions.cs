using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIActions : MonoBehaviour
{
    public GameObject move, pickup, open, drop, throwIt, change;
    public GameObject nameObject, storyObject;
    public TextMeshProUGUI objectNameText, objectStoryText;

    [SerializeField] public static UIActions instance;
    [SerializeField] public GameObject invisiblaWall1, invisiblaWall2, invisiblaWall3, invisiblaWall4;
    #region Singleton
    private void OnEnable()
    {
        if (UIActions.instance == null)
        {
            UIActions.instance = this;
        }
        else
        {
            if (UIActions.instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion


    private void Start()
    {
        StartCoroutine(TurnOfObjectAtTime(move, 5f));
    }

    IEnumerator TurnOfObjectAtTime(GameObject game, float timer)
    {
        yield return new WaitForSeconds(timer);
        game.SetActive(false);
    }
}
