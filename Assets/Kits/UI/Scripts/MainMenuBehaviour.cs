using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBarFill;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private GameData gameData;
    private GameObject lastSelectedObject = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newGameButton.onClick.AddListener(StartNewGame);
        continueButton.onClick.AddListener(ContinueGame);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);

        if (continueButton.enabled)
        {
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        } else
        {
            EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedObject);
        }
    }

    void StartNewGame()
    {
        Debug.Log("Ejecutado start new game");
        DataPersistanceManager.instance.NewGame();
        gameData = DataPersistanceManager.instance.RetrieveDataCopy();
        StartCoroutine(LoadSceneAsync(gameData.level));
    }

    private void ContinueGame()
    {
        Debug.Log("Ejecutado conitnue game");
        DataPersistanceManager.instance.LoadGame();
        gameData = DataPersistanceManager.instance.RetrieveDataCopy();
        StartCoroutine(LoadSceneAsync(gameData.level));
    }

    private void OpenSettings()
    {
        Debug.Log("Ejecutado abrir ajustes");
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneAsync(int sceneId) // Visualiza pantalla de carga y carga la escena, mostrando el progreso
    {
        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);
        yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f); // Carga tan rápido que no se ve, pero devuelve el progreso de la carga
            loadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }

}
