using XProtocol.Serializator;
using XProtocol;

namespace Client
{
    public partial class Form1 : Form
    {
        Player player;
        XClient client;

        public Form1()
        {
            InitializeComponent();
            try
            {
                ConnectClient();
            }
            catch
            {
                errorLabel.Text = "Не удалось подключиться к серверу";
                errorLabel.Visible = true;
            }

        }

        private void ConnectClient()
        {
            client = new XClient();
            client.OnPacketReceive += OnPacketRecieve;
            client.Connect("127.0.0.1", 4910);

            Console.WriteLine("Sending handshake packet..");

            client.QueuePacketSend(
                XPacketConverter.Serialize(
                    XPacketType.Handshake,
                    new XPacketHandshake
                    {
                        Id = -1
                    })
                    .ToPacket());
        }
        private void OnPacketRecieve(byte[] packet)
        {
            var parsed = XPacket.Parse(packet);

            if (parsed != null)
            {
                ProcessIncomingPacket(parsed);
            }
        }

        private void ProcessIncomingPacket(XPacket packet)
        {
            var type = XPacketTypeManager.GetTypeFromPacket(packet);

            switch (type)
            {
                case XPacketType.Handshake:
                    ProcessHandshake(packet);
                    break;
                case XPacketType.NewPlayer:
                    ProcessNewPlayer(packet);
                    break;
                case XPacketType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessHandshake(XPacket packet)
        {
            var handshake = XPacketConverter.Deserialize<XPacketHandshake>(packet);

            if (handshake.Id > 10)
                BeginInvoke(new Action(() =>
                {
                    errorLabel.Text = "Игра уже заполнена (максимум 10 игроков)";
                    errorLabel.Visible = true;
                }));
            else
            {

                player = new Player(handshake.Id);

                BeginInvoke(new Action(() =>
                {
                    startScreen.Visible = true;
                    nameTextBox.Text = $"Player{player.Id}";
                }));

            }
        }

        private void ProcessNewPlayer(XPacket packet)
        {
            var newPlayer = XPacketConverter.Deserialize<XPacketNewPlayer>(packet);
            BeginInvoke(new Action(() =>
            {
                playersList.Items.Add($"Player{newPlayer.Id}");
            }));
        }

        private void readyButton_Click(object sender, EventArgs e)
        {

        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            saveNameButton.Enabled = true;
            saveNameButton.Visible = true;
        }

        private void saveNameButton_Click(object sender, EventArgs e)
        {
            saveNameButton.Enabled = false;
            saveNameButton.Visible = false;
        }
    }
}