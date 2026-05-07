using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SmartHome.Models;

namespace SmartHome.Views
{
    public partial class LogView : UserControl
    {
        public ObservableCollection<LogEntry> Logs { get; } = new();

        public LogView()
        {
            InitializeComponent();
            LogList.ItemsSource = Logs;

            // 샘플 로그
            AddLog("시스템", "WPF 서버 시작 — 포트 8080", LogType.System);
            AddLog("현관",   "침입 감지 — 강제 개방 경보 발령", LogType.Danger);
            AddLog("주방",   "연기 감지 — 인덕션 강제 종료, 환풍기 3단 가동", LogType.Danger);
            AddLog("욕실",   "누수 감지 — 메인 밸브 자동 차단", LogType.Danger);
            AddLog("욕실",   "욕조 원격 급수 시작", LogType.Manual);
            AddLog("거실",   "날씨 연동 — 냉방 자동 가동 (목표 24°C)", LogType.Auto);
            AddLog("현관",   "택배 도착 감지 — 푸시 알림 송출", LogType.Auto);
        }

        public void AddLog(string room, string message, LogType type)
        {
            Dispatcher.Invoke(() =>
            {
                Logs.Insert(0, new LogEntry
                {
                    Time = DateTime.Now.ToString("HH:mm:ss"),
                    Room = room,
                    Message = message,
                    Type = type
                });
            });
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
            => Logs.Clear();
    }
}
