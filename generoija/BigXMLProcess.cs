using System;
using System.Xml;
using System.IO;

namespace Generoija
{
	public static class BigXMLProcess
	{
		private static readonly string pageString = "page";
		private static readonly string titleString = "title";

		private static readonly string namespaceString = "ns";
		private static readonly string wantedNsNumber = "0";

		private static readonly string textString = "text";

		private static readonly string englishTranslation = "*englanti:";

		public static void PrintNFirst(string filePath, int howMany)
		{
			using (XmlReader reader = XmlReader.Create(filePath))
			{
				reader.MoveToContent();

				int count = 0;

				while (BigXMLProcess.ReadToElement(reader, pageString) && count < howMany)
				{
					BigXMLProcess.ReadToElement(reader, titleString);
					string title = reader.ReadElementContentAsString();
					Console.WriteLine(title);
					count++;
				}
			}
		}

		public static void PrintNFirstTranslations(string filePath, int howMany)
		{
			using (XmlReader reader = XmlReader.Create(filePath))
			{
				reader.MoveToContent();

				int count = 0;

				while (BigXMLProcess.ReadToElement(reader, pageString) && count < howMany)
				{
					BigXMLProcess.ReadToElement(reader, titleString);
					string title = reader.ReadElementContentAsString();

					BigXMLProcess.ReadToElement(reader, namespaceString);
					string nameSpaceNumber = reader.ReadElementContentAsString();
					if (nameSpaceNumber.Equals(wantedNsNumber))
					{
						BigXMLProcess.ReadToElement(reader, textString);
						string text = reader.ReadElementContentAsString();
						var possibleMatch = ReturnSuitableMatch(text, englishTranslation);
						if (possibleMatch.Item1)
						{
							string[] splitted = CleanAndSplitSuitable(possibleMatch.Item2);

							Console.WriteLine($"{title} - {string.Join(", ", splitted)}");
							count++;
						}
					}		
				}
			}
		}

		public static bool ReadToElement(XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}

			return false;
		}

		public static bool ReadToElement(XmlReader reader, string name)
		{
			if (string.CompareOrdinal(reader.Name, name) == 0)
			{
				return true;
			}

			while (BigXMLProcess.ReadToElement(reader))
			{
				if (string.CompareOrdinal(reader.Name, name) == 0)
				{
					return true;
				}
			}

			return false;
		}

		public static (bool, string) ReturnSuitableMatch(string input, string startOfTheLine)
		{
			using (StringReader reader = new StringReader(input))
			{
				// Loop over the lines in the string.
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.StartsWith(startOfTheLine))
					{
						return (true, line);
					}
				}
			}

			return (false, "");
		}

		private static readonly char[] charsToRemove = new char[] { '[', ']'};

		private static readonly string doubleStartBracket = "{{";
		private static readonly string doubleEndBracket = "}}";

		public static string[] CleanAndSplitSuitable(string input)
		{
			int howManyToRemove = englishTranslation.Length;
			string withoutStart = input.Remove(0, howManyToRemove);

			string readyForRemoval = withoutStart.Trim();

			foreach (char c in charsToRemove)
			{
				readyForRemoval = readyForRemoval.Replace(c.ToString(), "");
			}

			string[] splitted = readyForRemoval.Split(',', System.StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < splitted.Length; i++)
			{
				if (splitted[i].Contains(doubleStartBracket) && splitted[i].Contains(doubleEndBracket))
				{
					if (splitted[i].Contains('|'))
					{
						string[] splittedAgain = splitted[i].Split('|');
						splitted[i] = splittedAgain[splittedAgain.Length - 1].Replace(doubleEndBracket, "");
					}
					else
					{
						int removeStartIndex = splitted[i].IndexOf(doubleStartBracket);
						int removeEndIndex = splitted[i].IndexOf(doubleEndBracket) + doubleEndBracket.Length;
						splitted[i] = splitted[i].Remove(removeStartIndex, removeEndIndex - removeStartIndex);
					}		
				}

				splitted[i] = splitted[i].Trim();
			}

			return splitted;
		}
	}
}