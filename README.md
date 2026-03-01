# Erosion3Tools

<img width="504" height="420" alt="image" src="https://github.com/user-attachments/assets/6dc0abc1-563f-4763-9627-4bc386d15b0b" />

## 项目概述

**Erosion3Tools** 是一个数据转换工具，基于 WPF 开发。
它支持从 Excel 电子表格转换地形数据和敌人信息为 JSON 格式，用于游戏引擎或地理信息系统集成。

## 核心功能

### 📊 Excel 转 JSON 转换
- 支持**地图数据**和**敌人数据**两种转换模式
- 自定义行列数参数，灵活适配不同尺寸的地图
- 拖拽式文件上传，操作简洁直观

### 🎨 颜色处理能力
- 从 Excel 单元格提取 HEX 颜色值
- 支持颜色名称映射（如 `#FF0000` → `red`）
- 100+ 种颜色库，覆盖常用颜色及其扩展集合

### 📁 支持的格式
- **输入**：`.xlsx` 格式的 Excel 工作簿
- **输出**：标准 JSON 格式，可直接集成到应用中

## 快速开始

### 系统要求
- Windows 10/11（64-bit）
- .NET 9.0 Runtime

### 安装方式

#### 方案 1：直接运行可执行文件
```bash
# 解压后在 EXE/ 文件夹找到
Erosion3Tools.exe
```

#### 方案 2：源码编译
```bash
# 克隆仓库
git clone https://github.com/LiuSong0413/Erosion3Tools.git
cd Erosion3Tools

# 使用 Visual Studio 2024 打开 .sln 文件
# 或通过 dotnet CLI 编译
dotnet build
dotnet run
```

### 使用步骤

1. **打开应用** → 启动 Erosion3Tools.exe
2. **选择转换类型** → "地图数据" 或 "敌人数据"
3. **设置维度** → 输入正确的行列数（需与 Excel 表格一致）
4. **拖入文件** → 将 `.xlsx` 文件拖至窗口中央
5. **查看结果** → JSON 文件自动保存到项目目录

## 项目结构

```
Erosion3Tools/
├── MainWindow.xaml          # UI 界面定义
├── MainWindow.xaml.cs       # UI 交互逻辑
├── ExcelReader.cs           # Excel 文件读取核心模块
├── ColorNameMapper.cs       # 颜色名称映射库
├── App.xaml                 # 应用资源和配置
└── TODO.md                  # 功能待办清单
```

## 核心技术栈

| 技术 | 版本 | 用途 |
|------|------|------|
| .NET | 9.0 | 应用运行时 |
| WPF | - | 桌面应用界面框架 |
| ClosedXML | 0.105.0 | Excel 文件读写 |
| Newtonsoft.Json | 13.0.4 | JSON 序列化/反序列化 |
| DocumentFormat.OpenXml | 3.4.1 | Office Open XML 支持 |

## 主要特性

- **拖拽上传**：支持直接拖入文件，无需文件对话框

### 🔧 数据处理
- **颜色智能识别**：自动检测单元格颜色
- **数据验证**：检查 Excel 维度是否与用户输入匹配
- **容错机制**：处理缺失或无效数据

### 📦 输出格式
生成的 JSON 支持以下结构：
```json
{
  "rows": 15,
  "cols": 15,
  "data": [
    ["red", "blue", "green"],
    ["#FF0000", "#0000FF", "#008000"],
    ...
  ]
}
```

### 扩展开发

#### 添加新的转换类型
1. 在 `MainWindow.xaml` 中添加 ComboBoxItem
2. 在 `MainWindow.xaml.cs` 的 `ConvertTypeComboBox_SelectionChanged` 中实现逻辑
3. 在 `ExcelReader.cs` 中添加相应的数据处理方法

#### 自定义颜色映射
修改 `ColorNameMapper.cs`：
- 扩展 `ColorMap` 字典
- 调整相似度计算阈值
- 支持自定义颜色库

## 常见问题

### Q: 为什么转换失败？
**A:** 检查以下几点：
1. Excel 文件未被其他程序锁定
2. 行列数参数与实际表格尺寸一致
3. 确保使用 `.xlsx` 格式，不支持 `.xls`

### Q: JSON 输出位置在哪？
**A:** 输出文件保存在项目根目录

### Q: 支持哪些颜色格式？
**A:** 
- HEX 格式：`#FF0000`、`#F00`
- RGB 单元格填充颜色
- 颜色名称映射（100+ 种）

---

**更新日期**：2026-03-01  
**当前版本**：1.0.0
