namespace ChatApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            String name = nameTextBox.Text;
            if (name == "")
            {
                name = "admin";
            }
            ChatForm chatForm = new ChatForm(true, "127.0.0.1", name);
            this.Hide();
            if (!chatForm.IsDisposed)
            {
                chatForm.ShowDialog();
            }
            this.Show();
        }

        private void joinButton_Click(object sender, EventArgs e)
        {
            String name = nameTextBox.Text;
            //if (name == "")
            //{
            //    MessageBox.Show("Please enter your name to start!");
            //    return;
            //}
            String ip = ipTextBox.Text;
            //if (!IPAddress.TryParse(ip, out IPAddress ipAddress))
            //{
            //    MessageBox.Show("Please Enter A Valid IP Address");
            //    return;
            //}
            //ChatForm chatForm = new ChatForm(name, false, ip);
            ChatForm chatForm = new ChatForm(false, "192.168.217.1", "John");
            //ChatForm chatForm = new ChatForm("John", false, "192.168.2.33");
            this.Hide();
            if (!chatForm.IsDisposed)
            {
                chatForm.ShowDialog();
            }
            this.Show();
        }
    }
}
