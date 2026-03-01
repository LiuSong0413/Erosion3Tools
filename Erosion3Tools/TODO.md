# 待实现功能备忘

## 颜色名称映射功能

### 功能描述
将 Excel 中的 HEX 颜色值（如 `#FF0000`）转换为颜色名称（如 `red`），提供更友好的输出格式。

### 实现方案
1. 恢复 `ColorNameMapper.cs` 中的代码
2. 在输出选项中添加"颜色名称"模式
3. 支持混合输出（HEX + 颜色名称）

### 颜色映射数据
- 基础颜色：red, green, blue, yellow, cyan, magenta, orange, purple, pink, brown, black, white, gray 等
- 扩展颜色：lightgreen, darkblue, hotpink, gold 等 100+ 种常用颜色
- 最近似匹配：当没有精确匹配时，计算最接近的基础颜色

### 输出格式示例
```json
{
  "colors": [
    ["red", "blue", "none"],
    ["#FF0000", "green", "yellow"]
  ]
}
```

### 优先级
- 高

### 备注
- 当前 `ColorNameMapper.cs` 已被注释，包含完整的颜色映射逻辑
- 可直接取消注释使用，或根据需要调整

---

## 其他待实现功能

### 自动检测 Excel 维度
- 自动读取 Excel 的实际行列数
- 在界面上显示并允许调整

### 数据预览
- 转换前预览颜色网格
- 实时显示颜色矩阵

### 批量处理
- 支持拖入多个 Excel 文件
- 批量转换并保存

### 颜色统计
- 统计使用的颜色种类
- 显示每种颜色的出现次数

### 配置保存
- 保存用户的行列数选择
- 保存输出格式偏好
