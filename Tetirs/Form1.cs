using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetirs
{
    public partial class Form1 : Form
    {
        GameEngine engine;

        int[,] map = new int[Rows, Cols];

        Block currentBlock;
        Block nextBlock;

        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            // 笔记本好像有个屏幕缩放的设置，通过这里统一一下窗体
            this.ClientSize = new Size(400, 520);
            this.MinimumSize = this.Size;

            engine = new GameEngine(20, 10);

            this.pbGameField.Paint += new PaintEventHandler(this.pbGameField_Paint);
            this.pbNextBlock.Paint += new PaintEventHandler(this.pbNextBlock_Paint);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            engine.GameTick();
            
            lblScore.Text = engine.Score.ToString();

            if (engine.IsGameOver)
        {
                gameTimer.Stop();
                DialogResult result = MessageBox.Show("游戏结束！要再来一局吗？", "俄罗斯方块", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
            {
                    engine.ResetGame();
                    engine.SpawnBlock();
                    btnControl.Text = "暂停游戏";
                    gameTimer.Start();
                }
                else
                {
                    this.Close();
                    }
                }
            }
            return true;
        }

            pbGameField.Invalidate();
            pbNextBlock.Invalidate();
                        }
                    }
                }
            }
        }

        //private void Form1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (currentBlock == null) return;

        //    switch (e.KeyCode)
        //    {
        //        case Keys.Left:
        //            if (IsValid(currentBlock.X - 1, currentBlock.Y)) currentBlock.X--;
        //            break;
        //        case Keys.Right:
        //            if (IsValid(currentBlock.X + 1, currentBlock.Y)) currentBlock.X++;
        //            break;
        //        case Keys.Down:
        //            if (IsValid(currentBlock.X, currentBlock.Y + 1)) currentBlock.Y++;
        //            break;
        //        case Keys.Up:
        //            currentBlock.Rotate();
        //            // 如果旋转后位置非法（比如撞墙），则尝试回滚
        //            if (!IsValid(currentBlock.X, currentBlock.Y))
        //            {
        //                // 简单的回滚：连续旋转三次回到原位
        //                for (int i = 0; i < 3; i++) currentBlock.Rotate();
        //            }
        //            break;
        //        case Keys.Space:
        //            if (currentBlock == null || !gameTimer.Enabled) return;
        //            while (IsValid(currentBlock.X, currentBlock.Y + 1))
        //            {
        //                currentBlock.Y++;
        //            }
        //            gameTimer_Tick(null, null);
        //            break;
        //    }
        //    pbGameField.Invalidate();
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (engine.CurrentBlock == null || !gameTimer.Enabled)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            switch (keyData)
            {
                case Keys.Left: engine.MoveLeft(); break;
                case Keys.Right: engine.MoveRight(); break;
                case Keys.Down: engine.MoveDown(); break;
                case Keys.Up: engine.RotateBlock(); break;
                case Keys.Space:
                    engine.DropToBottom();
                    gameTimer.Stop();
                    gameTimer.Start();
                    break;
            }

                case Keys.Right:
                    if (IsValid(currentBlock.X + 1, currentBlock.Y)) currentBlock.X++;
                    pbGameField.Invalidate();
            pbNextBlock.Invalidate();
                    return true;

                case Keys.Down:
                    if (IsValid(currentBlock.X, currentBlock.Y + 1)) currentBlock.Y++;
                    pbGameField.Invalidate();
                    return true;

                case Keys.Up:
                    currentBlock.Rotate();
                    if (!IsValid(currentBlock.X, currentBlock.Y))
                        for (int i = 0; i < 3; i++) currentBlock.Rotate();
                    pbGameField.Invalidate();
                    return true;

                case Keys.Space:
                    while (IsValid(currentBlock.X, currentBlock.Y + 1))
                    {
                        currentBlock.Y++;
                    }
                    gameTimer_Tick(null, null);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void pbGameField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            float CellSize = (float)pbGameField.Width / engine.Cols;

            // 1. 画背景网格
            Pen gridPen = new Pen(Color.FromArgb(40, 255, 255, 255));
            for (int i = 0; i <= engine.Rows; i++) g.DrawLine(gridPen, 0, i * CellSize, engine.Cols * CellSize, i * CellSize);
            for (int j = 0; j <= engine.Cols; j++) g.DrawLine(gridPen, j * CellSize, 0, j * CellSize, engine.Rows * CellSize);

            // 2. 画已经固定的地图方块
            for (int i = 0; i < engine.Rows; i++)
                {
                for (int j = 0; j < engine.Cols; j++)
                    {
                    if (engine.Map[i, j] != 0)
                        {
                        Color fixedColor = Color.FromArgb(engine.Map[i, j]);
                        float drawX = j * CellSize;
                        float drawY = i * CellSize;

                        g.FillRectangle(new SolidBrush(fixedColor), drawX, drawY, CellSize - 1, CellSize - 1);
                        g.DrawRectangle(Pens.White, drawX, drawY, CellSize - 1, CellSize - 1);
            }
        }

        private void ResetGame()
        {
            map = new int[Rows, Cols];
            score = 0;
            lblScore.Text = "0";
            btnControl.Text = "开始游戏";
            SpawnBlock();
            pbGameField.Invalidate();
        }

            // 3. 画当前正在下落的方块
            if (engine.CurrentBlock != null)
        {
                int size = engine.CurrentBlock.Shape.GetLength(0);
                for (int i = 0; i < size; i++)
            {
                    for (int j = 0; j < size; j++)
                {
                        if (engine.CurrentBlock.Shape[i, j] == 1)
                    {
                            float drawX = (engine.CurrentBlock.X + j) * CellSize;
                            float drawY = (engine.CurrentBlock.Y + i) * CellSize;

                            g.FillRectangle(new SolidBrush(engine.CurrentBlock.BlockColor), drawX, drawY, CellSize - 1, CellSize - 1);
                        g.DrawRectangle(Pens.White, drawX, drawY, CellSize - 1, CellSize - 1);
                    }
                }
            }
            }

            // 4. 画幽灵方块投影 (Ghost Piece)
            if (engine.CurrentBlock != null)
            {
                int size = engine.CurrentBlock.Shape.GetLength(0);
                int ghostY = engine.GetGhostY(); // 问引擎要幽灵方块的 Y 坐标

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (engine.CurrentBlock.Shape[i, j] == 1)
                        {
                            float drawX = (engine.CurrentBlock.X + j) * CellSize;
                            float drawY = (ghostY + i) * CellSize;

                            // 幽灵方块画成只有边缘的白框，内部是黑色的 (透明感)
                            g.DrawRectangle(Pens.White, drawX, drawY, CellSize - 1, CellSize - 1);

                            // 如果你想给内部加一点淡淡的同色半透明，可以解开下面这行的注释：
                            // g.FillRectangle(new SolidBrush(Color.FromArgb(40, engine.CurrentBlock.BlockColor)), drawX, drawY, CellSize - 1, CellSize - 1);
                        }
                    }
                }
            }
        }

        private void pbNextBlock_Paint(object sender, PaintEventArgs e)
        {
            if (engine.NextBlock == null) return;

            Graphics g = e.Graphics;

            // 【修改核心】：不再强行使用主游戏区的格子大小！
            // 取小窗口宽和高中的最小值，除以 5 (给 4x4 的方块留出边缘空白，防止贴边)
            float cellSize = Math.Min(pbNextBlock.Width, pbNextBlock.Height) / 5f;

            int size = engine.NextBlock.Shape.GetLength(0);

            // 1. 扫描方块：找出实体部分的真正边界
            int minX = size, maxX = -1, minY = size, maxY = -1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (engine.NextBlock.Shape[i, j] == 1)
                    {
                        if (j < minX) minX = j;
                        if (j > maxX) maxX = j;
                        if (i < minY) minY = i;
                        if (i > maxY) maxY = i;
                    }
                    else
                    {
                        this.Close();
                    }
                    return;
                }

            // 如果没有实体（按理说不可能发生），直接退出
            if (maxX == -1) return;

            // 2. 计算实体方块占据的实际宽度和高度
            float actualWidth = (maxX - minX + 1) * cellSize;
            float actualHeight = (maxY - minY + 1) * cellSize;

            // 3. 计算绝对居中的偏移量
            float offsetX = (pbNextBlock.Width - actualWidth) / 2f - minX * cellSize;
            float offsetY = (pbNextBlock.Height - actualHeight) / 2f - minY * cellSize;

            // 4. 加上偏移量画出方块
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (engine.NextBlock.Shape[i, j] == 1)
                    {
                        float drawX = offsetX + j * cellSize;
                        float drawY = offsetY + i * cellSize;

                        g.FillRectangle(new SolidBrush(engine.NextBlock.BlockColor), drawX, drawY, cellSize - 1, cellSize - 1);
                        g.DrawRectangle(Pens.White, drawX, drawY, cellSize - 1, cellSize - 1);
                    }
                }
            }
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            if (btnControl.Text == "继续游戏")
            {
                gameTimer.Start();
                btnControl.Text = "暂停游戏";
                this.Focus();
            }
            else if (btnControl.Text == "暂停游戏")
            {
                gameTimer.Stop();
                btnControl.Text = "继续游戏";
            }
            else if (btnControl.Text == "开始游戏")
            {
                engine.ResetGame();
                engine.SpawnBlock();
                gameTimer.Stop();
                gameTimer.Start();
                btnControl.Text = "暂停游戏";
                pbGameField.Invalidate();
                pbNextBlock.Invalidate();
                this.Focus();
            }

            this.ActiveControl = null;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // 0. 检查控件是否已创建
            if (pbGameField == null || pbNextBlock == null || btnControl == null) return;

            // 1. 【核心修复】：定义固定的游戏区大小，不随拉伸改变
            // 如果你设计器里改了大小，这里也要同步改
            int gameWidth = 250;
            int gameHeight = 500;
            int sidebarWidth = 110;
            int gap = 20;

            // 2. 计算总宽度（游戏区 + 间距 + 侧边栏）
            int totalContentWidth = gameWidth + gap + sidebarWidth;
            int startX = Math.Max(10, (this.ClientSize.Width - totalContentWidth) / 2);
            int startY = Math.Max(10, (this.ClientSize.Height - gameHeight) / 2);

            // 4. 重新排布位置
            pbGameField.SetBounds(startX, startY, gameWidth, gameHeight);

            int sidebarX = startX + gameWidth + gap;

            if (lbw1 != null) lbw1.Location = new Point(sidebarX, startY);
            pbNextBlock.Location = new Point(sidebarX, (lbw1 != null) ? lbw1.Bottom + 5 : startY);
            lblScore.Location = new Point(sidebarX, pbNextBlock.Bottom + 20);
            btnControl.Location = new Point(sidebarX, lblScore.Bottom + 20);
        }

        private void Form1_Load(object sender, EventArgs e) { }
    }
}
