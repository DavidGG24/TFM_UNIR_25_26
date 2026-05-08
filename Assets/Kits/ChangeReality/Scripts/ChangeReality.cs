using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ChangeReality : MonoBehaviour
{
    [SerializeField] private InputActionReference cambiar;
    [SerializeField] private GameObject realCharacter;
    [SerializeField] private GameObject shadowCharacter;
    public UnityEvent<int> onChangeReality;

    public int playerLayer;

    private void OnEnable()
    {
        cambiar.action.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerLayer = realCharacter.layer;
    }

    // Update is called once per frame
    void Update()
    {
        if (cambiar.action.triggered)
        {
            if (playerLayer == realCharacter.layer)
            {
                playerLayer = shadowCharacter.layer;
            } else
            {
                playerLayer = realCharacter.layer;
            }

            onChangeReality.Invoke(playerLayer);
        }
    }

    private void OnDisable()
    {
        cambiar.action.Disable();
    }
}
