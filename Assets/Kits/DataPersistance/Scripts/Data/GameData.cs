using System.Linq.Expressions;
using UnityEngine;
using static ChangeReality;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public int level;
    public KindOfReality playerReality;
    public bool[] obstacles;
    public bool[] activators;

    // Los valores definidos serán los valores por defecto
    // Al crear una nueva partida, se generará un archivo de guardado con estos datos
    public GameData()
    {
        this.playerPosition = new Vector3(3.20f, 6.5f, 0f);
        this.level = 1;
        this.playerReality = KindOfReality.Real;
        this.obstacles = new bool[] { false, false, false, false, false};
        this.activators = new bool[] { false, false };
    }
}
