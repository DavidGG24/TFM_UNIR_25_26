using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static ChangeReality;

public class PlayerDataManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private Material realSkybox;
    [SerializeField] private Material shadowSkybox;
    [SerializeField] private ChangeReality[] changeRealities;
    [SerializeField] private float skyboxbRotationVelocity = 1.2f;
    public KindOfReality playerReality;
    private Vector3 playerPosition;

    private void OnEnable()
    {
        SavePoint[] savePoints = FindObjectsByType<SavePoint>(FindObjectsSortMode.None);
        foreach (SavePoint sp in savePoints)
        {
            if (sp != this)
            {
                sp.UpdateEverySave.AddListener(UpdateMyPosition);
            }
        }

        foreach (ChangeReality cr in changeRealities)
        {
            cr.onChangeReality.AddListener(UpdatePlayerReality);
        }
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxbRotationVelocity);
    }

    public void LoadData(GameData data)
    {
        FindAnyObjectByType<PlayerBehaviour>().transform.position = data.playerPosition;

        playerReality = data.playerReality;

        if (FindObjectsByType<BreakableObstacle>(FindObjectsSortMode.None).Length > 0)
        {
            foreach (BreakableObstacle obstacle in FindObjectsByType<BreakableObstacle>(FindObjectsSortMode.None))
            {
                for (int i = 0; i < data.obstacles.Length; i++)
                {
                    if (obstacle.obstacleId == i)
                    {
                        obstacle.isDestroyed = data.obstacles[i];
                        break;
                    }
                }
            }
        }

        if (FindObjectsByType<ActivatorBehaviour>(FindObjectsSortMode.None).Length > 0)
        {
            foreach (ActivatorBehaviour activator in FindObjectsByType<ActivatorBehaviour>(FindObjectsSortMode.None))
            {
                for (int i = 0; i < data.activators.Length; i++)
                {
                    if (activator.id == i)
                    {
                        activator.estoyActivado = data.activators[i];
                        break;
                    }
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.playerPosition;
        Debug.Log("La posición " + playerPosition + " ha sido guardada satisfactoriamente");

        data.playerReality = this.playerReality;
        Debug.Log("La realidad " + playerReality + " ha sido guardada satisfactoriamente");

        if (FindObjectsByType<BreakableObstacle>(FindObjectsSortMode.None).Length > 0)
        {
            foreach (BreakableObstacle obstacle in FindObjectsByType<BreakableObstacle>(FindObjectsSortMode.None))
            {
                for (int i = 0; i < data.obstacles.Length; i++)
                {
                    if (obstacle.obstacleId == i)
                    {
                        data.obstacles[i] = obstacle.isDestroyed;
                        break;
                    }
                }
            }
        }

        if (FindObjectsByType<ActivatorBehaviour>(FindObjectsSortMode.None).Length > 0)
        {
            foreach (ActivatorBehaviour activator in FindObjectsByType<ActivatorBehaviour>(FindObjectsSortMode.None))
            {
                for (int i = 0; i < data.activators.Length; i++)
                {
                    if (activator.id == i)
                    {
                        data.activators[i] = activator.estoyActivado;
                        break;
                    }
                }
            }
        }
    }

    private void UpdateMyPosition(Vector3 newPos)
    {
        playerPosition = newPos;
        DataPersistanceManager.instance.SaveGame();
        Debug.Log("Get actualizado: " + newPos);
    }

    private void UpdatePlayerReality(KindOfReality newReality)
    {
        playerReality = newReality;

        if (playerReality == KindOfReality.Real)
        {
            RenderSettings.skybox = realSkybox;
        }
        else
        {
            RenderSettings.skybox = shadowSkybox;
        }
    }
}
