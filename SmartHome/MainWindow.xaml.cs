using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SmartHome.Views;

namespace SmartHome
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, UserControl> _views;
        private readonly DispatcherTimer _clock;

        public MainWindow()
        {
            InitializeComponent();

            // 뷰 초기화
            _views = new Dictionary<string, UserControl>
            {
                { "Entrance",   new EntranceView() },
                { "Living",     new LivingView() },
                { "Kitchen",    new KitchenView() },
                { "Bedroom",    new BedroomView() },
                { "Bathroom",   new BathroomView() },
                { "Connection", new ConnectionView() },
                { "Log",        new LogView() },
            };

            // 초기 화면
            MainContent.Content = _views["Entrance"];

            // 시계
            _clock = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _clock.Tick += (_, _) => ClockText.Text = DateTime.Now.ToString("HH:mm");
            _clock.Start();
            ClockText.Text = DateTime.Now.ToString("HH:mm");
        }

        // 사이드바 네비게이션
        private void NavItem_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag is string tag)
                if (_views.TryGetValue(tag, out var view))
                    MainContent.Content = view;
        }

        // TopBar 드래그
        private void TopBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void CloseButton_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();

        // Unity 연결 상태 업데이트 (WebSocket에서 호출)
        public void SetUnityConnected(bool connected)
        {
            Dispatcher.Invoke(() =>
            {
                WsDot.Fill = connected
                    ? (System.Windows.Media.Brush)FindResource("AccentGreen")
                    : (System.Windows.Media.Brush)FindResource("AccentRed");
                WsStatusText.Text = connected ? "Unity 연결됨" : "Unity 미연결";
            });
        }
    }
}
