//========================================================================
// This conversion was produced by the Free Edition of
// C++ to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;

public class Score
{
	public const int SCORE_WIN = 99999;
	public const int SCORE_UNVALUED = (int.MaxValue - 1);

	public int score;
	// TODO: �Œ�o�b�t�@������
	public LinkedList<Move> moveList = new LinkedList<Move>();

    public Score(Score scoreValue)
    {
        score = scoreValue.score;
        moveList = new LinkedList<Move>(scoreValue.moveList);
    }

    public Score(int scoreValue)
	{
		score = scoreValue;
        moveList.Clear();
    }

	public Score(int scoreValue, LinkedList<Move> moveListValue)
	{
		score = scoreValue;
		moveList = new LinkedList<Move>(moveListValue);
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
			moveList.AddLast(new Move(ite.Value));
            ite = ite.Next;
		}
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string toJson() const
	public string toJson()
	{
		string str = "{";
		str += "score:" + Convert.ToString(score);
		if (0 < moveList.Count)
		{
			str += ",moves:[";
			LinkedListNode<Move> ite = moveList.First;
//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			str += (string)(ite.Value);
//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			ite = ite.Next;
			while (ite != null)
			{
				str += "," + (string)ite.Value;
                ite = ite.Next;
			}
			str += "]";
		}
		else
		{
			str += ",moves:[]";
		}
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
		if (ImpliedObject.moveList.Count > rhs.moveList.Count)
		{
			return true;
		}
		if (ImpliedObject.moveList.Count < rhs.moveList.Count)
		{
			return false;
		}
#if true
        LinkedListNode<Move> ite = ImpliedObject.moveList.First;
        LinkedListNode<Move> rhsite = rhs.moveList.First;
        while( ite != null )
		{
//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			if (ite.Value < rhsite.Value)
			{
				return true;
			}
            ite = ite.Next;
            rhsite = rhsite.Next;
		}
		return false;
#else
		return (ImpliedObject.moveList < rhs.moveList);
#endif
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
		str = Convert.ToString(ImpliedObject.score) + "(";
		if (0 < ImpliedObject.moveList.Count)
		{
			LinkedListNode<Move> ite = ImpliedObject.moveList.First;
//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			str += (string)(ite.Value);
//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			ite = ite.Next;
			while (ite != null)
			{
				str += "," + (string)ite.Value;
                ite = ite.Next;
			}
		}
		str += ")";
		return str;
	}
}