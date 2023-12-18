using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatApp
{
    public partial class ChatForm : Form
    {
        public static ChatForm instance;

        TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
        private Dictionary<TcpClient, string> clientDictionary = new Dictionary<TcpClient, string>();
        private readonly object clientsLock = new object();

        TcpClient client;
        NetworkStream clientStream;

        bool isServer = false;
        bool isChatting = true;
        public int playerRole = -1;

        public bool isPlaying;

        public static string globalName;
        public static string globalId;

        public string client1Id;
        public string client2Id;

        public const int CHAT_TYPE = 1;
        public const int GAME_TYPE = 2;
        public const int SEND_ID_TYPE = 4;

        public const int INITIAL_GAME = 5;
        public const int JOIN_GAME = 6;
        public const int GAME_STEP = 7;
        public const int END_GAME = 8;
        public const int DRAW_GAME = 9;
        public const int SURRENDER = 10;

        public int[,] gameArr = new int[20, 20];

        string[] colorArr = { "Black", "Red", "Green", "Blue", "Purple" };
        Color[] colors = { Color.Black, Color.Red, Color.Green, Color.Blue, Color.Purple };

        public class JsonMessage()
        {
            public int type { get; set; }
            public Object dataObj { get; set; }
        }

        public class ChatData()
        {
            public int type { get; set; }
            public string id { get; set; }
            public String name { get; set; }
            public string targetId { get; set; }
            public string message { get; set; }
            public DateTime dateTime { get; set; }
            public int color { get; set; }
        }
        public ChatForm(bool isHost, string ip, string name)
        {
            InitializeComponent();
            waitLabel.Hide();
            joinButton.Hide();
            CheckForIllegalCrossThreadCalls = false;

            instance = this;

            globalName = name;

            colorComboBox.Items.AddRange(colorArr);
            colorComboBox.SelectedIndex = 0;

            if (isHost)
            {
                try
                {
                    createServer(name);
                    this.Text = "Admin";
                }
                catch (SocketException e)
                {
                    MessageBox.Show("Can only start a chat once at the same time. Please try again.");
                    this.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

            }
            else
            {
                this.Text = "Client";
                createClient(ip, name);
            }
        }



        static string GetLocalIpAddress()
        {
            // Get the local IP address of the machine
            string localIpAddress = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIpAddress = ipAddress.ToString();
                    break;
                }
            }

            return localIpAddress;
        }

        private void createServer(string name)
        {

            isServer = true;
            server = new TcpListener(IPAddress.Any, 6969);
            server.Start();

            nameLabel.Text += name;
            globalId = "admin";

            displayLocalMessage("Your IP is: " + GetLocalIpAddress());

            Thread serverListener = new Thread(createConnectionAsServer);
            serverListener.Start();
        }

        private void createClient(string ip, string name)
        {
            try
            {
                client = new TcpClient(ip, 6969);
                clientStream = client.GetStream();
            }
            catch (SocketException e)
            {
                Debug.WriteLine(e.Message);
                MessageBox.Show("No Server Found");
                this.Close();
                return;
            }


            nameLabel.Text += globalName;



            // Start a new thread to receive messages
            Thread receiveThread = new Thread(receiveMessageAsClient);
            receiveThread.Start();
            while (globalId == null)
            {
                continue;
            }
            string customMessage = " has joined the chat";
            sendChatData(customMessage);
        }

        private void createConnectionAsServer()
        {
            while (isChatting)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();

                    string clientId = Guid.NewGuid().ToString();

                    lock (clientsLock)
                    {
                        clients.Add(client);
                        clientDictionary.Add(client, clientId);
                    }

                    JsonMessage jsonMessage = new JsonMessage
                    {
                        type = SEND_ID_TYPE,
                        dataObj = new ChatData
                        {
                            message = clientId
                        }
                    };

                    string IdMessage = JsonSerializer.Serialize(jsonMessage);

                    byte[] bytesMessage = Encoding.ASCII.GetBytes(IdMessage);

                    NetworkStream networkStream = client.GetStream();
                    networkStream.Write(bytesMessage, 0, bytesMessage.Length);
                    networkStream.Flush();


                    // Start a new thread for this client
                    Thread clientThread = new Thread(receiveMessageAsServer);
                    clientThread.Start(client);
                }
                catch (SocketException e)
                {
                    Debug.WriteLine(e.Message);
                    this.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

            }
            Debug.WriteLine("Stop Creare Connection As Server");
        }

        private void receiveMessageAsServer(object clientObj)
        {
            TcpClient tcpClient = (TcpClient)clientObj;
            NetworkStream clientStream = tcpClient.GetStream();

            while (isChatting)
            {
                byte[] message = new byte[4096];
                int bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                    break;

                string clientMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
                try
                {
                    JsonMessage s = JsonSerializer.Deserialize<JsonMessage>(clientMessage);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    continue;
                }
                JsonMessage jsonMessage = JsonSerializer.Deserialize<JsonMessage>(clientMessage);
                if (jsonMessage != null)
                {
                    ChatData chatData = JsonSerializer.Deserialize<ChatData>(jsonMessage.dataObj.ToString());
                    if (jsonMessage.type == CHAT_TYPE)
                    {
                        if (chatData.type == INITIAL_GAME)
                        {
                            updateButton();
                            client1Id = chatData.id;
                            BroadcastMessage(clientMessage, chatData.id);
                        }

                        if (chatData.type == JOIN_GAME)
                        {
                            hideAllButton();
                            client2Id = chatData.id;
                            if (playerRole != -1)
                            {
                                Invoke(new Action(() =>
                                {
                                    PlayForm playForm = new PlayForm(isServer, playerRole, client1Id, client2Id);
                                    playForm.Show();
                                }));
                            }
                        }
                        string messageToDisplay = chatData.name + " (" + chatData.dateTime + "): " + chatData.message;
                        displayLocalMessage(messageToDisplay, chatData.color);
                        BroadcastMessage(clientMessage, chatData.id);
                    }
                    if (jsonMessage.type == GAME_TYPE)
                    {
                        if (chatData.type == GAME_STEP)
                        {
                            Debug.WriteLine(chatData.message);
                            if (chatData.targetId == globalId)
                            {
                                Debug.WriteLine(chatData.message);
                                int.TryParse(chatData.message.Split(':')[0], out int x);
                                int.TryParse(chatData.message.Split(':')[1], out int y);
                                if (playerRole == 2)
                                {
                                    PlayForm.instance.checkClickable(x, y, 1);
                                    PlayForm.instance.drawX(x, y);
                                    PlayForm.instance.playerTurn = 2;
                                    PlayForm.instance.changeTurnLabel();
                                }
                                else
                                {
                                    PlayForm.instance.checkClickable(x, y, 2);
                                    PlayForm.instance.drawO(x, y);
                                    PlayForm.instance.playerTurn = 1;
                                    PlayForm.instance.changeTurnLabel();
                                }
                                PlayForm.instance.isClickable = true;
                            }
                            else
                            {
                                BroadcastMessage(clientMessage, chatData.id);
                            }
                        }

                        if (chatData.type == END_GAME)
                        {
                            if (chatData.targetId == globalId)
                            {
                                PlayForm.instance.Enabled = false;
                                MessageBox.Show(chatData.name + " win!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                                Array.Clear(gameArr, 0, gameArr.Length);
                                PlayForm.instance.Close();

                            }
                            BroadcastMessage(clientMessage, chatData.id);
                            string messageToDisplay = "Notification: " + $"{chatData.name} win!";
                            displayLocalMessage(messageToDisplay, 2);
                            updateButton(1);
                        }

                        if (chatData.type == DRAW_GAME)
                        {
                            if (chatData.targetId == globalId)
                            {
                                if (chatData.message == "request")
                                {
                                    bool check = PlayForm.instance.showDrawRequest();
                                    if (check)
                                    {
                                        Array.Clear(gameArr, 0, gameArr.Length);
                                        sendGameData("accept", chatData.id, DRAW_GAME);
                                        string messageToDisplay = "Notification: The battle end as a draw.";
                                        displayLocalMessage(messageToDisplay, 2);
                                        updateButton(1);
                                    }
                                    else
                                    {
                                        sendGameData("decline", chatData.id, DRAW_GAME);
                                    }
                                }
                                if (chatData.message == "accept" || chatData.message == "decline")
                                {
                                    PlayForm.instance.handleDrawReply((chatData.message == "accept") ? true : false);
                                }
                            }
                            BroadcastMessage(clientMessage, chatData.id);
                        }
                        if (chatData.type == SURRENDER)
                        {
                            if (chatData.targetId == globalId)
                            {
                                PlayForm.instance.Enabled = false;
                                MessageBox.Show("You win!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                                Array.Clear(gameArr, 0, gameArr.Length);
                                PlayForm.instance.Close();
                            }
                            BroadcastMessage(clientMessage, chatData.id);
                            string messageToDisplay = "Notification: " + $"{chatData.name} has surrendered!";
                            displayLocalMessage(messageToDisplay, 2);
                            updateButton(1);
                        }

                    }
                }
            }

            // Remove the client when the loop exits
            lock (clientsLock)
            {
                clients.Remove(tcpClient);
                if (client != null)
                {
                    clientDictionary.Remove(client);
                }
            }

            tcpClient.Close();
            Debug.WriteLine("Stop Receive Message As Server");
        }

        private void receiveMessageAsClient()
        {
            while (isChatting)
            {
                byte[] message = new byte[4096];
                int bytesRead;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    break;
                }

                if (bytesRead == 0)
                    break;

                string receivedMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
                JsonMessage jsonMessage = JsonSerializer.Deserialize<JsonMessage>(receivedMessage);
                if (jsonMessage != null)
                {
                    ChatData chatData = JsonSerializer.Deserialize<ChatData>(jsonMessage.dataObj.ToString());
                    if (jsonMessage.type == CHAT_TYPE)
                    {
                        if (chatData.type == INITIAL_GAME)
                        {
                            updateButton();
                            client1Id = chatData.id;
                        }

                        if (chatData.type == JOIN_GAME)
                        {
                            client2Id = chatData.id;
                            hideAllButton();
                            if (playerRole != -1)
                            {
                                Invoke(new Action(() =>
                                {
                                    PlayForm playForm = new PlayForm(isServer, playerRole, client1Id, client2Id);
                                    playForm.Show();
                                }));
                            }

                        }
                        string messageToDisplay = chatData.name + " (" + chatData.dateTime + "): " + chatData.message;
                        displayLocalMessage(messageToDisplay, chatData.color);
                    }

                    if (jsonMessage.type == SEND_ID_TYPE)
                    {
                        globalId = chatData.message;
                    }

                    if (jsonMessage.type == GAME_TYPE)
                    {
                        if (chatData.type == GAME_STEP && chatData.targetId == globalId)
                        {
                            Debug.WriteLine(chatData.message);
                            int.TryParse(chatData.message.Split(':')[0], out int x);
                            int.TryParse(chatData.message.Split(':')[1], out int y);

                            if (playerRole == 2)
                            {
                                PlayForm.instance.checkClickable(x, y, 1);
                                PlayForm.instance.drawX(x, y);
                                PlayForm.instance.playerTurn = 2;
                                PlayForm.instance.changeTurnLabel();
                            }
                            else
                            {
                                PlayForm.instance.checkClickable(x, y, 2);
                                PlayForm.instance.drawO(x, y);
                                PlayForm.instance.playerTurn = 1;
                                PlayForm.instance.changeTurnLabel();
                            }
                            PlayForm.instance.isClickable = true;
                        }

                        if (chatData.type == END_GAME)
                        {
                            if (chatData.targetId == globalId)
                            {
                                PlayForm.instance.Enabled = false;
                                MessageBox.Show(chatData.name + " win!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                                PlayForm.instance.Close();
                                Array.Clear(gameArr, 0, gameArr.Length);
                            }
                            string messageToDisplay = "Notification: " + $"{chatData.name} win!";
                            displayLocalMessage(messageToDisplay, 2);
                            updateButton(1);
                        }

                        if (chatData.type == DRAW_GAME)
                        {
                            if (chatData.targetId == globalId)
                            {
                                if (chatData.message == "request")
                                {
                                    bool check = PlayForm.instance.showDrawRequest();
                                    if (check)
                                    {
                                        Array.Clear(gameArr, 0, gameArr.Length);
                                        sendGameData("accept", chatData.id, DRAW_GAME);
                                        string messageToDisplay = "Notification: The battle end as a draw.";
                                        displayLocalMessage(messageToDisplay, 2);
                                        updateButton(1);
                                    }
                                    else
                                    {
                                        sendGameData("decline", chatData.id, DRAW_GAME);
                                    }
                                }
                                if (chatData.message == "accept" || chatData.message == "decline")
                                {
                                    PlayForm.instance.handleDrawReply((chatData.message == "accept") ? true : false);
                                }
                            }
                        }
                        if (chatData.type == SURRENDER)
                        {
                            if (chatData.targetId == globalId)
                            {
                                PlayForm.instance.Enabled = false;
                                MessageBox.Show("You win!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                                Array.Clear(gameArr, 0, gameArr.Length);
                                PlayForm.instance.Close();
                            }
                            string messageToDisplay = "Notification: " + $"{chatData.name} has surrendered!";
                            displayLocalMessage(messageToDisplay, 2);
                            updateButton(1);
                        }

                    }
                }
            }

            MessageBox.Show("Disconnected from the server.");
            Close();
        }

        public void displayLocalMessage(string message, int colorNumber = -1)
        {
            contentTextBox.SelectionColor = (colorNumber == -1) ? Color.Black : colors[colorNumber];
            contentTextBox.AppendText("\r\n" + message);
        }

        public void sendChatData(string customMessage = "", int type = -1)
        {
            JsonMessage jsonMessage = new JsonMessage
            {
                type = CHAT_TYPE,
                dataObj = new ChatData
                {
                    id = globalId,
                    name = globalName,
                    message = (customMessage == "") ? chatTextBox.Text : customMessage,
                    color = colorComboBox.SelectedIndex,
                    dateTime = DateTime.Now,

                    type = type,
                }
            };
            string message = JsonSerializer.Serialize(jsonMessage);

            if (isServer)
            {
                BroadcastMessage(message);
            }
            else
            {
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                clientStream.Write(messageBytes, 0, messageBytes.Length);
                clientStream.Flush();
            }
            string messageToDisplay = "You (" + DateTime.Now.ToString() + "): " + ((customMessage == "") ? chatTextBox.Text : customMessage);
            displayLocalMessage(messageToDisplay, colorComboBox.SelectedIndex);
            chatTextBox.Clear();
        }

        public void sendGameData(string customMessage, string _targetId, int _type)
        {
            JsonMessage jsonMessage = new JsonMessage
            {
                type = GAME_TYPE,
                dataObj = new ChatData
                {
                    id = globalId,
                    name = globalName,
                    message = (customMessage == "") ? chatTextBox.Text : customMessage,
                    color = 1,
                    dateTime = DateTime.Now,
                    type = _type,
                    targetId = _targetId
                }
            };
            string message = JsonSerializer.Serialize(jsonMessage);

            if (isServer)
            {
                if (_type == END_GAME)
                {
                    BroadcastMessage(message);
                }
                else
                {
                    TcpClient _client = clientDictionary.FirstOrDefault(x => x.Value == _targetId).Key;
                    NetworkStream stream = _client.GetStream();
                    byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                    stream.Write(messageBytes, 0, messageBytes.Length);
                    stream.Flush();
                }
            }
            else
            {
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                clientStream.Write(messageBytes, 0, messageBytes.Length);
                clientStream.Flush();
            }
        }

        public void BroadcastMessage(string message, string id = "")
        {
            foreach (TcpClient client in clients)
            {
                if (clientDictionary[client] != id)
                {
                    NetworkStream clientStream = client.GetStream();

                    byte[] broadcastMessage = Encoding.ASCII.GetBytes(message);
                    clientStream.Write(broadcastMessage, 0, broadcastMessage.Length);
                    clientStream.Flush();
                }
            }
        }
        private void chatTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool emptyCheck = chatTextBox.Text.Trim() != "";
            if (e.KeyCode == Keys.Enter && emptyCheck)
            {
                sendChatData();
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            bool emptyCheck = chatTextBox.Text.Trim() != "";
            if (emptyCheck)
            {
                sendChatData();
            }
        }
        private void startGameButton_Click(object sender, EventArgs e)
        {
            startGameButton.Hide();
            waitLabel.Show();
            JsonMessage jsonMessage = new JsonMessage
            {
                type = CHAT_TYPE,
                dataObj = new ChatData
                {
                    id = globalId,
                    name = globalName,
                    type = INITIAL_GAME,
                    dateTime = DateTime.Now,
                    message = "request for an XO Game"
                }

            };
            string message = JsonSerializer.Serialize(jsonMessage);
            playerRole = 1;
            client1Id = globalId;
            if (isServer)
            {
                BroadcastMessage(message);

                client1Id = globalId;
            }
            else
            {
                sendChatData("request for an XO Game", INITIAL_GAME);
            }
        }

        private void joinButton_Click(object sender, EventArgs e)
        {
            hideAllButton();
            JsonMessage jsonMessage = new JsonMessage
            {
                type = CHAT_TYPE,
                dataObj = new ChatData
                {
                    id = globalId,
                    name = globalName,
                    type = JOIN_GAME,
                    dateTime = DateTime.Now,
                    message = "has joined the game"
                }
            };
            string message = JsonSerializer.Serialize(jsonMessage);
            playerRole = 2;
            client2Id = globalId;
            if (isServer)
            {
                BroadcastMessage(message);
                PlayForm playform = new PlayForm(isServer, playerRole, client1Id, client2Id);
                playform.Show();
            }
            else
            {
                sendChatData("has joined the game", JOIN_GAME);
                PlayForm playform = new PlayForm(isServer, playerRole, client1Id, client2Id);
                playform.Show();
            }
        }

        public void updateButton(int addAction = -1)
        {
            if (addAction == -1)
            {
                if (waitLabel.InvokeRequired)
                {
                    waitLabel.Invoke((Action)(() => waitLabel.Visible = true));
                }
                else
                {
                    waitLabel.Visible = true;
                }
                if (joinButton.InvokeRequired)
                {
                    joinButton.Invoke((Action)(() => joinButton.Visible = true));
                }
                else
                {
                    joinButton.Visible = true;
                }
                if (startGameButton.InvokeRequired)
                {
                    startGameButton.Invoke((Action)(() => startGameButton.Visible = false));
                }
                else
                {
                    startGameButton.Visible = false;
                }
            }
            else
            {
                if (startGameButton.InvokeRequired)
                {
                    startGameButton.Invoke((Action)(() => startGameButton.Visible = true));
                }
                else
                {
                    startGameButton.Visible = true;
                }

            }
        }

        private void hideAllButton()
        {
            if (waitLabel.InvokeRequired)
            {
                waitLabel.Invoke((Action)(() => waitLabel.Visible = false));
            }
            else
            {
                waitLabel.Visible = false;
            }
            if (joinButton.InvokeRequired)
            {
                joinButton.Invoke((Action)(() => joinButton.Visible = false));
            }
            else
            {
                joinButton.Visible = false;
            }
            if (startGameButton.InvokeRequired)
            {
                startGameButton.Invoke((Action)(() => startGameButton.Visible = false));
            }
            else
            {
                startGameButton.Visible = false;
            }
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            isChatting = false;
            if (PlayForm.instance != null)
            {
                PlayForm.instance.Close();
            }
            if (isServer)
            {
                if (server != null)
                {
                    server.Stop();
                }
                foreach (TcpClient client in clients)
                {
                    client.Close();
                }
            }
        }
    }
}
