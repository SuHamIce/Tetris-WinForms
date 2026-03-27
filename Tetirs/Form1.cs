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
        const int Rows = 20;
        const int Cols = 10;
        // const int CellSize = 25;

        int[,] map = new int[Rows, Cols];

        Block currentBlock;
        Block nextBlock;

        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            this.pbGameField.Paint += new PaintEventHandler(this.pbGameField_Paint);
            this.pbNextBlock.Paint += new PaintEventHandler(this.pbNextBlock_Paint);
        }

        // 生成新方块的逻辑
        private void SpawnBlock()
        {
            
            if (nextBlock == null)
            {
                nextBlock = new Block(rand.Next(0, 7));
            }

            currentBlock = nextBlock;

            currentBlock.X = Cols / 2 - 2;
            currentBlock.Y = 0;

            nextBlock = new Block(rand.Next(0, 7));

            pbGameField.Invalidate();
            pbNextBlock.Invalidate();
        }

        private bool IsValid(int targetX, int targetY)
        {
            int size = currentBlock.Shape.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                  
                    if (currentBlock.Shape[i, j] == 1)
                    {
                        int nextX = targetX + j;
                        int nextY = targetY + i;

                        // 1. 检查是否撞到了左右墙壁
                        if (nextX < 0 || nextX >= Cols)
                            return false;

                        // 2. 检查是否撞到了地板
                        if (nextY >= Rows)
                            return false;

                        // 3. 检查是否和地图上已有的方块重叠（非0代表有方块）
                        // 注意：只有当 nextY >= 0 时才去地图里查，因为方块刚生成时可能有一部分在屏幕上方
                        if (nextY >= 0 && map[nextY, nextX] != 0)
                            return false;
                    }
                }
            }
            return true;
        }

        private void FreezeBlock()
        {
            int size = currentBlock.Shape.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (currentBlock.Shape[i, j] == 1)
                    {
                        int mapX = currentBlock.X + j;
                        int mapY = currentBlock.Y + i;

                        if (mapY >= 0 && mapY < Rows && mapX >= 0 && mapX < Cols)
                        {
                            map[mapY, mapX] = currentBlock.BlockColor.ToArgb();
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
            if (currentBlock == null || !gameTimer.Enabled)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            switch (keyData)
            {
                case Keys.Left:
                    if (IsValid(currentBlock.X - 1, currentBlock.Y)) currentBlock.X--;
                    pbGameField.Invalidate();
                    return true;

                case Keys.Right:
                    if (IsValid(currentBlock.X + 1, currentBlock.Y)) currentBlock.X++;
                    pbGameField.Invalidate();
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

        int score = 0; // 在类顶部声明得分变量

        private void CheckLines()
        {
            int linesCleared = 0;

            for (int i = Rows - 1; i >= 0; i--) // 从底向上扫描
            {
                bool isFull = true;
                for (int j = 0; j < Cols; j++)
                {
                    if (map[i, j] == 0) // 只要有一个空格，这一行就没满
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    linesCleared++;
                    for (int k = i; k > 0; k--)
                    {
                        for (int j = 0; j < Cols; j++)
                        {
                            map[k, j] = map[k - 1, j];
                        }
                    }
                    // 最顶行清空
                    for (int j = 0; j < Cols; j++) map[0, j] = 0;

                    i++; // 关键：行下移后，需要重新检查当前这一行（因为新掉下来的行也可能是满的）
                }
            }

            if (linesCleared > 0)
            {
                // 经典的计分规则：1行100，2行300，3行500，4行800
                int[] bonus = { 0, 100, 300, 500, 800 };
                score += bonus[linesCleared];
                lblScore.Text = $"{score}";
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

        private void pbGameField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; // 获取画笔对象

            float currentSize = (float)pbGameField.Width / Cols;
            int CellSize = (int)currentSize;

            // 画背景暗色网格 (为了视觉上的完美，方便玩家对齐)
            Pen gridPen = new Pen(Color.FromArgb(40, 255, 255, 255)); // 半透明的白线
            for (int i = 0; i <= Rows; i++)
                g.DrawLine(gridPen, 0, i * CellSize, Cols * CellSize, i * CellSize);
            for (int j = 0; j <= Cols; j++)
                g.DrawLine(gridPen, j * CellSize, 0, j * CellSize, Rows * CellSize);


            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (map[i, j] != 0) // 非0说明有方块
                    {
                        Color fixedColor = Color.FromArgb(map[i, j]);

                        float drawX = j * CellSize;
                        float drawY = i * CellSize;

                        g.FillRectangle(new SolidBrush(fixedColor), drawX, drawY, CellSize - 1, CellSize - 1);
                        g.DrawRectangle(Pens.White, drawX, drawY, CellSize - 1, CellSize - 1);
                    }
                }
            }

            // 遍历当前方块的 2D 数组，把为 1 的地方画出来
            if (currentBlock != null)
            {
                int size = currentBlock.Shape.GetLength(0); // 拿到矩阵大小 (3或4)

                for (int i = 0; i < size; i++)     // 遍历行 (对应 Y)
                {
                    for (int j = 0; j < size; j++) // 遍历列 (对应 X)
                    {
                        if (currentBlock.Shape[i, j] == 1) // 发现方块的实体
                        {
                            int drawX = (currentBlock.X + j) * CellSize;
                            int drawY = (currentBlock.Y + i) * CellSize;

                            Rectangle rect = new Rectangle(drawX, drawY, CellSize, CellSize);
                            g.FillRectangle(new SolidBrush(currentBlock.BlockColor), rect);

                            g.DrawRectangle(Pens.White, rect);
                        }
                    }
                }
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (currentBlock == null) return;

            if (IsValid(currentBlock.X, currentBlock.Y + 1))
            {
                currentBlock.Y++;
            }
            else
            {
                FreezeBlock();
                CheckLines();

                if (!IsValid(Cols / 2 - 2, 0))
                {
                    gameTimer.Stop();
                    DialogResult result = MessageBox.Show("游戏结束！要再来一局吗？", "俄罗斯方块", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        ResetGame();
                    }
                    else
                    {
                        this.Close();
                    }
                    return;
                }

                SpawnBlock();
            }
            pbGameField.Invalidate();
        }

        private void pbNextBlock_Paint(object sender, PaintEventArgs e)
        {
            if (nextBlock == null) return;

            Graphics g = e.Graphics;
            float previewCellSize = pbNextBlock.Width / 4f;

            int size = nextBlock.Shape.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (nextBlock.Shape[i, j] == 1)
                    {
                        float drawX = j * previewCellSize;
                        float drawY = i * previewCellSize;

                        g.FillRectangle(new SolidBrush(nextBlock.BlockColor), drawX, drawY, previewCellSize - 1, previewCellSize - 1);
                        g.DrawRectangle(Pens.White, drawX, drawY, previewCellSize - 1, previewCellSize - 1);
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
                SpawnBlock();
                gameTimer.Start();
                btnControl.Text = "暂停游戏";
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
            int sidebarWidth = 110; // 侧边栏（按钮和预览框）的大致宽度
            int gap = 20;

            // 2. 计算总宽度（游戏区 + 间距 + 侧边栏）
            int totalContentWidth = gameWidth + gap + sidebarWidth;

            // 3. 计算起始坐标
            int startX = (this.ClientSize.Width - totalContentWidth) / 2;
            int startY = (this.ClientSize.Height - gameHeight) / 2;

            // 如果窗口太小，坐标不能为负，否则会消失
            startX = Math.Max(10, startX);
            startY = Math.Max(10, startY);

            // 4. 重新排布位置
            pbGameField.SetBounds(startX, startY, gameWidth, gameHeight);

            int sidebarX = startX + gameWidth + gap;

            if (lbw1 != null) lbw1.Location = new Point(sidebarX, startY);
            pbNextBlock.Location = new Point(sidebarX, (lbw1 != null) ? lbw1.Bottom + 5 : startY);
            lblScore.Location = new Point(sidebarX, pbNextBlock.Bottom + 20);
            btnControl.Location = new Point(sidebarX, lblScore.Bottom + 20);
        }
    }
}
