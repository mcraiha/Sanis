using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Generoija
{
	public static class JsonProcess
	{
		public static void JuiceJsonFiles(string firstTranslationInputFilePath, string secondTranslationInputFilePath, string firstTranslationOutputFilePath, string secondTranslationOutputFilePath)
		{
			SortedDictionary<string, Translation> translationsFirst = JsonConvert.DeserializeObject<SortedDictionary<string, Translation>>(File.ReadAllText(firstTranslationInputFilePath));
			SortedDictionary<string, Translation> translationsSecond = JsonConvert.DeserializeObject<SortedDictionary<string, Translation>>(File.ReadAllText(secondTranslationInputFilePath));

			foreach (var pair in translationsFirst)
			{
				pair.Value.l = new string[pair.Value.t.Length];
				for (int i = 0; i < pair.Value.l.Length; i++)
				{
					if (translationsFirst.ContainsKey(pair.Value.t[i]) && translationsSecond.ContainsKey(pair.Value.t[i]))
					{
						pair.Value.l[i] = "b";
					}
					else if (translationsFirst.ContainsKey(pair.Value.t[i]))
					{
						pair.Value.l[i] = "o";
					}
					else if (translationsSecond.ContainsKey(pair.Value.t[i]))
					{
						pair.Value.l[i] = "t";
					}
					else
					{
						pair.Value.l[i] = "";
					}
				}
			}

			foreach (var pair in translationsSecond)
			{
				pair.Value.l = new string[pair.Value.t.Length];
				for (int i = 0; i < pair.Value.l.Length; i++)
				{
					if (translationsFirst.ContainsKey(pair.Value.t[i]) && translationsSecond.ContainsKey(pair.Value.t[i]))
					{
						pair.Value.l[i] = "b";
					}
					else if (translationsSecond.ContainsKey(pair.Value.t[i]))
					{
						pair.Value.l[i] = "o";
					}
					else if (translationsFirst.ContainsKey(pair.Value.t[i]))
					{
						pair.Value.l[i] = "t";
					}
					else
					{
						pair.Value.l[i] = "";
					}
				}
			}

			File.WriteAllText(firstTranslationOutputFilePath, JsonConvert.SerializeObject(translationsFirst, Formatting.None));

			File.WriteAllText(secondTranslationOutputFilePath, JsonConvert.SerializeObject(translationsSecond, Formatting.None));
		}
	}
}