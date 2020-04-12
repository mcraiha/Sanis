using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Generoija
{
	public static class BigXMLProcess
	{
		private static readonly string pageString = "page";
		private static readonly string titleString = "title";

		private static readonly string namespaceString = "ns";
		private static readonly string wantedNsNumber = "0";

		private static readonly string textString = "text";

		//private static readonly string englishTranslation = "*englanti:";

		private static readonly string licenseName = "Creative Commons Attribution-ShareAlike 3.0 Unported License (CC-BY-SA)";

		private static readonly string licenseUrl = "https://en.wiktionary.org/wiki/Wiktionary:Text_of_Creative_Commons_Attribution-ShareAlike_3.0_Unported_License";

		public static void PrintNFirst(string inputXMLfilePath, int howMany)
		{
			using (XmlReader reader = XmlReader.Create(inputXMLfilePath))
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

		public static void PrintNFirst(string inputXMLfilePath, int howMany, string outputFilePath)
		{
			using (XmlReader reader = XmlReader.Create(inputXMLfilePath))
			{
				reader.MoveToContent();

				int count = 0;
				using (TextWriter writer = File.CreateText(outputFilePath))
				{
					while (BigXMLProcess.ReadToElement(reader, pageString) && count < howMany)
					{
						BigXMLProcess.ReadToElement(reader, titleString);
						string title = reader.ReadElementContentAsString();
						writer.WriteLine(title);
						count++;
					}
				}
			}
		}

		public static void PrintNFirstTranslations(string inputXMLfilePath, int howMany, string translationLanguage)
		{
			using (XmlReader reader = XmlReader.Create(inputXMLfilePath))
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
						var possibleMatch = ReturnSuitableMatch(text, translationLanguage);
						if (possibleMatch.Item1)
						{
							string[] splitted = CleanAndSplitSuitable(possibleMatch.Item2, translationLanguage);

							/* if (title.Contains("book"))
							{
								Console.WriteLine(possibleMatch.Item2);
							}*/


							Console.WriteLine($"{title} - {string.Join(", ", splitted)}");
							count++;
						}
					}		
				}
			}
		}

		public static void CreateNFirstTranslationsJSON(string inputXMLfilePath, int howMany, string translationLanguage, string bannedWordsListPath, string outputFilePath)
		{
			// Try to init blocklist for banned words
			if (!Blocklist.LoadBlocklist(bannedWordsListPath))
			{
				Console.WriteLine($"Cannot load blocklist from {bannedWordsListPath}");
				return;
			}

			Console.WriteLine($"Blocklist loaded from {bannedWordsListPath} with {Blocklist.GetWordCount()} words");

			using (XmlReader reader = XmlReader.Create(inputXMLfilePath))
			{
				reader.MoveToContent();

				int count = 0;

				// Use sorted dictionary because order of entries will be sorted when creating JSON (this hopefully improves compression ratio)
				SortedDictionary<string, object> translations = new SortedDictionary<string, object>();

				// Add version number and license info to JSON file in case someone needs this kind of meta info
				translations["_version"] = ParseDateAsVersion(Path.GetFileName(inputXMLfilePath));
				translations["_license"] = licenseName;
				translations["_licenseUrl"] = licenseUrl;

				while (BigXMLProcess.ReadToElement(reader, pageString) && count < howMany)
				{
					BigXMLProcess.ReadToElement(reader, titleString);
					string title = reader.ReadElementContentAsString();

					if (Blocklist.IsWordBlocked(title))
					{
						Console.WriteLine($"Skipping word {title} since it is in blocklist");
						continue;
					}

					BigXMLProcess.ReadToElement(reader, namespaceString);
					string nameSpaceNumber = reader.ReadElementContentAsString();
					if (nameSpaceNumber.Equals(wantedNsNumber))
					{
						BigXMLProcess.ReadToElement(reader, textString);
						string text = reader.ReadElementContentAsString();
						var possibleMatch = ReturnSuitableMatch(text, translationLanguage);
						if (possibleMatch.Item1)
						{
							string[] splitted = CleanAndSplitSuitable(possibleMatch.Item2, translationLanguage);

							translations[title] = new Translation() { t = splitted};
							count++;
						}
					}		
				}

				Console.WriteLine($"JSON {outputFilePath} contains {count} entries");

				// Output everything to single file
				using (StreamWriter file = File.CreateText(outputFilePath))
				{
					JsonSerializer serializer = new JsonSerializer();
    				serializer.Serialize(file, translations);
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

		// For entries that are additionally splitted with |
		private static readonly List<string> stringsToRemove = new List<string> { "langname=", "interwiki=" };

		private static readonly string doubleStartBracket = "{{";
		private static readonly string doubleEndBracket = "}}";

		public static string[] CleanAndSplitSuitable(string input, string translationLanguage)
		{
			int howManyToRemove = translationLanguage.Length;
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
						List<string> splittedAgainList = new List<string>(splittedAgain);

						// Remove certain unwanted elements
						splittedAgainList.RemoveAll(item => item.StartsWith(stringsToRemove[0]) || item.StartsWith(stringsToRemove[1]));

						int wantedIndex = splittedAgainList.Count - 1;
						Console.WriteLine($"{wantedIndex} {splittedAgainList[wantedIndex]}");
						if (splittedAgainList[wantedIndex].Length < 2 && wantedIndex > 0)
						{
							wantedIndex--;
							Console.WriteLine("--miinu");
						}

						splitted[i] = splittedAgainList[wantedIndex].Replace(doubleEndBracket, "");
					}
					else
					{
						int removeStartIndex = splitted[i].IndexOf(doubleStartBracket);
						int removeEndIndex = splitted[i].IndexOf(doubleEndBracket) + doubleEndBracket.Length;
						splitted[i] = splitted[i].Remove(removeStartIndex, removeEndIndex - removeStartIndex);
					}		
				}
				else if (splitted[i].Contains('|'))
				{
					string[] splittedAgain = splitted[i].Split('|');
					int wantedIndex = splittedAgain.Length - 1;
					splitted[i] = splittedAgain[wantedIndex];
				}

				splitted[i] = splitted[i].Trim();
			}

			return splitted;
		}

		public static int ParseDateAsVersion(string input)
		{
			string[] splitted = input.Split('-');
			return int.Parse(splitted[1]);
		}
	}
}