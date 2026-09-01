using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneBehaviour : SavePoint // Hereda de SavePoint para poder modificar el punto de spawn
{
    [SerializeField] private int targetScene; // Escena a cargar
    [SerializeField] private GameObject loadingScreen; // Pantalla de carga
    [SerializeField] private Image loadingBarFill; // Barra de progreso
    [SerializeField] private Vector3 spawnPoint; // Punto de spawn en la escena cargada
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Al entrar en contacto, setea la posición especificada y empieza la función de carga
        {
            playerPosition = spawnPoint;
            UpdateEverySave.Invoke(playerPosition);
            Debug.Log("Cogida la posición " + playerPosition);
            //other.gameObject.SetActive(false);
            AudioListener.volume = 0f;
            StartCoroutine(LoadSceneAsync(targetScene));
        }
    }

    IEnumerator LoadSceneAsync(int sceneId) // Visualiza pantalla de carga y carga la escena, mostrando el progreso
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        DataPersistanceManager.instance.SaveGame(targetScene);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f); // Carga tan rápido que no se ve, pero devuelve el progreso de la carga
            loadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }
}
