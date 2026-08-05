using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBarFill;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private GameObject playerCharacter;

    private GameData gameData;
    private GameObject lastSelectedObject = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        continueButton.onClick.AddListener(ContinueGame);
        settingsButton.onClick.AddListener(OpenSettings);
        returnButton.onClick.AddListener(ReturnToMenu);
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        playerCharacter.SetActive(false);
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        playerCharacter.SetActive(true);
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

    private void ContinueGame()
    {
        Debug.Log("Ejecutado conitnue game");
        gameObject.SetActive(false);
        //Time.timeScale = 1f;
    }

    private void OpenSettings()
    {
        Debug.Log("Ejecutado abrir ajustes");
    }

    private void ReturnToMenu()
    {
        //DataPersistanceManager.instance.SaveGame();
        StartCoroutine(LoadSceneAsync(0));
    }

    IEnumerator LoadSceneAsync(int sceneId) // Visualiza pantalla de carga y carga la escena, mostrando el progreso
    {
        gameObject.SetActive(false);
        loadingScreen.SetActive(true);
        //yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f); // Carga tan rápido que no se ve, pero devuelve el progreso de la carga
            loadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }
}
