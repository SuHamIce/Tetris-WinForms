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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblScore = new System.Windows.Forms.Label();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.lbw1 = new System.Windows.Forms.Label();
            this.btnControl = new System.Windows.Forms.Button();
            this.pbNextBlock = new System.Windows.Forms.PictureBox();
            this.pbGameField = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbNextBlock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameField)).BeginInit();
            this.SuspendLayout();
            // 
            // lblScore
            // 
            this.lblScore.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblScore.Font = new System.Drawing.Font("字魂27号-布丁体", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblScore.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblScore.Location = new System.Drawing.Point(270, 204);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(100, 40);
            this.lblScore.TabIndex = 1;
            this.lblScore.Text = "0";
            this.lblScore.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // gameTimer
            // 
            this.gameTimer.Interval = 500;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // lbw1
            // 
            this.lbw1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbw1.Font = new System.Drawing.Font("字魂27号-布丁体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbw1.ForeColor = System.Drawing.SystemColors.Info;
            this.lbw1.Location = new System.Drawing.Point(268, 23);
            this.lbw1.Name = "lbw1";
            this.lbw1.Size = new System.Drawing.Size(100, 63);
            this.lbw1.TabIndex = 1;
            this.lbw1.Text = "下一个方块";
            this.lbw1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnControl
            // 
            this.btnControl.Location = new System.Drawing.Point(271, 265);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(99, 72);
            this.btnControl.TabIndex = 3;
            this.btnControl.Text = "开始游戏";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.btnControl_Click);
            // 
            // pbNextBlock
            // 
            this.pbNextBlock.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pbNextBlock.Location = new System.Drawing.Point(269, 89);
            this.pbNextBlock.Name = "pbNextBlock";
            this.pbNextBlock.Size = new System.Drawing.Size(100, 100);
            this.pbNextBlock.TabIndex = 2;
            this.pbNextBlock.TabStop = false;
            this.pbNextBlock.Paint += new System.Windows.Forms.PaintEventHandler(this.pbNextBlock_Paint);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(400, 600);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "俄罗斯方块";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbNextBlock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameField)).EndInit();
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

