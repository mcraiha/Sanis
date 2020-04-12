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
				if (args.Length != 3 && args.Length != 4)
				{
					PrintHeadersHelp();
					return;
				}

				if (args.Length == 3)
				{
					BigXMLProcess.PrintNFirst(inputXMLfilePath: args[2], howMany: int.Parse(args[1]));
				}
				else if (args.Length == 4)
				{
					BigXMLProcess.PrintNFirst(inputXMLfilePath: args[2], howMany: int.Parse(args[1]), outputFilePath: args[3]);
				}
			}
			else if (command.Equals("kaannokset"))
			{
				if (args.Length != 4)
				{
					PrintTranslationsHelp();
					return;
				}

				BigXMLProcess.PrintNFirstTranslations(inputXMLfilePath: args[3], howMany: int.Parse(args[1]), translationLanguage: RemoveQuotesIfNeeded(args[2]));
			}
			else if (command.Equals("teejson"))
			{
				if (args.Length != 6)
				{
					PrintCreateJSONHelp();
					return;
				}

				BigXMLProcess.CreateNFirstTranslationsJSON(inputXMLfilePath: args[3], howMany: int.Parse(args[1]), translationLanguage: args[2], bannedWordsListPath: args[4], outputFilePath: args[5]);
			}
			else if (command.Equals("mehusta"))
			{
				if (args.Length != 5)
				{
					PrintCreateJuicedJSONsHelp();
					return;
				}

				JsonProcess.JuiceJsonFiles(args[1], args[2], args[3], args[4]);
			}
		}

		private static void PrintHeadersHelp()
		{
			Console.WriteLine("otsikot haluttu_määrä tiedosto_sisään [mahdollinen_tiedosto_ulos]");
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

		private static void PrintCreateJuicedJSONsHelp()
		{
			Console.WriteLine("mehustus kielitiedosto1.json kielitiedosto2.json kielitiedosto1_mehustettu.json kielitiedosto2_mehustettu.json");
			Console.WriteLine("esim: dotnet run finnish.json english.json finnish_boosted.json english_boosted.json");
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
