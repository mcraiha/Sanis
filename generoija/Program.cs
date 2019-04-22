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

				BigXMLProcess.PrintNFirstTranslations(args[3], int.Parse(args[1]), args[2]);
			}
			else if (command.Equals("teejson"))
			{
				if (args.Length != 6)
				{
					PrintCreateJSONHelp();
					return;
				}

				BigXMLProcess.CreateNFirstTranslationsJSON(args[3], int.Parse(args[1]), args[2], args[4], args[5]);
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
		}

		private static void PrintCreateJSONHelp()
		{
			Console.WriteLine("teejson haluttu_määrä etsittävä tiedosto_sisään estolista tiedosto_ulos");
			Console.WriteLine("esim: dotnet run teejson 100 *englanti: fiwiktionary-20190320-pages-articles-multistream.xml bannedwords.txt tulos.json");
		}
	}
}
