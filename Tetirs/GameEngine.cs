using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tetirs
{
    public class GameEngine
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public int[,] Map { get; private set; }
        public Block CurrentBlock { get; private set; }
        public Block NextBlock { get; private set; }
        public int Score { get; private set; }
        public bool IsGameOver { get; private set; }

        private Random rand = new Random();

        // 7-Bag 随机算法的“形状袋子”
        private List<int> pieceBag = new List<int>();

        public GameEngine(int rows = 20, int cols = 10)
        {
            Rows = rows;
            Cols = cols;
            ResetGame();
        }

        public void ResetGame()
        {
            Map = new int[Rows, Cols];
            Score = 0;
            IsGameOver = false;
            CurrentBlock = null;
            NextBlock = null;
            pieceBag.Clear();
        }

        private int GetNextPieceType()
        {
            if (pieceBag.Count == 0)
            {
                for (int i = 0; i < 7; i++) pieceBag.Add(i);

                int n = pieceBag.Count;
                while (n > 1)
                {
                    n--;
                    int k = rand.Next(n + 1);
                    int value = pieceBag[k];
                    pieceBag[k] = pieceBag[n];
                    pieceBag[n] = value;
                }
            }
            int type = pieceBag[0];
            pieceBag.RemoveAt(0);
            return type;
        }

        public void SpawnBlock()
        {
            if (NextBlock == null) NextBlock = new Block(GetNextPieceType());
            CurrentBlock = NextBlock;
            CurrentBlock.X = Cols / 2 - 2;
            CurrentBlock.Y = 0;

            if (CurrentBlock.Type == 0)
            {
                CurrentBlock.Y = -1;
            }

            NextBlock = new Block(GetNextPieceType());

            if (!IsValid(CurrentBlock.X, CurrentBlock.Y))
            {
                IsGameOver = true;
            }
        }

        public int GetGhostY()
        {
            if (CurrentBlock == null) return 0;
            int ghostY = CurrentBlock.Y;
            while (IsValid(CurrentBlock.X, ghostY + 1))
            {
                ghostY++;
            }
            return ghostY;
        }

        private bool IsValid(int targetX, int targetY)
        {
            int size = CurrentBlock.Shape.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (CurrentBlock.Shape[i, j] == 1)
                    {
                        int nextX = targetX + j;
                        int nextY = targetY + i;

                        if (nextX < 0 || nextX >= Cols) return false;
                        if (nextY >= Rows) return false;
                        if (nextY >= 0 && Map[nextY, nextX] != 0) return false;
                    }
                }
            }
            return true;
        }

        private void FreezeBlock()
        {
            int size = CurrentBlock.Shape.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (CurrentBlock.Shape[i, j] == 1)
                    {
                        int mapX = CurrentBlock.X + j;
                        int mapY = CurrentBlock.Y + i;
                        if (mapY >= 0 && mapY < Rows && mapX >= 0 && mapX < Cols)
                        {
                            Map[mapY, mapX] = CurrentBlock.BlockColor.ToArgb();
                        }
                    }
                }
            }
        }

        private void CheckLines()
        {
            int linesCleared = 0;
            for (int i = Rows - 1; i >= 0; i--)
            {
                bool isFull = true;
                for (int j = 0; j < Cols; j++)
                {
                    if (Map[i, j] == 0) { isFull = false; break; }
                }

                if (isFull)
                {
                    linesCleared++;
                    for (int k = i; k > 0; k--)
                    {
                        for (int j = 0; j < Cols; j++) Map[k, j] = Map[k - 1, j];
                    }
                    for (int j = 0; j < Cols; j++) Map[0, j] = 0;
                    i++;
                }
            }

            if (linesCleared > 0)
            {
                int[] bonus = { 0, 100, 300, 500, 800 };
                Score += bonus[linesCleared];
            }
        }

        public void MoveLeft()
        {
            if (IsValid(CurrentBlock.X - 1, CurrentBlock.Y)) CurrentBlock.X--;
        }

        public void MoveRight()
        {
            if (IsValid(CurrentBlock.X + 1, CurrentBlock.Y)) CurrentBlock.X++;
        }

        public bool MoveDown()
        {
            if (IsValid(CurrentBlock.X, CurrentBlock.Y + 1))
            {
                CurrentBlock.Y++;
                return true;
            }
            return false;
        }

        public void RotateBlock()
        {
            CurrentBlock.Rotate();
            if (IsValid(CurrentBlock.X, CurrentBlock.Y)) return;

            if (IsValid(CurrentBlock.X - 1, CurrentBlock.Y)) { CurrentBlock.X -= 1; return; }
            if (IsValid(CurrentBlock.X + 1, CurrentBlock.Y)) { CurrentBlock.X += 1; return; }

            if (IsValid(CurrentBlock.X + 2, CurrentBlock.Y)) { CurrentBlock.X += 2; return; }

            if (!IsValid(CurrentBlock.X, CurrentBlock.Y + 1) && IsValid(CurrentBlock.X, CurrentBlock.Y - 1))
            {
                CurrentBlock.Y -= 1;
                return;
            }

            CurrentBlock.RollbackRotate();
        }

        public void DropToBottom()
        {
            while (MoveDown()) { }
            GameTick();
        }

        public void GameTick()
        {
            if (CurrentBlock == null || IsGameOver) return;

            if (!MoveDown())
            {
                FreezeBlock();
                CheckLines();
                SpawnBlock();
            }
        }
    }
}