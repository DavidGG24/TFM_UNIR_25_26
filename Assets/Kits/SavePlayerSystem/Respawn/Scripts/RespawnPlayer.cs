using System;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerBehaviour[] playerCharacters = FindObjectsByType<PlayerBehaviour>(FindObjectsSortMode.None);
        foreach (PlayerBehaviour pc in playerCharacters)
        {
            if (pc != this)
            {
                pc.MakeRespawn.AddListener(MakeRespawn);
            }
        }
    }

    private void MakeRespawn()
    {
        DataPersistanceManager.instance.LoadGame();
    }
}
