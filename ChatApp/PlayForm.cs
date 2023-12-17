using System.Diagnostics;
using System.Net.Sockets;

namespace ChatApp
{
    public partial class PlayForm : Form
    {
        public static PlayForm instance;

        TcpListener server;
        TcpClient client;

        string client1Id;
        string client2Id;

        public int playerRole;

        private char clickChar;
        int cellSize = 30;

        bool isServer = false;
        public int playerTurn = 1;

        public PlayForm(bool _isServer, int playerSlot, string _client1Id, string _client2Id)
        {
            InitializeComponent();

            instance = this;

            clickChar = (playerSlot == 2) ? 'O' : 'X';
            if (_isServer)
            {
                isServer = true;
                this.Text = "Admin's Game Screen";
            }
            else
            {
                this.Text = $"Client {playerSlot}'s Game Screen";
            }
            playerRole = playerSlot; // 1:player use X  2: player use Y
            //client1Id = _client1Id;
            //client2Id = _client2Id;
            client1Id = ChatForm.instance.client1Id;
            client2Id = ChatForm.instance.client2Id;

            if (playerTurn == playerRole)
            {
                turnLabel.Text = "Your Turn";

            }
            else
            {
                turnLabel.Text = "Opponent's Turn";
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (playerTurn == playerRole)
            {
                int x = (e.X % 30 == 0) ? e.X + 1 : e.X;
                int y = (e.Y % 30 == 0) ? e.Y + 1 : e.Y;
                turnLabel.Text = "You have clicked" + x + "," + y;

                if (checkClickable(x, y))
                {
                    if (clickChar == 'X') { drawX(x, y); } else { drawO(x, y); }
                    ChatForm.instance.sendGameData($"{x}:{y}", (playerRole == 1) ? client2Id : client1Id, 7);
                    if (checkWin(x, y))
                    {
                        MessageBox.Show("You win", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                        ChatForm.instance.sendGameData("", (playerRole == 1) ? client2Id : client1Id, 8);
                        this.Enabled = false;
                        Close();
                    }
                }
                playerTurn = (playerRole == 1) ? 2 : 1;
            }
        }

        public bool checkClickable(int x, int y, int isOpponent = -1)
        {
            int xArr = (int)((y + 1) / 30);
            int yArr = (int)((x + 1) / 30);
            Debug.WriteLine($"{xArr}-{yArr}");
            if (ChatForm.instance.gameArr[xArr, yArr] == 1 || ChatForm.instance.gameArr[xArr, yArr] == 2)
            {
                return false;
            }
            ChatForm.instance.gameArr[xArr, yArr] = (isOpponent == -1) ? playerRole : isOpponent;

            return true;
        }

        private bool checkWin(int _x, int _y)
        {
            int x = (int)((_y) / 30);
            int y = (int)((_x) / 30);
            int[,] arr = new int[20, 20];
            Queue<int> queue = new Queue<int>();

            //case: *****
            queue.Clear();
            Array.Copy(ChatForm.instance.gameArr, arr, ChatForm.instance.gameArr.Length);
            for (int i = (y - 4 > 0) ? y - 4 : 0; i < ((y + 4 < 20) ? y + 4 : 20); i++)
            {
                queue.Enqueue(arr[x, i]);
                if (queue.Count == 5)
                {
                    if (queue.Contains(0))
                    {
                        queue.Dequeue();
                    }
                    else
                    {
                        if (queue.Sum() == 5 || queue.Sum() == 10)
                        {
                            return true;
                        }
                    }
                }
            }

            /* case
             * 
             * 
             * 
             */
            queue.Clear();
            for (int i = (x - 4 > 0) ? x - 4 : 0; i < ((x + 4 < 20) ? x + 4 : 20); i++)
            {
                queue.Enqueue(arr[i, y]);
                if (queue.Count == 5)
                {
                    if (queue.Contains(0))
                    {
                        queue.Dequeue();
                    }
                    else
                    {
                        if (queue.Sum() == 5 || queue.Sum() == 10)
                        {
                            return true;
                        }
                    }
                }
            }

            /* Case *
             *        *
             *          *
             *            *
             *              * 
             */
            int runX = (x - 4 > 0) ? x - 4 : 0;
            int endX = (x + 4 < 20) ? x + 4 : 19;

            int runY = (y - 4 > 0) ? y - 4 : 0;
            int endY = (y + 4 < 20) ? y + 4 : 19;

            queue.Clear();
            while (runX <= endX && runY <= endY)
            {
                queue.Enqueue(arr[runX, runY]);
                if (queue.Count == 5)
                {
                    if (queue.Contains(0))
                    {
                        queue.Dequeue();
                    }
                    else
                    {
                        if (queue.Sum() == 5 || queue.Sum() == 10)
                        {
                            return true;
                        }
                    }
                }
                runX++;
                runY++;
            }

            /* case         *
             *             *
             *            *
             *           *
             *          *
            */
            runX = (x - 4 > 0) ? x - 4 : 0;
            endX = (x + 4 < 20) ? x + 4 : 19;

            runY = (y + 4 < 20) ? y + 4 : 19;
            endY = (y - 4 > 0) ? y - 4 : 0;
            queue.Clear();
            while (runX <= endX && runY >= endY)
            {
                queue.Enqueue(arr[runX, runY]);
                if (queue.Count == 5)
                {
                    if (queue.Contains(0))
                    {
                        queue.Dequeue();
                    }
                    else
                    {
                        if (queue.Sum() == 5 || queue.Sum() == 10)
                        {
                            return true;
                        }
                    }
                }
                runX++;
                runY--;
            }

            return false;
        }

        public void drawX(int x, int y)
        {
            Pen pen = new Pen(Color.Red, 3);
            Graphics g = pictureBox1.CreateGraphics();
            int xRoot = (int)(x / cellSize) * cellSize;
            int yRoot = (int)(y / cellSize) * cellSize;
            g.DrawLine(pen, xRoot + 5, yRoot + 5, xRoot + cellSize - 5, yRoot + cellSize - 5);
            g.DrawLine(pen, xRoot + cellSize - 5, yRoot + 5, xRoot + 5, yRoot + cellSize - 5);
        }

        public void drawO(int x, int y)
        {
            Graphics g = pictureBox1.CreateGraphics();
            Pen pen = new Pen(Color.Blue, 3);
            int xRoot = (int)(x / cellSize) * cellSize + 5;
            int yRoot = (int)(y / cellSize) * cellSize + 5;
            int diameter = 20;
            g.DrawEllipse(pen, xRoot, yRoot, diameter, diameter);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen blackPen = new Pen(Color.Black, 1);
            int cellSize = 30;
            for (int i = 1; i < 20; i++)
            {
                g.DrawLine(blackPen, i * cellSize, 0, i * cellSize, pictureBox1.Height);
                g.DrawLine(blackPen, 0, i * cellSize, pictureBox1.Width, i * cellSize);
            }
        }
    }
}
