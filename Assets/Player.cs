using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TileBasedMovement
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(!moving)
        {
            FindSelectableTiles();
            CheckMouse();
        }
        else
        {
            Move();
        }
        void CheckMouse()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if(Physics.Raycast(ray,out hit))
                {
                    if(hit.collider.tag == "Tile")
                    {
                        Tile t = hit.collider.GetComponent<Tile>();

                        if(t._selectable)
                        {
                            MoveToTile(t);
                        }
                    }
                }
            }
        }
    }
}
