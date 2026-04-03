using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tetirs
{
    public class Block
    {
        public int[,] Shape;
        public Color BlockColor;
        public int X, Y;

        public int Type { get; private set; }
        private bool isVertical = false;
        
        private static Random rand = new Random();
        private static List<Color> colorBag = new List<Color>();
        private static readonly Color[] palette = { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Purple, Color.Cyan, Color.Yellow };

        private static Color GetNextColor()
        {
            if (colorBag.Count == 0)
            {
                colorBag.AddRange(palette);
                int n = colorBag.Count;
                while (n > 1)
                {
                    n--;
                    int k = rand.Next(n + 1);
                    Color value = colorBag[k];
                    colorBag[k] = colorBag[n];
                    colorBag[n] = value;
                }
            }
            Color nextColor = colorBag[0];
            colorBag.RemoveAt(0);
            return nextColor;
        }

        public Block(int type)
        {
            Type = type;
            X = 3;
            Y = 0;

            // 根据类型初始化不同的方块 (标准的 7 种)
            switch (type)
            {
                case 0: // I型
                    Shape = new int[4, 4] { { 0, 0, 0, 0 }, { 1, 1, 1, 1 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
                    break;
                case 1: // J型
                    Shape = new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
                    break;
                case 2: // L型
                    Shape = new int[3, 3] { { 0, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 } };
                    break;
                case 3: // O型
                    Shape = new int[2, 2] { { 1, 1 }, { 1, 1 } };
                    break;
                case 4: // S型
                    Shape = new int[3, 3] { { 0, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 } };
                    break;
                case 5: // T型
                    Shape = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
                    break;
                case 6: // Z型
                    Shape = new int[3, 3] { { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
                    break;
            }

            this.BlockColor = GetNextColor();
        }

        // 旋转逻辑（完美适配不同大小的矩阵）
        public void Rotate()
        {
            if (Type == 3)
            {
                return;
            }
            else if (Type == 0 || Type == 4 || Type == 6)
            {
                isVertical = !isVertical;
                if (Type == 0) // I型
                {
                    if (isVertical)
                        Shape = new int[4, 4] { { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 } };
                    else
                        Shape = new int[4, 4] { { 0, 0, 0, 0 }, { 1, 1, 1, 1 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
                }
                else if (Type == 4) // S型
                {
                    if (isVertical)
                        Shape = new int[3, 3] { { 0, 1, 0 }, { 0, 1, 1 }, { 0, 0, 1 } };
                    else
                        Shape = new int[3, 3] { { 0, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 } };
                }
                else if (Type == 6) // Z型
                {
                    if (isVertical)
                        Shape = new int[3, 3] { { 0, 0, 1 }, { 0, 1, 1 }, { 0, 1, 0 } };
                    else
                        Shape = new int[3, 3] { { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
                }
            }
            else
            {
            int size = Shape.GetLength(0);
            int[,] newShape = new int[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    newShape[i, j] = Shape[size - 1 - j, i];
            Shape = newShape;
        }
    }

        public void RollbackRotate()
        {
            if (Type == 3) return;

            if (Type == 0 || Type == 4 || Type == 6)
            {
                Rotate();
            }
            else
            {
                for (int i = 0; i < 3; i++) Rotate();
            }
        }
    }
}