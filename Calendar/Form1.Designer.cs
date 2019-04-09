namespace Calendar
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
            this.titlePanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.dateSelectPanel = new System.Windows.Forms.Panel();
            this.weekPanel = new System.Windows.Forms.Panel();
            this.calendarContentPanel = new System.Windows.Forms.Panel();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.lunarInfoPanel = new System.Windows.Forms.Panel();
            this.noteTextBox = new System.Windows.Forms.TextBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.updataButton = new System.Windows.Forms.Button();
            this.noteLabel = new System.Windows.Forms.Label();
            this.titlePanel.SuspendLayout();
            this.dateSelectPanel.SuspendLayout();
            this.calendarContentPanel.SuspendLayout();
            this.lunarInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.Purple;
            this.titlePanel.Controls.Add(this.titleLabel);
            this.titlePanel.Location = new System.Drawing.Point(21, 23);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(272, 66);
            this.titlePanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.titleLabel.Location = new System.Drawing.Point(50, 21);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(154, 24);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "简易版万年历";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Font = new System.Drawing.Font("等线", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dateLabel.Location = new System.Drawing.Point(189, 19);
            this.dateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(232, 31);
            this.dateLabel.TabIndex = 1;
            this.dateLabel.Text = "0000年00月00日";
            this.dateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dateLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dateLabel_MouseClick);
            // 
            // dateSelectPanel
            // 
            this.dateSelectPanel.BackColor = System.Drawing.Color.Purple;
            this.dateSelectPanel.Controls.Add(this.dateLabel);
            this.dateSelectPanel.Location = new System.Drawing.Point(21, 124);
            this.dateSelectPanel.Name = "dateSelectPanel";
            this.dateSelectPanel.Size = new System.Drawing.Size(648, 62);
            this.dateSelectPanel.TabIndex = 2;
            this.dateSelectPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.dateSelectPanel_Paint);
            this.dateSelectPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dateSelectPanel_MouseClick);
            // 
            // weekPanel
            // 
            this.weekPanel.BackColor = System.Drawing.Color.Purple;
            this.weekPanel.Location = new System.Drawing.Point(21, 186);
            this.weekPanel.Name = "weekPanel";
            this.weekPanel.Size = new System.Drawing.Size(648, 51);
            this.weekPanel.TabIndex = 4;
            this.weekPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.weekPanel_Paint);
            // 
            // calendarContentPanel
            // 
            this.calendarContentPanel.BackColor = System.Drawing.Color.Purple;
            this.calendarContentPanel.Controls.Add(this.monthCalendar1);
            this.calendarContentPanel.Location = new System.Drawing.Point(21, 236);
            this.calendarContentPanel.Name = "calendarContentPanel";
            this.calendarContentPanel.Size = new System.Drawing.Size(648, 304);
            this.calendarContentPanel.TabIndex = 5;
            this.calendarContentPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.calendarContentPanel_Paint);
            this.calendarContentPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.calendarContentPanel_MouseClick);
            this.calendarContentPanel.MouseLeave += new System.EventHandler(this.calendarContentPanel_MouseLeave);
            this.calendarContentPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.calendarContentPanel_MouseMove);
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(151, 0);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
            // 
            // lunarInfoPanel
            // 
            this.lunarInfoPanel.BackColor = System.Drawing.Color.Purple;
            this.lunarInfoPanel.Controls.Add(this.noteTextBox);
            this.lunarInfoPanel.Controls.Add(this.deleteButton);
            this.lunarInfoPanel.Controls.Add(this.updataButton);
            this.lunarInfoPanel.Controls.Add(this.noteLabel);
            this.lunarInfoPanel.Location = new System.Drawing.Point(693, 124);
            this.lunarInfoPanel.Name = "lunarInfoPanel";
            this.lunarInfoPanel.Size = new System.Drawing.Size(246, 420);
            this.lunarInfoPanel.TabIndex = 6;
            this.lunarInfoPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.lunarInfoPanel_Paint);
            // 
            // noteTextBox
            // 
            this.noteTextBox.BackColor = System.Drawing.Color.DarkMagenta;
            this.noteTextBox.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.noteTextBox.Location = new System.Drawing.Point(20, 34);
            this.noteTextBox.Multiline = true;
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.Size = new System.Drawing.Size(208, 314);
            this.noteTextBox.TabIndex = 3;
            // 
            // deleteButton
            // 
            this.deleteButton.BackColor = System.Drawing.Color.DarkMagenta;
            this.deleteButton.CausesValidation = false;
            this.deleteButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.deleteButton.Location = new System.Drawing.Point(142, 367);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 38);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "删除";
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // updataButton
            // 
            this.updataButton.BackColor = System.Drawing.Color.DarkMagenta;
            this.updataButton.CausesValidation = false;
            this.updataButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.updataButton.Location = new System.Drawing.Point(34, 367);
            this.updataButton.Name = "updataButton";
            this.updataButton.Size = new System.Drawing.Size(75, 38);
            this.updataButton.TabIndex = 1;
            this.updataButton.Text = "更新";
            this.updataButton.UseVisualStyleBackColor = false;
            this.updataButton.Click += new System.EventHandler(this.updataButton_Click);
            // 
            // noteLabel
            // 
            this.noteLabel.AutoSize = true;
            this.noteLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.noteLabel.Location = new System.Drawing.Point(71, 207);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(116, 18);
            this.noteLabel.TabIndex = 0;
            this.noteLabel.Text = "写点什么吧！";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Violet;
            this.ClientSize = new System.Drawing.Size(968, 572);
            this.Controls.Add(this.lunarInfoPanel);
            this.Controls.Add(this.calendarContentPanel);
            this.Controls.Add(this.weekPanel);
            this.Controls.Add(this.dateSelectPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            this.dateSelectPanel.ResumeLayout(false);
            this.dateSelectPanel.PerformLayout();
            this.calendarContentPanel.ResumeLayout(false);
            this.lunarInfoPanel.ResumeLayout(false);
            this.lunarInfoPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Panel dateSelectPanel;
        private System.Windows.Forms.Panel weekPanel;
        private System.Windows.Forms.Panel calendarContentPanel;
        private System.Windows.Forms.Panel lunarInfoPanel;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button updataButton;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.TextBox noteTextBox;
    }
}

