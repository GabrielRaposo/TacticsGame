using System.Collections.Generic;
using UnityEngine;

//Grupo criado para simplificar o acesso às tiles do jogo
//Todas tiles que foram criadas no jogo devem ser filhas do componente que carrega este script
public class TilesGroup : MonoBehaviour {

    public static TilesGroup instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public List<GameObject> GetTiles()
    {
        List<GameObject> tiles = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            tiles.Add(transform.GetChild(i).gameObject);
        }

        return tiles;
    }
}
