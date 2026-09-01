using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvokeResumeGame : MonoBehaviour
{
    [SerializeField] InputActionReference pause;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject playerCharacter;

    private bool isGamePaused;

    private void OnEnable()
    {
        pause.action.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.action.triggered)
        {
            isGamePaused = !isGamePaused;
            //Time.timeScale = isGamePaused ? 0f : 1f;
            //playerCharacter.SetActive(!isGamePaused);
            pauseMenu.SetActive(isGamePaused);
        }
    }

    private void OnDisable()
    {
        pause.action.Disable();
    }
}
