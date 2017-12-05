using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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
    public partial class Leaderboard : Window
    {
        private Boolean click;
        public Leaderboard()
        {
            InitializeComponent();
        }
        private void highscores() {
            click = !click;
            if (File.Exists("../../highscores.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream Fs = new FileStream("../../highscores.dat", FileMode.Open, FileAccess.Read);
                try
                {
                    BinaryFormatter F = new BinaryFormatter();

                    string a = (string)F.Deserialize(Fs);
                }
                catch (SerializationException em)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + em.Message);

                }
                finally
                {
                    Fs.Close();
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (i == 0) { 
                    h1_scores.Text += "Top 10 Scores" + Environment.NewLine;
                    h1_scores.Text += "1st" + lines[i] + Environment.NewLine;
                }
                if (i == 1)
                    h1_scores.Text += "2nd" + lines[i] + Environment.NewLine;
                if (i == 2)
                    h1_scores.Text += "3rd" + lines[i] +Environment.NewLine;
                if (i == 3)
                    h1_scores.Text += "4th" + lines[i] + Environment.NewLine;
                if (i == 4)
                    h1_scores.Text += "5th" + lines[i] + Environment.NewLine;
                if (i == 5)
                    h1_scores.Text += "6th" + lines[i] + Environment.NewLine;
                if (i == 6)
                    h1_scores.Text += "7th" + lines[i] + Environment.NewLine;
                if (i == 7)
                    h1_scores.Text += "8th" + lines[i] + Environment.NewLine;
                if (i == 8)
                    h1_scores.Text += "9th" + lines[i] + Environment.NewLine;
                if (i == 9)
                    h1_scores.Text += "10th" + lines[i] + Environment.NewLine;
            
            }
        }
    }
}
