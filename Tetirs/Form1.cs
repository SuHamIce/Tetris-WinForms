using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetirs
{
    public partial class Form1 : Form
    {
        GameEngine engine;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ClientSize = new Size(400, 520);
            this.MinimumSize = this.Size;

            engine = new GameEngine(20, 10);

            this.pbGameField.Paint += new PaintEventHandler(this.pbGameField_Paint);
            this.pbNextBlock.Paint += new PaintEventHandler(this.pbNextBlock_Paint);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            engine.GameTick();

            // UI 同步更新
            lblScore.Text = $"得分：{engine.Score}";
            pbGameField.Invalidate();
            pbNextBlock.Invalidate();

            // 游戏结束判定
            if (engine.IsGameOver)
            {
                gameTimer.Stop();
                DialogResult result = MessageBox.Show("游戏结束！要再来一局吗？", "俄罗斯方块", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    engine.ResetGame();
                    engine.SpawnBlock();
                    lblScore.Text = "得分：0";
                    btnControl.Text = "暂停游戏";
                    gameTimer.Start();
                }
                else
                {
                    this.Close();
                }
            }
        }

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

            pbGameField.Invalidate();
            pbNextBlock.Invalidate();
            return true;
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
                int ghostY = engine.GetGhostY();

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (engine.CurrentBlock.Shape[i, j] == 1)
                        {
                            float drawX = (engine.CurrentBlock.X + j) * CellSize;
                            float drawY = (ghostY + i) * CellSize;
                            g.DrawRectangle(Pens.White, drawX, drawY, CellSize - 1, CellSize - 1);
                        }
                    }
                }
            }
        }

        private void pbNextBlock_Paint(object sender, PaintEventArgs e)
        {
            if (engine.NextBlock == null) return;

            Graphics g = e.Graphics;
            float cellSize = Math.Min(pbNextBlock.Width, pbNextBlock.Height) / 5f;

            int size = engine.NextBlock.Shape.GetLength(0);

            // 1. 扫描方块：找出实体边界
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
                }
            }

            if (maxX == -1) return;

            // 2. 计算居中偏移量
            float actualWidth = (maxX - minX + 1) * cellSize;
            float actualHeight = (maxY - minY + 1) * cellSize;
            float offsetX = (pbNextBlock.Width - actualWidth) / 2f - minX * cellSize;
            float offsetY = (pbNextBlock.Height - actualHeight) / 2f - minY * cellSize;

            // 3. 画出预览方块
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
                lblScore.Text = "得分：0";

                pbGameField.Invalidate();
                pbNextBlock.Invalidate();
                this.Focus();
            }

            this.ActiveControl = null;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (pbGameField == null || pbNextBlock == null || btnControl == null) return;

            int gameWidth = 250;
            int gameHeight = 500;
            int sidebarWidth = 110;
            int gap = 20;

            int totalContentWidth = gameWidth + gap + sidebarWidth;
            int startX = Math.Max(10, (this.ClientSize.Width - totalContentWidth) / 2);
            int startY = Math.Max(10, (this.ClientSize.Height - gameHeight) / 2);

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