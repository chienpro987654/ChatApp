namespace ChatApp
{
    public partial class PlayForm : Form
    {
        public PlayForm()
        {
            InitializeComponent();
            drawGrid();
        }

        private char Player1;
        private char Player2;

        public void drawGrid()
        {
            Graphics g = pictureBox1.CreateGraphics();
            Pen blackPen = new Pen(Color.Black, 3);
            for (int i = 0; i < 1000; i++)
            {
                g.DrawLine(blackPen, i * 1000, 0, i * 1000, 1000 * 1000);
                g.DrawLine(blackPen, 0, i * 1000, i * 1000, 1000 * 1000);
            }
        }

        private void Game_Load(object sender, EventArgs e)
        {

        }
    }
}
