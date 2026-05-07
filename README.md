# 🏠 스마트홈 통합 관제 시스템

> **C# WPF + Unity를 연동한 실시간 디지털 트윈(Digital Twin) 스마트홈 제어 플랫폼**
>
> WPF가 메인 컨트롤러(서버) 역할을 수행하고, Unity가 3D 시각화 인터페이스(클라이언트)로 동작합니다.  
> WebSocket과 JSON 패킷으로 두 애플리케이션을 실시간 연동하여 가상 주거 공간에서 집안의 모든 기기를 직관적으로 제어합니다.

<br/>

## 📌 프로젝트 개요

본 프로젝트는 IoT 기술인 **디지털 트윈** 개념을 주거 공간에 접목한 시스템입니다.

- **WPF (Control Layer)**: 집안의 모든 IoT 기기를 제어하는 메인 컨트롤러. JSON 패킷 생성 및 WebSocket 서버 역할
- **Unity (Visualization Layer)**: 실제와 유사한 3D 가상 주거 공간을 구성하는 모니터링 인터페이스. WebSocket 클라이언트로 수신된 JSON에 맞춰 오브젝트 상태를 즉각 업데이트
- **WebSocket + JSON (Communication Layer)**: 두 프로세스 간의 실시간 양방향 데이터 동기화

<br/>

## 🏗️ 시스템 아키텍처

```
┌─────────────────────────────────┐        ┌──────────────────────────────────┐
│      WPF (Control Layer)        │        │   Unity (Visualization Layer)    │
│                                 │        │                                  │
│  ┌─────────┐  ┌──────────────┐  │        │  ┌───────────┐  ┌────────────┐  │
│  │  GUI    │  │  WebSocket   │  │  JSON  │  │ WebSocket │  │  3D Scene  │  │
│  │ 제어판  │→ │  Server      │──┼───────→┼─→│ Client    │→ │  오브젝트  │  │
│  │(5개 공간)│  │ (Port 8080)  │  │        │  │           │  │  상태 업데이트│ │
│  └─────────┘  └──────────────┘  │        │  └───────────┘  └────────────┘  │
│         ↓                       │        │                                  │
│  ┌─────────────────────────┐    │        │  - 조명 (Light)                  │
│  │ LogView / ConnectionView│    │        │  - 애니메이션 (Animator)          │
│  └─────────────────────────┘    │        │  - 파티클 시스템                  │
└─────────────────────────────────┘        └──────────────────────────────────┘
         .NET 8 / C#                                    Unity (C#)
```

<br/>

## ✨ 공간별 주요 기능

### 🚪 현관 — 보안 및 환대
| 기능 | 설명 |
|---|---|
| 현관 조명 자동 제어 | 움직임 감지 시 자동 ON/OFF, 밝기 슬라이더 조절 |
| 침입 감지 / 경보 | 강제 개방 감지 시 경고 알림 및 조명 점멸, 경보 해제 버튼 |
| 도어락 원격 제어 | 잠금/열림 상태 토글 및 실시간 상태 표시 |
| 스마트 택배 알림 | 도착 감지 시 푸시 알림 및 가상 오브젝트 표시 |

### 🛋️ 거실 — 환경 및 편의
| 기능 | 설명 |
|---|---|
| 실시간 날씨 연동 | 날씨 데이터에 따른 조도/온도 자동 조절 |
| 모드 통합 제어 | 영화 시청, 취침 등 모드별 가전 일괄 제어 |
| 미세먼지 대응 | 농도에 따른 창문 개폐 및 공기청정기 자동 가동 |

### 🍳 주방 — 안전 및 위생
| 기능 | 설명 |
|---|---|
| 가스/연기 감지 | 인덕션 사용 중 감지 시 강제 종료 및 환풍기 가동 |
| 과열 방지 | 장시간 사용 시 자동 종료 |
| 냉장고 알림 | 문 열림 방치 시 WPF 푸시 알림 |

### 🛏️ 안방 — 수면 케어
| 기능 | 설명 |
|---|---|
| 알람 연동 | 설정 시간에 맞춰 조명 서서히 켜짐 및 블라인드 자동 개방 |
| 수면 모드 | 대기 전력 차단 및 저소음 환경 조성 |

### 🚿 욕실 — 편의 및 건강
| 기능 | 설명 |
|---|---|
| 습도 감지 환풍기 | 샤워 시 습도에 따른 환풍기 강도 자동 조절 |
| 누수 감지 | 감지 시 메인 밸브 자동 차단 및 경고 |
| 원격 욕조 제어 | 원격 급수 및 실시간 온도 모니터링 |

<br/>

## 🛠️ 기술 스택

| 항목 | 내용 |
|---|---|
| **언어** | C# |
| **프레임워크** | .NET 8.0, WPF (Windows Presentation Foundation) |
| **UI** | XAML, Segoe UI, Custom Style (Dark Theme) |
| **통신** | WebSocket (`System.Net.WebSockets`, `System.Net.WebSockets.Client 4.3.2`) |
| **직렬화** | `System.Text.Json` |
| **시각화** | Unity (3D 가상 공간, Animator, Particle System) |
| **협업** | Git / GitHub |

<br/>

## 📁 프로젝트 구조

```
SmartHome/
├── App.xaml / App.xaml.cs          # 애플리케이션 진입점 & 전역 리소스
├── MainWindow.xaml / .cs           # 메인 윈도우 — 사이드바 네비게이션, TopBar, 시계
├── Models/
│   └── Packet.cs                   # WebSocket JSON 패킷 구조 & LogEntry 모델
├── Services/
│   └── WebSocketServer.cs          # WebSocket 서버 — Unity와 실시간 통신
└── Views/
    ├── EntranceView.xaml / .cs     # 현관 — 조명, 보안, 도어락, 택배 알림
    ├── LivingView.xaml / .cs       # 거실 — 날씨 연동, 모드 제어, 공기질
    ├── KitchenView.xaml / .cs      # 주방 — 가스/연기 감지, 냉장고 알림
    ├── BedroomView.xaml / .cs      # 안방 — 알람 연동, 수면 모드
    ├── BathroomView.xaml / .cs     # 욕실 — 환풍기, 누수 감지, 욕조 제어
    ├── ConnectionView.xaml / .cs   # 연결 상태 모니터링 — 패킷 로그, 송수신 카운트
    └── LogView.xaml / .cs          # 통합 로그 — Danger / Auto / Manual / System
```

<br/>

## 📦 WebSocket 패킷 구조

WPF와 Unity 간의 통신은 아래 JSON 규격으로 이루어집니다.

```json
{
  "room": "entrance",
  "device": "light",
  "action": "update",
  "payload": {
    "auto": true,
    "brightness": 70
  },
  "timestamp": "2025-05-07T21:28:00"
}
```

| 필드 | 설명 | 예시 |
|---|---|---|
| `room` | 제어 공간 | `entrance`, `living`, `kitchen`, `bedroom`, `bathroom` |
| `device` | 제어 기기 | `light`, `security`, `door`, `fan`, `window`, ... |
| `action` | 동작 타입 | `update`, `alert`, `reset` |
| `payload` | 기기별 상세 데이터 | `{ "brightness": 70, "auto": true }` |
| `timestamp` | 발생 시각 | `2025-05-07T21:28:00` |

<br/>

## 🚀 실행 방법

### 요구 환경

- Windows 10 / 11
- .NET 8.0 SDK
- Visual Studio 2022 (WPF 워크로드 포함)

### WPF 서버 실행

```bash
# 프로젝트 클론
git clone https://github.com/bethebestKR/SmartHome-Csharp-Project.git
cd SmartHome-Csharp-Project/SmartHome

# 빌드 및 실행
dotnet run
```

또는 Visual Studio에서 `SmartHome.sln` 열고 F5 실행.

### Unity 클라이언트 연결

Unity 프로젝트에서 WebSocket 클라이언트를 `ws://localhost:8080/` 으로 연결합니다.  
WPF 앱의 ConnectionView 탭에서 연결 상태를 확인할 수 있습니다.

<br/>

## 👥 팀원 및 역할

| 이름 | 역할 |
|---|---|
| 마영창 | 통신 레이어 WPF 총괄 / 프로토콜 설계 — **WebSocket 서버(WPF) 구현, 전체 통합 테스트** |
| 최현민 | Unity 총괄 / 3D 시각화 — 씬 구성, 에셋 배치, 파티클/애니메이터 |
| 황시선 | Unity 기능 구현 / 통신 클라이언트 — JSON 파싱 및 오브젝트 상태 업데이트 |


<br/>

---
