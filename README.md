# Windows Tray Memory

[![License](https://img.shields.io/github/license/Rywent/WinTrayMemory)](LICENSE)
[![GitHub Release](https://img.shields.io/github/v/release/Rywent/WinTrayMemory)](https://github.com/Rywent/WinTrayMemory/releases)
![Platform](https://img.shields.io/badge/platform-Windows-blue)
![Windows 10](https://img.shields.io/badge/Windows-10-blue?style=flat&logo=windows&logoColor=white)
![Windows 11](https://img.shields.io/badge/Windows-11-0078D4?style=flat&logo=windows11&logoColor=white)
![Windows 10 & 11](https://img.shields.io/badge/Windows-10%20&%2011-00a4ef?style=flat&logo=windows&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![C# 12](https://img.shields.io/badge/C%23-12-blueviolet?style=flat&logo=c-sharp&logoColor=white)
![WPF](https://img.shields.io/badge/UI-WPF-green)

A tray RAM monitor and cleaner for Windows. Tracks heavy processes in real time and frees memory through native WinAPI.

## Architecture & design patterns

The project is designed using **MVVM**

* **UI:** WPF views, each screen has its own View and ViewModel;
* **Memory:** WinAPI wrapper (`EmptyWorkingSet`, `NtSetSystemInformation`) and live memory stats;
* **Processes:** heaviest processes list, Safe / Warning / System classification;
* **Data:** SQLite via EF Core — settings and custom process rules;
* **Hosting:** Generic Host for DI (`Microsoft.Extensions.Hosting`);

## Core Features

* **Heavy process monitor:** only the processes that actually eat RAM, not the full Task Manager list;
* **Smart Clean:** documented Windows APIs, no registry tweaks;
* **Kill protection:** Warning asks for confirm, System is locked until you allow it in settings;
* **Auto clean:** runs when usage hits the threshold you set;
* **Custom processes:** add your own rules (Safe / Warning / System);
* **Portable:** unzip, run, done. Optional autostart and update check;

## Tech Stack

* **Framework** : .NET 8 / WPF
* **Language** : C# 12
* **MVVM** : CommunityToolkit.Mvvm
* **Database** : SQLite
* **ORM** : Entity Framework Core
* **Charts** : LiveCharts 2
* **Tray** : H.NotifyIcon.Wpf

## Main window

**RAM, CPU, uptime**

<p align="center">
  <img src="images/MainPage/ShortInformation.png" alt="RAM, CPU, uptime" width="720">
</p>

Three cards at the top of the window.

**Memory usage**

<p align="center">
  <img src="images/MainPage/MemoryUsage.png" alt="Memory usage" width="720">
</p>

Used vs available RAM, with a bar that changes color as usage grows.

**Smart Clean & heaviest processes**

<p align="center">
  <img src="images/MainPage/ActionAndHeaviestProcesses.png" alt="Smart Clean and heaviest processes" width="720">
</p>

One button to free RAM. List of the heaviest processes with type (Safe / Warning / System) and a kill action.

**Memory detailing**

<p align="center">
  <img src="images/MainPage/MemoryDetailing.png" alt="Memory detailing" width="720">
</p>

Pie chart: Applications (Private), Resources (Shared), System.

<p align="center">
  <img src="images/MainPage/MemoryDetailingExample.png" alt="Memory detailing tooltip" width="720">
</p>

Hover a slice to see the exact value.

**Memory timeline**

<p align="center">
  <img src="images/MainPage/MemoryTimeline.png" alt="Memory timeline" width="720">
</p>

RAM usage over the last 60 seconds.

<p align="center">
  <img src="images/MainPage/MemoryTimelineExample.png" alt="Memory timeline tooltip" width="720">
</p>

Hover a point on the line for the value at that second.

**Memory pressure**

<p align="center">
  <img src="images/MainPage/MemoryPressure.png" alt="Memory pressure" width="720">
</p>

Commit percent, hard faults per second, status: Good / Warning / Critical.

## Settings

<p align="center">
  <img src="images/SettingsPage/SettingsPreview.png" alt="Settings" width="720">
</p>

Settings window. Sections on the left, options on the right.

**General**

<p align="center">
  <img src="images/SettingsPage/General.png" alt="General settings" width="720">
</p>

Autostart, update check, and whether System processes can be killed.

**Process monitoring**

<p align="center">
  <img src="images/SettingsPage/ProcessMonitoring.png" alt="Process monitoring" width="720">
</p>

How often the list refreshes, RAM threshold for "heavy", and max items in the list.

**Process categories**

<p align="center">
  <img src="images/SettingsPage/ProcessCategories.png" alt="Process categories" width="720">
</p>

Built-in Safe / Warning / System lists, plus your own rules.

**Smart cleaning options**

<p align="center">
  <img src="images/SettingsPage/SmartCleaningOptions.png" alt="Smart cleaning options" width="720">
</p>

What Smart Clean actually touches: working sets, standby list, etc.

**Auto clean**

<p align="center">
  <img src="images/SettingsPage/AutoClean.png" alt="Auto clean" width="720">
</p>

Turn on automatic cleanup when RAM usage crosses the limit.

## Tray icon

Windows hides new tray icons by default. To keep WinTrayMemory on the taskbar:

**1. Settings → Personalization → Taskbar**

<p align="center">
  <img src="images/Tutorial/Personalization.jpg" alt="Windows Personalization" width="720">
</p>

**2. Other system tray icons**

<p align="center">
  <img src="images/Tutorial/TaskBar.jpg" alt="Other system tray icons" width="720">
</p>

**3. Enable WinTrayMemory**

<p align="center">
  <img src="images/Tutorial/tray.png" alt="WinTrayMemory tray toggle" width="720">
</p>

## Install

Latest build is in [Releases](https://github.com/Rywent/WinTrayMemory/releases). Unzip and run `WinTrayMemory.exe`. Admin rights are needed for Smart Clean.

## License

MIT
