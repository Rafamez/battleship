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
{/*
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// 
    /// </summary>
    public partial class Leaderboard : Window
    {
        private Boolean click;
        private string a = "";
        private List<String> lines = new List<String>();
        private String[] linesArray;
        public Leaderboard()
        {
            InitializeComponent();
        }
      /*  private void highscores() {
            click = !click;
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
                linesArray = new String[lines.Count+1];
                //copy the contents into a array
                for (int i = 0; i < lines.Count; i++) {
                    linesArray[i]=lines[i];
                }
                int b = Array.IndexOf(linesArray, "Score: ");
                String[] scores = new String[linesArray.Length];
                //getting the scores so you can compare the scores and sort it after 
                for (int j = 0; j < linesArray.Length; j++)
                {
                    scores[j] = Regex.Match(linesArray[j], @"\d+").Value;

                }
                
                // Regex regex = new Regex(@"^\d[4]");
               // Pattern p = Pattern.compile("\\d+");
  
                //Array.Sort(linesArray);
            }
          
            for (int i = 0; i < 10; i++)
            {
                if (i == 0) { 
                    Highscores_1.Text += "Top 10 Scores" + Environment.NewLine;
                    Highscores_1.Text += "1st" + lines[i] + Environment.NewLine;
                }
                if (i == 1)
                    Highscores_1.Text += "2nd" + lines[i] + Environment.NewLine;
                if (i == 2)
                    Highscores_1.Text += "3rd" + lines[i] +Environment.NewLine;
                if (i == 3)
                    Highscores_1.Text += "4th" + lines[i] + Environment.NewLine;
                if (i == 4)
                    Highscores_1.Text += "5th" + lines[i] + Environment.NewLine;
                if (i == 5)
                    Highscores_1.Text += "6th" + lines[i] + Environment.NewLine;
                if (i == 6)
                    Highscores_1.Text += "7th" + lines[i] + Environment.NewLine;
                if (i == 7)
                    Highscores_1.Text += "8th" + lines[i] + Environment.NewLine;
                if (i == 8)
                    Highscores_1.Text += "9th" + lines[i] + Environment.NewLine;
                if (i == 9)
                    Highscores_1.Text += "10th" + lines[i] + Environment.NewLine;
            
            }
<<<<<<< HEAD
        }
        public static void QuickSort<T>(T[] data, int left, int right)
where T : IComparable<T>
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
=======
        }
>>>>>>> 9f952af1047bcd993fe2d33d317fc14134a99f1b
    }*/
}
