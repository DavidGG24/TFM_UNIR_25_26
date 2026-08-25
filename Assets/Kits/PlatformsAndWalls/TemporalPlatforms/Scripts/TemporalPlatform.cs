using System;
using UnityEngine;
using static ChangeReality;

public class TemporalPlatform : MonoBehaviour
{
    [SerializeField] private TemporalActivatorBehaviour button;

    private float timeActivated;
    private float timeInActive;
    private void OnEnable()
    {
        button.onTemporalActivated.AddListener(ActivatePlatform);
    }


    void Start()
    {
        timeActivated = 0f;
        timeInActive = 0f;
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
            }
        }
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeActivated + timeInActive)
        {
            GetComponent<Collider>().isTrigger = true;
            GetComponent<Collider>().enabled = false;
            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }

    private void ActivatePlatform(float timeActivated, float timeInActive)
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            }

            if (GetComponent<ApplyRealityLogic>())
            {
                if (GetComponent<ApplyRealityLogic>().myReality == GetComponent<ApplyRealityLogic>().currentReality || GetComponent<ApplyRealityLogic>().myReality == KindOfReality.Both)
                {
                    GetComponent<Collider>().isTrigger = false;
                    GetComponent<Collider>().enabled = true;
                }
            }
        }
        this.timeActivated = timeActivated;
        this.timeInActive = timeInActive;
    }
}
