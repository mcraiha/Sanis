using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Generoija
{
	public static class JsonProcess
	{
		public static void JuiceJsonFiles(string firstTranslationInputFilePath, string secondTranslationInputFilePath, string firstTranslationOutputFilePath, string secondTranslationOutputFilePath)
		{
			SortedDictionary<string, object> translationsFirst = JsonConvert.DeserializeObject<SortedDictionary<string, object>>(File.ReadAllText(firstTranslationInputFilePath));
			SortedDictionary<string, object> translationsFirstForModification = new SortedDictionary<string, object>(translationsFirst);
			Console.WriteLine($"Ensimmaisessa pareja yhteensa {translationsFirst.Count}");

			SortedDictionary<string, object> translationsSecond = JsonConvert.DeserializeObject<SortedDictionary<string, object>>(File.ReadAllText(secondTranslationInputFilePath));
			SortedDictionary<string, object> translationsSecondForModification = new SortedDictionary<string, object>(translationsSecond);
			Console.WriteLine($"Toisessa pareja yhteensa {translationsSecond.Count}");

			foreach (var pair in translationsFirst)
			{
				if (pair.Key.StartsWith("_"))
				{
					continue;
				}

				Translation translation = ((JObject)(pair.Value)).ToObject<Translation>();
				translation.l = new string[translation.t.Length];

				for (int i = 0; i < translation.l.Length; i++)
				{
					if (translationsFirst.ContainsKey(translation.t[i]) && translationsSecond.ContainsKey(translation.t[i]))
					{
						translation.l[i] = "b";
					}
					else if (translationsFirst.ContainsKey(translation.t[i]))
					{
						translation.l[i] = "o";
					}
					else if (translationsSecond.ContainsKey(translation.t[i]))
					{
						translation.l[i] = "t";
					}
					else
					{
						translation.l[i] = "";
					}
				}

				translationsFirstForModification[pair.Key] = translation;
			}

			foreach (var pair in translationsSecond)
			{
				if (pair.Key.StartsWith("_"))
				{
					continue;
				}

				Translation translation = ((JObject)(pair.Value)).ToObject<Translation>();
				translation.l = new string[translation.t.Length];

				for (int i = 0; i < translation.l.Length; i++)
				{
					if (translationsFirst.ContainsKey(translation.t[i]) && translationsSecond.ContainsKey(translation.t[i]))
					{
						translation.l[i] = "b";
					}
					else if (translationsSecond.ContainsKey(translation.t[i]))
					{
						translation.l[i] = "o";
					}
					else if (translationsFirst.ContainsKey(translation.t[i]))
					{
						translation.l[i] = "t";
					}
					else
					{
						translation.l[i] = "";
					}
				}

				translationsSecondForModification[pair.Key] = translation;
			}

			File.WriteAllText(firstTranslationOutputFilePath, JsonConvert.SerializeObject(translationsFirstForModification, Formatting.None));

			File.WriteAllText(secondTranslationOutputFilePath, JsonConvert.SerializeObject(translationsSecondForModification, Formatting.None));
		}
	}
}