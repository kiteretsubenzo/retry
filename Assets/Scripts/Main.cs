using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ai = new Ai();
        ai.AddWorker(ref ai);

        StartCoroutine("Search");
    }
	
	// Update is called once per frame
	void Update () {
        
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
    }

    private Ai ai;
}
