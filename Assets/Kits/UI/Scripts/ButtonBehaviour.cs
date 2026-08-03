using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    private Button boton;
    private TMP_Text texto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texto = GetComponentInChildren<TMP_Text>();
        boton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            texto.color = Color.white;
        } else
        {
            texto.color = Color.black;
        }
    }
}
