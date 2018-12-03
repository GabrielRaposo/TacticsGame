using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool current;
    private bool target;
    private bool selectable; 

    public bool Current
    {
        get { return current; }
        set { current = value; UpdateColor(); }
    }

    public bool Target
    {
        get { return target; }
        set { target = value; UpdateColor(); }
    }

    public bool Selectable
    {
        get { return selectable; }
        set { selectable = value; UpdateColor(); }
    }

    public bool blocked;
    public int difficulty = 1; //quantos passos um jogador gasta ao tentar sair deste bloco

    [HideInInspector] public GameObject ocupation = null; //guarda o objeto que atualmente o ocupa
    [HideInInspector] public List<Tile> adjacencyList = new List<Tile>();

    //Busca em largura
    [HideInInspector] public bool visited;
    [HideInInspector] public Tile parent;
    [HideInInspector] public int distance;

    //A*
    [HideInInspector] public float f = 0;
    [HideInInspector] public float g = 0;
    [HideInInspector] public float h = 0;

    private SpriteRenderer _renderer;

    void Start ()
    {
        _renderer = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (_renderer)
        {
            if (blocked)
            {
                _renderer.color = Color.black;
            }
            else if (current)
            {
                _renderer.color = Color.magenta;
            }
            else if (target)
            {
                _renderer.color = Color.green;
            }
            else if (selectable)
            {
                _renderer.color = Color.red;
            }
            else
            {
                _renderer.color = Color.white;
            }
        }
    }

    public void Reset()
    {
        adjacencyList.Clear();

        current = target = selectable = false;
        UpdateColor();

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    public void FindNeighbors(float jumpHeight, Tile target)
    {
        Reset();

        CheckTile(Vector2.up * .5f + Vector2.right,   jumpHeight, target);
        CheckTile(Vector2.up * .5f + Vector2.left,    jumpHeight, target);
        CheckTile(Vector2.down * .5f + Vector2.right, jumpHeight, target);
        CheckTile(Vector2.down * .5f + Vector2.left,  jumpHeight, target);
    }

    public void CheckTile(Vector2 direction, float jumpHeight, Tile target)
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + direction, Vector2.one * .5f, 0);
        if (collider)
        {
            Tile tile = collider.GetComponent<Tile>();
            if(tile != null && !tile.blocked)
            {
                if(tile.ocupation == null || tile == target)
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }

}
