using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool _current = false;
    public bool _walkable = true;
    public bool _target = false;
    public bool _selectable = false;
    public List<Tile> adjacencyList = new List<Tile>();
    //BFS
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    [SerializeField] private Color _baseColor, _offsetColor;

    [SerializeField] private GameObject _highlight;
    private Color originalColor;

    public void Init(bool isOffset)
    {
        //Color meshColor = _material.color;//GetComponent<Material>().color;
        GetComponent<MeshRenderer>().material.color = isOffset ? _offsetColor : _baseColor;
        originalColor = GetComponent<Renderer>().material.color ;
    }
    private void Start() {
        originalColor = GetComponent<Renderer>().material.color ;
    }
    private void OnMouseEnter() {
        _highlight.SetActive(true);
    }
    private void OnMouseExit() {
        _highlight.SetActive(false);
    }
    private void Update() {
        
        if(_current)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if(_target)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if(_selectable)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            GetComponent<Renderer>().material.color = originalColor;
        }
    }

    public void Reset() {
        adjacencyList.Clear();
        _current = false;
        _walkable = true;
        _target = false;
        _selectable = false;
    
    //BFS
        visited = false;
        parent = null;
        distance = 0;
    }
    public void FindNeighbors(float jumpHeight)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight);
        CheckTile(-Vector3.forward, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
        CheckTile(-Vector3.right, jumpHeight);
    }
    public void CheckTile(Vector3 direction,float jumpHeight)
    {
        Vector3 halfExtents = new Vector3(0.25f,(1+jumpHeight)/2f,0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction,halfExtents);
        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if(tile != null && tile._walkable)
            {
                RaycastHit hit;
                if(!Physics.Raycast(tile.transform.position, Vector3.up,out hit, 1))
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }
}
