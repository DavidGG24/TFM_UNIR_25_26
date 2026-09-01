using TMPro;
using UnityEngine;
using static ChangeReality;

public class ChangeTutorialText : MonoBehaviour
{
    [SerializeField] string tutorialText;
    [SerializeField] float timeActive;

    private TMP_Text playerText;
    private bool activated = false;

    private float transparency = 0f;
    private float timeStarted;
    private KindOfReality currentReality;

    // Update is called once per frame
    void Update()
    {
        if (playerText)
        {
            if (activated && transparency < 1f)
            {
                transparency = transparency + (Time.deltaTime / 2);
                if (currentReality == KindOfReality.Real)
                {
                    playerText.color = new Color(1f, 1f, 1f, transparency);
                    playerText.outlineColor = Color.black;
                } else
                {
                    playerText.color = new Color(0f, 0f, 0f, transparency);
                    playerText.outlineColor = Color.white;
                }
            }

            if (transparency >= 1f && timeStarted > Time.time)
            {
                timeStarted = Time.time;
            }

            if (timeStarted + timeActive < Time.time)
            {
                activated = false;
                timeStarted = 0f;
                transparency = 0f;
                if (currentReality == KindOfReality.Real)
                {
                    playerText.color = new Color(1f, 1f, 1f, transparency);
                }
                else
                {
                    playerText.color = new Color(0f, 0f, 0f, transparency);
                }
                playerText = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.transform.childCount > 0)
        {
            Debug.Log("Ha entrado el player al tutorial de: " + tutorialText);
            playerText = other.transform.GetChild(other.transform.childCount - 1).GetComponentInChildren<TMP_Text>();
            playerText.text = tutorialText;
            playerText.enabled = true;
            timeStarted = 99999999999999999999f;
            activated = true;
            transparency = 0f;
            currentReality = DataPersistanceManager.instance.RetrieveDataCopy().playerReality;
        }
    }
}
