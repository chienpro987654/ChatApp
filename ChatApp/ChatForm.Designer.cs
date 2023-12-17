namespace ChatApp
{
    partial class ChatForm
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
            contentTextBox = new TextBox();
            chatTextBox = new TextBox();
            label1 = new Label();
            sendButton = new Button();
            nameLabel = new Label();
            waitLabel = new Label();
            joinButton = new Button();
            startGameButton = new Button();
            SuspendLayout();
            // 
            // contentTextBox
            // 
            contentTextBox.Location = new Point(12, 12);
            contentTextBox.Multiline = true;
            contentTextBox.Name = "contentTextBox";
            contentTextBox.ReadOnly = true;
            contentTextBox.ScrollBars = ScrollBars.Vertical;
            contentTextBox.Size = new Size(762, 383);
            contentTextBox.TabIndex = 0;
            // 
            // chatTextBox
            // 
            chatTextBox.Location = new Point(12, 455);
            chatTextBox.Name = "chatTextBox";
            chatTextBox.Size = new Size(651, 27);
            chatTextBox.TabIndex = 1;
            chatTextBox.KeyDown += chatTextBox_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 432);
            label1.Name = "label1";
            label1.Size = new Size(148, 20);
            label1.TabIndex = 2;
            label1.Text = "Enter your chat here: ";
            // 
            // sendButton
            // 
            sendButton.Location = new Point(680, 454);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(94, 29);
            sendButton.TabIndex = 3;
            sendButton.Text = "Send";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += sendButton_Click;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            nameLabel.Location = new Point(11, 407);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(180, 20);
            nameLabel.TabIndex = 4;
            nameLabel.Text = "You has joined as name: ";
            // 
            // waitLabel
            // 
            waitLabel.AutoSize = true;
            waitLabel.Location = new Point(339, 411);
            waitLabel.Name = "waitLabel";
            waitLabel.Size = new Size(122, 20);
            waitLabel.TabIndex = 5;
            waitLabel.Text = "Wait for players...";
            // 
            // joinButton
            // 
            joinButton.Location = new Point(467, 407);
            joinButton.Name = "joinButton";
            joinButton.Size = new Size(94, 29);
            joinButton.TabIndex = 6;
            joinButton.Text = "Join";
            joinButton.UseVisualStyleBackColor = true;
            joinButton.Click += joinButton_Click;
            // 
            // startGameButton
            // 
            startGameButton.Location = new Point(380, 407);
            startGameButton.Name = "startGameButton";
            startGameButton.Size = new Size(148, 29);
            startGameButton.TabIndex = 7;
            startGameButton.Text = "Start An XO Game!";
            startGameButton.UseVisualStyleBackColor = true;
            startGameButton.Click += startGameButton_Click;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(786, 496);
            Controls.Add(startGameButton);
            Controls.Add(joinButton);
            Controls.Add(waitLabel);
            Controls.Add(nameLabel);
            Controls.Add(sendButton);
            Controls.Add(label1);
            Controls.Add(chatTextBox);
            Controls.Add(contentTextBox);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "ChatForm";
            Text = "ChatForm";
            FormClosing += ChatForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox contentTextBox;
        private TextBox chatTextBox;
        private Label label1;
        private Button sendButton;
        private Label nameLabel;
        private Label waitLabel;
        private Button joinButton;
        private Button startGameButton;
    }
}