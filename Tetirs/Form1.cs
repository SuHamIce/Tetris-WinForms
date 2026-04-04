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

            btnControl.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 80, 85);

            // 1. 窗体全局背景设为高级碳黑色
            this.BackColor = Color.FromArgb(32, 33, 36);

            // 2. 标签字体变白，背景变透明（完美融入背景）
            if (lblScore != null)
            {
                lblScore.ForeColor = Color.White;
                lblScore.BackColor = Color.Transparent;
                lblScore.Font = new Font("Consolas", 12, FontStyle.Bold); // 顺手加粗一下字体
            }

            if (lbw1 != null)
            {
                lbw1.ForeColor = Color.White;
                lbw1.BackColor = Color.Transparent;
            }

            // 3. 按钮扁平化（去掉老土的 3D 凸起感）
            if (btnControl != null)
            {
                btnControl.FlatStyle = FlatStyle.Flat;
                btnControl.BackColor = Color.FromArgb(50, 50, 55);
                btnControl.ForeColor = Color.White;
                btnControl.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 100);
                btnControl.Cursor = Cursors.Hand; // 鼠标移上去变成小手
            }

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
            lblScore.Text = $"{engine.Score}";
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
                    lblScore.Text = "0";
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

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            float CellSize = (float)pbGameField.Width / engine.Cols;

            // 1. 画背景网格
            Pen gridPen = new Pen(Color.FromArgb(40, 255, 255, 255)); // 保持 40 的极低透明度
            gridPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot; // 【灵魂所在】改成点状虚线！
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

                        float padding = 1.5f; // 给方块留出物理间隙
                        g.FillRectangle(new SolidBrush(fixedColor), drawX + padding, drawY + padding, CellSize - padding * 2, CellSize - padding * 2);
                        g.DrawRectangle(new Pen(Color.FromArgb(60, 0, 0, 0), 2), drawX + padding, drawY + padding, CellSize - padding * 2, CellSize - padding * 2);
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

                            float padding = 1.5f;
                            g.FillRectangle(new SolidBrush(engine.CurrentBlock.BlockColor), drawX + padding, drawY + padding, CellSize - padding * 2, CellSize - padding * 2);
                            g.DrawRectangle(new Pen(Color.FromArgb(60, 0, 0, 0), 2), drawX + padding, drawY + padding, CellSize - padding * 2, CellSize - padding * 2);
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
                           
                            Color ghostColor = engine.CurrentBlock.BlockColor;
                            float padding = 1.5f;

                            g.FillRectangle(new SolidBrush(Color.FromArgb(30, ghostColor)), drawX + padding, drawY + padding, CellSize - padding * 2, CellSize - padding * 2);
                            g.DrawRectangle(new Pen(Color.FromArgb(150, ghostColor), 2), drawX + padding, drawY + padding, CellSize - padding * 2, CellSize - padding * 2);
                        }
                    }
                }
            }

            // 5. 画一圈科技感外边框 (类似街机屏幕的边框)
            Pen screenBorder = new Pen(Color.FromArgb(100, 255, 255, 255), 2); // 半透明银白色，宽度2
            g.DrawRectangle(screenBorder, 1, 1, pbGameField.Width - 2, pbGameField.Height - 2);
            
            if (engine.IsGameOver)
            {
                
                g.FillRectangle(new SolidBrush(Color.FromArgb(180, 0, 0, 0)), 0, 0, pbGameField.Width, pbGameField.Height);

                Font gameOverFont = new Font("Impact", 24, FontStyle.Bold);
                SizeF textSize = g.MeasureString("游戏结束", gameOverFont);
                float textX = (pbGameField.Width - textSize.Width) / 2;
                float textY = (pbGameField.Height - textSize.Height) / 2;

                g.DrawString("游戏结束", gameOverFont, Brushes.Red, textX, textY);
            }
        }

        private void pbNextBlock_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            Pen smallBorder = new Pen(Color.FromArgb(100, 255, 255, 255), 2);
            g.DrawRectangle(smallBorder, 1, 1, pbNextBlock.Width - 2, pbNextBlock.Height - 2);

            if (engine.NextBlock == null) return;

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

                        float padding = 1.5f;
                        g.FillRectangle(new SolidBrush(engine.NextBlock.BlockColor), drawX + padding, drawY + padding, cellSize - padding * 2, cellSize - padding * 2);
                        g.DrawRectangle(new Pen(Color.FromArgb(60, 0, 0, 0), 2), drawX + padding, drawY + padding, cellSize - padding * 2, cellSize - padding * 2);
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
                lblScore.Text = "0";

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