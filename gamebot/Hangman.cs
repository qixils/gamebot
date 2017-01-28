using System;
using Discord;
using System.Collections.Generic;

namespace gamebot
{
	public class Hangman
	{
		static Random rnd = new Random();

		public string WordChooser()
		{
			var words = new List<string> { "stabyourself", "meme", "is that saso", "reunion" };
			// ^ list of words
			int r = rnd.Next(words.Count); // picks a random int based on the number of words
			string word = (string)words[r]; // grab string at the selected int

			return word; // return the string back to the program
		}
	}
}

