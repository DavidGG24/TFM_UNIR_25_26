using System;
using UnityEngine;

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
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeActivated + timeInActive)
        {
            gameObject.SetActive(false);
        }
    }

    private void ActivatePlatform(float timeActivated, float timeInActive)
    {
        gameObject.SetActive(true);
        this.timeActivated = timeActivated;
        this.timeInActive = timeInActive;
    }
}
