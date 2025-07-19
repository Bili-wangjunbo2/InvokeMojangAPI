# InvokeMojangAPI

> 一个C#库，支持用户快捷调用MojangAPI

## 📦 功能特性

- ✅ 获取UUID
- ✅ 获取皮肤
- ✅ 获取披风
 
## 🚀 快速开始

### 环境要求

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- Windows 10+

### 示例代码

```bash
MojangAPI api = new MojangAPI();
Console.WriteLine(await api.GetPlayerUUID("Bili_wangjunbo2"));
Console.WriteLine(await api.GetPlayerCape(await api.GetPlayerUUID("Bili_wangjunbo2")));
