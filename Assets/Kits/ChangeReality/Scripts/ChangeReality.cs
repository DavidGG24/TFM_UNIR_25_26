using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChangeReality : MonoBehaviour
{
    public enum KindOfReality
    {
        Real,
        Shadow,
        Both
    }

    [SerializeField] private InputActionReference cambiar;
    [SerializeField] private bool onlyChangeInside;
    public UnityEvent<KindOfReality> onChangeReality;
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] mirrorClips;

    private KindOfReality playerReality;
    private bool isPlayerInside;

    private void OnEnable()
    {
        cambiar.action.Enable();
    }

    void Start()
    {
        isPlayerInside = false;
        StartCoroutine(StartChangeReality());
    }

    void Update()
    {
        if ((cambiar.action.triggered && !onlyChangeInside) || (cambiar.action.triggered && onlyChangeInside && isPlayerInside))
        {
            playerReality = FindFirstObjectByType<PlayerDataManager>().playerReality;
            
            if (GetComponent<ApplyRealityLogic>().myReality == playerReality || GetComponent<ApplyRealityLogic>().myReality == KindOfReality.Both)
            {
                if (playerReality == KindOfReality.Real)
                {
                    playerReality = KindOfReality.Shadow;
                    //RenderSettings.skybox = shadowSkybox;
                    GetComponent<AudioSource>().clip = mirrorClips[0];
                }
                else
                {
                    playerReality = KindOfReality.Real;
                    //RenderSettings.skybox = realSkybox;
                    GetComponent<AudioSource>().clip = mirrorClips[1];
                }

                onChangeReality.Invoke(playerReality);
                GetComponent<AudioSource>().Play();
                DataPersistanceManager.instance.SaveGame(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private IEnumerator StartChangeReality()
    {
        yield return new WaitForEndOfFrame();
        playerReality = FindFirstObjectByType<PlayerDataManager>().playerReality;
        onChangeReality.Invoke(playerReality);
        Debug.Log("He empezado y ahora soy: " + playerReality);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private void OnDisable()
    {
        cambiar.action.Disable();
    }
}
