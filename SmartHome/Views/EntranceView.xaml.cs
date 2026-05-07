using System.Windows;
using System.Windows.Controls;

namespace SmartHome.Views
{
    public partial class EntranceView : UserControl
    {
        public EntranceView()
        {
            InitializeComponent();
        }

        private void LightAuto_Changed(object sender, RoutedEventArgs e)
        {
            bool isOn = LightAutoToggle.IsChecked == true;
            // TODO: WebSocket으로 패킷 송신
            // App.WsServer.SendAsync(new Packet { Room="entrance", Device="light", Payload={ ["auto"]=isOn } });
        }

        private void Brightness_Changed(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            // TODO: 슬라이더 값 변경 시 패킷 송신
        }

        private void SecurityAlert_Reset(object sender, RoutedEventArgs e)
        {
            // TODO: 경보 해제 패킷 송신
        }

        private void DoorLock_Changed(object sender, RoutedEventArgs e)
        {
            bool isLocked = DoorLockToggle.IsChecked == true;
            // TODO: 도어락 상태 패킷 송신
        }
    }
}
