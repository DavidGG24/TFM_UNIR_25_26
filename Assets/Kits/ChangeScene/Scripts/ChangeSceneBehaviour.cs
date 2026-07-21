using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneBehaviour : MonoBehaviour
{
    [SerializeField] private int targetScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataPersistanceManager.instance.NewGame();
            DataPersistanceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync(targetScene);
        }
    }
}
