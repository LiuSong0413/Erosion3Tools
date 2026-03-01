using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace Erosion3Tools
{
    public enum ConvertType
    {
        Map,
        EnemyData
    }

    public partial class MainWindow : Window
    {
        private string _excelPath = "";
        private string _jsonPath = "";
        private ConvertType _convertType = ConvertType.Map;

        public MainWindow()
        {
            InitializeComponent();
            UpdateUiByConvertType();
        }

        // 拖放事件
        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string path = files[0];
                    if (Path.GetExtension(path).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        _excelPath = path;
                        _jsonPath = Path.ChangeExtension(_excelPath, ".json");
                        TxtTip.Text = $"已选择文件：{Path.GetFileName(_excelPath)}";
                        TryAutoDetectMapDimensions();
                        Debug.WriteLine($"文件已选择: {_excelPath}");
                    }
                    else
                    {
                        MessageBox.Show("请拖入 .xlsx 文件");
                    }
                }
            }
        }

        // 点击“转换”按钮
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_excelPath))
            {
                MessageBox.Show("请先拖入一个 Excel 文件！");
                return;
            }

            if (!File.Exists(_excelPath))
            {
                MessageBox.Show($"文件不存在: {_excelPath}");
                return;
            }

            try
            {
                if (_convertType == ConvertType.Map)
                {
                    ConvertMapData();
                }
                else
                {
                    ConvertEnemyData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"转换失败：{ex.Message}\n\n详细错误：{ex.StackTrace}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine($"错误: {ex}");
            }
        }

        private void ConvertMapData()
        {
            if (!int.TryParse(RowTextBox.Text, out int rows) ||
                !int.TryParse(ColTextBox.Text, out int cols) ||
                rows <= 0 || cols <= 0)
            {
                MessageBox.Show("请输入有效的行数和列数（正整数）！");
                return;
            }

            var data = ExcelReader.ReadExcelColors(_excelPath, rows, cols);
            if (data.Count == 0 || data[0].Count == 0)
            {
                MessageBox.Show("未读取到任何地图数据，请检查Excel文件格式");
                return;
            }

            string jsonText = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_jsonPath, jsonText, Encoding.UTF8);

            MessageBox.Show($"地图转换成功！\n已生成文件：{_jsonPath}\n\n数据维度：{data.Count}行 x {data[0].Count}列");
        }

        private void ConvertEnemyData()
        {
            var data = ExcelReader.ReadEnemyData(_excelPath);
            if (data.Count == 0)
            {
                MessageBox.Show("未读取到任何敌人数据，请检查Excel文件格式（第一行字段名，第一列敌人ID）");
                return;
            }

            string jsonText = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_jsonPath, jsonText, Encoding.UTF8);

            MessageBox.Show($"敌人数据转换成功！\n已生成文件：{_jsonPath}\n\n敌人数量：{data.Count}");
        }

        private void ConvertTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _convertType = ConvertTypeComboBox.SelectedIndex == 1 ? ConvertType.EnemyData : ConvertType.Map;
            UpdateUiByConvertType();
            TryAutoDetectMapDimensions();
        }

        private void UpdateUiByConvertType()
        {
            if (RowLabel == null || RowTextBox == null || ColLabel == null || ColTextBox == null || TxtTip == null || BottomTipTextBlock == null)
            {
                return;
            }

            bool isMap = _convertType == ConvertType.Map;
            var visible = isMap ? Visibility.Visible : Visibility.Collapsed;

            RowLabel.Visibility = visible;
            RowTextBox.Visibility = visible;
            ColLabel.Visibility = visible;
            ColTextBox.Visibility = visible;

            if (isMap)
            {
                TxtTip.Text = string.IsNullOrWhiteSpace(_excelPath)
                    ? "请拖入 .xlsx 文件（地图）"
                    : $"已选择文件：{Path.GetFileName(_excelPath)}";
                BottomTipTextBlock.Text = "提示：拖入地图 Excel 会自动检测行列数（可手动修改）";
            }
            else
            {
                TxtTip.Text = string.IsNullOrWhiteSpace(_excelPath)
                    ? "请拖入 .xlsx 文件（敌人数据）"
                    : $"已选择文件：{Path.GetFileName(_excelPath)}";
                BottomTipTextBlock.Text = "提示：敌人表第一行是字段名，第一列是EnemyId";
            }
        }

        private void TryAutoDetectMapDimensions()
        {
            if (_convertType != ConvertType.Map || string.IsNullOrWhiteSpace(_excelPath))
            {
                return;
            }

            try
            {
                if (ExcelReader.TryDetectWorksheetDimensions(_excelPath, out int detectedRows, out int detectedCols))
                {
                    RowTextBox.Text = detectedRows.ToString();
                    ColTextBox.Text = detectedCols.ToString();
                    BottomTipTextBlock.Text = $"已自动检测：{detectedRows} 行 x {detectedCols} 列（可手动修改）";
                }
                else
                {
                    BottomTipTextBlock.Text = "未检测到有效区域，请手动填写行列数";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"自动检测地图维度失败: {ex}");
                BottomTipTextBlock.Text = "自动检测失败，请手动填写行列数";
            }
        }
    }
}
