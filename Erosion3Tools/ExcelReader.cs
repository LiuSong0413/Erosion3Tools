using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using System.Globalization;

namespace Erosion3Tools
{
    // 单元格数据：包含颜色和文本
    public class CellData
    {
        public string Color { get; set; } = "none";
        public string Text { get; set; } = "";
    }

    public static class ExcelReader
    {
        public static bool TryDetectWorksheetDimensions(string path, out int rows, out int cols)
        {
            rows = 0;
            cols = 0;

            using (var workbook = new XLWorkbook(path))
            {
                var ws = workbook.Worksheets.FirstOrDefault();
                if (ws == null)
                {
                    return false;
                }

                // 地图表可能只靠颜色标记，使用 All 可以把仅有样式的单元格也纳入检测。
                var usedRange = ws.RangeUsed(XLCellsUsedOptions.All);
                if (usedRange == null)
                {
                    return false;
                }

                int firstRow = usedRange.RangeAddress.FirstAddress.RowNumber;
                int lastRow = usedRange.RangeAddress.LastAddress.RowNumber;
                int firstCol = usedRange.RangeAddress.FirstAddress.ColumnNumber;
                int lastCol = usedRange.RangeAddress.LastAddress.ColumnNumber;

                rows = lastRow - firstRow + 1;
                cols = lastCol - firstCol + 1;
                return rows > 0 && cols > 0;
            }
        }

        public static List<List<CellData>> ReadExcelColors(string path, int rows, int cols)
        {
            var result = new List<List<CellData>>();

            using (var workbook = new XLWorkbook(path))
            {
                var ws = workbook.Worksheets.First();
                var range = ws.Range(1, 1, rows, cols);

                for (int i = 1; i <= rows; i++)
                {
                    var row = new List<CellData>();

                    for (int j = 1; j <= cols; j++)
                    {
                        var cell = range.Cell(i, j);
                        var cellData = new CellData();
                        var fill = cell.Style.Fill;
                        string colorString = "none";

                        try
                        {
                            var bgColor = fill.BackgroundColor;

                            if (bgColor != null)
                            {
                                Color color;

                                if (bgColor.ColorType == XLColorType.Theme)
                                {
                                    color = workbook.Theme.ResolveThemeColor(bgColor.ThemeColor).Color;
                                }
                                else
                                {
                                    color = bgColor.Color;
                                }

                                if (color != Color.Empty && color != Color.Transparent)
                                {
                                    colorString = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                                }
                            }
                        }
                        catch
                        {
                            try
                            {
                                var patternColor = fill.PatternColor;
                                if (patternColor != null)
                                {
                                    Color color;

                                    if (patternColor.ColorType == XLColorType.Theme)
                                    {
                                        color = workbook.Theme.ResolveThemeColor(patternColor.ThemeColor).Color;
                                    }
                                    else
                                    {
                                        color = patternColor.Color;
                                    }

                                    if (color != Color.Empty && color != Color.Transparent)
                                    {
                                        colorString = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                                    }
                                }
                            }
                            catch
                            {
                                // 保持"none"
                            }
                        }

                        // 获取单元格文本
                        var cellValue = cell.GetString();
                        cellData.Color = colorString;
                        cellData.Text = string.IsNullOrEmpty(cellValue) ? "" : cellValue;

                        row.Add(cellData);
                    }

                    result.Add(row);
                }
            }

            return result;
        }

        public static List<Dictionary<string, object>> ReadEnemyData(string path)
        {
            var result = new List<Dictionary<string, object>>();

            using (var workbook = new XLWorkbook(path))
            {
                var ws = workbook.Worksheets.First();
                var usedRange = ws.RangeUsed();
                if (usedRange == null)
                {
                    return result;
                }

                int firstRow = usedRange.RangeAddress.FirstAddress.RowNumber;
                int lastRow = usedRange.RangeAddress.LastAddress.RowNumber;
                int firstCol = usedRange.RangeAddress.FirstAddress.ColumnNumber;
                int lastCol = usedRange.RangeAddress.LastAddress.ColumnNumber;

                // 默认第一列是主键（敌人ID或事件ID），第一行是字段名。
                int idCol = firstCol;
                int headerRow = firstRow;
                string idKey = ws.Cell(headerRow, idCol).GetString().Trim();
                if (string.IsNullOrWhiteSpace(idKey))
                {
                    idKey = "EnemyId";
                }

                var headers = new List<(int colIndex, string key)>();
                for (int col = idCol + 1; col <= lastCol; col++)
                {
                    string key = ws.Cell(headerRow, col).GetString().Trim();
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        headers.Add((col, key));
                    }
                }

                for (int row = headerRow + 1; row <= lastRow; row++)
                {
                    string idValue = ws.Cell(row, idCol).GetString().Trim();
                    if (string.IsNullOrWhiteSpace(idValue))
                    {
                        continue;
                    }

                    var enemyData = new Dictionary<string, object>
                    {
                        [idKey] = idValue
                    };

                    foreach (var (colIndex, key) in headers)
                    {
                        string rawValue = ws.Cell(row, colIndex).GetString().Trim();
                        if (string.IsNullOrWhiteSpace(rawValue))
                        {
                            continue;
                        }

                        if (int.TryParse(rawValue, out int intValue))
                        {
                            enemyData[key] = intValue;
                        }
                        else if (double.TryParse(rawValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleValue))
                        {
                            enemyData[key] = doubleValue;
                        }
                        else
                        {
                            enemyData[key] = rawValue;
                        }
                    }

                    result.Add(enemyData);
                }
            }

            return result;
        }

        // 调试方法：检查前几个单元格
        public static void DebugColors(string path)
        {
            try
            {
                using (var workbook = new XLWorkbook(path))
                {
                    var ws = workbook.Worksheets.First();
                    Console.WriteLine("=== 调试颜色读取 ===");

                    for (int i = 1; i <= 3; i++)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            var cell = ws.Cell(i, j);
                            var fill = cell.Style.Fill;

                            Console.WriteLine($"Cell {cell.Address}:");
                            Console.WriteLine($"  Pattern: {fill.PatternType}");
                            Console.WriteLine($"  BgColor Type: {fill.BackgroundColor.ColorType}");
                            Console.WriteLine($"  BgColor: {fill.BackgroundColor.Color}");

                            if (fill.BackgroundColor.ColorType == XLColorType.Theme)
                            {
                                var resolved = workbook.Theme.ResolveThemeColor(fill.BackgroundColor.ThemeColor);
                                Console.WriteLine($"  Resolved: #{resolved.Color.R:X2}{resolved.Color.G:X2}{resolved.Color.B:X2}");
                            }
                        }
                    }
                    Console.WriteLine("====================\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"调试错误: {ex.Message}");
            }
        }

    }
}
