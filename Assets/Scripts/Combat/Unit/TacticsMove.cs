using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    const float OFFSET = .25f;

    //public bool turn = false;
    public float moveSpeed = 2;
    public float jumpHeight = 2;

    //protected bool moving = false;

    private List<Tile> selectableTiles = new List<Tile>();
    private List<GameObject> tiles;
    private Tile currentTile;

    private Stack<Tile> path = new Stack<Tile>();
    private Vector3 velocity = new Vector3();
    private Vector3 heading = new Vector3();

    protected TacticsUnit unit;
    protected SortingOrderByPosition orderByPosition;

    [HideInInspector] public Tile actualTargetTile;

    public void Init()
    {
        tiles = TilesGroup.instance.GetTiles();
        unit = GetComponent<TacticsUnit>();
        orderByPosition = GetComponent<SortingOrderByPosition>();

        GetCurrentTile();
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.ocupation = gameObject;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit2D hit = Physics2D.Raycast(target.transform.position, Vector2.down * .5f);
        Tile tile = null;

        if (hit)
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }


    public void ComputeAdjacencyList(float jumpHeight, Tile target)
    {
        //tiles = TilesGroup.instance.GetTiles();

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight, target);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyList(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();
        process.Enqueue(currentTile);
        currentTile.visited = true;
        currentTile.parent = null;

        while(process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.Selectable = true;

            if(t.distance < unit.movement)
            {
                foreach(Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = tile.difficulty + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void BuildPathToTile(Tile tile)
    {
        path.Clear();
        tile.Target = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }

        StartCoroutine(Move());
    }

    //Ajeitar depois
    protected IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (currentTile != null)
            {
                currentTile.ocupation = null;
            }

            if(path.Count > 0)
            {
                Tile t = path.Peek();
                Vector3 target = t.transform.position;
                target.y += OFFSET;

                if(Vector2.Distance(transform.position, target) > 0.1f)
                {
                    CalculateHeading(target);
                    SetVelocity();

                    transform.position += velocity * Time.deltaTime;
                    if (orderByPosition) orderByPosition.RefreshOrder();
                }
                else
                {
                    transform.position = target;
                    if (orderByPosition) orderByPosition.RefreshOrder();

                    path.Pop();
                }

            }
            else
            {
                RemoveSelectedTiles();

                GetCurrentTile();
                currentTile.ocupation = gameObject;
                break;
            }
        }

        EndMovement();
    }

    public void RemoveSelectedTiles()
    {
        if(currentTile != null)
        {
            currentTile = null;
        }

        foreach(Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    private void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    private void SetVelocity()
    {
        velocity = heading * moveSpeed;
    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach(Tile t in list)
        {
            if(t.f < lowest.f)
            {
                lowest = t;
            }
        }
        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= unit.movement)
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= unit.movement; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }


    protected void FindPath(Tile target)
    {
        ComputeAdjacencyList(jumpHeight, target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closeList = new List<Tile>();

        openList.Add(currentTile);
        currentTile.parent = null;
        currentTile.h = Vector2.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);
            closeList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                BuildPathToTile(actualTargetTile); // <----------------------------------------
                return;
            }

            foreach (Tile tile in t.adjacencyList)
            {
                if (closeList.Contains(tile)) continue;

                if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector2.Distance(tile.transform.position, t.transform.position);
                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector2.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector2.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }

        //todo - what do you do if there is nopath to the target tile?
        Debug.Log("Path not found");
    }

    public virtual void CallMovement() { }
    public virtual void EndMovement() { }
}
