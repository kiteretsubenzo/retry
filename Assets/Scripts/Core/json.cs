//========================================================================
// This conversion was produced by the Free Edition of
// C++ to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System.Collections.Generic;

public class Json
{
	public static List<string> split(string str, char c)
	{
		string[] splited = str.Split(c);
		List<string> ret = new List<string>();
		foreach (string splitedstr in splited) {
			ret.Add (splitedstr);
		}
		return ret;
	}

	public static Dictionary<string, string> fromJson(string str)
	{
		Dictionary<string, string> hash = new Dictionary<string, string>();

		if (str[0] == '{')
		{
			str = str.Substring(1);
		}
		if (str[str.Length - 1] == '}')
		{
			str = str.Substring (0, str.Length - 1);
		}

		int startIndex = 0;
		while (true)
		{
			int index = str.IndexOf(':', startIndex);
			if (index == -1)
			{
				break;
			}
			string key = str.Substring(startIndex, index - startIndex);

			startIndex = index + 1;
			if (str[startIndex] == '{')
			{
				index = str.IndexOf('}', startIndex);
				index = str.IndexOf(',', index);
			}
			else if (str[startIndex] == '[')
			{
				index = str.IndexOf(']', startIndex);
				index = str.IndexOf(',', index);
			}
			else
			{
				index = str.IndexOf(',', startIndex);
			}

			if (index == -1)
			{
				hash[key] = str.Substring(startIndex);
				break;
			}

			hash[key] = str.Substring(startIndex, index - startIndex);

			startIndex = index + 1;
		}

		return hash;
	}

	public static LinkedList<string> fromJsonArray(string str)
	{
        LinkedList<string> ret = new LinkedList<string>();

        if ( str.IndexOf(',') < 0 )
        {
            return ret;
        }

		if (str[0] == '[')
		{
			str = str.Substring(1);
		}
		if (str[str.Length - 1] == ']')
		{
			str = str.Substring (0, str.Length - 1);
		}

		string[] splited = str.Split (',');
		foreach (string splitedstr in splited) {
			ret.AddLast (splitedstr);
		}
		return ret;
	}
}
