using System;

namespace Generoija
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Komento puuttuu!");
				return;
			}

			string command = args[0];

			if (command.Equals("otsikot"))
			{
				if (args.Length != 3)
				{
					PrintHeadersHelp();
					return;
				}

				BigXMLProcess.PrintNFirst(args[2], int.Parse(args[1]));
			}
			else if (command.Equals("kaannokset"))
			{
				if (args.Length != 4)
				{
					PrintTranslationsHelp();
					return;
				}

				BigXMLProcess.PrintNFirstTranslations(filePath: args[3], howMany: int.Parse(args[1]), translationLanguage: RemoveQuotesIfNeeded(args[2]));
			}
			else if (command.Equals("teejson"))
			{
				if (args.Length != 6)
				{
					PrintCreateJSONHelp();
					return;
				}

				BigXMLProcess.CreateNFirstTranslationsJSON(inputFilePath: args[3], howMany: int.Parse(args[1]), translationLanguage: args[2], bannedWordsListPath: args[4], outputFilePath: args[5]);
			}
			
		}

		private static void PrintHeadersHelp()
		{
			Console.WriteLine("otsikot haluttu_määrä tiedosto_sisään");
			Console.WriteLine("esim: dotnet run otsikot 2000 fiwiktionary-20190320-pages-articles-multistream.xml");
		}

		private static void PrintTranslationsHelp()
		{
			Console.WriteLine("kaannokset haluttu_määrä etsittävä tiedosto_sisään");
			Console.WriteLine("esim: dotnet run kaannokset 100 *englanti: fiwiktionary-20190320-pages-articles-multistream.xml");
			Console.WriteLine("dotnet run kaannokset 100 \"* Finnish:\" enwiktionary-20190420-pages-articles-multistream.xml");
		}

		private static void PrintCreateJSONHelp()
		{
			Console.WriteLine("teejson haluttu_määrä etsittävä tiedosto_sisään estolista tiedosto_ulos");
			Console.WriteLine("esim: dotnet run teejson 100 *englanti: fiwiktionary-20190320-pages-articles-multistream.xml bannedwords.txt tulos.json");
		}

		private static string RemoveQuotesIfNeeded(string input)
		{
			if (input != null && input.Length > 1 && input[0] == '"' && input[input.Length - 1] == '"')
			{
				return input.Substring(1, input.Length - 2);
			}

			return input;
		}
	}
}
