using System;
using UnityEngine;
using UnityEngine.Events;
using static ChangeReality;

public class PlayerDataManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private Material realSkybox;
    [SerializeField] private Material shadowSkybox;
    [SerializeField] private ChangeReality[] changeRealities;
    [SerializeField] private float skyboxbRotationVelocity = 1.2f;
    public KindOfReality playerLayer;
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
            cr.onChangeReality.AddListener(UpdatePlayerLayer);
        }
    }

    private void Awake()
    {
        playerLayer = KindOfReality.Real;
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxbRotationVelocity);
    }

    public void LoadData(GameData data)
    {
        FindAnyObjectByType<PlayerBehaviour>().transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.playerPosition;
        Debug.Log("La posición " + playerPosition + " ha sido guardada gustosamente");
    }

    private void UpdateMyPosition(Vector3 newPos)
    {
        playerPosition = newPos;
        DataPersistanceManager.instance.SaveGame();
        Debug.Log("Get actualizado: " + newPos);
    }

    private void UpdatePlayerLayer(KindOfReality newLayer)
    {
        playerLayer = newLayer;

        if (playerLayer == KindOfReality.Real)
        {
            RenderSettings.skybox = realSkybox;
        }
        else
        {
            RenderSettings.skybox = shadowSkybox;
        }
    }
}
