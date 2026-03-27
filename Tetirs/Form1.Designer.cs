namespace Tetirs
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbGameField = new System.Windows.Forms.PictureBox();
            this.lblScore = new System.Windows.Forms.Label();
            this.pbNextBlock = new System.Windows.Forms.PictureBox();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.lbw1 = new System.Windows.Forms.Label();
            this.btnControl = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNextBlock)).BeginInit();
            this.SuspendLayout();
            // 
            // pbGameField
            // 
            this.pbGameField.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pbGameField.Location = new System.Drawing.Point(12, 23);
            this.pbGameField.Name = "pbGameField";
            this.pbGameField.Size = new System.Drawing.Size(250, 500);
            this.pbGameField.TabIndex = 0;
            this.pbGameField.TabStop = false;
            // 
            // lblScore
            // 
            this.lblScore.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblScore.Location = new System.Drawing.Point(268, 171);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(100, 29);
            this.lblScore.TabIndex = 1;
            this.lblScore.Text = "0";
            this.lblScore.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbNextBlock
            // 
            this.pbNextBlock.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pbNextBlock.Location = new System.Drawing.Point(268, 23);
            this.pbNextBlock.Name = "pbNextBlock";
            this.pbNextBlock.Size = new System.Drawing.Size(100, 100);
            this.pbNextBlock.TabIndex = 2;
            this.pbNextBlock.TabStop = false;
            this.pbNextBlock.Paint += new System.Windows.Forms.PaintEventHandler(this.pbNextBlock_Paint);
            // 
            // gameTimer
            // 
            this.gameTimer.Interval = 500;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // lbw1
            // 
            this.lbw1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lbw1.Font = new System.Drawing.Font("新愚公峥嵘体黑体版", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbw1.Location = new System.Drawing.Point(268, 126);
            this.lbw1.Name = "lbw1";
            this.lbw1.Size = new System.Drawing.Size(100, 45);
            this.lbw1.TabIndex = 1;
            this.lbw1.Text = "得分";
            this.lbw1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnControl
            // 
            this.btnControl.Location = new System.Drawing.Point(268, 217);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(99, 72);
            this.btnControl.TabIndex = 3;
            this.btnControl.Text = "开始游戏";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.btnControl_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 544);
            this.Controls.Add(this.btnControl);
            this.Controls.Add(this.pbNextBlock);
            this.Controls.Add(this.lbw1);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.pbGameField);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(400, 600);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "俄罗斯方块";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbGameField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNextBlock)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbGameField;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.PictureBox pbNextBlock;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label lbw1;
        private System.Windows.Forms.Button btnControl;
    }
}

