using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour {

    public GameObject[] Pawns;
    public GameObject CellPrefab;
    public GameObject CellsParent;

    public const int BOARD_WIDTH = 9;
    public const int BOARD_HEIGHT = 9;

	// Use this for initialization
	void Start () {
        ai = new Ai();
        ai.AddWorker(ref ai);

        float cellSize = 0.2f;
        for(int j=0; j<BOARD_HEIGHT; j++)
        {
            for(int i=0; i<BOARD_WIDTH; i++)
            {
                float x = cellSize * i;
                float z = cellSize * -j;
                x -= cellSize * BOARD_WIDTH / 2.0f - cellSize / 2.0f;
                z += cellSize * BOARD_HEIGHT / 2.0f - cellSize / 2.0f;
                GameObject cell = Instantiate(CellPrefab, new Vector3(x, 0.01f, z), Quaternion.identity) as GameObject;
                cell.name = "Cell(" + (i+1) + "," + (j+1) + ")";
                cell.transform.parent = CellsParent.transform;
            }
        }

        //StartCoroutine("Search");
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void ClickCell(GameObject target)
    {
        Debug.Log("click " + target.name);
    }

    private IEnumerator Search()
    {
        string str = @"h18 y04 e04 g04 u00 r00 k03
 . . . . . . . . .
 . . . . . .r_ . .
 . . . . . .U_ .o_
 . . . . . . .^r .
 . . . . . .^U . .
 . . . . . . . . .
 . . . . . . . . .
 . . . . . . . . .
 . . . . . . . . .
h00 y00 e00 g00 u00 r00 k01
first";
        Debug.Log(str);
        Board board = new Board();
        board.Init(str);

        Score aiScore = new Score(GlobalMembers.SCORE_NONE);

        ai.SetMode("scouttest");
        ai.SetSearchScore(new Score(-Score.SCORE_WIN));
        //ai.SetSearchScore(Score("{score:99999,moves:[n0701o0700Rf,k0000n0601nf]}"));
        //ai.SetLimit(true);
        ai.SetDebug(false);

        ai.Start(board);

        while (ai.Tick() == false)
        {
            yield return null;
        }

        ai.GetResult(out aiScore);
        Debug.Log((string)aiScore);
        Debug.Log(aiScore.moveList.DebugString());
    }

    private Ai ai;
}
