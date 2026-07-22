using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneBehaviour : MonoBehaviour
{
    [SerializeField] private int targetScene;
    [SerializeField] private GameObject loadingScreen;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataPersistanceManager.instance.NewGame();
            StartCoroutine(LoadSceneAsync(targetScene));
        }
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operationSave = DataPersistanceManager.instance.SaveGame();
        loadingScreen.SetActive(true);

        while (!operationSave.isDone)
        {
            yield return null;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
