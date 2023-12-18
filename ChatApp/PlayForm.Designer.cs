namespace ChatApp
{
    partial class PlayForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            drawButton = new Button();
            turnLabel = new Label();
            drawLabel = new Label();
            surrButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(106, 30);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(600, 600);
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            // 
            // drawButton
            // 
            drawButton.Location = new Point(176, 656);
            drawButton.Name = "drawButton";
            drawButton.Size = new Size(94, 29);
            drawButton.TabIndex = 10;
            drawButton.Text = "Draw";
            drawButton.UseVisualStyleBackColor = true;
            drawButton.Click += drawButton_Click;
            // 
            // turnLabel
            // 
            turnLabel.AutoSize = true;
            turnLabel.Location = new Point(391, 7);
            turnLabel.Name = "turnLabel";
            turnLabel.Size = new Size(17, 20);
            turnLabel.TabIndex = 11;
            turnLabel.Text = "0";
            // 
            // drawLabel
            // 
            drawLabel.AutoSize = true;
            drawLabel.Location = new Point(276, 660);
            drawLabel.Name = "drawLabel";
            drawLabel.Size = new Size(162, 20);
            drawLabel.TabIndex = 12;
            drawLabel.Text = "Sending draw request...";
            // 
            // surrButton
            // 
            surrButton.Location = new Point(513, 656);
            surrButton.Name = "surrButton";
            surrButton.Size = new Size(94, 29);
            surrButton.TabIndex = 13;
            surrButton.Text = "Surrender";
            surrButton.UseVisualStyleBackColor = true;
            surrButton.Click += surrButton_Click;
            // 
            // PlayForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 709);
            Controls.Add(surrButton);
            Controls.Add(drawLabel);
            Controls.Add(turnLabel);
            Controls.Add(drawButton);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "PlayForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox1;
        private Button drawButton;
        private Label turnLabel;
        private Label drawLabel;
        private Button surrButton;
    }
}
