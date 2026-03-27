using System;
using System.Drawing;

namespace Tetirs
{
    public class Block
    {
        public int[,] Shape;
        public Color BlockColor;
        public int X, Y;

        public Block(int type)
        {
            // 初始位置：顶部居中
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
            Random colorRand = new Random();
            Color[] palette = { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Purple, Color.Cyan, Color.Yellow };
            this.BlockColor = palette[colorRand.Next(palette.Length)];
        }

        // 旋转逻辑（完美适配不同大小的矩阵）
        public void Rotate()
        {
            int size = Shape.GetLength(0);
            int[,] newShape = new int[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    newShape[i, j] = Shape[size - 1 - j, i];
            Shape = newShape;
        }
    }
}