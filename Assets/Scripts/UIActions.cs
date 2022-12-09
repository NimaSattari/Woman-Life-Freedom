using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] GameObject[] pictureAlbum;
    [SerializeField] GameObject[] audioAlbum;
    [SerializeField] Image storySprite;

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

    public void PictureActive(bool activity)
    {
        foreach(GameObject gameObject in pictureAlbum)
        {
            gameObject.SetActive(activity);
        }
    }

    public void AudioActive(bool activity)
    {
        foreach (GameObject gameObject in audioAlbum)
        {
            gameObject.SetActive(activity);
        }
    }

    public void ChangeStorySprite(Sprite sprite)
    {
        storySprite.sprite = sprite;
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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NextScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
