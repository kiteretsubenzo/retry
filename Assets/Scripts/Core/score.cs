//========================================================================
// This conversion was produced by the Free Edition of
// C++ to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;

public class ScoreMoveList
{
	public ScoreMoveList()
	{
	}

	public void clear()
	{
		index = -1;
	}

	public void push_back(Move move)
	{
		index++;
		moveList[index] = move;
	}

    public void copy(ScoreMoveList moveListValue)
    {
        for(int i=0; i<moveListValue.size(); i++)
        {
            moveList[i] = moveListValue.moveList[i];
        }
        index = moveListValue.index;
    }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint size() const
	public uint size()
	{
		return (uint)(index + 1);
	}

	public void pop_back()
	{
		index--;
	}

	public Move front()
	{
		return moveList[0];
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string DebugString() const
	public string DebugString()
	{
		string str = "";
		for (uint i = 0; i < size(); i++)
		{
			str += moveList[i].DebugString() + " : ";
		}
		return str;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator <(const ScoreMoveList& rhs) const
	public static bool operator < (ScoreMoveList ImpliedObject, ScoreMoveList rhs)
	{
		if (ImpliedObject.size() > rhs.size())
		{
			return true;
		}
		if (ImpliedObject.size() < rhs.size())
		{
			return false;
		}

		for (uint i = 0; i < ImpliedObject.size(); i++)
		{
			if (ImpliedObject.moveList[i] < rhs.moveList[i])
			{
				return true;
			}
		}
		return false;
	}

    public static bool operator >(ScoreMoveList ImpliedObject, ScoreMoveList rhs)
    {
        return !(ImpliedObject < rhs) && ImpliedObject != rhs;
    }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const ScoreMoveList& rhs) const
    public static bool operator == (ScoreMoveList ImpliedObject, ScoreMoveList rhs)
	{
		if (ImpliedObject.index != rhs.index)
		{
			return false;
		}

		for (int i = 0; i < ImpliedObject.index; i++)
		{
			if (ImpliedObject.moveList[i] != rhs.moveList[i])
			{
				return false;
			}
		}
		return true;
	}

    public static bool operator !=(ScoreMoveList ImpliedObject, ScoreMoveList rhs)
    {
        return !(ImpliedObject == rhs);
    }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string toJson() const
    public string toJson()
	{
		string str = "moves:[";
		if (0 < size())
		{
			str += (string)(moveList[0]);
			for (uint i = 1; i < size(); i++)
			{
				str += "," + (string)(moveList[i]);
			}
		}
		str += "]";
		return str;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator string() const
	public static implicit operator string(ScoreMoveList ImpliedObject)
	{
		string str = "(";
		if (0 < ImpliedObject.size())
		{
			str += (string)(ImpliedObject.moveList[0]);
			for (uint i = 1; i < ImpliedObject.size(); i++)
			{
				str += "," + (string)(ImpliedObject.moveList[i]);
			}
		}
		str += ")";
		return str;
	}

	private Move[] moveList = new Move[64];
	private int index = -1;
}

public class Score
{
	public const int SCORE_WIN = 99999;
	public const int SCORE_UNVALUED = (int.MaxValue - 1);

	public int score;
	public ScoreMoveList moveList = new ScoreMoveList();

    public Score(Score scoreValue)
    {
        score = scoreValue.score;
        //moveList = scoreValue.moveList;
        moveList.copy(scoreValue.moveList);
    }

    public Score(int scoreValue)
	{
		score = scoreValue;
	}

	public Score(int scoreValue, ScoreMoveList moveListValue)
	{
		score = scoreValue;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: moveList = moveListValue;
		//moveList = moveListValue;
        moveList.copy(moveListValue);
    }

	public Score(string json)
	{
		Dictionary<string, string> strs = Json.fromJson(json);
		score = Convert.ToInt32(strs["score"]);
		LinkedList<string> moves = Json.fromJsonArray(strs["moves"]);

        LinkedListNode<string> ite = moves.First;
        while( ite != null )
		{
//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			moveList.push_back(new Move(ite.Value));
            ite = ite.Next;
		}
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string toJson() const
	public string toJson()
	{
		string str = "{";
		str += "score:" + Convert.ToString(score);
		str += "," + moveList.toJson();
		return str + "}";
	}

	public static Score Min(Score lhs, Score rhs)
	{
		if (lhs.score != SCORE_UNVALUED && rhs.score != SCORE_UNVALUED)
		{
			if (lhs < rhs)
			{
				return lhs;
			}
			return rhs;
		}

		if (lhs.score == SCORE_UNVALUED)
		{
			return rhs;
		}

		return lhs;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Score Negate() const
	public Score Negate()
	{
		if (score == SCORE_UNVALUED)
		{
			return this;
		}
		return new Score(-score, moveList);
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const Score& rhs) const
	public static bool operator == (Score ImpliedObject, Score rhs)
	{
		return (ImpliedObject.score == rhs.score && ImpliedObject.moveList == rhs.moveList);
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator !=(const Score& rhs) const
	public static bool operator != (Score ImpliedObject, Score rhs)
	{
		return !(ImpliedObject == rhs);
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator <(const Score& rhs) const
	public static bool operator < (Score ImpliedObject, Score rhs)
	{
		if (ImpliedObject.score < rhs.score)
		{
			return true;
		}
		if (ImpliedObject.score > rhs.score)
		{
			return false;
		}
		return (ImpliedObject.moveList < rhs.moveList);
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator >(const Score& rhs) const
	public static bool operator > (Score ImpliedObject, Score rhs)
	{
		return !(ImpliedObject < rhs) && ImpliedObject != rhs;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator <=(const Score& rhs) const
	public static bool operator <= (Score ImpliedObject, Score rhs)
	{
		return ImpliedObject < rhs || ImpliedObject == rhs;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator >=(const Score& rhs) const
	public static bool operator >= (Score ImpliedObject, Score rhs)
	{
		return ImpliedObject > rhs || ImpliedObject == rhs;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator string() const
	public static implicit operator string(Score ImpliedObject)
	{
		string str;
		str = Convert.ToString(ImpliedObject.score);
		str += (string)(ImpliedObject.moveList);
		return str;
	}
}