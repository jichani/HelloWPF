using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ch28_영어단어맞추기
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
        private string wrongStatus;
        private string selectedEng;
        private string selectedKor;
        private string messageText;
        private List<char>btns = new List<char>();

        public string WrongStatus {
            get => wrongStatus;
            set {
                wrongStatus = value;
                OnPropertyChanged("WrongStatus");
            }
        }

        public string SelectedEng
        {
            get => selectedEng;
            set
            {
                selectedEng = value;
                OnPropertyChanged("selectedEng");
            }
        }
        public string SelectedKor
        {
            get => selectedKor;
            set
            {
                selectedKor = value;
                OnPropertyChanged("selectedKor");
            }
        }
        public string MessageText
        {
            get => messageText;
            set
            {
                messageText = value;
                OnPropertyChanged("messageText");
            }
        }
        List<char> SelectedWord = new List<char>();
        List<string> words = new List<string>()
        {
            "boy,소년",
            "school,학교",
            "fish,물고기",
            "car,자동차",
            "book,책",
        };
        int wrong = 0;
        int maxWrong = 3;
        string compareWord = string.Empty;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            btns.AddRange("abcdefghijklmnopqrstuvwxyz");
            ic.ItemsSource = btns;
            RandomWord();
            ChangeWord(SelectedEng, SelectedWord);
            WrongStatus = $"틀린횟수: {wrong} of {maxWrong}";
        }

        private void ChangeWord(string selectedEng,List<char> selWord)
        {
            char[] result = selectedEng.Select(x=>(selWord.IndexOf(x)>=0 ? x: '*')).ToArray();
            SelectedEng = string.Join(' ',result);
        }

        private void RandomWord()
        {
            string[] selChar = words[new Random().Next(0, words.Count)].Split(",");
            selectedEng = selChar[0];
            selectedKor = selChar[1];
            compareWord = selectedEng;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if(btn != null) 
            {
                var result = btn.Content.ToString();
                CheckWord(result[0]);
            }
        }

        private void CheckWord(char v)
        {
            if(SelectedWord.IndexOf(v) == -1)
            {
                SelectedWord.Add(v);
            }
            if(compareWord.IndexOf(v) >= 0)
            {
                ChangeWord(compareWord, SelectedWord);
                CheckWin();
            }
            else if (compareWord.IndexOf(v) == -1)
            {
                wrong++;
                Status();
                CheckLost();
            }
        }

        private void Status()
        {
            WrongStatus = $"틀린횟수: {wrong} of {maxWrong}";
        }

        private void CheckWin()
        {
            if(compareWord == SelectedEng.Replace(" ", ""))
            {
                MessageText = "You Win!";
            }
        }
        private void CheckLost()
        {
            if(wrong == maxWrong)
            {
                MessageText = "You Lost!";
                foreach(var a in ic.Items)
                {
                    var btn = (UIElement)ic.ItemContainerGenerator.ContainerFromItem(a);
                    if(btn != null)
                    {
                        btn.IsEnabled = false;
                    }
                }
            }
        }
    }
}
