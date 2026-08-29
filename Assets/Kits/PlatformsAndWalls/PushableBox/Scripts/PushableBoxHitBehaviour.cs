using System.Collections.Generic;
using UnityEngine;

public class PushableBoxHitBehaviour : MonoBehaviour
{
    [Header("Ground Layer")]
    [SerializeField] private LayerMask GroundLayer;
    [Header("Audio Clips")]
    [SerializeField] AudioClip[] hitClips;

    private bool isOnGround = true;
    private bool canPlayHit = false;
    public List<Collision> everyFloorITouch = new List<Collision>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (everyFloorITouch.Count == 0)
        {
            isOnGround = false;
        }

        if (!isOnGround)
        {
            canPlayHit = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == GroundLayer)
        {
            isOnGround = true;
            everyFloorITouch.Add(collision);
        }

        if (canPlayHit)
        {
            int selectedClip = Mathf.RoundToInt(UnityEngine.Random.Range(0f, hitClips.Length - 1));

            GetComponent<AudioSource>().clip = hitClips[selectedClip];
            GetComponent<AudioSource>().Play();

            canPlayHit = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == GroundLayer)
        {
            if (everyFloorITouch.Contains(collision))
            {
                everyFloorITouch.Remove(collision);
            }
        }
    }
}
