using System;
using System.IO;
using System.Collections.Generic;

namespace Generoija
{
	public static class Blocklist
	{
		private static HashSet<string> blocklist = new HashSet<string>();
		public static bool LoadBlocklist(string filepath)
		{
			blocklist.Clear();

			try
			{
				var lines = File.ReadAllLines(filepath);
				for (var i = 0; i < lines.Length; i++) 
				{
					blocklist.Add(lines[i]);
				}
			}
			catch 
			{
				return false;
			}

			return true;
		}

		public static bool IsWordBlocked(string wordToCheck)
		{
			return blocklist.Contains(wordToCheck);
		}

		public static int GetWordCount()
		{
			return blocklist.Count;
		}
	}
}