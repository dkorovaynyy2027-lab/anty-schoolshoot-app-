<div align="center">
  <img src="https://i.pinimg.com/736x/ab/43/c7/ab43c74ea438e100a0a62516c13e1fac.jpg" alt="eBezpeka Banner" width="400" style="border-radius: 20px; box-shadow: 0 10px 30px rgba(0,0,0,0.3); margin-bottom: 20px;"/>

  # 🛡️ eBezpeka (College Alert)
  ### *Professional Emergency Response System for Educational Institutions*

  [![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/apps/maui)
  [![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/apps/aspnet)
  [![SignalR](https://img.shields.io/badge/SignalR-0078D4?style=for-the-badge&logo=google-cloud&logoColor=white)](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction)
  [![Platform: Android](https://img.shields.io/badge/Android-3DDC84?style=for-the-badge&logo=android&logoColor=white)](https://www.android.com/)
  [![Platform: iOS](https://img.shields.io/badge/iOS-000000?style=for-the-badge&logo=apple&logoColor=white)](https://www.apple.com/ios/)

  [🇺🇦 Українська версія](#-ukrainian-version) | [🇬🇧 English Version](#-english-version)
</div>

---

## 🇬🇧 English Version

### 📖 Project Overview
**eBezpeka** is a robust, low-latency emergency alert system designed specifically for colleges and schools. It facilitates instant communication between students and security personnel during critical incidents. The system operates within a local Wi-Fi network, ensuring maximum reliability and data privacy without reliance on external cloud services.

### 🌟 Key Features
- **Instant SOS Signaling:** Students can trigger an alert by specifying their building and floor.
- **Admin Control Center:** Real-time dashboard for security staff to monitor, confirm, or dismiss alerts.
- **Global Broadcast:** Once confirmed, an alert is instantly pushed to every connected device in the institution.
- **Safety Status Tracking:** Students can report their status (*Safe*, *In Danger*, or *Away*) during an active alert.
- **Live Statistics:** Admins receive real-time updates on the total number of students in each safety category.
- **Platform Agnostic:** Built with .NET MAUI for seamless performance on Android and iOS.

### 🏗️ System Architecture
The project follows a **Client-Server** model powered by **SignalR**:
1.  **Server (ASP.NET Core):** Acts as the central hub, managing connections, role-based groups, and message broadcasting.
2.  **Client (.NET MAUI):** The mobile application that maintains a persistent WebSocket connection to the server for sub-millisecond responsiveness.

### 🛠️ Tech Stack
- **Frontend:** .NET MAUI (C# / XAML)
- **Backend:** ASP.NET Core (SignalR Hubs)
- **Communication:** WebSockets (SignalR)
- **Notifications:** Plugin.LocalNotification for system-level alerts and vibrations.

### 🚀 Getting Started

#### 1. Server Configuration
Navigate to the server directory and run the application:
```bash
cd CollegeAlert/Server/CollegeAlert.Server
dotnet run
```
*The server defaults to port `5050` to avoid conflicts.*

#### 2. Client Setup
1.  Identify your server's local IP address (e.g., `192.168.0.102`).
2.  Update the URL in `CollegeAlert/Services/AlertService.cs`:
    ```csharp
    string url = "http://YOUR_IP:5050/alerthub";
    ```
3.  Deploy to your device:
    - **Android:** `dotnet build -t:Run -f net10.0-android`
    - **iOS:** `dotnet build -t:Run -f net10.0-ios -p:RuntimeIdentifier=ios-arm64`

---

## 🇺🇦 Ukrainian Version

### 📖 Огляд проекту
**eBezpeka** — це надійна система екстреного оповіщення з низькою затримкою, розроблена спеціально для коледжів та шкіл. Вона забезпечує миттєвий зв'язок між студентами та службою безпеки під час надзвичайних ситуацій. Система працює в межах локальної мережі Wi-Fi, що гарантує максимальну стабільність та приватність даних без залежності від зовнішніх хмарних сервісів.

### 🌟 Основні можливості
- **Миттєвий сигнал SOS:** Студенти можуть подати сигнал тривоги, вказавши корпус та поверх.
- **Центр керування адміністратора:** Панель реального часу для охорони для моніторингу, підтвердження або скасування тривог.
- **Загальне оповіщення:** Після підтвердження тривога миттєво розсилається на кожен підключений пристрій у закладі.
- **Відстеження статусу безпеки:** Студенти можуть звітувати про свій стан (*У безпеці*, *У небезпеці* або *Відсутній*) під час активної тривоги.
- **Жива статистика:** Адміністратори отримують оновлення в реальному часі про кількість студентів у кожній категорії безпеки.
- **Кросплатформеність:** Розроблено на .NET MAUI для стабільної роботи на Android та iOS.

### 🏗️ Архітектура системи
Проект базується на моделі **Клієнт-Сервер** з використанням **SignalR**:
1.  **Сервер (ASP.NET Core):** Виступає центральним вузлом, керує підключеннями, групами за ролями та розсилкою повідомлень.
2.  **Клієнт (.NET MAUI):** Мобільний додаток, що підтримує постійне WebSocket-з'єднання із сервером для реакції за лічені мілісекунди.

### 🛠️ Технологічний стек
- **Frontend:** .NET MAUI (C# / XAML)
- **Backend:** ASP.NET Core (SignalR Hubs)
- **Зв'язок:** WebSockets (SignalR)
- **Сповіщення:** Plugin.LocalNotification для системних повідомлень та вібрації.

### 🚀 Запуск та налаштування

#### 1. Налаштування сервера
Перейдіть до директорії сервера та запустіть проект:
```bash
cd CollegeAlert/Server/CollegeAlert.Server
dotnet run
```
*Сервер використовує порт `5050` за замовчуванням.*

#### 2. Налаштування клієнта
1.  Дізнайтеся локальну IP-адресу вашого сервера (наприклад, `192.168.1.5`).
2.  Оновіть URL у файлі `CollegeAlert/Services/AlertService.cs`:
    ```csharp
    string url = "http://ВАШ_IP:5050/alerthub";
    ```
3.  Встановіть на пристрій:
    - **Android:** `dotnet build -t:Run -f net10.0-android`
    - **iOS:** `dotnet build -t:Run -f net10.0-ios -p:RuntimeIdentifier=ios-arm64`

---

## 📡 SignalR Hub API (Technical Documentation)

### Client to Server Methods
- `SendAlertSignal(string building, string floor)`: Initiates a pending SOS signal.
- `ConfirmAlert(string incidentId, string building, string floor)`: Escalates a signal to a global alert.
- `UpdateStatus(string deviceId, string status)`: Updates student's safety status.
- `EndAlert()`: Terminates the active alert state.
- `JoinGroup(string role)`: Assigns the connection to "Admin" or default group.

### Server to Client Events
- `ReceivedPendingSignal(SignalDto signal)`: Sent to Admins when SOS is pressed.
- `AlertConfirmed(string id, string building, string floor)`: Sent to All when Admin confirms danger.
- `UpdateStats(StatsDto stats)`: Sent to Admins with live status counts.
- `AlertEnded()`: Sent to All to reset the app UI.

---

## ⚠️ Important Considerations (Background Tasks)

- **Android:** Reliable in background due to flexible background service policies.
- **iOS:** iOS aggressively suspends network connections in the background. For production, **Apple Push Notification Service (APNs)** is required to wake devices. In the current local-only version, the app must be **active** on the screen to receive alerts reliably on iPhone.

---

## 🔐 Security
- **Admin Access:** Protected by a local password (default: `123`).
- **Local Network:** No data leaves the institution's Wi-Fi network.

---

## 🛠️ Troubleshooting & Debugging

- **Connection Logs:** The app uses a custom `AppLog` system. You can view real-time logs in the IDE console (Visual Studio / VS Code) to track SignalR state and UI interactions.
- **Firewall Issues:** Ensure that the server machine allows incoming connections on port `5050`.
- **IP Mismatch:** If the app fails to connect, double-check that the phone and server are on the same subnet and the IP address in `AlertService.cs` is correct.

---

## 🗺️ Roadmap & Future Improvements

- [ ] **Push Notifications:** Integration with FCM (Firebase) and APNs (Apple) for background alert delivery.
- [ ] **Auth Integration:** Support for Microsoft/Google Single Sign-On (SSO).
- [ ] **Incident History:** A database-backed log of all past alerts and status reports.
- [ ] **Web Admin Panel:** A standalone React/Vue dashboard for larger screens.
- [ ] **Multi-Building Support:** Hierarchical structure for large campuses.

---
<div align="center">
  <i>Developed with ❤️ by <b>ptrklxord</b></i>
</div>
