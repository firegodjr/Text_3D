namespace Text_3D_Engine
{
    partial class GameWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TextLayerWhite = new System.Windows.Forms.Label();
            this.renderTick = new System.Windows.Forms.Timer(this.components);
            this.TextLayerRed = new System.Windows.Forms.Label();
            this.TextLayerGray = new System.Windows.Forms.Label();
            this.TextLayerText = new System.Windows.Forms.Label();
            this.TextLayerGreen = new System.Windows.Forms.Label();
            this.TextLayerYellow = new System.Windows.Forms.Label();
            this.TextLayerBlue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TextLayerWhite
            // 
            this.TextLayerWhite.AutoSize = true;
            this.TextLayerWhite.BackColor = System.Drawing.Color.Transparent;
            this.TextLayerWhite.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLayerWhite.ForeColor = System.Drawing.Color.White;
            this.TextLayerWhite.Location = new System.Drawing.Point(0, 0);
            this.TextLayerWhite.Name = "TextLayerWhite";
            this.TextLayerWhite.Size = new System.Drawing.Size(42, 15);
            this.TextLayerWhite.TabIndex = 0;
            this.TextLayerWhite.Text = "White";
            // 
            // renderTick
            // 
            this.renderTick.Interval = 1;
            this.renderTick.Tick += new System.EventHandler(this.DoGameTick);
            // 
            // TextLayerRed
            // 
            this.TextLayerRed.AutoSize = true;
            this.TextLayerRed.BackColor = System.Drawing.Color.Transparent;
            this.TextLayerRed.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLayerRed.ForeColor = System.Drawing.Color.Red;
            this.TextLayerRed.Location = new System.Drawing.Point(0, 0);
            this.TextLayerRed.Name = "TextLayerRed";
            this.TextLayerRed.Size = new System.Drawing.Size(28, 15);
            this.TextLayerRed.TabIndex = 1;
            this.TextLayerRed.Text = "Red";
            // 
            // TextLayerGray
            // 
            this.TextLayerGray.AutoSize = true;
            this.TextLayerGray.BackColor = System.Drawing.Color.Transparent;
            this.TextLayerGray.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLayerGray.ForeColor = System.Drawing.Color.Gray;
            this.TextLayerGray.Location = new System.Drawing.Point(0, 0);
            this.TextLayerGray.Name = "TextLayerGray";
            this.TextLayerGray.Size = new System.Drawing.Size(35, 15);
            this.TextLayerGray.TabIndex = 2;
            this.TextLayerGray.Text = "Gray";
            // 
            // TextLayerText
            // 
            this.TextLayerText.AutoSize = true;
            this.TextLayerText.BackColor = System.Drawing.Color.Transparent;
            this.TextLayerText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLayerText.ForeColor = System.Drawing.Color.White;
            this.TextLayerText.Location = new System.Drawing.Point(0, 0);
            this.TextLayerText.Name = "TextLayerText";
            this.TextLayerText.Size = new System.Drawing.Size(35, 15);
            this.TextLayerText.TabIndex = 3;
            this.TextLayerText.Text = "Text";
            // 
            // TextLayerGreen
            // 
            this.TextLayerGreen.AutoSize = true;
            this.TextLayerGreen.BackColor = System.Drawing.Color.Transparent;
            this.TextLayerGreen.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLayerGreen.ForeColor = System.Drawing.Color.Lime;
            this.TextLayerGreen.Location = new System.Drawing.Point(0, 0);
            this.TextLayerGreen.Name = "TextLayerGreen";
            this.TextLayerGreen.Size = new System.Drawing.Size(42, 15);
            this.TextLayerGreen.TabIndex = 4;
            this.TextLayerGreen.Text = "Green";
            // 
            // TextLayerYellow
            // 
            this.TextLayerYellow.AutoSize = true;
            this.TextLayerYellow.BackColor = System.Drawing.Color.Transparent;
            this.TextLayerYellow.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLayerYellow.ForeColor = System.Drawing.Color.Yellow;
            this.TextLayerYellow.Location = new System.Drawing.Point(0, 0);
            this.TextLayerYellow.Name = "TextLayerYellow";
            this.TextLayerYellow.Size = new System.Drawing.Size(49, 15);
            this.TextLayerYellow.TabIndex = 5;
            this.TextLayerYellow.Text = "Yellow";
            // 
            // TextLayerBlue
            // 
            this.TextLayerBlue.AutoSize = true;
            this.TextLayerBlue.BackColor = System.Drawing.Color.Transparent;
            this.TextLayerBlue.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLayerBlue.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.TextLayerBlue.Location = new System.Drawing.Point(0, 0);
            this.TextLayerBlue.Name = "TextLayerBlue";
            this.TextLayerBlue.Size = new System.Drawing.Size(35, 15);
            this.TextLayerBlue.TabIndex = 6;
            this.TextLayerBlue.Text = "Blue";
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(720, 374);
            this.Controls.Add(this.TextLayerBlue);
            this.Controls.Add(this.TextLayerYellow);
            this.Controls.Add(this.TextLayerGreen);
            this.Controls.Add(this.TextLayerText);
            this.Controls.Add(this.TextLayerGray);
            this.Controls.Add(this.TextLayerRed);
            this.Controls.Add(this.TextLayerWhite);
            this.DoubleBuffered = true;
            this.Name = "GameWindow";
            this.Text = "Form1";
            this.Activated += new System.EventHandler(this.GameForm_Activated);
            this.Deactivate += new System.EventHandler(this.GameForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.ResizeBegin += new System.EventHandler(this.GameForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.GameForm_ResizeEnd);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TextLayerWhite;
        private System.Windows.Forms.Timer renderTick;
        private System.Windows.Forms.Label TextLayerRed;
        private System.Windows.Forms.Label TextLayerGray;
        private System.Windows.Forms.Label TextLayerText;
        private System.Windows.Forms.Label TextLayerGreen;
        private System.Windows.Forms.Label TextLayerYellow;
        private System.Windows.Forms.Label TextLayerBlue;
    }
}

