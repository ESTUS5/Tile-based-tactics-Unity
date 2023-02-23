using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _camera;

    private Dictionary<Vector3,Tile> _tiles;



    private void Start() {
        GenerateGrid();
    }
    void GenerateGrid(){
        _tiles = new Dictionary<Vector3, Tile>();
        for(int x = 0; x< _width; x++)
        {
            for(int y = 0; y<_height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x,0,y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);


                _tiles[new Vector3(x,0,y)] = spawnedTile;
            }
        }

        _camera.transform.position = new Vector3((float)_width/2 -0.5f,(float)_height/2 -0.5f,-10);
    }
}
