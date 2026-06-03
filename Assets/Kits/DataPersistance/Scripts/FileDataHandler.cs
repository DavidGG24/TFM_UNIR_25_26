using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        // Usa Path.Combine para tener en cuenta el sistema de rutas de diferentes SO
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Carga los datos serializados del archivo
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserializa los datos cargados desde el archivo de JSON a C#
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Ha ocurrido un error al intentar cargar los datos del archivo: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // Usa Path.Combine para tener en cuenta el sistema de rutas de diferentes SO
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // Crea el directorio en el que se va a escribir el archivo si no existe todavía
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serializa el objeto de datos de juego de C# a JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // Escribe los datos serializados en el archivo
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Ha ocurrido un error al intentar guardar en el archivo: " + fullPath + "\n" + e);
        }
    }
}
