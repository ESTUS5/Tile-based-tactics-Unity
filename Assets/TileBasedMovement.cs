using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBasedMovement : MonoBehaviour
{
    List<Tile> _selectableTiles = new List<Tile>();
    GameObject[] _tiles;

    Stack<Tile> _path = new Stack<Tile>();
    Tile _currentTile;

    public bool moving = false;
    public int _move = 5;
    public float _jumpHeight = 2;
    public float _moveSpeed = 2;
    Vector3 _velocity = new Vector3();
    Vector3 _heading = new Vector3();
    float _halfHeight = 0;
    protected void Init()
    {
        _tiles = GameObject.FindGameObjectsWithTag("Tile"); //you can use this if envo doesnt change
        Debug.Log(_tiles.Length);
        _halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    public void GetCurrentTile()
    {
        _currentTile = GetTargetTile(gameObject);
        _currentTile._current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if(Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }

    public void ComputeAdjacencyLists()
    {
        //_tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in _tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(_jumpHeight);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists();
        GetCurrentTile();
        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(_currentTile);
        _currentTile.visited = true;
        while(process.Count > 0)
        {
            Tile t = process.Dequeue();
            _selectableTiles.Add(t);
            t._selectable = true;
            if(t.distance < _move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if(!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }

            }
        }
    }
    public void MoveToTile(Tile tile)
    {
        _path.Clear();
        tile._target = true;
        moving = true;

        Tile next = tile;
        while(next != null)
        {
            _path.Push(next);
            next = next.parent;
        }
    }
    public void Move()
    {
        if(_path.Count > 0)
        {
            Tile t = _path.Peek();
            Vector3 target = t.transform.position;

            //calculate unit position on target tile.
            target.y += _halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if(Vector3.Distance(transform.position,target)>= 0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = _heading;
                transform.position += _velocity * Time.deltaTime;
            }
            else
            {
                //center reached
                transform.position = target;
                _path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            moving = false;
        }

    }
    protected void RemoveSelectableTiles()
    {
        if(_currentTile != null)
        {
            _currentTile._current = false;
            _currentTile = null;
        }
        foreach (Tile tile in _selectableTiles)
        {
            tile.Reset();
        }
        _selectableTiles.Clear();
    }
    void CalculateHeading(Vector3 target)
    {
        _heading = target - transform.position;
        _heading.Normalize();
    }
    void SetHorizontalVelocity()
    {
        _velocity = _heading * _moveSpeed;
    }
}
