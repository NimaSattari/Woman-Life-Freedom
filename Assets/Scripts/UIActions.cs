using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIActions : MonoBehaviour
{
    #region Singleton
    [SerializeField] public static UIActions instance;
    [SerializeField] GameObject pauseMenu;

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

    private TextMeshProUGUI objectNameText, objectStoryText;

    [SerializeField] GameObject nameObject, storyObject;

    private void Awake()
    {
        objectNameText = nameObject.GetComponentInChildren<TextMeshProUGUI>();
        objectStoryText = storyObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ReactToObjectSeen(string nameOfObject)
    {
        nameObject.SetActive(true);
        objectNameText.text = nameOfObject;
    }

    public void ReactToObjectPick(string nameOfObject, string storyOfObject)
    {
        nameObject.SetActive(true);
        objectNameText.text = nameOfObject;
        if (!string.IsNullOrEmpty(storyOfObject))
        {
            storyObject.SetActive(true);
            objectStoryText.text = storyOfObject;
        }
    }

    public void ReactToObjectReverse()
    {
        nameObject.SetActive(false);
        storyObject.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QUitGame()
    {
        Application.Quit();
    }
}
