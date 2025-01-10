using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Threading;

namespace Bingo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Label[,] _player1Card;
        private Label[,] _player2Card;
        private List<int> _bingoNumbers = new List<int>();
        private DispatcherTimer _gameTimer;
        private Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            _gameTimer = new DispatcherTimer();
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Interval = TimeSpan.FromSeconds(2);
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            InitializeGrid(player1Grid);
            InitializeGrid(player2Grid);
        }

        private void InitializeGrid(Grid bingoGrid)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (row == 2 && col == 2)
                    {
                        Image image = new Image();
                        image.Source = new BitmapImage(new Uri(@"logoPXL.png", UriKind.RelativeOrAbsolute));
                        image.Stretch = Stretch.Uniform;
                        Grid.SetColumn(image, col);
                        Grid.SetRow(image, row);
                        bingoGrid.Children.Add(image);
                    }
                    else
                    {
                        Label label = new Label();
                        Grid.SetColumn(label, col);
                        Grid.SetRow(label, row);
                        label.HorizontalAlignment = HorizontalAlignment.Stretch;
                        label.VerticalAlignment = VerticalAlignment.Stretch;
                        label.HorizontalContentAlignment = HorizontalAlignment.Center;
                        label.VerticalContentAlignment = VerticalAlignment.Center;
                        label.BorderBrush = Brushes.Black;
                        label.BorderThickness = new Thickness(1, 1, 1, 1);
                        bingoGrid.Children.Add(label);
                    }
                }
            }
        }

        private Label[,] GeneratePlayerCard(Grid grid)
        {
            Label[,] playerCard = new Label[5, 5];

            Label[] gridLabels = grid.Children.OfType<Label>().ToArray();
            List<int> usedNumbers = new List<int>();

            foreach (Label label in gridLabels)
            {
                int row = Grid.GetRow(label);
                int col = Grid.GetColumn(label);

                int randomNumber = 0;
                    
                switch (col)
                {
                    case 0:
                        do
                        {
                            randomNumber = _random.Next(1, 16);
                        } while(usedNumbers.Contains(randomNumber));
                        break;
                    case 1:
                        do
                        {
                            randomNumber = _random.Next(16, 31);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 2:
                        do
                        {
                            randomNumber = _random.Next(31, 46);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 3:
                        do
                        {
                            randomNumber = _random.Next(46, 61);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 4:
                        do
                        {
                            randomNumber = _random.Next(61, 76);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                }

                usedNumbers.Add(randomNumber);
                label.Background = Brushes.Transparent;
                label.Content = randomNumber.ToString();
                playerCard[row, col] = label;
            }


            return playerCard;
        }

        private void OnNewGameClicked(object sender, RoutedEventArgs e)
        {
            _player1Card = GeneratePlayerCard(player1Grid);
            _player2Card = GeneratePlayerCard(player2Grid);

            _bingoNumbers.Clear();
            numbersListBox.Items.Clear();

            _gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            int randomNumber = 0;
            do
            {
                randomNumber = _random.Next(1, 76);
            } while(_bingoNumbers.Contains(randomNumber));

            _bingoNumbers.Add(randomNumber);
            numbersListBox.Items.Add(randomNumber.ToString());
            numbersListBox.ScrollIntoView(randomNumber);

            if (CheckNumber(randomNumber, _player1Card, out int row, out int col))
            {
                if(CheckFullRowOrColumn(_player1Card, row, col))
                {
                    _gameTimer.Stop();
                    MessageBox.Show("Speler 1 gewonnen!");
                }
            }
            if (CheckNumber(randomNumber, _player2Card, out int row2, out int col2))
            {
                if (CheckFullRowOrColumn(_player2Card, row, col))
                {
                    _gameTimer.Stop();
                    MessageBox.Show("Speler 2 gewonnen!");
                }
            }
            
        }

        private bool CheckNumber(int number, Label[,] playerCard, out int row, out int col)
        {
            row = 0;
            col = 0;
            foreach(Label label in playerCard)
            {
                if(label != null && label.Content.Equals(number.ToString()))
                {
                    label.Background = Brushes.Red;
                    row = Grid.GetRow(label);
                    col = Grid.GetColumn(label);
                    return true;
                }
            }

            return false;
        }

        private bool CheckFullRowOrColumn(Label[,] playerCard, int rowToCheck, int colToCheck)
        {
       
            bool fullRow = true;
            for(int col = 0; col < 5; col++)
            {
                if (playerCard[rowToCheck, col] != null)
                {
                    if (playerCard[rowToCheck, col].Background != Brushes.Red)
                    {
                        fullRow = false;
                        break;
                    }
                }
            }

            if(fullRow)
            {
                return true;
            }
            

      
            bool fullCol = true;
            for (int row = 0; row < 5; row++) 
            {
                if (playerCard[row, colToCheck] != null)
                {
                    if (playerCard[row, colToCheck].Background != Brushes.Red)
                    {
                        fullCol = false;
                        break;
                    }
                }
            }

            if (fullCol)
            {
                return true;
            }
            

            return false;
        }


    }
}
