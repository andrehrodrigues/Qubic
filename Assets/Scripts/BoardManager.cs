using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    public Material mat;
    public Material playerOneMaterial;
    public Material playerTwoMaterial;

    public int boardSize;

    private int selectionX;
    private int selectionY;
    private int selectionZ;

    private SimulatedBoard playingBoard;
    private bool[,,] playerMoves;
    private List<PlayerMoves> spheres2dGrid = new List<PlayerMoves>();
    private List<PlayerMoves> spheres3dGrid = new List<PlayerMoves>();

    private GameObject sphere2dGrid;
    private GameObject sphere3dGrid;

    private int selectedGridNumber;
    private bool playerOne;

    IA pc;

    // Use this for initialization
    void Start()
    {
        playerOne = true;
        selectionX = -100;
        selectionY = -100;
        selectionZ = -100;
        selectedGridNumber = 0;
        playerMoves = new bool[4, 4, 4];
        boardSize = 4;
        playingBoard = new SimulatedBoard(boardSize);
        pc = new IA();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX < 0 && selectionX > -100 && selectionY >= 0)
            {
                if (!playingBoard.isOcupied(selectionX + 5, selectedGridNumber, selectionY))
                {
                    playingBoard.simulatedBoard[selectionX + 5, selectedGridNumber, selectionY, (playerOne? 1:2)] = true;
                    spheres2dGrid.Add(new PlayerMoves(selectionX, selectionY, selectionZ, DrawSphere2dGrid(selectionX, selectionY, selectionZ, playerOne), selectedGridNumber, playerOne));
                    spheres3dGrid.Add(new PlayerMoves(selectionX+5, selectedGridNumber, selectionY, DrawSphere3dGrid(selectionX, selectionY, selectionZ, playerOne), selectedGridNumber, playerOne));

                    if (gameFinished()) {
                        Application.LoadLevel("end");
                    }
                    playerOne = !playerOne;

                    JogadaIA();

                }
            }
        }
    }

    void OnRenderObject()
    {
        DrawAllGrids();
    }

    private void DrawAllGrids()
    {

        GL.PushMatrix();
        mat.SetPass(0);
        GL.Begin(GL.LINES);

        for (int k = 0; k < 4; k++)
        {
            Vector3 widthLine = new Vector3(4, 0, 0);
            Vector3 heigthLine = new Vector3(0, 0, 4);

            if (k == selectedGridNumber)
            {
                GL.Color(Color.red);
            }
            else
            {
                GL.Color(Color.black);
            }

            for (int i = 0; i <= 4; i++)
            {
                Vector3 start = new Vector3(0, k, i);

                GL.Vertex(start);
                GL.Vertex(start + widthLine);

                for (int j = 0; j <= 4; j++)
                {
                    start = new Vector3(j, k, 0);

                    GL.Vertex(start);
                    GL.Vertex(start + heigthLine);
                }
            }
        }

        GL.End();
        GL.PopMatrix();

        GL.PushMatrix();
        mat.SetPass(0);
        GL.Begin(GL.LINES);

        for (int i = 0; i <= 4; i++)
        {
            Vector3 widthLine = new Vector3(-4, 0, 0);
            Vector3 heigthLine = new Vector3(0, 4, 0);

            Vector3 start = new Vector3(-2, i, 0);

            GL.Vertex(start);
            GL.Vertex(start + widthLine);

            for (int j = -2; j >= -6; j--)
            {
                start = new Vector3(j, 0, 0);

                GL.Vertex(start);
                GL.Vertex(start + heigthLine);
            }
        }

        GL.End();
        GL.PopMatrix();
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("BoardLayer")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.y;
            selectionZ = (int)hit.point.z;
        }
        else
        {
            selectionX = -100;
            selectionY = -100;
            selectionZ = -100;
        }

        GameObject.Destroy(sphere2dGrid);
        GameObject.Destroy(sphere3dGrid);
        sphere2dGrid = DrawSphere2dGrid(selectionX, selectionY, selectionZ, playerOne);
        sphere3dGrid = DrawSphere3dGrid(selectionX, selectionY, selectionZ, playerOne);
    }

    private GameObject DrawSphere2dGrid(int x, int y, int z, bool playerOne)
    {
        if (x < 0 && y >= 0)
        {
            GameObject sp;
            sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sp.transform.position = GetTileCenter(x, y, z);
            sp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Renderer sphere_inner_color = sp.GetComponent<Renderer>();
            sphere_inner_color.material = (playerOne ? playerOneMaterial : playerTwoMaterial);

            return sp;
        }
        return null;
    }

    private GameObject DrawSphere3dGrid(int x, int y, int z, bool playerOne)
    {
        if (x < 0 && y >= 0)
        {
            GameObject sp;
            sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sp.transform.position = GetTileCenter(x + 5f, selectedGridNumber, y) - new Vector3(0f, 0.5f, -0.5f);
            sp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Renderer sphere_inner_color = sp.GetComponent<Renderer>();
            sphere_inner_color.material = (playerOne ? playerOneMaterial : playerTwoMaterial);

            return sp;
        }
        return null;
    }

    private Vector3 GetTileCenter(float x, float y, float z)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + (x == 0 ? 1 : System.Math.Sign(x)) * TILE_OFFSET;
        origin.y += (TILE_SIZE * y) + TILE_OFFSET;
        origin.z = z;
        return origin;
    }

    public void ChangeGridNumber(int gridNumber)
    {
        foreach (PlayerMoves pm in spheres2dGrid)
        {
            if (pm.grid == selectedGridNumber)
            {
                pm.go.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (pm.grid == gridNumber)
            {
                pm.go.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        selectedGridNumber = gridNumber;
    }

    public bool gameFinished()
    {

        int movePosX = selectionX + 5;
        int movePosY = selectedGridNumber;
        int movePosZ = selectionY;

        bool victory = false;

        //Tests for winning conditions on the X-axys on 2D
        int countMoves = 0;
        for (int a = 0; a < boardSize && !victory; a++)
        {
            //Debug.Log("Ta no grid:"+playerMoves[a, movePosY, movePosZ]+" "+ a + " " + movePosY + " " + movePosZ);
            if (playingBoard.simulatedBoard[a, movePosY, movePosZ, (playerOne? 1:2)])
            {
                countMoves++;
                if (countMoves == boardSize){ Debug.Log("ganhou em 1");  victory = true; }
            }
        }


        //Tests for winning conditions on the Z-axys on 2D
        countMoves = 0;
        for (int a = 0; a < boardSize && !victory; a++)
        {
            if (playingBoard.simulatedBoard[movePosX, movePosY, a, (playerOne ? 1 : 2)])
            {
                countMoves++;
                if (countMoves == boardSize) { Debug.Log("ganhou em 2"); victory = true; }
            }
        }

        //Tests for winning conditions on the diagonals on 2D
        countMoves = 0;
        for (int a = 0; a < boardSize && !victory; a++)
        {
            if (playingBoard.simulatedBoard[a, movePosY, a, (playerOne ? 1 : 2)] )
            {
                countMoves++;
                if (countMoves == boardSize) { Debug.Log("ganhou em 3"); victory = true; }
            }
        }
        countMoves = 0;
        for (int a = 0; a < boardSize && !victory; a++)
        {
            if (playingBoard.simulatedBoard[boardSize -1 - a, movePosY, boardSize - 1 - a, (playerOne ? 1 : 2)])
            {
                countMoves++;
                if (countMoves == boardSize) { Debug.Log("ganhou em 4"); victory = true; }
            }
        }

        //Tests for winning conditions on the column on 3D
        countMoves = 0;
        for (int a = 0; a < boardSize && !victory; a++)
        {
            if (playingBoard.simulatedBoard[movePosX, a, movePosZ, (playerOne ? 1 : 2)] )
            {
                countMoves++;
                if (countMoves == boardSize) { Debug.Log("ganhou em 5"); victory = true; }
            }
        }

        int[] countDiags = new int[4];

        //Tests for winning conditions on the column diagonal on 3D
        for (int a = 0; a < boardSize && !victory; a++)
        {
            for (int b = 0; b < boardSize; b++)
            {
                if (playingBoard.simulatedBoard[b, boardSize - 1 - b , a, (playerOne ? 1 : 2)])
                {
                    countDiags[0]++;                    
                    if (countDiags[0] == boardSize) { Debug.Log("ganhou em 6"); victory = true; }
                }
                if (playingBoard.simulatedBoard[boardSize - 1 - b, boardSize - 1 - b, a, (playerOne ? 1 : 2)] )
                {
                    countDiags[1]++;
                    if (countDiags[1] == boardSize) { Debug.Log("ganhou em 7"); victory = true; }
                }
                if (playingBoard.simulatedBoard[a, boardSize - 1 - b, b, (playerOne ? 1 : 2)] )
                {
                    countDiags[2]++;                    
                    if (countDiags[2] == boardSize) { Debug.Log("ganhou em 8"); victory = true; }
                }
                if (playingBoard.simulatedBoard[a, boardSize - 1 - b, boardSize - 1 - b, (playerOne ? 1 : 2)] )
                {
                    countDiags[3]++;
                    if (countDiags[3] == boardSize) { Debug.Log("ganhou em 9"); victory = true; }
                }
            }
            //Cleans the vector to be reused
            countDiags[0] = 0;
            countDiags[1] = 0;
            countDiags[2] = 0;
            countDiags[3] = 0;
        }

        //Cleans the vector to be reused in the columns
        countDiags[0] = 0;
        countDiags[1] = 0;
        countDiags[2] = 0;
        countDiags[3] = 0;

        //Tests for winning conditions on the diagonal on 3D
        int y = 3;
        for (int a = 0; a < boardSize && !victory; a++)
        {
                if (playingBoard.simulatedBoard[a, y , boardSize - 1 - a, (playerOne ? 1 : 2)] )
                {
                    countDiags[0]++;
                    Debug.Log(countDiags[0]);
                    if (countDiags[0] == boardSize) { Debug.Log("ganhou em 10"); victory = true; }
                }
                if (playingBoard.simulatedBoard[boardSize - 1 - a, y, boardSize - 1 - a, (playerOne ? 1 : 2)] )
                {
                    countDiags[1]++;
                    Debug.Log(countDiags[1]);
                    if (countDiags[1] == boardSize) { Debug.Log("ganhou em 11"); victory = true; }
                }
                if (playingBoard.simulatedBoard[a, y, a, (playerOne ? 1 : 2)] )
                {
                    countDiags[2]++;
                    Debug.Log(countDiags[3]);
                    if (countDiags[2] == boardSize) { Debug.Log("ganhou em 12"); victory = true; }
                }
                if (playingBoard.simulatedBoard[boardSize - 1 - a, y , a, (playerOne ? 1 : 2)] )
                {
                    countDiags[3]++;
                    Debug.Log(countDiags[3]);
                    if (countDiags[3] == boardSize) { Debug.Log("ganhou em 13"); victory = true; }
                }
            y--;
        }

        return victory;
    }


    public bool findMovement(int x, int y, int z, bool playerOne)
    {
        foreach (PlayerMoves pm in spheres3dGrid)
        {
            if (pm.posX == x && pm.posY == y && pm.posZ == z && pm.playerOne == playerOne)
            {
                return true;
            }
        }
        return false;
    }

    private void JogadaIA() {
        SimulatedBoard actualBoard = playingBoard;
        pc.PredictMovents(actualBoard,boardSize);

    }
}
