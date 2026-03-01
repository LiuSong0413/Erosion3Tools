//using System;
//using System.Collections.Generic;
//using System.Drawing;

//namespace Erosion3Tools
//{
//    public static class ColorNameMapper
//    {
//        //常用颜色映射（HEX到名称）
//        private static readonly Dictionary<string, string> HexToName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
//        {
//            // 基础颜色
//            {"#FF0000", "red"},
//            {"#00FF00", "lime"},
//            {"#0000FF", "blue"},
//            {"#FFFF00", "yellow"},
//            {"#FF00FF", "magenta"},
//            {"#00FFFF", "cyan"},
//            {"#000000", "black"},
//            {"#FFFFFF", "white"},
//            {"#808080", "gray"},
//            {"#FFA500", "orange"},
//            {"#800080", "purple"},
//            {"#008080", "teal"},
//            {"#800000", "maroon"},
//            {"#008000", "green"},
//            {"#000080", "navy"},

//            // 粉色系
//            {"#FFC0CB", "pink"},
//            {"#FFB6C1", "lightpink"},
//            {"#FF69B4", "hotpink"},
//            {"#FF1493", "deeppink"},
//            {"#DB7093", "palevioletred"},

//            // 红色系
//            {"#DC143C", "crimson"},
//            {"#F08080", "lightcoral"},
//            {"#CD5C5C", "indianred"},
//            {"#FA8072", "salmon"},
//            {"#E9967A", "darksalmon"},

//            // 橙色系
//            {"#FF8C00", "darkorange"},
//            {"#FFD700", "gold"},
//            {"#FFDAB9", "peachpuff"},
//            {"#FFE4B5", "moccasin"},

//            // 黄色系
//            {"#F0E68C", "khaki"},
//            {"#FFFACD", "lemonchiffon"},
//            {"#FAFAD2", "lightgoldenrodyellow"},

//            // 绿色系
//            {"#90EE90", "lightgreen"},
//            {"#98FB98", "palegreen"},
//            {"#00FA9A", "mediumspringgreen"},
//            {"#7CFC00", "lawngreen"},
//            {"#7FFF00", "chartreuse"},
//            {"#32CD32", "limegreen"},
//            {"#228B22", "forestgreen"},
//            {"#2E8B57", "seagreen"},
//            {"#6B8E23", "olivedrab"},
//            {"#556B2F", "darkolivegreen"},

//            // 蓝色系
//            {"#ADD8E6", "lightblue"},
//            {"#87CEEB", "skyblue"},
//            {"#87CEFA", "lightskyblue"},
//            {"#4682B4", "steelblue"},
//            {"#6495ED", "cornflowerblue"},
//            {"#1E90FF", "dodgerblue"},
//            {"#4169E1", "royalblue"},
//            {"#0000CD", "mediumblue"},
//            {"#00008B", "darkblue"},
// {"#191970", "midnightblue"},

//            // 紫色系
//            {"#DA70D6", "orchid"},
//            {"#EE82EE", "violet"},
//            {"#DDA0DD", "plum"},
//            {"#BA55D3", "mediumorchid"},
//            {"#9370DB", "mediumpurple"},
//            {"#8A2BE2", "blueviolet"},

//            // 棕色系
//            {"#A52A2A", "brown"},
//            {"#D2691E", "chocolate"},
//            {"#CD853F", "peru"},
//            {"#DAA520", "goldenrod"},
//            {"#B8860B", "darkgoldenrod"},

//            // 白色/灰色系
//            {"#F5F5F5", "whitesmoke"},
//            {"#DCDCDC", "gainsboro"},
//            {"#D3D3D3", "lightgrey"},
//            {"#A9A9A9", "darkgray"},
//            {"#696969", "dimgray"},
//            {"#2F4F4F", "darkslategray"},

//            // 其他
//            {"#FF7F50", "coral"},
//            {"#FF6347", "tomato"},
//            {"#40E0D0", "turquoise"},
//            {"#48D1CC", "mediumturquoise"},
//            {"#00CED1", "darkturquoise"},
//            {"#AFEEEE", "paleturquoise"},
//            {"#7FFFD4", "aquamarine"},
//            {"#66CDAA", "mediumaquamarine"},
//            {"#20B2AA", "lightseagreen"},
//            {"#5F9EA0", "cadetblue"},
//            {"#007BA7", "cerulean"},
//        };

//        // 最接近的颜色名称（当没有精确匹配时）
//        private static readonly List<(string Name, Color Color)> BasicColors = new List<(string, Color)>
//        {
//            ("red", Color.Red),
//            ("green", Color.Green),
//            ("blue", Color.Blue),
//            ("yellow", Color.Yellow),
//            ("cyan", Color.Cyan),
//            ("magenta", Color.Magenta),
//            ("orange", Color.Orange),
//            ("purple", Color.Purple),
//            ("pink", Color.Pink),
//            ("brown", Color.Brown),
//            ("black", Color.Black),
//            ("white", Color.White),
//            ("gray", Color.Gray),
//            ("lime", Color.Lime),
//            ("teal", Color.Teal),
//            ("navy", Color.Navy),
//            ("maroon", Color.Maroon),
//        };

//        /// <summary>
//        /// 从HEX字符串获取颜色名称
//        /// </summary>
//        public static string GetColorName(string hex)
//        {
//            if (string.IsNullOrEmpty(hex) || hex == "none")
//                return "none";

//            // 尝试精确匹配
//            if (HexToName.TryGetValue(hex, out string name))
//                return name;

//            // 如果没有精确匹配，计算最接近的基础颜色
//            try
//            {
//                var color = ColorTranslator.FromHtml(hex);
//                return FindClosestColorName(color);
//            }
//            catch
//            {
//                return hex; // 返回原始HEX
//            }
//        }

//        /// <summary>
//        /// 从Color对象获取颜色名称
//        /// </summary>
//        public static string GetColorName(Color color)
//        {
//            if (color == Color.Empty || color == Color.Transparent)
//                return "none";

//            //先尝试通过HEX匹配
//            string hex = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
//            if (HexToName.TryGetValue(hex, out string name))
//                return name;

//            // 计算最接近的基础颜色
//            return FindClosestColorName(color);
//        }

//        /// <summary>
//        /// 计算最接近的基础颜色名称
//        /// </summary>
//        private static string FindClosestColorName(Color target)
//        {
//            double minDistance = double.MaxValue;
//            string closestName = "unknown";

//            foreach (var (name, color) in BasicColors)
//            {
//                double distance = CalculateColorDistance(target, color);
//                if (distance < minDistance)
//                {
//                    minDistance = distance;
//                    closestName = name;
//                }
//            }

//            return closestName;
//        }

//        /// <summary>
//        /// 计算两个颜色之间的欧几里得距离
//        /// </summary>
//        private static double CalculateColorDistance(Color c1, Color c2)
//        {
//            int rDiff = c1.R - c2.R;
//            int gDiff = c1.G - c2.G;
//            int bDiff = c1.B - c2.B;
//            return Math.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
//        }
//    }
//}
