//========================================================================
// This conversion was produced by the Free Edition of
// C++ to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

public class BoardDef
{
	public const int WIDTH = 9;
	public const int HEIGHT = 9;
}

public class PawnDef
{
	public const sbyte HU = 0;
	public const sbyte KYOH = 1;
	public const sbyte KEI = 2;
	public const sbyte GIN = 3;
	public const sbyte KAKU = 4;
	public const sbyte HI = 5;
	public const sbyte KIN = 6;
	public const sbyte GYOKU = 7;
	public const sbyte HUN = 8;
	public const sbyte KYOHN = 9;
	public const sbyte KEIN = 10;
	public const sbyte GINN = 11;
	public const sbyte UMA = 12;
	public const sbyte RYU = 13;
	public const sbyte MAX = 14;
	public const sbyte NONE = MAX;
	public const sbyte CAPTURE_MAX = GYOKU;
}

public class PlayerDef
{
	public const sbyte FIRST = 0;
	public const sbyte SECOND = 1;
	public const sbyte NONE = 2;
	public const sbyte WALL = 3;

	public const sbyte MAX = 2;
}