using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IA {


    public void PredictMovents(SimulatedBoard board, int boardSize) {

        int depth = 3;
        int player = 2;
               
        GenerateTrees(board, boardSize, player, depth);

        int x = 0;

    }


    private void GenerateTrees(SimulatedBoard board, int boardSize, int player, int depth) {

        if (depth > 0)
        {
            List<SimulatedBoard> childs = board.childs;

            for (int a = 0; a < boardSize; a++)
            {

                for (int b = 0; b < boardSize; b++)
                {

                    for (int c = 0; c < boardSize; c++)
                    {
                        SimulatedBoard child = board.getCopy();
                        if (!child.isOcupied(a, b, c))
                        {
                            child.simulatedBoard[a, b, c, player] = true;
                            childs.Add(child);

                            GenerateTrees(child, boardSize, (player == 1 ? 2 : 1), depth - 1);
                        }
                    }
                }
            }
        }

        



    }






}
