using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ES
{
    public static class ExtensionForString
    {

        public static StringBuilder _GetBuilder(this string selfStr)
        {
            return new StringBuilder(selfStr);
        }


        public static StringBuilder _AddPre(this StringBuilder self, string prefixString)
        {
            self.Insert(0, prefixString);
            return self;
        }
        public static int _AsInt(this string selfStr, int defaulValue = 0)
        {
            var retValue = defaulValue;
            return int.TryParse(selfStr, out retValue) ? retValue : defaulValue;
        }

        public static long _AsLong(this string self, long defaultValue = 0)
        {
            var retValue = defaultValue;
            return long.TryParse(self, out retValue) ? retValue : defaultValue;
        }


        public static DateTime _AsDateTime(this string selfStr, DateTime defaultValue = default(DateTime))
        {
            return DateTime.TryParse(selfStr, out var retValue) ? retValue : defaultValue;
        }



        public static float _AsFloat(this string selfStr, float defaultValue = 0)
        {
            return float.TryParse(selfStr, out var retValue) ? retValue : defaultValue;
        }


        public static bool _ContainsChineseCharacterOnly(this string input)
        {
            return Regex.IsMatch(input, @"[\u4e00-\u9fa5]");
        }
        public static bool _ContainChineseCharacterOrChineseSymbol(this string input)
        {
            return Regex.IsMatch(input, @"[\u4e00-\u9fa5]") || Regex.IsMatch(input, @"[。？！，、；：“”‘’（）《》——……·【】·^ ∧ ¶ =  # / ∞ △ ※ ●＋ － × ÷ ± ≌ ∽ ≤ ≥ ≠ ≡ ∫ ∮ ∑ ∏]");
        }
        public static bool _ContainChineseCharacterOrNormalSymbol(this string input)
        {
            return Regex.IsMatch(input, @"[\u4e00-\u9fa5]")
                || Regex.IsMatch(input, @"[。？！，、；：“”‘’（）《》——……·【】·^ ∧ ¶ =  # / ∞ △ ※ ●＋ － × ÷ ± ≌ ∽ ≤ ≥ ≠ ≡ ∫ ∮ ∑ ∏]")
                || Regex.IsMatch(input, @"[. , ? ! ' "" : ; ... — –  ( )   { } + − × ÷ = ≠ ≈ ± ≤ ≥ % ° °C °F π ∫ ∑ ∏ √ ∞ / \ | # & * ~ @ $ £ € ¥ ¢ ^]");

        }

        public static bool _HasSpace(this string input)
        {
            return input.Contains(" ");
        }


        public static string _RemoveString(this string str, params string[] targets)
        {
            return targets.Aggregate(str, (current, t) => current.Replace(t, string.Empty));
        }


        public static string _StringJoin(this IEnumerable<string> self, string separator)
        {
            return string.Join(separator, self);
        }



        // ================== 基础截取方法 ==================

        /// <summary>
        /// 从字符串前端截取到指定分隔符
        /// </summary>
        public static string _KeepBefore(this string source, string separator,
                                        bool includeSeparator = false,
                                        StringComparison comparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(source)) return source;

            int index = source.IndexOf(separator, comparison);
            if (index < 0) return source; // 未找到分隔符返回原字符串

            return includeSeparator ?
                source.Substring(0, index + separator.Length) :
                source.Substring(0, index);
        }

        /// <summary>
        /// 从字符串后端截取到指定分隔符
        /// </summary>
        public static string _KeepAfter(this string source, string separator,
                                           bool includeSeparator = false,
                                           StringComparison comparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(source)) return source;

            int index = source.LastIndexOf(separator, comparison);
            if (index < 0) return source; // 未找到分隔符返回原字符串

            return includeSeparator ?
                source.Substring(index) :
                source.Substring(index + separator.Length);
        }

        // ================== 保留操作 ==================

        /// <summary>
        /// 保留分隔符之前的内容
        /// </summary>
        public static string _KeepBeforeCutFlag(this string source, string separator,
                                       StringComparison comparison = StringComparison.Ordinal)
        {
            return source._KeepBefore(separator, false, comparison);
        }

        /// <summary>
        /// 保留分隔符之后的内容
        /// </summary>
        public static string _KeepAfterCutFlag(this string source, string separator,
                                      StringComparison comparison = StringComparison.Ordinal)
        {
            return source._KeepAfter(separator, false, comparison);
        }

        /// <summary>
        /// 保留两个分隔符之间的内容
        /// </summary>
        public static string _KeepBetween(this string source, string startSeparator, string endSeparator,
                                        bool includeSeparators = false,
                                        StringComparison comparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(source)) return source;

            int startIndex = source.IndexOf(startSeparator, comparison);
            if (startIndex < 0) return string.Empty;

            int endIndex = source.IndexOf(endSeparator, startIndex + startSeparator.Length, comparison);
            if (endIndex < 0) return string.Empty;

            if (includeSeparators)
            {
                return source.Substring(startIndex, endIndex - startIndex + endSeparator.Length);
            }

            return source.Substring(
                startIndex + startSeparator.Length,
                endIndex - startIndex - startSeparator.Length
            );
        }

        // ================== 字符版本（优化性能） ==================

        /// <summary>
        /// 保留指定字符之前的内容
        /// </summary>
        public static string _KeepBeforeByChar(this string source, char separator)
        {
            if (string.IsNullOrEmpty(source)) return source;

            int index = source.IndexOf(separator);
            return index < 0 ? source : source.Substring(0, index);
        }

        /// <summary>
        /// 保留指定字符之后的内容
        /// </summary>
        public static string _KeepAfterByChar(this string source, char separator)
        {
            if (string.IsNullOrEmpty(source)) return source;

            int index = source.LastIndexOf(separator);
            return index < 0 ? source : source.Substring(index + 1);
        }
    }

}