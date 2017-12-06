using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace battleship
{
	/// <summary>
	/// Interaction logic for Leaderboard.xaml
	/// 
	/// </summary>
	public partial class Leaderboard : Window, ISerializable
	{
		private Boolean click;
		private string a = "";
		private List<String> lines = new List<String>();
		private String[] linesArray;
		public Leaderboard()
		{
			InitializeComponent();
		}
		private void highscores()
		{
			click = !click;
			//DESERIALIZING THE FILE 
			if (File.Exists("../../highscores.txt"))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream Fs = new FileStream("../../highscores.txt", FileMode.Open, FileAccess.Read);
				try
				{
					BinaryFormatter F = new BinaryFormatter();

					a = (string)F.Deserialize(Fs);
				}
				catch (SerializationException em)
				{
					Console.WriteLine("Failed to deserialize. Reason: " + em.Message);

				}
				finally
				{
					Fs.Close();
				}
				//Copy Contents of string a into a list 
				lines = a.Split('*').ToList();
				//use the length of the lines list for the new linesArray
				linesArray = new String[lines.Count + 1];
				//copy the contents into a array
				for (int i = 0; i < lines.Count; i++)
				{
					linesArray[i] = lines[i];
				}
				//Array Needed to get the scores 
				String[] scores = new String[linesArray.Length];
				//COUNT FOR GETTING THE SCORES 
				int count = 0;



				//getting the scores so you can compare the scores and sort the linesArray after 
				foreach (string line in linesArray)
				{
					// creating another array so that you can get the values 
					string[] hole = line.Split(' ');
					//you need to find and copy and put only the scores into the scores array
					if (count == 0)
						scores[count] = hole[count];
					if (count % 2 == 0)
					{
						scores[count - 1] = hole[count];
					}
					count++;
				}
				//sort the array for scores 
				Array.Sort(scores);
				//try to match the scores array with the lines array if does not not find and replace the instance in the other indexes of the array 
				for (int i = 0; i < scores.Length; i++)
				{
					//check if the lines arrays have the right score if they do dont sort and go to next
					if (linesArray[i].Contains(scores[i]) == true)
					{
						continue;
					}
					//if it isnt true then you will need to swap
					else
					{
						//loop to find if the scores from linesArray is false 
						for (int j = 0; j < linesArray.Length; j++)
						{
							//keeps going until you find the linesArray index that matches the score
							if (linesArray[j].Contains(scores[i]) == false)
							{
								continue;
							}
							//when found swap the indexs of the linesArray[i] with linesArray[j](the proper order)
							else
							{
								swap(linesArray[i], linesArray[j]);
							}
						}
					}

				}
			}
			//get the top 10 highscores and then display them into the highscores_1 text block
			for (int i = 0; i < 10; i++)
			{
				if (i == 0)
				{
					Highscores_1.Text += "Top 10 Scores" + Environment.NewLine;

					Highscores_1.Text += "Name" + "Score" + "Time" + Environment.NewLine;

					Highscores_1.Text += "1st" + linesArray[i] + Environment.NewLine;
				}
				if (i == 1)
					Highscores_1.Text += "2nd" + linesArray[i] + Environment.NewLine;
				if (i == 2)
					Highscores_1.Text += "3rd" + linesArray[i] + Environment.NewLine;
				if (i == 3)
					Highscores_1.Text += "4th" + linesArray[i] + Environment.NewLine;
				if (i == 4)
					Highscores_1.Text += "5th" + linesArray[i] + Environment.NewLine;
				if (i == 5)
					Highscores_1.Text += "6th" + linesArray[i] + Environment.NewLine;
				if (i == 6)
					Highscores_1.Text += "7th" + linesArray[i] + Environment.NewLine;
				if (i == 7)
					Highscores_1.Text += "8th" + linesArray[i] + Environment.NewLine;
				if (i == 8)
					Highscores_1.Text += "9th" + linesArray[i] + Environment.NewLine;
				if (i == 9)
					Highscores_1.Text += "10th" + linesArray[i] + Environment.NewLine;

			}

		}
		//created method to sort a list 
		//
		public static void QuickSort<T>(T[] data, int left, int right) where T : IComparable<T>
		{
			T temp;
			int i, j;
			T pivot;
			i = left;
			j = right;
			pivot = data[(left + right) / 2];
			do
			{
				while ((data[i].CompareTo(pivot) < 0) && (i < right)) i++;
				while ((pivot.CompareTo(data[j]) < 0) && (j > left)) j--;
				if (i <= j)
				{
					temp = data[i];
					data[i] = data[j];
					data[j] = temp;
					i++;
					j--;
				}
			} while (i <= j);

			if (left < j) QuickSort(data, left, j);
			if (i < right) QuickSort(data, i, right);
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}


		//method to swap indexes 
		public static void swap(String a, String b)
		{
			String temp = a;
			a = b;
			b = temp;
		}
	}
}

