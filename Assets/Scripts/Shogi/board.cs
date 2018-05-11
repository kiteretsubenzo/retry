//========================================================================
// This conversion was produced by the Free Edition of
// C++ to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

#define PRIORITY_MULTISET
#define PRIORITY_BUFFER
#define PRIORITY_NONE
#define USE_PRIORITY

using System;
using System.Collections.Generic;

using uchar = System.SByte;
using Pawn = System.SByte;
using Player = System.SByte;

//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define USE_PRIORITY PRIORITY_BUFFER

public class CELL
{
	public Player player = new Player();
	public Pawn pawn = new Pawn();

	public CELL()
	{
	}
	public CELL(Player playerValue, Pawn pawnValue)
	{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.player = playerValue;
		this.player = playerValue;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.pawn = pawnValue;
		this.pawn = pawnValue;
	}
}

//C++ TO C# CONVERTER TODO TASK: C# does not allow setting or comparing #define constants:
#if false
public partial class MoveList
{
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool empty() const
	public bool empty()
	{
		return list.Length == 0;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: size_t size() const
	public size_t size()
	{
		return list.Length;
	}
	public void clear()
	{
		list.Clear();
	}
	public void push(Move move)
	{
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL list 'insert' method in C#:
		list.insert(move);
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Move front() const
	public Move front()
	{
		return *(list.GetEnumerator());
	}
	public void pop_front()
	{
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL list 'erase' method in C#:
		list.erase(list.GetEnumerator());
	}
	public void sort()
	{
	}
	private std::multiset<Move> list = new std::multiset<Move>();
}
//C++ TO C# CONVERTER TODO TASK: C# does not allow setting or comparing #define constants:
#elif USE_PRIORITY == PRIORITY_BUFFER
public partial class MoveList
{
	public void copy(MoveList moveList)
	{
		int offset = moveList.first;
//C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
        Array.Copy(moveList.list, offset, list, offset, moveList.size());
        first = moveList.first;
		last = moveList.last;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool empty() const
	public bool empty()
	{
		return first == last;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint size() const
	public uint size()
	{
		return (uint)(last - first);
	}
	public void clear()
	{
		first = 0;
		last = 0;
	}
	public void push(Move move)
	{
		list[last] = move;
		last++;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Move front() const
	public Move front()
	{
		return list[first];
	}
	public void pop_front()
	{
		first++;
	}
	public void sort()
	{
		//list.sort();
	}
	private Move[] list = new Move[1024];
	private int first = 0;
	private int last = 0;
}
#else
public partial class MoveList
{
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool empty() const
	public bool empty()
	{
		return list.Length == 0;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: size_t size() const
	public size_t size()
	{
		return list.Length;
	}
	public void clear()
	{
		list.Clear();
	}
	public void push(Move move)
	{
		list.AddLast(move);
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Move front() const
	public Move front()
	{
		return *(list.GetEnumerator());
	}
	public void pop_front()
	{
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL list 'erase' method in C#:
		list.erase(list.GetEnumerator());
	}
	public void sort()
	{
		list.sort();
	}
	private LinkedList<Move> list = new LinkedList<Move>();
}
#endif

public class Board
{
	public Board()
	{
		for (uchar i = 0; i < PlayerDef.MAX; i++)
		{
			for (uchar j = 0; j < PawnDef.CAPTURE_MAX; j++)
			{
				captured[i, j] = 0;
			}
		}

		for (uchar j = 1; j <= BoardDef.HEIGHT; j++)
		{
			for (uchar i = 1; i <= BoardDef.WIDTH; i++)
			{
				matrix[j, i] = new CELL(PlayerDef.NONE, PawnDef.NONE);
			}
		}

		for (uchar i = 0; i < BoardDef.WIDTH + 2; i++)
		{
			matrix[0, i] = new CELL(PlayerDef.WALL, PawnDef.NONE);
			matrix[BoardDef.HEIGHT + 1, i] = new CELL(PlayerDef.WALL, PawnDef.NONE);
		}

		for (uchar j = 1; j <= BoardDef.HEIGHT; j++)
		{
			matrix[j, 0] = new CELL(PlayerDef.WALL, PawnDef.NONE);
			matrix[j, BoardDef.WIDTH + 1] = new CELL(PlayerDef.WALL, PawnDef.NONE);
		}

		turn = PlayerDef.FIRST;
		enemy = PlayerDef.SECOND;
	}

	public void Init(string str)
	{
		List<string> strs = Json.split(str, '\n');

		for (int i = 0; i < PlayerDef.MAX; i++)
		{
			gyokux[i] = 0;
			gyokuy[i] = 0;
		}

		for (int i = 0; i < PawnDef.CAPTURE_MAX; i++)
		{
			string first = strs[BoardDef.HEIGHT + 1].Substring(i * 4 + 1, 2);
			string second = strs[0].Substring(i * 4 + 1, 2);
			captured[PlayerDef.FIRST, i] = uchar.Parse(first);
			captured[PlayerDef.SECOND, i] = uchar.Parse(second);
		}

		for (int j = 2; j <= BoardDef.HEIGHT + 1; j++)
		{
			for (int i = 1; i <= BoardDef.WIDTH; i++)
			{
				char[] c = {strs[j - 1][(i - 1) * 2], strs[j - 1][(i - 1) * 2 + 1]};
				if (c[0] == (sbyte)' ')
				{
					matrix[j - 1, i].player = PlayerDef.NONE;
					matrix[j - 1, i].pawn = PawnDef.NONE;
				}
				else
				{
                    char type;
					Player player = PlayerDef.NONE;
					if (c[0] == (sbyte)'^')
					{
						player = PlayerDef.FIRST;
						type = c[1];
					}
					else
					{
						player = PlayerDef.SECOND;
						type = c[0];
					}
					matrix[j - 1, i].player = player;
					matrix[j - 1, i].pawn = GlobalMembers.charToPawn[type];

					if (type == (sbyte)'o')
					{
						gyokux[player] = (uchar)(i);
						gyokuy[player] = (uchar)(j - 1);
					}
				}
			}
		}

		if (strs[BoardDef.HEIGHT + 2] == "first")
		{
			turn = PlayerDef.FIRST;
			enemy = PlayerDef.SECOND;
		}
		else if (strs[BoardDef.HEIGHT + 2] == "second")
		{
			turn = PlayerDef.SECOND;
			enemy = PlayerDef.FIRST;
		}
	}

	// TODO
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string BoardToString() const
	public string BoardToString()
	{
		string sout = "";

		for (int i = 0; i < PawnDef.CAPTURE_MAX; i++)
		{
			sout += GlobalMembers.Pawn_CHAR[i];
			if (captured[PlayerDef.SECOND, i] < 10)
			{
				sout += '0';
			}
			sout += Convert.ToString(captured[PlayerDef.SECOND, i]);
			sout += ' ';
		}
		sout += '\n';

		for (int j = 2; j <= BoardDef.HEIGHT + 1; j++)
		{
			for (int i = 1; i <= BoardDef.WIDTH; i++)
			{
				if (matrix[j - 1, i].player == PlayerDef.FIRST)
				{
					sout += '^';
					sout += GlobalMembers.Pawn_CHAR[matrix[j - 1, i].pawn];
				}
				else if (matrix[j - 1, i].player == PlayerDef.SECOND)
				{
					sout += GlobalMembers.Pawn_CHAR[matrix[j - 1, i].pawn];
					sout += '_';
				}
				else
				{
					sout += " .";
				}
			}
			sout += '\n';
		}

		for (int i = 0; i < PawnDef.CAPTURE_MAX; i++)
		{
			sout += GlobalMembers.Pawn_CHAR[i];
			if (captured[PlayerDef.FIRST, i] < 10)
			{
				sout += '0';
			}
			sout += Convert.ToString(captured[PlayerDef.FIRST, i]);
			sout += ' ';
		}
		sout += '\n';

		if (turn == PlayerDef.FIRST)
		{
			sout += "first";
		}
		else
		{
			sout += "second";
		}

		return sout;
	}

	public MoveList GetMoveList(MoveList moveList)
	{
		moveList.clear();

		uchar lineMax = new uchar();
		uchar lineMin = new uchar();
		uchar lineTop = new uchar();
		uchar lineMid = new uchar();

		if (turn == PlayerDef.FIRST)
		{
			lineMax = 3;
			lineMin = 1;
			lineTop = 1;
			lineMid = 2;
		}
		else
		{
			lineMax = BoardDef.HEIGHT;
			lineMin = BoardDef.HEIGHT - 2;
			lineTop = BoardDef.HEIGHT;
			lineMid = BoardDef.HEIGHT - 1;
		}
		int forward = -1;
		if (turn == PlayerDef.SECOND)
		{
			forward = +1;
		}
		for (sbyte j = 1; j <= BoardDef.HEIGHT; j++)
		{
			for (sbyte i = 1; i <= BoardDef.WIDTH; i++)
			{
				CELL cell = matrix[(uchar)j, (uchar)i];
				if (cell.player == enemy)
				{
					continue;
				}
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: Pawn pawn = cell.pawn;
				Pawn pawn = cell.pawn;
				int x;
				int y;
				switch (pawn)
				{
				case PawnDef.HU:
					x = i;
					y = j + forward;
					if (y != lineTop)
					{
						AddMove(PawnDef.NONE, i, j, x, y, false, moveList);
					}
					if (lineMin <= y && y <= lineMax)
					{
						AddMove(PawnDef.NONE, i, j, x, y, true, moveList);
					}
					break;
				case PawnDef.KYOH:
					y = j + forward;
					for (bool ret = true; 0 < y && y <= BoardDef.HEIGHT && ret; y += forward)
					{
						if (y != lineTop)
						{
							ret &= AddMove(PawnDef.NONE, i, j, i, y, false, moveList);
						}
						if (lineMin <= y && y <= lineMax)
						{
							ret &= AddMove(PawnDef.NONE, i, j, i, y, true, moveList);
						}
					}
					break;
				case PawnDef.KEI:
					x = i - 1;
					y = j - forward - forward;
					if (0 < y && y < BoardDef.HEIGHT + 1)
					{
						if (y != lineTop && y != lineMid)
						{
							AddMove(PawnDef.NONE, i, j, x, y, false, moveList);
						}
						if (lineMin <= y && y <= lineMax)
						{
							AddMove(PawnDef.NONE, i, j, x, y, true, moveList);
						}
					}
					break;
				case PawnDef.GIN:
					AddMove(PawnDef.NONE, i, j, i - 1, j + forward, false, moveList);
					AddMove(PawnDef.NONE, i, j, i, j + forward, false, moveList);
					AddMove(PawnDef.NONE, i, j, i + 1, j + forward, false, moveList);
					AddMove(PawnDef.NONE, i, j, i - 1, j - forward, false, moveList);
					AddMove(PawnDef.NONE, i, j, i + 1, j - forward, false, moveList);
					if ((lineMin <= (j + forward) && (j + forward) <= lineMax) || (lineMin <= j && j <= lineMax))
					{
						AddMove(PawnDef.NONE, i, j, i - 1, j + forward, true, moveList);
						AddMove(PawnDef.NONE, i, j, i, j + forward, true, moveList);
						AddMove(PawnDef.NONE, i, j, i + 1, j + forward, true, moveList);
						AddMove(PawnDef.NONE, i, j, i - 1, j - forward, true, moveList);
						AddMove(PawnDef.NONE, i, j, i + 1, j - forward, true, moveList);
					}
					break;
				case PawnDef.KIN:
				case PawnDef.HUN:
				case PawnDef.KYOHN:
				case PawnDef.KEIN:
				case PawnDef.GINN:
					AddMove(PawnDef.NONE, i, j, i - 1, j + forward, false, moveList);
					AddMove(PawnDef.NONE, i, j, i, j + forward, false, moveList);
					AddMove(PawnDef.NONE, i, j, i + 1, j + forward, false, moveList);
					AddMove(PawnDef.NONE, i, j, i - 1, j, false, moveList);
					AddMove(PawnDef.NONE, i, j, i + 1, j, false, moveList);
					AddMove(PawnDef.NONE, i, j, i, j - forward, false, moveList);
					break;
				case PawnDef.KAKU:
					x = i + 1;
					y = j + 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						if (lineMin <= y && y <= lineMax)
						{
							if (AddMove(PawnDef.NONE, i, j, x, y, true, moveList) == false)
							{
								break;
							}
						}
						x++;
						y++;
					}
					x = i + 1;
					y = j - 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						if (lineMin <= y && y <= lineMax)
						{
							if (AddMove(PawnDef.NONE, i, j, x, y, true, moveList) == false)
							{
								break;
							}
						}
						x++;
						y -= 1;
					}
					x = i - 1;
					y = j + 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						if (lineMin <= y && y <= lineMax)
						{
							if (AddMove(PawnDef.NONE, i, j, x, y, true, moveList) == false)
							{
								break;
							}
						}
						x -= 1;
						y++;
					}
					x = i - 1;
					y = j - 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						if (lineMin <= y && y <= lineMax)
						{
							if (AddMove(PawnDef.NONE, i, j, x, y, true, moveList) == false)
							{
								break;
							}
						}
						x -= 1;
						y -= 1;
					}
					break;
				case PawnDef.UMA:
					x = i + 1;
					y = j + 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						x++;
						y++;
					}
					x = i + 1;
					y = j - 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						x++;
						y -= 1;
					}
					x = i - 1;
					y = j + 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						x -= 1;
						y++;
					}
					x = i - 1;
					y = j - 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						x -= 1;
						y -= 1;
					}
					AddMove(PawnDef.NONE, i, j, i + 1, j, false, moveList);
					AddMove(PawnDef.NONE, i, j, i - 1, j, false, moveList);
					AddMove(PawnDef.NONE, i, j, i, j + 1, false, moveList);
					AddMove(PawnDef.NONE, i, j, i, j - 1, false, moveList);
					break;
				case PawnDef.HI:
					x = i + 1;
					y = j;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						if (lineMin <= y && y <= lineMax)
						{
							if (AddMove(PawnDef.NONE, i, j, x, y, true, moveList) == false)
							{
								break;
							}
						}
						x++;
					}
					x = i - 1;
					y = j;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						if (lineMin <= y && y <= lineMax)
						{
							if (AddMove(PawnDef.NONE, i, j, x, y, true, moveList) == false)
							{
								break;
							}
						}
						x -= 1;
					}
					x = i;
					y = j + 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						if (lineMin <= y && y <= lineMax)
						{
							if (AddMove(PawnDef.NONE, i, j, x, y, true, moveList) == false)
							{
								break;
							}
						}
						y++;
					}
					x = i;
					y = j - 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						if (lineMin <= y && y <= lineMax)
						{
							if (AddMove(PawnDef.NONE, i, j, x, y, true, moveList) == false)
							{
								break;
							}
						}
						y -= 1;
					}
					break;
				case PawnDef.RYU:
					x = i + 1;
					y = j;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						x++;
					}
					x = i - 1;
					y = j;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						x -= 1;
					}
					x = i;
					y = j + 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						y++;
					}
					x = i;
					y = j - 1;
					while (true)
					{
						if (AddMove(PawnDef.NONE, i, j, x, y, false, moveList) == false)
						{
							break;
						}
						y -= 1;
					}
					AddMove(PawnDef.NONE, i, j, i + 1, j + 1, false, moveList);
					AddMove(PawnDef.NONE, i, j, i - 1, j + 1, false, moveList);
					AddMove(PawnDef.NONE, i, j, i + 1, j - 1, false, moveList);
					AddMove(PawnDef.NONE, i, j, i - 1, j - 1, false, moveList);
					break;
				case PawnDef.GYOKU:
					AddMove(PawnDef.NONE, i, j, i - 1, j - 1, false, moveList);
					AddMove(PawnDef.NONE, i, j, i, j - 1, false, moveList);
					AddMove(PawnDef.NONE, i, j, i + 1, j - 1, false, moveList);
					AddMove(PawnDef.NONE, i, j, i - 1, j, false, moveList);
					AddMove(PawnDef.NONE, i, j, i + 1, j, false, moveList);
					AddMove(PawnDef.NONE, i, j, i - 1, j + 1, false, moveList);
					AddMove(PawnDef.NONE, i, j, i, j + 1, false, moveList);
					AddMove(PawnDef.NONE, i, j, i + 1, j + 1, false, moveList);
					break;
				default:
					// 空きだったら打ち
					for (uchar roll = 0; roll < PawnDef.CAPTURE_MAX; roll++)
					{
						if (captured[turn, roll] == 0)
						{
							continue;
						}
						switch (roll)
						{
						case PawnDef.HU:
							if (j != lineTop)
							{
								uchar k = new uchar();
								for (k = 1; k <= BoardDef.HEIGHT; k++)
								{
									if (matrix[k, i].player == turn && matrix[k, i].pawn == PawnDef.HU)
									{
										break;
									}
								}
								if (BoardDef.HEIGHT < k)
								{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: AddMove(roll, 0, 0, i, j, false, moveList);
									AddMove(roll, 0, 0, i, j, false, moveList);
								}
							}
							break;
						case PawnDef.KYOH:
							if (j != lineTop)
							{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: AddMove(roll, 0, 0, i, j, false, moveList);
								AddMove(roll, 0, 0, i, j, false, moveList);
							}
							break;
						case PawnDef.KEI:
							if (j != lineTop && j != lineMid)
							{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: AddMove(roll, 0, 0, i, j, false, moveList);
								AddMove(roll, 0, 0, i, j, false, moveList);
							}
							break;
						case PawnDef.GIN:
						case PawnDef.KIN:
						case PawnDef.KAKU:
						case PawnDef.HI:
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: AddMove(roll, 0, 0, i, j, false, moveList);
							AddMove(roll, 0, 0, i, j, false, moveList);
							break;
						default:
							break;
						}
					}

					break;
				}
			}
		}

//C++ TO C# CONVERTER TODO TASK: C# does not allow setting or comparing #define constants:
#if USE_PRIORITY != PRIORITY_NONE
		moveList.sort();
#endif
		return moveList;
	}

	public void Forward(Move move)
	{
		if (move.reserve != PawnDef.NONE)
		{
			captured[turn, move.reserve]--;
			matrix[move.to.y, move.to.x].player = turn;
			matrix[move.to.y, move.to.x].pawn = move.reserve;

			SwitchTurn();
			return;
		}

		Pawn pawn = move.from.pawn;
		if (move.upgrade)
		{
			Upgrade(ref pawn);
		}
		matrix[move.to.y, move.to.x].player = turn;
		matrix[move.to.y, move.to.x].pawn = pawn;
		matrix[move.from.y, move.from.x].player = PlayerDef.NONE;
		matrix[move.from.y, move.from.x].pawn = PawnDef.NONE;

		if (move.to.pawn != PawnDef.NONE)
		{
			captured[turn, Down(move.to.pawn)]++;
		}
		if (move.from.pawn == PawnDef.GYOKU)
		{
			gyokux[turn] = move.to.x;
			gyokuy[turn] = move.to.y;
		}

		SwitchTurn();
	}

	public void Back(Move move)
	{
		if (move.reserve != PawnDef.NONE)
		{
			captured[enemy, move.reserve]++;

			matrix[move.to.y, move.to.x].player = PlayerDef.NONE;
			matrix[move.to.y, move.to.x].pawn = PawnDef.NONE;

			SwitchTurn();
			return;
		}

		Pawn pawn = move.from.pawn;
		if (move.upgrade)
		{
			Downgrade(ref pawn);
		}
		matrix[move.from.y, move.from.x].player = enemy;
		matrix[move.from.y, move.from.x].pawn = pawn;

		if (move.to.pawn != PawnDef.NONE)
		{
			captured[enemy, Down(move.to.pawn)]--;

			matrix[move.to.y, move.to.x].player = turn;
			matrix[move.to.y, move.to.x].pawn = move.to.pawn;
		}
		else
		{
			matrix[move.to.y, move.to.x].player = PlayerDef.NONE;
			matrix[move.to.y, move.to.x].pawn = PawnDef.NONE;
		}

		if (move.from.pawn == PawnDef.GYOKU)
		{
			gyokux[enemy] = move.from.x;
			gyokuy[enemy] = move.from.y;
		}

		SwitchTurn();
	}

	public int GetEvaluate(MoveList moveList)
	{
		return (int)(moveList.size());
	}
	public int GetPriority(Move move)
	{
		int priority = 0;
//C++ TO C# CONVERTER TODO TASK: C# does not allow setting or comparing #define constants:
#if USE_PRIORITY != PRIORITY_NONE && USE_PRIORITY != PRIORITY_BUFFER
		// 王手がかかってるか？
		SwitchTurn();
		if (IsEnd())
		{
			priority += 1000;
		}
		SwitchTurn();
		// 駒を取るか？
		if (move.to.pawn != Pawn_NONE)
		{
			priority += (int)(move.to.pawn) + (int)Pawn_MAX;
		}
		// 成るか？
		if (move.upgrade)
		{
			priority += (int)(move.from.pawn);
		}
#endif
		return priority;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void PrintBoard() const
	public void PrintBoard()
	{
		for (uchar i = 0; i < PawnDef.CAPTURE_MAX; i++)
		{
			Console.Write(GlobalMembers.Pawn_KANJI[i]);
			Console.Write(":");
			Console.Write((uint)captured[PlayerDef.SECOND, i]);
			Console.Write(" ");
		}
		Console.Write("\n");
		Console.Write("９８７６５４３２１");
		Console.Write("\n");
		for (uchar j = 1; j <= BoardDef.HEIGHT; j++)
		{
			for (uchar i = 1; i <= BoardDef.WIDTH; i++)
			{
				switch (matrix[j, i].player)
				{
				case PlayerDef.FIRST:
					Console.Write("^");
					Console.Write(GlobalMembers.Pawn_CHAR[matrix[j, i].pawn]);
					break;
				case PlayerDef.SECOND:
					Console.Write(GlobalMembers.Pawn_CHAR[matrix[j, i].pawn]);
					Console.Write("_");
					break;
				case PlayerDef.NONE:
					Console.Write(" .");
					break;
				default:
					break;
				}
			}
			Console.Write(" ");
			Console.Write(GlobalMembers.numberToKanji[j - 1]);
			Console.Write("\n");
		}
		for (uchar i = 0; i < PawnDef.CAPTURE_MAX; i++)
		{
			Console.Write(GlobalMembers.Pawn_KANJI[i]);
			Console.Write(":");
			Console.Write((uint)captured[PlayerDef.FIRST, i]);
			Console.Write(" ");
		}
		Console.Write("\n");
		//std::cout << Player_STRING[(int)turn] << std::endl;
		if (turn == PlayerDef.FIRST)
		{
			Console.Write("先手番");
			Console.Write("\n");
		}
		else
		{
			Console.Write("後手番");
			Console.Write("\n");
		}
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const Board& rhs) const
	public static bool operator == (Board ImpliedObject, Board rhs)
	{
		return (ImpliedObject.matrix == rhs.matrix && ImpliedObject.turn == rhs.turn);
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator !=(const Board& rhs) const
	public static bool operator != (Board ImpliedObject, Board rhs)
	{
		return (ImpliedObject.matrix != rhs.matrix || ImpliedObject.turn != rhs.turn);
	}

	protected bool AddMove(Pawn roll, int fromx, int fromy, int tox, int toy, bool upgrade, MoveList moveList)
	{
		if (matrix[toy, tox].player == turn || matrix[toy, tox].player == PlayerDef.WALL)
		{
			return false;
		}

		Move move = new Move(roll, (uchar)fromx, (uchar)fromy, (uchar)tox, (uchar)toy, matrix[fromy, fromx].pawn, matrix[toy, tox].pawn, upgrade, 0);
		if (roll != PawnDef.NONE)
		{
			move.from.x = 0;
			move.from.y = 0;
			move.from.pawn = PawnDef.NONE;
		}

		Forward(move);
		// 負ける手は指さない
		if (IsEnd() == false)
		{
			move.priority = GetPriority(move);
			moveList.push(move);
		}
		Back(move);

		return matrix[toy, tox].pawn == PawnDef.NONE;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool IsEnd() const
	protected bool IsEnd()
	{
		// 玉の位置を求める
		sbyte gyokux = this.gyokux[enemy];
		sbyte gyokuy = this.gyokuy[enemy];
		if (gyokux == 0)
		{
			return false;
		}
		CELL cell = new CELL();
		if (turn == PlayerDef.FIRST)
		{
			// 玉の周囲
			if (GetCell(gyokux - 1, gyokuy, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn))
				if (IsGyokuKinUpgrade(cell.pawn))
				{
					return true;
				}
			}
			if (GetCell(gyokux + 1, gyokuy, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn))
				if (IsGyokuKinUpgrade(cell.pawn))
				{
					return true;
				}
			}
			if (GetCell(gyokux, gyokuy - 1, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn))
				if (IsGyokuKinUpgrade(cell.pawn))
				{
					return true;
				}
			}
			if (GetCell(gyokux, gyokuy + 1, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef::HU || cell.pawn == PawnDef::GIN)
				if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef.HU || cell.pawn == PawnDef.GIN)
				{
					return true;
				}
			}
			if (GetCell(gyokux - 1, gyokuy - 1, ref cell))
			{
				if (cell.pawn == PawnDef.GYOKU || cell.pawn == PawnDef.GIN || cell.pawn == PawnDef.RYU)
				{
					return true;
				}
			}
			if (GetCell(gyokux + 1, gyokuy - 1, ref cell))
			{
				if (cell.pawn == PawnDef.GYOKU || cell.pawn == PawnDef.GIN || cell.pawn == PawnDef.RYU)
				{
					return true;
				}
			}
			if (GetCell(gyokux + 1, gyokuy + 1, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef::GIN)
				if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef.GIN)
				{
					return true;
				}
			}
			if (GetCell(gyokux - 1, gyokuy + 1, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef::GIN)
				if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef.GIN)
				{
					return true;
				}
			}

			// 桂
			if (gyokuy < BoardDef.HEIGHT)
			{
				if (GetCell(gyokux - 1, gyokuy + 2, ref cell))
				{
					if (cell.pawn == PawnDef.KEI)
					{
						return true;
					}
				}
				if (GetCell(gyokux + 1, gyokuy + 2, ref cell))
				{
					if (cell.pawn == PawnDef.KEI)
					{
						return true;
					}
				}
			}

			// 香
			for (int j = gyokuy + 1; j <= BoardDef.HEIGHT; j++)
			{
				if (matrix[j, gyokux].player == PlayerDef.NONE)
				{
					continue;
				}
				if (matrix[j, gyokux].player == enemy)
				{
					break;
				}
				if (matrix[j, gyokux].pawn == PawnDef.KYOH)
				{
					return true;
				}
				break;
			}
		}
		else
		{
			// 玉の周囲
			if (GetCell(gyokux - 1, gyokuy, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn))
				if (IsGyokuKinUpgrade(cell.pawn))
				{
					return true;
				}
			}
			if (GetCell(gyokux + 1, gyokuy, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn))
				if (IsGyokuKinUpgrade(cell.pawn))
				{
					return true;
				}
			}
			if (GetCell(gyokux, gyokuy - 1, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef::HU || cell.pawn == PawnDef::GIN)
				if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef.HU || cell.pawn == PawnDef.GIN)
				{
					return true;
				}
			}
			if (GetCell(gyokux, gyokuy + 1, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn))
				if (IsGyokuKinUpgrade(cell.pawn))
				{
					return true;
				}
			}
			if (GetCell(gyokux - 1, gyokuy - 1, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef::GIN)
				if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef.GIN)
				{
					return true;
				}
			}
			if (GetCell(gyokux + 1, gyokuy - 1, ref cell))
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef::GIN)
				if (IsGyokuKinUpgrade(cell.pawn) || cell.pawn == PawnDef.GIN)
				{
					return true;
				}
			}
			if (GetCell(gyokux + 1, gyokuy + 1, ref cell))
			{
				if (cell.pawn == PawnDef.GYOKU || cell.pawn == PawnDef.GIN || cell.pawn == PawnDef.RYU)
				{
					return true;
				}
			}
			if (GetCell(gyokux - 1, gyokuy + 1, ref cell))
			{
				if (cell.pawn == PawnDef.GYOKU || cell.pawn == PawnDef.GIN || cell.pawn == PawnDef.RYU)
				{
					return true;
				}
			}

			// 桂
			if (2 <= gyokuy)
			{
				if (GetCell(gyokux - 1, gyokuy - 2, ref cell))
				{
					if (cell.pawn == PawnDef.KEI)
					{
						return true;
					}
				}
				if (GetCell(gyokux + 1, gyokuy - 2, ref cell))
				{
					if (cell.pawn == PawnDef.KEI)
					{
						return true;
					}
				}
			}

			// 香
			for (int j = gyokuy - 1; 0 < j; j--)
			{
				if (matrix[j, gyokux].player == PlayerDef.NONE)
				{
					continue;
				}
				if (matrix[j, gyokux].player == enemy)
				{
					break;
				}
				if (matrix[j, gyokux].pawn == PawnDef.KYOH)
				{
					return true;
				}
				break;
			}
		}

		{
			int i;
			int j;
			// 飛龍
			i = gyokux + 1;
			while (true)
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (matrix[gyokuy][i].player == turn && Down(matrix[gyokuy][i].pawn) == PawnDef::HI)
				if (matrix[gyokuy, i].player == turn && Down(matrix[gyokuy, i].pawn) == PawnDef.HI)
				{
					return true;
				}
				if (matrix[gyokuy, i].player != PlayerDef.NONE)
				{
					break;
				}
				i++;
			}
			i = gyokux - 1;
			while (true)
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (matrix[gyokuy][i].player == turn && Down(matrix[gyokuy][i].pawn) == PawnDef::HI)
				if (matrix[gyokuy, i].player == turn && Down(matrix[gyokuy, i].pawn) == PawnDef.HI)
				{
					return true;
				}
				if (matrix[gyokuy, i].player != PlayerDef.NONE)
				{
					break;
				}
				i--;
			}
			j = gyokuy + 1;
			while (true)
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (matrix[j][gyokux].player == turn && Down(matrix[j][gyokux].pawn) == PawnDef::HI)
				if (matrix[j, gyokux].player == turn && Down(matrix[j, gyokux].pawn) == PawnDef.HI)
				{
					return true;
				}
				if (matrix[j, gyokux].player != PlayerDef.NONE)
				{
					break;
				}
				j++;
			}
			j = gyokuy - 1;
			while (true)
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (matrix[j][gyokux].player == turn && Down(matrix[j][gyokux].pawn) == PawnDef::HI)
				if (matrix[j, gyokux].player == turn && Down(matrix[j, gyokux].pawn) == PawnDef.HI)
				{
					return true;
				}
				if (matrix[j, gyokux].player != PlayerDef.NONE)
				{
					break;
				}
				j--;
			}

			// 角馬
			i = gyokux + 1;
			j = gyokuy + 1;
			while (true)
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (matrix[j][i].player == turn && Down(matrix[j][i].pawn) == PawnDef::KAKU)
				if (matrix[j, i].player == turn && Down(matrix[j, i].pawn) == PawnDef.KAKU)
				{
					return true;
				}
				if (matrix[j, i].player != PlayerDef.NONE)
				{
					break;
				}
				i++;
				j++;
			}
			i = gyokux - 1;
			j = gyokuy + 1;
			while (true)
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (matrix[j][i].player == turn && Down(matrix[j][i].pawn) == PawnDef::KAKU)
				if (matrix[j, i].player == turn && Down(matrix[j, i].pawn) == PawnDef.KAKU)
				{
					return true;
				}
				if (matrix[j, i].player != PlayerDef.NONE)
				{
					break;
				}
				i--;
				j++;
			}
			i = gyokux - 1;
			j = gyokuy - 1;
			while (true)
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (matrix[j][i].player == turn && Down(matrix[j][i].pawn) == PawnDef::KAKU)
				if (matrix[j, i].player == turn && Down(matrix[j, i].pawn) == PawnDef.KAKU)
				{
					return true;
				}
				if (matrix[j, i].player != PlayerDef.NONE)
				{
					break;
				}
				i--;
				j--;
			}
			i = gyokux + 1;
			j = gyokuy - 1;
			while (true)
			{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: if (matrix[j][i].player == turn && Down(matrix[j][i].pawn) == PawnDef::KAKU)
				if (matrix[j, i].player == turn && Down(matrix[j, i].pawn) == PawnDef.KAKU)
				{
					return true;
				}
				if (matrix[j, i].player != PlayerDef.NONE)
				{
					break;
				}
				i++;
				j--;
			}
		}

		return false;
	}

	protected CELL GetCell(int x, int y)
	{
		return matrix[y, x];
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool GetCell(int tox, int toy, CELL &cell) const
	protected bool GetCell(int tox, int toy, ref CELL cell)
	{
		if (matrix[toy, tox].player != turn)
		{
			return false;
		}

		cell = matrix[toy, tox];

		return true;
	}

	protected void SwitchTurn()
	{
		if (turn == PlayerDef.FIRST)
		{
			turn = PlayerDef.SECOND;
			enemy = PlayerDef.FIRST;
		}
		else
		{
			turn = PlayerDef.FIRST;
			enemy = PlayerDef.SECOND;
		}
	}

	protected CELL[,] matrix = new CELL[BoardDef.HEIGHT + 2, BoardDef.WIDTH + 2];
	protected Player turn = new Player();
	protected Player enemy = new Player();

	private uchar[,] captured = new uchar[PlayerDef.MAX, PawnDef.CAPTURE_MAX];

	private uchar[] gyokux = new uchar[PlayerDef.MAX];
	private uchar[] gyokuy = new uchar[PlayerDef.MAX];

	private static void Upgrade(ref Pawn type)
	{
		type |= 0x08;
	}
	private static void Downgrade(ref Pawn type)
	{
		type &= 0x07;
	}
	private static Pawn Down(Pawn type)
	{
		return (Pawn)(type & 0x07);
	}
	private static bool IsUpgrade(Pawn type)
	{
		return ((type & 0x08) != 0);
	}
	private static bool IsGyokuKinUpgrade(Pawn type)
	{
		return (PawnDef.KIN <= type);
	}
}
