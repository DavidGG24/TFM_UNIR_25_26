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
    //[SerializeField] private GameObject realCharacter;
    //[SerializeField] private GameObject shadowCharacter;
    public UnityEvent<KindOfReality> onChangeReality;

    private KindOfReality playerLayer;

    private void OnEnable()
    {
        cambiar.action.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerLayer = KindOfReality.Real;
    }

    // Update is called once per frame
    void Update()
    {
        if (cambiar.action.triggered)
        {
            if (playerLayer == KindOfReality.Real)
            {
                playerLayer = KindOfReality.Shadow;
            } else
            {
                playerLayer = KindOfReality.Real;
            }

            onChangeReality.Invoke(playerLayer);
        }
    }

    private void OnDisable()
    {
        cambiar.action.Disable();
    }
}
