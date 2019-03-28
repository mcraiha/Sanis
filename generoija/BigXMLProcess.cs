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
                            Console.WriteLine($"{title} - {splitted[0]}");
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

        private static readonly char[] charsToTrim = new char[] { '[', ' ', ']'};
        public static string[] CleanAndSplitSuitable(string input)
        {
            int howManyToRemove = englishTranslation.Length;
            string clear = input.Remove(0, howManyToRemove);
            string[] splitted = clear.Split(',', System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < splitted.Length; i++)
            {
                splitted[i] = splitted[i].Trim(charsToTrim);

            }

            return splitted;
        }
	}
}