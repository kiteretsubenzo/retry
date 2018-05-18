using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using uchar = System.SByte;
using Pawn = System.SByte;
using Player = System.SByte;

public class Main : MonoBehaviour {

    public GameObject[] Pawns;
    public GameObject CellPrefab;
    public GameObject CellsParent;
    public GameObject PawnsParent;
    public GameObject UpgradeObject;
    public GameObject ReservesParent;
    public GameObject ReservesCountPrefab;

	// Use this for initialization
	void Start () {
        ai = new Ai();
        ai.AddWorker(ref ai);
        /*
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
        */
        /*
        string str = @"h16 y02 e04 g04 u02 r01 k03
 . . . . .^R . .y_
 . . . . . . .o_h_
 . . . . . . . . .
 . . . . . . .h_ .
 . . . . . . .^h .
 . . . . . .^y . .
 . . . . . . . . .
 . . . . . . . . .
 . . . . . . . . .
h00 y00 e00 g00 u00 r00 k01
first";
        */
        string str = @"h16 y02 e04 g04 u02 r01 k03
 . . . . . . .^Ry_
 . . . . . . . .h_
 . . . . . . . .o_
 . . . . . . .h_ .
 . . . . . . .^h .
 . . . . . .^y . .
 . . . . . . . . .
 . . . . . . . . .
 . . . . . . . . .
h00 y00 e00 g00 u00 r00 k01
first";

        Debug.Log(str);
        board = new Board();
        board.Init(str);

        for(int j=1; j<= BoardDef.HEIGHT; j++)
        {
            for(int i=1; i<= BoardDef.WIDTH; i++)
            {
                float x = cellSize * (i-1);
                float z = cellSize * -(j-1);
                x -= cellSize * BoardDef.WIDTH / 2.0f - cellSize / 2.0f;
                z += cellSize * BoardDef.HEIGHT / 2.0f - cellSize / 2.0f;
                GameObject cell = Instantiate(CellPrefab, new Vector3(x, 0.01f, z), Quaternion.identity) as GameObject;
                cell.name = "Cell(" + i + "," + j + ")";
                cell.transform.parent = CellsParent.transform;
            }
        }

        for (int j = 1; j <= BoardDef.HEIGHT; j++)
        {
            for (int i = 1; i <= BoardDef.WIDTH; i++)
            {
                CELL cell = board.GetCell(i, j);
                if( cell.player != PlayerDef.FIRST && cell.player != PlayerDef.SECOND )
                {
                    continue;
                }
                CreatePawn(cell.player, cell.pawn, i, j);
            }
        }

        Vector3 target;

        for (int i = 0; i < PawnDef.CAPTURE_MAX; i++)
        {
            float x = cellSize * (i + BoardDef.WIDTH - PawnDef.CAPTURE_MAX);
            float z = cellSize * -(BoardDef.HEIGHT + 1.5f - 1);
            x -= cellSize * BoardDef.WIDTH / 2.0f - cellSize / 2.0f;
            z += cellSize * BoardDef.HEIGHT / 2.0f - cellSize / 2.0f;
            GameObject cell = Instantiate(CellPrefab, new Vector3(x, 0.01f, z), Quaternion.identity) as GameObject;
            cell.name = "Reserve(" + PlayerDef.FIRST + "," + i + ")";

            cell.transform.parent = ReservesParent.transform;
            GameObject count = Instantiate(ReservesCountPrefab, new Vector3(x, 0.01f, z), Quaternion.identity) as GameObject;
            count.transform.parent = ReservesParent.transform;
            target = Camera.main.transform.position;
            target.x = count.transform.position.x;
            count.transform.LookAt(target);
            count.name = "ReservesCount(0," + i + ")";
            count.SetActive(true);

            CreateReserve(PlayerDef.FIRST, (Pawn)i);
            SetReserveCount(PlayerDef.FIRST, (Pawn)i, board.GetReserve(PlayerDef.FIRST, (Pawn)i));
        }

        for (int i = 0; i < PawnDef.CAPTURE_MAX; i++)
        {
            float x = cellSize * i;
            float z = cellSize * -(-0.5f - 1);
            x -= cellSize * BoardDef.WIDTH / 2.0f - cellSize / 2.0f;
            z += cellSize * BoardDef.HEIGHT / 2.0f - cellSize / 2.0f;
            GameObject cell = Instantiate(CellPrefab, new Vector3(x, 0.01f, z), Quaternion.identity) as GameObject;
            cell.transform.parent = ReservesParent.transform;
            cell.name = "Reserve(" + PlayerDef.SECOND + "," + i + ")";

            GameObject count = Instantiate(ReservesCountPrefab, new Vector3(x, 0.01f, z), Quaternion.identity) as GameObject;
            count.transform.parent = ReservesParent.transform;
            target = Camera.main.transform.position;
            target.x = count.transform.position.x;
            count.transform.LookAt(target);
            count.name = "ReservesCount(1," + i + ")";
            count.SetActive(true);

            CreateReserve(PlayerDef.SECOND, (Pawn)i);
            SetReserveCount(PlayerDef.SECOND, (Pawn)i, board.GetReserve(PlayerDef.SECOND, (Pawn)i));
        }

        target = Camera.main.transform.position;
        target.x = UpgradeObject.transform.position.x;
        UpgradeObject.transform.LookAt(target);

        ResetHighlight();

        //StartCoroutine("Search");
    }
	
	// Update is called once per frame
	void Update () {
        //UpgradeObject.transform.LookAt(camera.transform.position);
    }

    public void ClickCell(GameObject target)
    {
        Debug.Log("click " + target.name);
        //GameObject pawn = CellToPawn(target);
        if (0 <= target.name.IndexOf("Reserve"))
        {
            Player player;
            Pawn pawn;
            CellToReserve(target, out player, out pawn);
            SelectReserve(player, pawn);
        }
        else
        {
            int x, y;
            CellToIndex(target, out x, out y);
            SelectPawn(x, y);
        }
    }

    public void ClickPawn(GameObject target)
    {
        Debug.Log("click " + target.name);
        GameObject cell = PawnToCell(target);
        if (0 <= cell.name.IndexOf("Reserve"))
        {
            Player player;
            Pawn pawn;
            CellToReserve(cell, out player, out pawn);
            SelectReserve(player, pawn);
        }
        else
        {
            int x, y;
            CellToIndex(cell, out x, out y);
            SelectPawn(x, y);
        }
    }

    public void ClickUp()
    {
        Debug.Log("Up");
        selectMove.upgrade = true;
        BoardMove(selectMove);
        TurnEnd();
    }

    public void ClickStay()
    {
        Debug.Log("Stay");
        selectMove.upgrade = false;
        BoardMove(selectMove);
        TurnEnd();
    }

    private void CreatePawn(Player player, Pawn pawn, int i, int j)
    {
        Pawn down = Board.Down(pawn);
        bool upgrade = Board.IsUpgrade(pawn);

        GameObject cell = IndexToCell(i, j);
        float x = cell.transform.position.x;
        float z = cell.transform.position.z;

        float yangle = 0.0f;
        float zangle = 0.0f;
        if (player == PlayerDef.SECOND)
        {
            yangle = 180.0f;
        }

        if (upgrade)
        {
            zangle = 180.0f;
        }

        Quaternion angle = Quaternion.identity;
        angle.eulerAngles = new Vector3(0, yangle, zangle);

        GameObject pawnObject = Instantiate(Pawns[down], new Vector3(x, 0.025f, z), angle) as GameObject; ;
        pawnObject.transform.parent = PawnsParent.transform;
    }

    private void CreateReserve(Player player, Pawn pawn)
    {
        Pawn down = Board.Down(pawn);

        GameObject cell = GetReserve(player, pawn);
        float x = cell.transform.position.x;
        float z = cell.transform.position.z;

        float yangle = 0.0f;
        float zangle = 0.0f;
        if (player == PlayerDef.SECOND)
        {
            yangle = 180.0f;
        }

        Quaternion angle = Quaternion.identity;
        angle.eulerAngles = new Vector3(0, yangle, zangle);

        GameObject pawnObject = Instantiate(Pawns[down], new Vector3(x, 0.025f, z), angle) as GameObject; ;
        pawnObject.transform.parent = PawnsParent.transform;
    }

    private void SetReserveCount(Player player, Pawn pawn, int count)
    {
        GameObject reservesCount = GetReserveCount(player, pawn);
        GameObject text = reservesCount.transform.Find("Text").gameObject;
        TextMesh textMesh = text.GetComponent<TextMesh>();
        textMesh.text = count.ToString();
    }

    private void SelectPawn(int x, int y)
    {
        CELL cell = board.GetCell(x, y);
        if (step == "idle")
        {
            board.GetMoveList(moveListTmp);
            List<Move> moveList = new List<Move>();

            while (0 < moveListTmp.size())
            {
                Move move = moveListTmp.front();

                if (move.from.x == x && move.from.y == y)
                {
                    moveList.Add(move);
                }

                moveListTmp.pop_front();
            }

            if(moveList.Count == 0)
            {
                return;
            }

            selectMove.from.x = (uchar)x;
            selectMove.from.y = (uchar)y;
            selectMove.from.pawn = cell.pawn;

            foreach (Move moveTmp in moveList)
            {
                GameObject highlight = IndexToCell(moveTmp.to.x, moveTmp.to.y);
                highlight.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }

            step = "select";
        }
        else if (step == "select")
        {
            board.GetMoveList(moveListTmp);
            List<Move> moveList = new List<Move>();

            if (selectMove.reserve == PawnDef.NONE)
            {
                while (0 < moveListTmp.size())
                {
                    Move move = moveListTmp.front();

                    if (move.from.x == selectMove.from.x && move.from.y == selectMove.from.y && move.to.x == x && move.to.y == y)
                    {
                        moveList.Add(move);
                    }

                    moveListTmp.pop_front();
                }
            }
            else
            {
                while (0 < moveListTmp.size())
                {
                    Move move = moveListTmp.front();

                    if (move.reserve == selectMove.reserve && move.to.x == x && move.to.y == y)
                    {
                        moveList.Add(move);
                    }

                    moveListTmp.pop_front();
                }
            }

            selectMove.to.x = (uchar)x;
            selectMove.to.y = (uchar)y;
            selectMove.to.pawn = cell.pawn;

            if (1 < moveList.Count)
            {
                UpgradeObject.SetActive(true);
                step = "upgrade";
                return;
            }

            if (moveList.Count == 0)
            {
                // 選択キャンセル
                selectMove = new Move(GlobalMembers.MOVE_ZERO);
                ResetHighlight();
                step = "idle";
                return;
            }

            if (moveList[0].upgrade == true)
            {
                int top = 0;
                if (board.GetPlayer() == PlayerDef.SECOND)
                {
                    top = BoardDef.HEIGHT;
                }
                if( (selectMove.from.pawn == PawnDef.HU && selectMove.to.y != top) ||
                    (selectMove.from.pawn == PawnDef.KYOH && selectMove.to.y != top) ||
                    selectMove.from.pawn == PawnDef.KAKU || selectMove.from.pawn == PawnDef.HI
                    )
                {
                    // 成らない事もできる
                    UpgradeObject.SetActive(true);
                    step = "upgrade";
                    return;
                }
            }

            Debug.Log("move");
            selectMove.upgrade = moveList[0].upgrade;
            BoardMove(selectMove);
            TurnEnd();
        }
    }

    private void SelectReserve(Player player, Pawn pawn)
    {
        if (step == "idle")
        {
            if (player != board.GetPlayer())
            {
                return;
            }

            board.GetMoveList(moveListTmp);
            List<Move> moveList = new List<Move>();

            while (0 < moveListTmp.size())
            {
                Move move = moveListTmp.front();

                if (move.reserve == pawn)
                {
                    moveList.Add(move);
                }

                moveListTmp.pop_front();
            }

            if( moveList.Count == 0 )
            {
                return;
            }

            selectMove.reserve = pawn;

            foreach (Move moveTmp in moveList)
            {
                GameObject highlight = IndexToCell(moveTmp.to.x, moveTmp.to.y);
                highlight.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }

            step = "select";
        }
        else if (step == "select")
        {
            selectMove = new Move(GlobalMembers.MOVE_ZERO);
            ResetHighlight();
            step = "idle";
        }
    }

    private void ResetHighlight()
    {
        foreach (Transform cell in CellsParent.transform)
        {
            cell.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.05f);
        }
    }

    private GameObject CellToPawn(GameObject cell)
    {
        Ray ray = new Ray(cell.transform.position - new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 2.0f, LayerMask.GetMask("Pawn"));
        return hit.transform.gameObject;
    }

    private GameObject PawnToCell(GameObject pawn)
    {
        Ray ray = new Ray(pawn.transform.position + new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f));
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 2.0f, LayerMask.GetMask("Cell"));
        return hit.transform.gameObject;
    }
    
    private void CellToIndex(GameObject cell, out int x, out int y)
    {
        string name = cell.name;
        name = name.Replace("Cell(", "");
        name = name.Replace(")", "");
        string[] splited = name.Split(',');
        x = int.Parse(splited[0]);
        y = int.Parse(splited[1]);
    }

    private void CellToReserve(GameObject cell, out Player player, out Pawn pawn)
    {
        string name = cell.name;
        name = name.Replace("Reserve(", "");
        name = name.Replace(")", "");
        string[] splited = name.Split(',');
        player = (Player)int.Parse(splited[0]);
        pawn = (Pawn)int.Parse(splited[1]);
    }

    private GameObject IndexToCell(int x, int y)
    {
        Transform cell = CellsParent.transform.Find("Cell(" + x + "," + y + ")");
        return cell.gameObject;
    }

    private GameObject GetReserve(Player player, Pawn pawn)
    {
        Transform reserve = ReservesParent.transform.Find("Reserve(" + player + "," + pawn + ")");
        return reserve.gameObject;
    }

    private GameObject GetReserveCount(Player player, Pawn pawn)
    {
        Transform reservesCount = ReservesParent.transform.Find("ReservesCount(" + player + "," + pawn + ")");
        return reservesCount.gameObject;
    }

    private void BoardMove(Move move)
    {
        if( move.from.pawn != PawnDef.NONE )
        {
            GameObject cell = IndexToCell(move.from.x, move.from.y);
            GameObject pawn = CellToPawn(cell);
            Destroy(pawn);
        }

        if (move.to.pawn != PawnDef.NONE)
        {
            GameObject cell = IndexToCell(move.to.x, move.to.y);
            GameObject pawn = CellToPawn(cell);
            Destroy(pawn);
        }

        if (move.reserve != PawnDef.NONE)
        {
            CreatePawn(board.GetPlayer(), move.reserve, move.to.x, move.to.y);
        }
        else
        {
            Pawn pawnTmp = move.from.pawn;
            if (move.upgrade == true)
            {
                Board.Upgrade(ref pawnTmp);
            }
            CreatePawn(board.GetPlayer(), pawnTmp, move.to.x, move.to.y);
        }

        board.Forward(move);

        for (int i = 0; i < PawnDef.CAPTURE_MAX; i++)
        {
            SetReserveCount(PlayerDef.FIRST, (Pawn)i, board.GetReserve(PlayerDef.FIRST, (Pawn)i));
            SetReserveCount(PlayerDef.SECOND, (Pawn)i, board.GetReserve(PlayerDef.SECOND, (Pawn)i));
        }

        Debug.Log(board.GetPlayer());
        board.GetMoveList(moveListTmp);
        if( moveListTmp.size() == 0 )
        {
            Debug.Log("gameset");
        }
    }

    private void TurnEnd()
    {
        selectMove = new Move(GlobalMembers.MOVE_ZERO);
        UpgradeObject.SetActive(false);
        ResetHighlight();

        if (board.GetPlayer() == PlayerDef.SECOND)
        {
            step = "ai";
            StartCoroutine("Search");
        }
        else
        {
            step = "idle";
        }
    }

    private IEnumerator Search()
    {
        ai.SetMode("scouttest");
        ai.SetDebug(false);

        Score aiScore = new Score();
        //ai.SetSearchScore(Score("{score:99999,moves:[n0701o0700Rf,k0000n0601nf]}"));
        ai.SetSearchScore(new Score(Score.SCORE_WIN));
        //ai.SetSearchScore(new Score());
        ai.SetLimit(0);
        ai.Start(board);

        while (ai.Tick() == false)
        {
            yield return null;
        }

        ai.GetResult(out aiScore);
        aiScore = aiScore.Negate();
        Debug.Log((string)aiScore);
        Debug.Log(aiScore.moveList.DebugString());

        Move move = aiScore.moveList.front();
        BoardMove(move);
        TurnEnd();
    }

    private Board board;
    private Ai ai;
    private Move selectMove = new Move(GlobalMembers.MOVE_ZERO);
    private string step = "idle";
    private MoveList moveListTmp = new MoveList();
    private float cellSize = 0.2f;
}
