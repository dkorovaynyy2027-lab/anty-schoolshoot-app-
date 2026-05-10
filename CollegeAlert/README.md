<div align="center">
  <img src="https://i.pinimg.com/736x/ab/43/c7/ab43c74ea438e100a0a62516c13e1fac.jpg" alt="eBezpeka Banner" width="400" style="border-radius: 20px; box-shadow: 0 10px 30px rgba(0,0,0,0.3); margin-bottom: 20px;"/>

  # 🛡️ eBezpeka (College Alert)
  ### *Professional Emergency Response System for Educational Institutions*
  ### *Професійна система екстреного оповіщення для навчальних закладів*

  [![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/apps/maui)
  [![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/apps/aspnet)
  [![SignalR](https://img.shields.io/badge/SignalR-0078D4?style=for-the-badge&logo=google-cloud&logoColor=white)](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction)
  [![Platform: Android](https://img.shields.io/badge/Android-3DDC84?style=for-the-badge&logo=android&logoColor=white)](https://www.android.com/)
  [![Platform: iOS](https://img.shields.io/badge/iOS-000000?style=for-the-badge&logo=apple&logoColor=white)](https://www.apple.com/ios/)

  [🇺🇦 Перейти до української версії](#-українська-версія) | [🇬🇧 Switch to English Version](#-english-version)
</div>

---

## 🇺🇦 Українська версія

### 📖 Огляд проекту
**eBezpeka** — це надійна система екстреного оповіщення з низькою затримкою, розроблена спеціально для освітніх установ. Вона забезпечує миттєвий двосторонній зв'язок між студентами/викладачами та службою безпеки. Головна особливість — робота в межах локальної мережі Wi-Fi, що робить систему автономною від глобального інтернету та хмарних сервісів.

### 🌟 Основні можливості
- **🚨 Миттєвий SOS:** Можливість відправити сигнал тривоги одним натисканням, вказавши конкретну локацію (корпус та поверх).
- **🛡️ Пульт охорони:** Окремий інтерфейс для адміністраторів, де відображаються всі активні сигнали в реальному часі.
- **📢 Глобальне оповіщення:** Після підтвердження загрози адміністратором, сигнал тривоги миттєво розсилається на всі підключені пристрої.
- **📊 Моніторинг статусів:** Під час тривоги студенти можуть вказати свій статус:
  - ✅ **Safe** (У безпеці)
  - ⚠️ **In Danger** (У небезпеці)
  - 🚶 **Away** (Відсутній у закладі)
- **📈 Жива статистика:** Охорона бачить кількість людей у кожній категорії, що допомагає координувати евакуацію.
- **🔔 Системні сповіщення:** Використання звукових сигналів та вібрації навіть якщо телефон у кишені.

### 🏗️ Архітектура та Технології
Проект побудований на сучасному стеку Microsoft:
1.  **Сервер (ASP.NET Core):** Обробляє логіку груп, підключення та трансляцію повідомлень. Використовує **SignalR** (WebSockets) для мінімальної затримки.
2.  **Мобільний додаток (.NET MAUI):** Єдиний код для Android та iOS, що забезпечує нативну продуктивність.
3.  **Локальна мережа:** Система спроектована для розгортання на сервері всередині коледжу.

### 🚀 Налаштування та Запуск

#### 1. Запуск Сервера
Перейдіть до папки сервера та запустіть проект:
```bash
cd CollegeAlert/Server/CollegeAlert.Server
dotnet run
```
*За замовчуванням сервер працює на порту `5050`.*

#### 2. Налаштування Клієнта
Перед збіркою необхідно вказати IP-адресу вашого сервера у файлі `CollegeAlert/Services/AlertService.cs`:
```csharp
string url = "http://ВАШ_ЛОКАЛЬНИЙ_IP:5050/alerthub";
```

#### 3. Компіляція
- **Android:** `dotnet build -t:Run -f net10.0-android`
- **iOS:** `dotnet build -t:Run -f net10.0-ios -p:RuntimeIdentifier=ios-arm64`

### 📡 Документація API (SignalR Hub)
- `SendAlertSignal`: Відправити сигнал SOS від студента.
- `ConfirmAlert`: Підтвердити тривогу (тільки для адмінів).
- `UpdateStatus`: Оновити стан безпеки користувача.
- `EndAlert`: Оголосити відбій тривоги.

---

## 🇬🇧 English Version

### 📖 Project Overview
**eBezpeka** is a robust, low-latency emergency alert system designed for educational institutions. It facilitates instant two-way communication between students/staff and security personnel. The primary advantage is its operation within a local Wi-Fi network, making it autonomous from the global internet and external cloud services.

### 🌟 Key Features
- **🚨 Instant SOS:** Send a distress signal with one tap, specifying building and floor.
- **🛡️ Security Dashboard:** A dedicated interface for administrators to monitor all active signals in real-time.
- **📢 Global Broadcast:** Once an admin confirms a threat, the alert is instantly pushed to every connected device.
- **📊 Status Monitoring:** During an alert, students can report their safety status:
  - ✅ **Safe**
  - ⚠️ **In Danger**
  - 🚶 **Away**
- **📈 Live Statistics:** Security sees the head count for each category to coordinate evacuation.
- **🔔 System Notifications:** Audible alerts and vibrations to ensure the message is received.

### 🏗️ Architecture & Tech Stack
The project is built on the modern Microsoft stack:
1.  **Server (ASP.NET Core):** Manages group logic, connections, and message broadcasting via **SignalR** (WebSockets).
2.  **Mobile App (.NET MAUI):** Cross-platform application for Android and iOS with native performance.
3.  **Local Network:** Designed to run on a local server within the institution.

### 🚀 Setup & Installation

#### 1. Running the Server
Navigate to the server directory and start the project:
```bash
cd CollegeAlert/Server/CollegeAlert.Server
dotnet run
```
*The server defaults to port `5050`.*

#### 2. Configuring the Client
Update the server IP address in `CollegeAlert/Services/AlertService.cs` before building:
```csharp
string url = "http://YOUR_LOCAL_IP:5050/alerthub";
```

#### 3. Compilation
- **Android:** `dotnet build -t:Run -f net10.0-android`
- **iOS:** `dotnet build -t:Run -f net10.0-ios -p:RuntimeIdentifier=ios-arm64`

### 📡 API Documentation (SignalR Hub)
- `SendAlertSignal`: Initiates an SOS signal from a student.
- `ConfirmAlert`: Escalates to a global alert (Admin only).
- `UpdateStatus`: Updates the user's safety state.
- `EndAlert`: Terminates the active alert.

---

## 🛠️ Виправлення помилок / Troubleshooting

- **UA:** Переконайтеся, що брандмауер сервера дозволяє вхідні з'єднання на порт `5050`. Якщо додаток не бачить сервер, перевірте, чи знаходяться вони в одній Wi-Fi мережі.
- **EN:** Ensure the server firewall allows incoming traffic on port `5050`. If the app can't connect, verify both devices are on the same Wi-Fi subnet.

---

## 🗺️ План розвитку / Roadmap

- [ ] **Push Notifications:** Інтеграція з FCM/APNs для роботи у фоновому режимі (Background mode).
- [ ] **History:** База даних для логування всіх минулих інцидентів.
- [ ] **Web Admin:** Веб-панель для зручного моніторингу з великих екранів.
- [ ] **Map Integration:** Візуалізація місця виклику на плані будівлі.

---
<div align="center">
  <i>Developed with ❤️ by <b>ptrklxord</b></i>
</div>
