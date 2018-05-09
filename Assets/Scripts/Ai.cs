using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai {

    struct JOB
    {
        public uint jobId;
        public List<Move> moves;
        public Score window;
        public int deep;
        public Board board;

        public JOB(uint jobIdValue, List<Move> movesValue, Score windowValue, int deepValue, Board boardValue)
        {
            jobId = jobIdValue;
            moves = movesValue;
            window = windowValue;
            deep = deepValue;
            board = boardValue;
        }

        public JOB(uint jobIdValue, Move moveValue, Score windowValue, int deepValue, Board boardValue)
        {
            jobId = jobIdValue;
            moves = new List<Move>();
            moves.Add(moveValue);
            window = windowValue;
            deep = deepValue;
            board = boardValue;
        }
    };

    //Ai();
    //~Ai();

    public void AddWorker(ref Ai aiValue)
    {
        workers.Add(new AiWorker(ref aiValue));
    }

    public void SetBoard(Board boardValue) { board = boardValue; }
    public void SetMode(string modeValue) { mode = modeValue; }
    public void SetSearchScore(Score score) { searchScore = score; }
    public void SetLimit(bool limitValue) { limit = limitValue; }
    public void SetDebug(bool debugValue) { debug = debugValue; }
    public void Start(Board boardValue)
    {
        jobs.Clear();
        waits.Clear();
        results.Clear();
        isStop = false;
        board = boardValue;
        bestScore = new Score(int.MinValue);

        if (mode == "scout" || mode == "scouttest")
        {
            List<Move> moves = new List<Move>();
            moves.Add(GlobalMembers.MOVE_ZERO);
            bestScore = new Score(0);
            JOB job = new JOB( GetJobId(), GlobalMembers.MOVE_ZERO, searchScore.Negate(), 4, board );
            jobs.Add(job);
        }
        else if (mode == "minimax")
        {
            List<Move> moves = new List<Move>();
            moves.Add(GlobalMembers.MOVE_ZERO);
            JOB job = new JOB(GetJobId(), moves, GlobalMembers.SCORE_NONE, 4, board);
            jobs.Add(job);
        }
    }

    public void CallBack(string str)
    {
        Dictionary < string, string> strs = Json.fromJson(str);
        if (waits.ContainsKey(strs["jobid"]))
        {
            results.Add(str);
        }
    }

    public void GetJob(out string job)
    {
        if (isStop)
        {
            job = "stop";
            return;
        }

        if (0 < jobs.Count)
        {
            JOB jobStruct = jobs[0];
            string jobIdString = jobStruct.jobId.ToString();
            job = "jobid:" + jobIdString;
            job += ",window:" + jobStruct.window.toJson();
            job += ",deep:" + jobStruct.deep.ToString();
            if (limit)
            {
                job += ",limit:true";
            }
            else
            {
                job += ",limit:false";
            }
            if (debug)
            {
                job += ",debug:true";
            }
            else
            {
                job += ",debug:false";
            }
            job += ",board:" + jobStruct.board.BoardToString();
            waits[jobIdString] = jobStruct.moves;
            jobs.RemoveAt(0);
        }
        else
        {
            job = "empty";
        }
    }
    public bool IsAlive(string jobId)
    {
        return waits.ContainsKey(jobId);
    }

    public void GetResult(out Score scoreValue)
    {
        scoreValue = bestScore;
    }

    public bool Tick()
    {
        // 結果を回収
        foreach( AiWorker worker in workers )
        {
            worker.Search();
        }

        while (0 < results.Count)
        {
            Dictionary < string, string> strs = Json.fromJson(results[0]);
            string jobId = strs["jobid"];
            string scoreString = strs["score"];
            string countString = strs["count"];
            Score score = new Score(scoreString);
            if (mode == "scout")
            {
                if (debug)
                {
                    Debug.Log( "score is " + (string)score + " best score is " + (string)bestScore );
                }
                Debug.Log("score is " + (string)score + " best score is " + (string)bestScore);
                if (bestScore == score)
                {
                    bestScore = score.Negate();
                }
                else
                {
                    bestScore = score;
                    JOB job = new JOB(GetJobId(), GlobalMembers.MOVE_ZERO, bestScore.Negate(), 4, board);
                    jobs.Add(job);
                }
            }
            else if (mode == "scouttest")
            {
                if (debug)
                {
                     Debug.Log( "score is " + (string)score + " best score is " + (string)bestScore );
                }
                bestScore = score.Negate();
            }
            else if (mode == "minimax")
            {
                if (debug)
                {
                    Debug.Log( "score is " + (string)score + " best score is " + (string)bestScore );
                }
                if (bestScore < score.Negate())
                {
                    bestScore = score.Negate();
                }
            }

            waits.Remove(jobId);
            results.RemoveAt(0);
        }

        if (0 < jobs.Count || 0 < waits.Count || 0 < results.Count)
        {
            return false;
        }

        if (debug)
        {
            Debug.Log( "-> best score is " + (string)bestScore );
        }

        return true;
    }

    //public void Stop();



    private Board board;

    private string mode = "minimax";
    private Score searchScore = new Score(int.MinValue);
    private bool limit = false;
    private bool debug = true;

    private Score bestScore = new Score(GlobalMembers.SCORE_NONE);

    private List<AiWorker> workers = new List<AiWorker>();

    private List<JOB> jobs = new List<JOB>();
    private Dictionary<string, List<Move>> waits = new Dictionary<string, List<Move>>();
    private List<string> results = new List<string>();

    private uint jobId = 0;
    private uint GetJobId() { return jobId++; }

    private bool isStop = false;
}
