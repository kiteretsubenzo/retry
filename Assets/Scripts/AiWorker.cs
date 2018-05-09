using System.Collections;
using System.Collections.Generic;

public class AiWorker : Worker {

    public AiWorker(ref Ai aiValue) : base()
    {
        ai = aiValue;
        board = new Board();
    }

    public override void CallBack(string str)
    {
        ai.CallBack(str);
    }

    public override void GetJob(out string job)
    {
        ai.GetJob(out job);
    }

    public override bool IsAlive(string jobId)
    {
        return ai.IsAlive(jobId);
    }

    private Ai ai = null;
}
