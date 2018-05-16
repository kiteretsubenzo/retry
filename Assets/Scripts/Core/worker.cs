//========================================================================
// This conversion was produced by the Free Edition of
// C++ to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//class Ai;

public abstract class Worker
{
	public Board board = null;

	public abstract void CallBack(string str);
	public abstract void GetJob(out string job);
	public abstract bool IsAlive(string jobId);

	public bool Search()
	{
		switch (state)
		{
		case STATE.IDLE:
		{
			string job;
			GetJob(out job);
			if (job == "empty")
			{
				return false;
			}
			if (job == "stop")
			{
				return true;
			}

			SearchInit(job);

			state = STATE.SEARCH;
		}
		break;

		case STATE.SEARCH:
			//jobが生きているか確認
			if (IsAlive(jobId) == false)
			{
				state = STATE.IDLE;
				break;
			}

			if (SearchImplementation() == true)
			{
				state = STATE.IDLE;
			}
			break;

		default:
			break;
		}

		return false;
	}


	private const int DEEP_MAX = 64;
	private MoveList moveListTmp = new MoveList();
	private Score scoreTmp = new Score();

	private class Node
	{
		public MoveList moves = new MoveList();
		public Score score = new Score();

		public Node()
		{
			moves.clear();
			score.clear();
		}

		public void setMoves(MoveList movesValue)
		{
			moves.copy(movesValue);
			score.clear();
		}
	}

	private class NodeStack
	{
		public NodeStack()
		{
            for( int i=0; i<nodeStack.Length; i++ )
            {
                nodeStack[i] = new Node();
            }
        }

        public void clear()
		{
			index = -1;
		}

		public void push_back(Node node)
		{
			index++;
			nodeStack[index] = node;
		}

		public void push_back(MoveList moveList)
		{
			index++;
			nodeStack[index].setMoves(moveList);
		}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint size() const
		public uint size()
		{
			return (uint)(index + 1);
		}

		public Node front()
		{
			return nodeStack[index];
		}

		public Node parent()
		{
			return nodeStack[index - 1];
		}

		public void pop_back()
		{
			index--;
		}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void GetHistory(ScoreMoveList &moveList) const
		public void GetHistory(ScoreMoveList moveList)
		{
			moveList.clear();
			for (uint i = 0; i < size(); i++)
			{
				if (0 < nodeStack[i].moves.size())
				{
					moveList.push_back(nodeStack[i].moves.front());
				}
			}
		}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void debugPrint() const
		public void debugPrint()
		{
			//std::cout << '\r' << std::flush;
			string str = "";
			for (uint i = 0; i < size(); i++)
			{
				Node ite = nodeStack[i];
				if (0 < ite.moves.size())
				{
					str += ":" + ite.moves.front().DebugString() + "(" + (string)(ite.score) + ")";
				}
				else
				{
					str += ":EMPTY(" + (string)(ite.score) + ")";
				}
			}
            Debug.Log(str);
        }

        private Node[] nodeStack = new Node[64];
		private int index = -1;
	}

	private enum STATE
	{
		IDLE,
		SEARCH
	}

	private STATE state = STATE.IDLE;

	private string jobId = "";
	private bool debug = true;
	private Score window = new Score();
	private Score windowNega = new Score();
	private uint deep = 0;
	private bool limit = false;

	//std::list<Node> nodeStack;
	private NodeStack nodeStack = new NodeStack();

	private void SearchInit(string job)
	{
		Dictionary<string, string> @params = Json.fromJson(job);
		string windowStr = @params["window"];
		string limitStr = @params["limit"];
		string deepStr = @params["deep"];
		string debugStr = @params["debug"];
		string boardStr = @params["board"];

		jobId = @params["jobid"];

		debug = (debugStr == "true");

		board.Init(boardStr);
		if (debug)
		{
			//board->PrintBoard();
		}

		if (windowStr != "")
		{
			window = new Score(windowStr);
		}
		else
		{
			window.clear();
		}
		windowNega = window.Negate();

		deep = uint.Parse(deepStr);
		limit = (limitStr == "true");

		nodeStack.clear();
		// ルート
		nodeStack.push_back(new Node());
		// 自分
		MoveList moveList = new MoveList();
		board.GetMoveList(moveList);
		nodeStack.push_back(moveList);
	}

	private bool SearchImplementation()
	{
		for (int i = 0; i < 0xff; i++)
		{
			bool debugPrint = true && debug;

			// forward
			while (true)
			{
				if (debugPrint)
				{
					nodeStack.debugPrint();
				}

				// 子ノードを取得
				Node childItr = nodeStack.front();

				// 盤面を進める
				board.Forward(childItr.moves.front());

				// 着手を取得
				board.GetMoveList(moveListTmp);

				// 新しい盤面に着手が無かったら勝負あり
				if (moveListTmp.empty())
				{
					childItr.score.score = Score.SCORE_WIN;
					nodeStack.GetHistory(childItr.score.moveList);
					break;
				}

				if (deep <= nodeStack.size())
				{
					// 新しい子が末端だったら追加せずに評価

					// 親ノードに評価をマージ
					if (limit == false || windowNega <= childItr.score)
					{
						scoreTmp.setScore(board.GetEvaluate(moveListTmp));
						childItr.score = Score.Min(childItr.score, scoreTmp.Negate());
					}

					nodeStack.GetHistory(childItr.score.moveList);

					break;
				}

				// 子供を追加してもう一回
				nodeStack.push_back(moveListTmp);
			}

			// back
			while (true)
			{
				if (debugPrint)
				{
					nodeStack.debugPrint();
				}

				// 子ノードを取得
				Node childItr = nodeStack.front();

				// 親ノードに得点をマージ
				if (2 <= nodeStack.size())
				{
					Node parentItr = nodeStack.parent();
					if (limit == true && windowNega <= parentItr.score)
					{
						parentItr.score = childItr.score.Negate();
					}
					else
					{
						parentItr.score = Score.Min(parentItr.score, childItr.score.Negate());
					}
				}

				// 子ノードの着手を戻す
				board.Back(childItr.moves.front());

				// スコアがwindowの外側だったら終わり
				if (window.score != Score.SCORE_UNVALUED && (limit == false || childItr.score < windowNega))
				{
					if ((nodeStack.size() & 0x1) == 0)
					{
						if (window < childItr.score)
						{
							childItr.moves.clear();
						}
					}
					else
					{
						if (windowNega < childItr.score)
						{
							childItr.moves.clear();
						}
					}
				}

				// 次の指し手を取得
				if (1 < childItr.moves.size())
				{
					childItr.moves.pop_front();
					childItr.score.clear();
					break;
				}

				// 次の指し手が無いので今のノードは終わり
				nodeStack.pop_back();
				//std::cout << "end" << nodeStack.size() << std::endl;

				// ルートノードなので終わり
				if (nodeStack.size() <= 1)
				{
					CallBack("jobid:" + jobId + ",score:" + nodeStack.front().score.toJson() + ",count:" + Convert.ToString(i));
					return true;
				}
			}
		}

		// いったんお返し
		return false;
	}
}
