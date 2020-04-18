// Jonas Vallat, David Rihs
// 18.04.2020

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris
{ 
    public partial class MainWindow : Window
    {
        private int[,] currentPiece = null;
        private bool nextPieceDrawed = false;
        private bool playing = false;
        private bool isGameOver = false;
        private int speed;
        private int score = 0;

        private int currentPieceWidth;
        private int currentPieceHeigth;
        private int currentPieceID;
        private int nextPieceID;
        private int rotation = 0;
        private bool rotated = false;
        private bool bottomCollision = false;
        private bool leftCollision = false;
        private bool rightCollision = false;

        private int rowCount = 0;
        private int columnCount = 0;
        private int left = 0;
        private int down = 0;
        private int gridColumn;
        private int gridRow;

        List<int> currentPieceRow = null;
        List<int> currentPieceColumn = null;

        Color[] piecesColors = { Colors.Cyan, Colors.Blue, Colors.Orange, Colors.Yellow, Colors.Green, Colors.Purple, Colors.Red };
        string[] piecesNames = { "", "I", "J", "L", "O", "S", "T", "Z" };

        //All diffetent pieces under all rotations possibles
        //O piece
        public int[,] OPiece0 = new int[2, 2] { { 1, 1 }, { 1, 1 } };

        //I Piece
        public int[,] IPiece0 = new int[2, 4] { { 1, 1, 1, 1 }, { 0, 0, 0, 0 } };
        public int[,] IPiece90 = new int[4, 2] { { 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 } };

        //T Piece
        public int[,] TPiece0 = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
        public int[,] TPiece90 = new int[3, 2] { { 1, 0 }, { 1, 1 }, { 1, 0 } };
        public int[,] TPiece180 = new int[2, 3] { { 1, 1, 1 }, { 0, 1, 0 } };
        public int[,] TPiece270 = new int[3, 2] { { 0, 1 }, { 1, 1 }, { 0, 1 } };

        //S Piece
        public int[,] SPiece0 = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
        public int[,] SPiece90 = new int[3, 2] { { 1, 0 }, { 1, 1 }, { 0, 1 } };

        //Z Piece
        public int[,] ZPiece0 = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
        public int[,] ZPiece90 = new int[3, 2] { { 0, 1 }, { 1, 1 }, { 1, 0 } };

        //J Piece
        public int[,] JPiece0 = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        public int[,] JPiece90 = new int[3, 2] { { 1, 1 }, { 1, 0 }, { 1, 0 } };
        public int[,] JPiece180 = new int[2, 3] { { 1, 1, 1 }, { 0, 0, 1 } };
        public int[,] JPiece270 = new int[3, 2] { { 0, 1 }, { 0, 1 }, { 1, 1 } };

        //L Piece
        public int[,] LPiece0 = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
        public int[,] LPiece90 = new int[3, 2] { { 1, 0 }, { 1, 0 }, { 1, 1 } };
        public int[,] LPiece180 = new int[2, 3] { { 1, 1, 1 }, { 1, 0, 0 } };
        public int[,] LPiece270 = new int[3, 2] { { 1, 1 }, { 0, 1 }, { 0, 1 } };

        private DispatcherTimer timer;
        private Random random;

        public MainWindow()
        {
            InitializeComponent();
            speed = 600;

            gridRow = tetrisGrid.RowDefinitions.Count;
            gridColumn = tetrisGrid.ColumnDefinitions.Count;

            random = new Random();              //random next piece
            currentPieceID = random.Next(1, 8);
            nextPieceID = random.Next(1, 8);

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, speed);
            timer.Tick += pieceFall;

            gameOverText.Visibility = Visibility.Collapsed;

            KeyDown += KeyDownEvents;
        }

        private void KeyDownEvents(object sender, KeyEventArgs e)
        {

            if (!timer.IsEnabled) { return; }
            switch (e.Key.ToString())
            {
                case "Up":      //rotate piece
                    if (rotation > 180)
                    {
                        rotation = 0;
                    }
                    else rotation += 90;
                    rotatePiece(rotation);
                    break;
                case "Down":      //get piece down faster
                    down++;
                    break;
                case "Left":      //rotate piece left
                    checkPieceCollision();
                    if (!leftCollision)
                    {
                        left--;
                    }
                    leftCollision = false;
                    break;
                case "Right":      //rotate piece right
                    checkPieceCollision();
                    if (!rightCollision)
                    {
                        left++;
                    }
                    rightCollision = false;
                    break;
            }
            movePiece();      //update piece position
        }

        private void buttonClick(object sender, RoutedEventArgs e)      //Start/pause game
        {
            if (isGameOver)     //prepare new game
            {
                tetrisGrid.Children.Clear();
                nextPieceCanvas.Children.Clear();
                isGameOver = false;
                gameOverText.Visibility = Visibility.Hidden;
            }
            if (!timer.IsEnabled)   //restart timer/reset score
            {
                if (!playing)
                {
                    left = 3;
                    newPiece(currentPieceID, left);
                    scoreText.Content = "SCORE : 0";
                }
                timer.Start();
                playing = true;
                startButton.Content = "STOP";
            }
            else    //stop timer
            {
                timer.Stop();
                startButton.Content = "START";
            }
        }

        private void newPiece(int shapeNumber, int preLeft = 0, int preDown = 0)   //create new piece to control
        {
            currentPieceRow = new List<int>();
            currentPieceColumn = new List<int>();
            Rectangle square;

            deletePiece();  //lose control of previous piece

            if (!rotated)   //get new piece type
            {
                currentPiece = getVariableByString(piecesNames[shapeNumber].ToString() + "Piece0");
            }

            int height = currentPiece.GetLength(0);
            int width = currentPiece.GetLength(1);

            currentPieceHeigth = height;
            currentPieceWidth = width;

            if ((currentPiece == IPiece90)||(currentPiece == IPiece0))
            {
                currentPieceWidth = 1;
            }

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    if (currentPiece[row, column]==1)
                    {
                        square = getBasicSquare(piecesColors[shapeNumber - 1]);
                        tetrisGrid.Children.Add(square);
                        square.Name = "movingSquare" + Grid.GetRow(square) + Grid.GetColumn(square);

                        if (preDown >= tetrisGrid.RowDefinitions.Count - currentPieceHeigth)
                        {
                            preDown = tetrisGrid.RowDefinitions.Count - currentPieceHeigth;
                        }

                        Grid.SetRow(square, rowCount + preDown);
                        Grid.SetColumn(square, columnCount + preLeft);
                        currentPieceRow.Add(rowCount + preDown);
                        currentPieceColumn.Add(columnCount + preLeft);
                    }
                    columnCount++;
                }
                rowCount++;
                columnCount = 0;
            }
            if (!nextPieceDrawed)
            {
                drawNextPiece(nextPieceID);
            }
            columnCount = 0;
            rowCount = 0;
        }

        private void movePiece()    //update piece position
        {
            leftCollision = false;
            rightCollision = false;

            checkPieceCollision();

            if (left > (gridColumn - currentPieceWidth))
            {
                left = (gridColumn - currentPieceWidth);
            }
            else if (left < 0)
            {
                left = 0;
            }

            if (bottomCollision)
            {
                pieceStoped();
            }
            else newPiece(currentPieceID, left, down);
        }

        private void rotatePiece(int rotationToDo)
        {
            if (!checkRotationCollision())
            {
                if (piecesNames[currentPieceID].IndexOf("I") == 0)
                {
                    if (rotationToDo > 90)
                    {
                        rotationToDo = 0;
                        rotation = 0;
                    }
                    currentPiece = getVariableByString("IPiece" + rotationToDo);
                }
                else if (piecesNames[currentPieceID].IndexOf("J") == 0)
                {
                    currentPiece = getVariableByString("JPiece" + rotationToDo);
                }
                else if (piecesNames[currentPieceID].IndexOf("L") == 0)
                {
                    currentPiece = getVariableByString("LPiece" + rotationToDo);
                }
                else if (piecesNames[currentPieceID].IndexOf("O") == 0)
                {
                    return;
                }
                else if (piecesNames[currentPieceID].IndexOf("S") == 0)
                {
                    if (rotationToDo > 90)
                    {
                        rotationToDo = rotation = 0;
                    }
                    currentPiece = getVariableByString("SPiece" + rotationToDo);
                }
                else if (piecesNames[currentPieceID].IndexOf("T") == 0)
                {
                    currentPiece = getVariableByString("TPiece" + rotationToDo);
                }
                else if (piecesNames[currentPieceID].IndexOf("Z") == 0)
                {
                    if (rotationToDo > 90)
                    {
                        rotationToDo = rotation = 0;
                    }
                    currentPiece = getVariableByString("ZPiece" + rotationToDo);
                }
                rotated = true;
                newPiece(currentPieceID, left, down);
            }
            else rotation -= 90;
        }

        private void pieceFall(object sender, EventArgs e)  //piece periodically goes down
        {
            down++;
            movePiece();
        }

        private void pieceStoped()  //piece stopped by ground or other pieces
        {
            timer.Stop();
            if (down > 2)
            {
                UIElement child;
                Rectangle square;
                for (int i=0;  i < tetrisGrid.Children.Count; i++)
                {
                    child = tetrisGrid.Children[i];
                    if (child is Rectangle)
                    {
                        square = (Rectangle)child;
                        if (square.Name.IndexOf("movingSquare") == 0)
                        {
                            square.Name = square.Name.Replace("movingSquare", "arrived_");
                        }
                    }
                }
                checkFullLine();
                reset();
                timer.Start();
            }
            else gameOver();
        }

        private void deletePiece()  //stop movement of the squares of the piece
        {
            UIElement child;
            Rectangle square;
            for (int i = 0; i < tetrisGrid.Children.Count; i++)
            {
                child = tetrisGrid.Children[i];
                if (child is Rectangle)
                {
                    square = (Rectangle)child;
                    if (square.Name.IndexOf("movingSquare") == 0)
                    {
                        tetrisGrid.Children.Remove(child);
                        i = -1;
                    }
                }
            }
        }
        private bool checkRotationCollision()   //check for collisions caused by rotation of piece
        {
            if ((checkCollided(0, currentPieceWidth - 1)) ||
                (checkCollided(0, -(currentPieceWidth - 1))) ||
                (checkCollided(0, -1)) ||
                (checkCollided(-1, currentPieceWidth - 1)) ||
                (checkCollided(1, currentPieceWidth - 1)))
            {
                return true;
            }
            else return false;
        }

        private void checkPieceCollision()  //update collisions values
        {
            leftCollision = checkCollided(-1, 0);
            rightCollision = checkCollided(1, 0);
            bottomCollision = checkCollided(0, 1);
        }

        private bool checkCollided(int leftRightOffset, int bottomOffset)   //check for collisions
        {
            Rectangle square;
            int squareColumn = 0;
            int squareRow = 0;

            for (int i = 0; i <= 3; i++)
            {
                try
                {
                    squareColumn = currentPieceColumn[i];
                    squareRow = currentPieceRow[i];
                    square = (Rectangle)tetrisGrid.Children.Cast<UIElement>()
                    .FirstOrDefault(e => Grid.GetRow(e) == squareRow + bottomOffset && Grid.GetColumn(e) == squareColumn + leftRightOffset);
                    if (square != null)
                    {
                        if (square.Name.IndexOf("arrived") == 0)
                        {
                            return true;
                        }
                    }
                }
                catch { }
            }
            if (down > (gridRow - currentPieceHeigth))
            {
                return true;
            }
            else return false;
        }

        private void drawNextPiece(int pieceID)     //display next piece
        {
            nextPieceCanvas.Children.Clear();

            Rectangle square;
            int x = 0;
            int y = 0;

            int[,] nextPiece;
            nextPiece = getVariableByString(piecesNames[pieceID] + "Piece0");

            int height = nextPiece.GetLength(0);
            int width = nextPiece.GetLength(1);

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    if (nextPiece[row, column] == 1)
                    {
                        square = getBasicSquare(piecesColors[pieceID - 1]);
                        nextPieceCanvas.Children.Add(square);
                        Canvas.SetTop(square, y);
                        Canvas.SetLeft(square, x);
                    }
                    x += 25;
                }
                x = 0;
                y += 25;
            }
            nextPieceDrawed = true;
        }

        private void checkFullLine()       //check for completed line
        {
            int squareCount = 0;
            int gridColumn = tetrisGrid.ColumnDefinitions.Count;
            int gridRow = tetrisGrid.RowDefinitions.Count;
            Rectangle square;
            for (int i = gridRow; i >= 0; i--)
            {
                squareCount = 0;
                for (int j = gridColumn; j >= 0; j--)
                {
                    square = (Rectangle)tetrisGrid.Children.Cast<UIElement>()
                   .FirstOrDefault(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
                    if (square != null)
                    {
                        if (square.Name.IndexOf("arrived") == 0)
                        {
                            squareCount++;
                        }
                    }
                }
                if (squareCount == gridColumn)  //line is full => delete line
                {
                    deleteFullLine(i);
                    scoreText.Content = "SCORE : " + getScore(true).ToString();
                    checkFullLine();
                }
            }
        }

        private void deleteFullLine(int row)    //delete complete line
        {
            int nbColumns = tetrisGrid.ColumnDefinitions.Count;
            Rectangle square;
            for (int i = 0; i < nbColumns; i++)
            {
                try
                {
                    square = (Rectangle)tetrisGrid.Children.Cast<UIElement>()
                   .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == i);
                    tetrisGrid.Children.Remove(square);
                }
                catch { }
            }
            foreach (UIElement element in tetrisGrid.Children)  //move down upper squares
            {
                square = (Rectangle)element;
                if (square.Name.IndexOf("arrived") == 0 && Grid.GetRow(square) <= row)
                {
                    Grid.SetRow(square, Grid.GetRow(square) + 1);
                }
            }
        }

        private Rectangle getBasicSquare(Color rectColor)   //create basic square that form all pieces
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Height = 25;
            rectangle.Width = 25;
            rectangle.Stroke = Brushes.White;
            rectangle.StrokeThickness = 1;
            rectangle.Fill = new SolidColorBrush(rectColor);
            return rectangle;
        }

        private int[,] getVariableByString(string variable)     // Get variable via string name
        {
            return (int[,])this.GetType().GetField(variable).GetValue(this);
        }

        private int getScore(bool toIncrease)   //get/increase score
        {
            if (toIncrease)
            {
                score += 50;
            }
            return score;
        }

        private void reset()    //reset some values when game is over or piece hit ground
        {
            rotation = 0;
            left = 3;
            down = 0;
            rotated = false;

            currentPieceID = nextPieceID;
            if (!isGameOver)
            {
                newPiece(currentPieceID, left);
            }

            nextPieceDrawed = false;
            random = new Random();
            nextPieceID = random.Next(1, 8);
            leftCollision = false;
            rightCollision = false;
            bottomCollision = false;
        }
        private void gameOver()
        {
            gameOverText.Visibility = Visibility.Visible;

            playing = false;
            isGameOver = true;
            reset();

            rowCount = 0;
            columnCount = 0;
            left = 0;
            score = 0;

            currentPieceID = random.Next(1, 8);
            nextPieceID = random.Next(1, 8);
            timer.Interval = new TimeSpan(0, 0, 0, 0, speed);

            startButton.Content = "START";
        }
    }


}