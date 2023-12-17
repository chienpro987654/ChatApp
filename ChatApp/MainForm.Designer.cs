﻿namespace ChatApp
{
    partial class MainForm
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
            startButton = new Button();
            label1 = new Label();
            ipTextBox = new TextBox();
            joinButton = new Button();
            nameTextBox = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // startButton
            // 
            startButton.Location = new Point(314, 10);
            startButton.Name = "startButton";
            startButton.Size = new Size(140, 29);
            startButton.TabIndex = 1;
            startButton.Text = "Start A Chat Now!";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(460, 15);
            label1.Name = "label1";
            label1.Size = new Size(158, 20);
            label1.TabIndex = 2;
            label1.Text = "Or join an existing one";
            // 
            // ipTextBox
            // 
            ipTextBox.Location = new Point(632, 12);
            ipTextBox.Name = "ipTextBox";
            ipTextBox.Size = new Size(125, 27);
            ipTextBox.TabIndex = 3;
            // 
            // joinButton
            // 
            joinButton.Location = new Point(763, 11);
            joinButton.Name = "joinButton";
            joinButton.Size = new Size(94, 29);
            joinButton.TabIndex = 4;
            joinButton.Text = "Join";
            joinButton.UseVisualStyleBackColor = true;
            joinButton.Click += joinButton_Click;
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(174, 12);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(125, 27);
            nameTextBox.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 15);
            label2.Name = "label2";
            label2.Size = new Size(153, 20);
            label2.TabIndex = 5;
            label2.Text = "Enter your name here:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(873, 89);
            Controls.Add(nameTextBox);
            Controls.Add(label2);
            Controls.Add(joinButton);
            Controls.Add(ipTextBox);
            Controls.Add(label1);
            Controls.Add(startButton);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button startButton;
        private Label label1;
        private TextBox ipTextBox;
        private Button joinButton;
        private TextBox nameTextBox;
        private Label label2;
    }
}
