using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public int level;

    // Los valores definidos serán los valores por defecto
    // Al crear una nueva partida, se generará un archivo de guardado con estos datos
    public GameData()
    {
        this.playerPosition = new Vector3(3.20f, 6.5f, 0f);
        this.level = 1;
    }
}
