using Coffee.UIEffects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour, ISelectHandler
{
    [SerializeField] AudioClip[] selectClips;
    [SerializeField] AudioClip[] confirmClips;
    private Button boton;
    private TMP_Text texto;

    public void OnSelect(BaseEventData eventData)
    {
        PlaySound(selectClips);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texto = GetComponentInChildren<TMP_Text>();
        boton = GetComponent<Button>();

        boton.onClick.AddListener(PlaySoundConfirm);
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

        if (!boton.interactable)
        {
            texto.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    private void PlaySound(AudioClip[] clips)
    {
        int selectedClip = Mathf.RoundToInt(UnityEngine.Random.Range(0f, clips.Length - 1));

        transform.parent.parent.GetComponent<AudioSource>().clip = clips[selectedClip];
        transform.parent.parent.GetComponent<AudioSource>().volume = 0.7f;
        transform.parent.parent.GetComponent<AudioSource>().Play();
    }

    private void PlaySoundConfirm()
    {
        PlaySound(confirmClips);
    }
}
