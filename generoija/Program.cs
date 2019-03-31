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
				if (args.Length != 3)
				{
					PrintTranslationsHelp();
					return;
				}

				BigXMLProcess.PrintNFirstTranslations(args[2], int.Parse(args[1]));
			}
			else if (command.Equals("teejson"))
			{
				if (args.Length != 4)
				{
					PrintCreateJSONHelp();
					return;
				}

				BigXMLProcess.CreateNFirstTranslationsJSON(args[2], int.Parse(args[1]), args[3]);
			}
			
		}

		private static void PrintHeadersHelp()
		{
			Console.WriteLine("otsikot haluttu_määrä tiedosto");
			Console.WriteLine("esim: dotnet run otsikot 2000 fiwiktionary-20190320-pages-articles-multistream.xml");
		}

		private static void PrintTranslationsHelp()
		{
			Console.WriteLine("kaannokset haluttu_määrä tiedosto");
			Console.WriteLine("esim: dotnet run kaannokset 100 fiwiktionary-20190320-pages-articles-multistream.xml");
		}

		private static void PrintCreateJSONHelp()
		{
			Console.WriteLine("teejson haluttu_määrä tiedosto_sisään tiedosto_ulos");
			Console.WriteLine("esim: dotnet run teejson 100 fiwiktionary-20190320-pages-articles-multistream.xml tulos.json");
		}
	}
}
