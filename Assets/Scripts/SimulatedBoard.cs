using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimulatedBoard {

    public bool[,,,] simulatedBoard { set; get; }
    public List<SimulatedBoard> childs { set; get; }
    int boardSize;

    public SimulatedBoard(int boardSize) {
        simulatedBoard = new bool[boardSize,boardSize,boardSize,3];
        childs = new List<SimulatedBoard>();
        this.boardSize = boardSize;
    }

    public bool isOcupied(int x, int y, int z) {
        if (simulatedBoard[x, y, z, 1] || simulatedBoard[x, y, z, 1]) {
            return true;
        }
        return false;
    }

    public SimulatedBoard getCopy() {
        SimulatedBoard copy = new SimulatedBoard(boardSize);

        for (int a = 0; a < this.boardSize; a++) {
            for (int b = 0; b < this.boardSize; b++) {
                for (int c = 0; c < this.boardSize; c++) {
                    for (int d = 0; d < 3; d++) {
                        copy.simulatedBoard[a, b, c, d] = this.simulatedBoard[a, b, c, d];
                    }
                }
            }
        }

        foreach (SimulatedBoard sb in this.childs) {
            copy.childs.Add(sb);
        }
        copy.boardSize = this.boardSize;

        return copy;
    }

}
