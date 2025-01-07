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
        private Label[,] player1Card;
        private Label[,] player2Card;
        private List<int> bingoNumbers = new List<int>();
        private DispatcherTimer gameTimer;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Interval = TimeSpan.FromSeconds(5);
        }



        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            for(int row = 0; row < 5; row++)
            {
                for(int col = 0; col < 5; col++) 
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
                            randomNumber = random.Next(1, 16);
                        } while(usedNumbers.Contains(randomNumber));
                        break;
                    case 1:
                        do
                        {
                            randomNumber = random.Next(16, 31);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 2:
                        do
                        {
                            randomNumber = random.Next(31, 46);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 3:
                        do
                        {
                            randomNumber = random.Next(46, 61);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 4:
                        do
                        {
                            randomNumber = random.Next(61, 76);
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
            player1Card = GeneratePlayerCard(bingoGrid);
            // player2Card = GeneratePlayerCard(bingoGrid2);

            bingoNumbers.Clear();
            numbersListBox.Items.Clear();

            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            int randomNumber = 0;
            do
            {
                randomNumber = random.Next(1, 76);
            } while(bingoNumbers.Contains(randomNumber));

            bingoNumbers.Add(randomNumber);
            numbersListBox.Items.Add(randomNumber.ToString());

            if (CheckNumber(randomNumber, player1Card) == true)
            {
                if(CheckFullRowOrColumn(player1Card) == true)
                {
                    gameTimer.Stop();
                    MessageBox.Show("Gewonnen!");
                }
            }
            //CheckNumber(randomNumber, player2Card);
        }

        private bool CheckNumber(int number, Label[,] playerCard)
        {
            foreach(Label label in playerCard)
            {
                if(label != null && label.Content.Equals(number.ToString()))
                {
                    label.Background = Brushes.Red;
                    return true;
                }
            }

            return false;
        }

        private bool CheckFullRowOrColumn(Label[,] playerCard)
        {
            for(int row = 0; row < 5; row++) 
            {
                int sum = 0;
                for(int col = 0; col < 5; col++)
                {
                    if(playerCard[row, col].Background == Brushes.Red)
                    {
                        sum++;
                    }
                    else
                    {
                        break;
                    }
                }
                if(sum == 5)
                {
                    return true;
                }
            }

            return false;
        }


    }
}
