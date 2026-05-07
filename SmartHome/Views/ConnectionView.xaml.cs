using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SmartHome.Views
{
    public class PacketEntry
    {
        public string Direction { get; set; } = "SEND";
        public string Message   { get; set; } = "";
        public string Time      { get; set; } = DateTime.Now.ToString("HH:mm:ss");
    }

    public partial class ConnectionView : UserControl
    {
        public ObservableCollection<PacketEntry> Packets { get; } = new();
        private int _sendCount = 0;
        private int _recvCount = 0;
        private int _errorCount = 0;

        public ConnectionView()
        {
            InitializeComponent();
            PacketList.ItemsSource = Packets;
        }

        public void AddPacket(string direction, string message)
        {
            Dispatcher.Invoke(() =>
            {
                Packets.Insert(0, new PacketEntry
                {
                    Direction = direction,
                    Message   = message,
                    Time      = DateTime.Now.ToString("HH:mm:ss")
                });

                // 최근 20개만 유지
                if (Packets.Count > 20) Packets.RemoveAt(Packets.Count - 1);

                if (direction == "SEND") SendCount.Text = (++_sendCount).ToString();
                else RecvCount.Text = (++_recvCount).ToString();
            });
        }

        public void SetConnected(bool connected)
        {
            Dispatcher.Invoke(() =>
            {
                UnityDot.Fill = connected
                    ? (System.Windows.Media.Brush)FindResource("AccentGreen")
                    : (System.Windows.Media.Brush)FindResource("AccentRed");
                UnityStatusText.Text = connected ? "CONNECTED" : "WAITING";
            });
        }

        private void RestartServer_Click(object sender, RoutedEventArgs e)
        {
            // TODO: WebSocket 서버 재시작
            MessageBox.Show("서버를 재시작합니다.", "서버 재시작");
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Unity 연결 끊기
            MessageBox.Show("Unity 연결을 끊습니다.", "연결 해제");
        }
    }
}
