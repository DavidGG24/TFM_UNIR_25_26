using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    //[SerializeField] private GameObject realCharacter;
    //[SerializeField] private GameObject shadowCharacter;
    public UnityEvent<KindOfReality> onChangeReality;

    private KindOfReality playerLayer;
    private bool isPlayerInside;

    private void OnEnable()
    {
        cambiar.action.Enable();
        playerLayer = FindFirstObjectByType<PlayerDataManager>().playerLayer;
    }

    void Start()
    {
        isPlayerInside = false;
    }

    void Update()
    {
        if (cambiar.action.triggered)
        {
            Debug.Log($"Hola, mi playerLayer es: {playerLayer}");
        }
        if ((cambiar.action.triggered && !onlyChangeInside) || (cambiar.action.triggered && onlyChangeInside && isPlayerInside))
        {
            playerLayer = FindFirstObjectByType<PlayerDataManager>().playerLayer;
            
            if (GetComponent<ApplyRealityLogic>().myReality == playerLayer || GetComponent<ApplyRealityLogic>().myReality == KindOfReality.Both)
            {
                if (playerLayer == KindOfReality.Real)
                {
                    playerLayer = KindOfReality.Shadow;
                    //RenderSettings.skybox = shadowSkybox;
                }
                else
                {
                    playerLayer = KindOfReality.Real;
                    //RenderSettings.skybox = realSkybox;
                }

                onChangeReality.Invoke(playerLayer);
            }
        }
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
