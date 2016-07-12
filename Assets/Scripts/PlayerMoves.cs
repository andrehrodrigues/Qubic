using UnityEngine;
using System.Collections;

public class PlayerMoves{

    public GameObject go { set; get; }
    public int grid { set; get; }
    public bool playerOne { set; get; }

    public int posX { set; get; }
    public int posY { set; get; }
    public int posZ { set; get; }

    public PlayerMoves(int x,int y,int z,GameObject gameObject, int gridNumber, bool playerOne) {
        this.go = gameObject;
        this.grid = gridNumber;
        this.playerOne = playerOne;
        this.posX = x;
        this.posY = y;
        this.posZ = z;
    }

}
