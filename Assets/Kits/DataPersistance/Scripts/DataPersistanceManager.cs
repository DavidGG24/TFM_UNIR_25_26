using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistanceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Hay más de un Data Persistance Manager en escena.");
        }
        instance = this;

        if (fileName != "")
        {
            this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            this.dataPersistanceObjects = FindAllDataPersistanceObjects();
            LoadGame();
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();

        Debug.Log("Creada nueva posición: " + gameData.playerPosition);
        Debug.Log("Creados nuevos estados de obstáculos" + gameData.obstacles);
    }

    public void LoadGame()
    {
        if (fileName != "")
        {
            // Cargar toda la información guardada en un archivo usando el data handler
            this.gameData = dataHandler.Load();

            // si no hay datos que se puedan cargar, se inicializa a los valores por defecto.
            //if (this.gameData == null)
            //{
            //    Debug.Log("No se han encontrado datos. Inicializando a valores por defecto...");
            //    NewGame();
            //}

            if (dataPersistanceObjects.Count > 0)
            {
                // Llevar los datos cargados a los scripts que los necesiten
                foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
                {
                    dataPersistanceObj.LoadData(gameData);
                }
            }

            Debug.Log("Cargada posición: " + gameData.playerPosition);
            Debug.Log("Cargada realidad: " + gameData.playerReality);
        }
    }

    public GameData RetrieveDataCopy()
    {
        return this.gameData;
    }

    public void SaveGame()
    {
        if (fileName != "")
        {
            // Pasar los datos a otros scripts para que los actualicen
            foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
            {
                dataPersistanceObj.SaveData(ref this.gameData);
            }

            Debug.Log("Guardada posición: " + gameData.playerPosition);
            Debug.Log("Guardada realidad: " + gameData.playerReality);

            this.gameData.level = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : SceneManager.GetActiveScene().buildIndex;

            Debug.Log("Guardado nivel: " + gameData.level);

            // Guardar los datos a un archivo usando el data handler
            dataHandler.Save(this.gameData);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
