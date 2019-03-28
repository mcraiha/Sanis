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
					PrintHeadersHelp();
					return;
				}

				BigXMLProcess.PrintNFirstTranslations(args[2], int.Parse(args[1]));
			}

			
		}

		private static void PrintHeadersHelp()
		{
			Console.WriteLine("otsikot 100 tiedosto");
		}

		private static void PrintTranslationsHelp()
		{
			Console.WriteLine("kaannokset 100 tiedosto");
		}
	}
}
