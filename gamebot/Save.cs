using Newtonsoft.Json;
using System;
using System.IO;

namespace gamebot
{
	public class Save
	{
		public static string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar}gamebot{Path.DirectorySeparatorChar}";
		public static void Saves(object s, string file)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			File.WriteAllText(path + file, JsonConvert.SerializeObject(s));
		}
		public static T Load<T>(string file)
		{
			if (!File.Exists(path + file))
				throw new FileNotFoundException();
			Console.WriteLine(File.ReadAllText(path + file));
			return JsonConvert.DeserializeObject<T>(File.ReadAllText(path + file));
		}
	}
}
