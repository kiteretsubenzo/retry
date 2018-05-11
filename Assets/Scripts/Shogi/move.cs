//========================================================================
// This conversion was produced by the Free Edition of
// C++ to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;

using uchar = System.SByte;
using Pawn = System.SByte;

public class Move
{
	public class MovePawn
	{
		public uchar x = new uchar();
		public uchar y = new uchar();
		public Pawn pawn = new Pawn();

		public MovePawn()
		{
			x = 0;
			y = 0;
			pawn = PawnDef.NONE;
		}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const MovePawn& rhs) const
		public static bool operator == (MovePawn ImpliedObject, MovePawn rhs)
		{
			return (ImpliedObject.x == rhs.x && ImpliedObject.y == rhs.y && ImpliedObject.pawn == rhs.pawn);
		}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator !=(const MovePawn& rhs) const
		public static bool operator != (MovePawn ImpliedObject, MovePawn rhs)
		{
			return !(ImpliedObject == rhs);
		}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator <(const MovePawn& rhs) const
		public static bool operator < (MovePawn ImpliedObject, MovePawn rhs)
		{
			if (ImpliedObject.x > rhs.x)
			{
				return true;
			}
			if (ImpliedObject.x < rhs.x)
			{
				return false;
			}

			if (ImpliedObject.y < rhs.y)
			{
				return true;
			}
			if (ImpliedObject.y > rhs.y)
			{
				return false;
			}

			if (ImpliedObject.pawn < rhs.pawn)
			{
				return true;
			}
			if (ImpliedObject.pawn > rhs.pawn)
			{
				return false;
			}

			return false;
		}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator >(const MovePawn& rhs) const
		public static bool operator > (MovePawn ImpliedObject, MovePawn rhs)
		{
			return !(ImpliedObject < rhs) && ImpliedObject != rhs;
		}
	}

	public Pawn reserve = new Pawn();
	public MovePawn from = new MovePawn();
	public MovePawn to = new MovePawn();
	public bool upgrade = false;
	public int priority = 0;

	public Move()
	{
		this.reserve = PawnDef.NONE;
		this.upgrade = false;
		this.priority = 0;
		from = new MovePawn();
		to = new MovePawn();
	}

	public Move(Pawn reserveValue, uchar fromx, uchar fromy, uchar tox, uchar toy, Pawn fromPawn, Pawn toPawn, bool upgradeValue, int priorityValue)
	{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.reserve = reserveValue;
		this.reserve = reserveValue;
		this.upgrade = upgradeValue;
		this.priority = priorityValue;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: from.x = fromx;
		from.x = fromx;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: from.y = fromy;
		from.y = fromy;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: from.pawn = fromPawn;
		from.pawn = fromPawn;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: to.x = tox;
		to.x = tox;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: to.y = toy;
		to.y = toy;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: to.pawn = toPawn;
		to.pawn = toPawn;
	}

	public Move(string str)
	{
		from.x = uchar.Parse(str.Substring(0, 1));
		from.y = uchar.Parse(str.Substring(1, 1));
		if (from.x == 0 && from.y == 0)
		{
			reserve = GlobalMembers.charToPawn[str[2]];
			from.pawn = PawnDef.NONE;
		}
		else
		{
			reserve = PawnDef.NONE;
			from.pawn = GlobalMembers.charToPawn[str[2]];
		}
		to.x = uchar.Parse(str.Substring(3, 1));
		to.y = uchar.Parse(str.Substring(4, 1));
		to.pawn = GlobalMembers.charToPawn[str[5]];
		upgrade = (str[6] == 't');
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string DebugString() const
	public string DebugString()
	{
		if (reserve == PawnDef.NONE && from.x == to.x && from.y == to.y)
		{
			return "ZERO";
		}

		string str = "";
		//uchar tox, toy;
		str += GlobalMembers.numberToZenkaku[(uchar)to.x - 1] + GlobalMembers.numberToKanji[(uchar)to.y - 1];

		if (reserve != PawnDef.NONE)
		{
			//Pawn_ROLL reserve;
			str += " " + GlobalMembers.Pawn_KANJI[reserve] + " 打ち";
		}
		else
		{
			str += " " + GlobalMembers.Pawn_KANJI[from.pawn];
			str += "(" + Convert.ToString(BoardDef.WIDTH - from.x + 1) + "," + Convert.ToString(from.y) + ")";

			//bool upgrade;
			if (upgrade)
			{
				str += " 成り";
			}
		}

		return str;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const Move& rhs) const
	public static bool operator == (Move ImpliedObject, Move rhs)
	{
		return (ImpliedObject.reserve == rhs.reserve && ImpliedObject.from == rhs.from && ImpliedObject.to == rhs.to && ImpliedObject.upgrade == rhs.upgrade);
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator !=(const Move& rhs) const
	public static bool operator != (Move ImpliedObject, Move rhs)
	{
		return !(ImpliedObject == rhs);
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator <(const Move& rhs) const
	public static bool operator < (Move ImpliedObject, Move rhs)
	{
		if (ImpliedObject.priority > rhs.priority)
		{
			return true;
		}
		if (ImpliedObject.priority < rhs.priority)
		{
			return false;
		}

		if (ImpliedObject.reserve == PawnDef.NONE && rhs.reserve != PawnDef.NONE)
		{
			return true;
		}
		if (ImpliedObject.reserve != PawnDef.NONE && rhs.reserve == PawnDef.NONE)
		{
			return false;
		}
		if (ImpliedObject.reserve < rhs.reserve)
		{
			return true;
		}
		if (ImpliedObject.reserve > rhs.reserve)
		{
			return false;
		}

		if (ImpliedObject.from < rhs.from)
		{
			return true;
		}
		if (ImpliedObject.from > rhs.from)
		{
			return false;
		}
		if (ImpliedObject.to < rhs.to)
		{
			return true;
		}
		if (ImpliedObject.to > rhs.to)
		{
			return false;
		}

		if (ImpliedObject.upgrade == false && rhs.upgrade == true)
		{
			return true;
		}

		return false;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator >(const Move& rhs) const
	public static bool operator > (Move ImpliedObject, Move rhs)
	{
		return !(ImpliedObject < rhs) && ImpliedObject != rhs;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator string() const
	public static implicit operator string(Move ImpliedObject)
	{
		string stream = "";

		stream += Convert.ToString(ImpliedObject.from.x);
		stream += Convert.ToString(ImpliedObject.from.y);
		if (ImpliedObject.from.x == 0 && ImpliedObject.from.y == 0)
		{
			stream += GlobalMembers.Pawn_CHAR[ImpliedObject.reserve];
		}
		else
		{
			stream += GlobalMembers.Pawn_CHAR[ImpliedObject.from.pawn];
		}
		stream += Convert.ToString(ImpliedObject.to.x);
		stream += Convert.ToString(ImpliedObject.to.y);
		stream += GlobalMembers.Pawn_CHAR[ImpliedObject.to.pawn];
		if (ImpliedObject.upgrade)
		{
			stream += "t";
		}
		else
		{
			stream += "f";
		}

		return stream;
	}

	public int MOVES_MAX = (BoardDef.WIDTH + BoardDef.HEIGHT - 2) * BoardDef.WIDTH * BoardDef.HEIGHT * 2 + (PawnDef.CAPTURE_MAX - 1) * BoardDef.WIDTH * BoardDef.HEIGHT;
}