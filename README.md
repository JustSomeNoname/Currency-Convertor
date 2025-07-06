# 💱 Currency Convertor

A modern, lightweight Windows desktop application for real-time currency conversion built with WPF and .NET 8.

![Currency Convertor Screenshot](https://meow.justabrian.me/-uJMxKRZac5/imagea.png)

## ⚠️ Development Status

**This project is currently in development and may not work perfectly.** This is a personal learning project to explore WPF development and API integration. Expect bugs, incomplete features, and occasional crashes.

## ✨ Features

- 🌐 **Real-time Exchange Rates** - Uses Frankfurter.app API for up-to-date currency data
- ⚡ **Global Hotkey Support** - Press `Ctrl+Alt+C` to bring the app to focus (Currently broken tho \:voices:)
- 💾 **Settings Persistence** - Remembers your last used currencies and preferences
- 🔄 **Currency Swap** - Quick button to swap From/To currencies

## 🖥️ System Requirements

- **OS**: Windows 10/11 (x64)
- **Framework**: .NET 8.0 Runtime (included in self-contained build)
- **Internet**: Required for real-time exchange rates

## 🚀 Installation & Usage

### Build from Source
```bash
# Clone the repository
git clone https://github.com/JustSomeNoname/Currency-Convertor.git
cd Currency-Convertor

# Restore dependencies
dotnet restore

# Build and run
dotnet build
dotnet run

# Or publish as single executable
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
