using System;
using System.IO;
using System.Collections.Generic;
using Discord;

namespace gamebot
{
	public class Hangman
	{
		static Random rnd = new Random();
		static string curdur = Directory.GetCurrentDirectory();

		public Hangman()
		{
			WordChooser();
		}

		public string WordChooser() // Chooses a random word from a custom dictionary
		{
			var dictionary = File.ReadAllLines(curdur + "/words.txt"); // grabs array of lines in 'words.txt' file

			int r = rnd.Next(dictionary.Length); // picks a random int based on the number of words
			string word = (string)dictionary[r]; // grab string at the selected int

			return word; // return the string back to the program
		}
	}
}
