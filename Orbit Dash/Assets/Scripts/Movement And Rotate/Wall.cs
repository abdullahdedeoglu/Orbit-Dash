using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Wall wall;

    public Transform gameArena;

    private void Start()
    {
        wall.transform.parent = gameArena; 
    }
}
