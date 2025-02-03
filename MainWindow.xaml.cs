using System.Drawing;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CandyCrush
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button mainCandy = null;
        private Dictionary<Button, int> buttonID = new Dictionary<Button, int>();
        private Random rand = new Random();
        private Button[,] buttons = new Button[8, 8];
        private List<Button> adjacentX = new List<Button>();
        private List<Button> adjacentY = new List<Button>();


        public MainWindow()
        {
            InitializeComponent();
            GenerateButtons();
        }

        private Button[,] GenerateButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button button = new Button();
                    button.BorderThickness = new Thickness(3);
                    button.BorderBrush = Brushes.DarkBlue;

                    button.Click += ButtonClicked;

                    // Nastavení pozice v Gridu
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);

                    // Přidání tlačítka do Gridu
                    gridGame.Children.Add(button);

                    int buttonNumber = rand.Next(1, 6);
                    buttonID[button] = buttonNumber;

                    // Přidání tlačítka do pole tlačítek
                    buttons[i, j] = button;
                }
            }
            FixBoard();
            FixBoard();
            adjacentX.Clear();
            adjacentY.Clear();
            ColourBoard();
            return buttons;
        }
        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            Button clickedCandy = (Button)sender;

            if (mainCandy == null)
            {
                mainCandy = clickedCandy;
                mainCandy.BorderBrush = Brushes.Cyan;
            }
            else
            {
                var mainRow = Grid.GetRow(mainCandy);
                var mainCol = Grid.GetColumn(mainCandy);

                var clickedRow = Grid.GetRow(clickedCandy);
                var clickedCol = Grid.GetColumn(clickedCandy);

                if (CheckIfAdjacent(mainRow, mainCol, clickedRow, clickedCol))
                {
                    
                    int mainNumber = buttonID[mainCandy];
                    int clickedNumber = buttonID[clickedCandy];

                    buttonID[mainCandy] = clickedNumber;
                    buttonID[clickedCandy] = mainNumber;

                    if (!ValidateMove(clickedCandy) && !ValidateMove(mainCandy))
                    {
                        buttonID[mainCandy] = mainNumber;
                        buttonID[clickedCandy] = clickedNumber;

                        MessageBox.Show("Reconsider your move loser");  
                    }
                    else
                    {
                        if (ValidateMove(mainCandy))
                        {
                            BreakCandies();
                        }
                        if (ValidateMove(clickedCandy))
                        {
                            BreakCandies();
                        }
                    }
                    BreakCandies();
                    ColourBoard();
                }
                else
                {
                    MessageBox.Show("Please select an adjacent candy.");
                }

                mainCandy.BorderBrush = Brushes.DarkBlue;
                clickedCandy = null;
                mainCandy = null;
            }
        }

        private void ColourBoard()
        {
            foreach (var x in buttonID)
            {
                Button button = x.Key;
                int buttonNumber = x.Value;

                switch (buttonNumber)
                {
                    case 0:
                        button.Background = Brushes.LightGray;
                        break;
                    case 1:
                        
                        button.Background = Brushes.Red;
                        break;
                    case 2:
                        button.Background = Brushes.Blue;
                        break;
                    case 3:
                        button.Background = Brushes.Green;
                        break;
                    case 4:
                        button.Background = Brushes.Yellow;
                        break;
                    case 5:
                        button.Background = Brushes.Cyan;
                        break;
                    default:
                        break;
                }
            }
        }

        private bool CheckIfAdjacent(int mainx, int mainy, int secx, int secy)
        {
            if((Math.Abs(mainx - secx) == 1 && mainy == secy) || (Math.Abs(mainy - secy) == 1 && mainx == secx))
            {
                return true;
            }
            else { return false; }
        }

        private void FixBoard()
        {
            for (int i = 0; i < buttons.GetLength(0); i++)
            {
                for (int j = 0; j < buttons.GetLength(1); j++)
                {
                    int buttonColour = buttonID[buttons[i, j]];

                    int horizontalCount = CountContinuous(i, j, 0, 1, buttonID[buttons[i, j]]) + CountContinuous(i, j, 0, -1, buttonID[buttons[i, j]]);
                    
                    if(horizontalCount >= 2)
                    {
                        while (buttonID[buttons[i, j]] == buttonColour)
                        {
                            buttonID[buttons[i, j]] = rand.Next(1, 6);
                        }
                        
                        buttonColour = buttonID[buttons[i, j]];
                    }

                    int verticalCount = CountContinuous(i, j, 1, 0, buttonID[buttons[i, j]]) + CountContinuous(i, j, -1, 0, buttonID[buttons[i, j]]);

                    if(verticalCount >= 2)
                    {
                        while (buttonID[buttons[i, j]] == buttonColour)
                        {
                            buttonID[buttons[i, j]] = rand.Next(1, 6);
                        }
                    }
                }
            }
        }

        private int CountContinuous(int row, int col, int rowDelta, int colDelta, int swappedButtonID)
        {
            int count = 0;
            int x = row + rowDelta;
            int y = col + colDelta;

            while (IsValidPosition(x, y) && buttonID[buttons[x, y]] == swappedButtonID)
            {
                if (colDelta == 0) 
                {   
                    adjacentX.Add(buttons[x, y]); 
                }
                else if (rowDelta == 0) 
                {
                    adjacentY.Add(buttons[x, y]); 
                }
                count++;
                x += rowDelta;
                y += colDelta;
            }

            return count;
        }

        private bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }

        private bool ValidateMove(Button candy)
        {
            int row = Grid.GetRow(candy);
            int col = Grid.GetColumn(candy);

            int horizontalCount = CountContinuous(row, col, 1, 0, buttonID[candy]) + CountContinuous(row, col, -1, 0, buttonID[candy]);
            int verticalCount = CountContinuous(row, col, 0, 1, buttonID[candy]) + CountContinuous(row, col, 0, -1, buttonID[candy]);

            if(horizontalCount >= 2 || verticalCount >= 2) { return true; }

            return false;
        }

        private void BreakCandies()
        {
            for (int i = 0; i < buttons.GetLength(0); i++ )
            {
                for (int j = 0;  j < buttons.GetLength(1); j++)
                {

                    int z = buttonID[buttons[i, j]];
                    if (z == 0)
                    {
                        continue;
                    }

                    adjacentX.Clear();
                    adjacentY.Clear();

                    int x = CountContinuous(i, j, 1, 0, z) + CountContinuous(i, j, -1, 0, z);
                    if (x >= 2)
                    {
                        foreach (var brokenCandy in adjacentX)
                        {
                            buttonID[brokenCandy] = 0;
                        }
                    }

                    int y = CountContinuous(i, j, 0, 1, z) + CountContinuous(i, j, 0, -1, z);
                    if (y >= 2)
                    {

                        foreach (var brokenCandy in adjacentY)
                        {
                            buttonID[brokenCandy] = 0;
                        }
                    }

                    if (y >= 2 || x >= 2)
                    {
                        buttonID[buttons[i, j]] = 0;
                    }
                    MoveCandiesDown(buttons.GetLength(0) - 1, 0);
                }

            }
        }


        private void MoveCandiesDown(int r, int c)
        {
            for (int col = c; col < buttons.GetLength(1); col++)
            {
                for (int row = r; row > 0; row--)
                {
                    if (buttonID[buttons[row, col]] == 0)
                    {
                        if (buttonID[buttons[row - 1, col]] == 0) { MoveCandiesDown(row - 1, c); }
                        buttonID[buttons[row, col]] = buttonID[buttons[row - 1, col]];
                        buttonID[buttons[row - 1, col]] = 0;
                    }
                }
            }
            GenerateCandies();
        }

        private void GenerateCandies()
        {
            for (int i = 0; i < buttons.GetLength(0); i++)
            {
                for (int j = 0; j < buttons.GetLength(1); j++)
                {
                    if (buttonID[buttons[i, j]] == 0)
                    {
                        int newCandyID = rand.Next(1, 6);
                        buttonID[buttons[i, j]] = newCandyID;

                        ColourBoard();
                    }
                }
            }
        }
    }
}