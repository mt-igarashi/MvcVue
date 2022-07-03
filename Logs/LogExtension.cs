using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VueMvc.Logs
{
    /// <summary>
    /// ログ拡張クラス
    /// </summary>
    public static class LogExtension
    {
        /// <summary>
        /// セパレータ
        /// </summary>
        private const string SEPARATOR = ", ";

        /// <summary>
        /// 再帰レベル
        /// </summary>
        private const int DEPTH =3;

        /// <summary>
        /// フィールドを値を文字列化します。
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <param name="depth">再帰レベル</param>
        /// <typeparam name="T">型</typeparam>
        /// <returns>文字列</returns>
        private static string ToStringFields<T>(this T obj, int depth = 1)
        {
            var fields = obj
                         .GetType()
                         .GetFields(BindingFlags.Instance | BindingFlags.Public);

            var textList = new List<string>();
            foreach(var field in fields)
            {
                try {
                    var text = $"{field.Name} : {field.GetValue(obj).ToStringEntities(depth)}";
                    textList.Add(text);
                } catch (Exception) {
                    var text = $"{field.Name} : {field.GetType().Name}";
                    textList.Add(text);
                }
            }

            return string.Join(SEPARATOR, obj, textList);
        }
        
        /// <summary>
        /// プロパティの値を文字列化します。
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <param name="depth">再帰レベル</param>
        /// <typeparam name="T">型</typeparam>
        /// <returns>文字列</returns>
        private static string ToStringProperties<T>(this T obj, int depth = 1)
        {
            var properties = obj.GetType()
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Where(c => c.CanRead && c.GetIndexParameters().Length == 0);

            var textList = new List<string>();
            foreach(var property in properties) {
                try
                {
                    var text = $"{property.Name} : {property.GetValue(obj, null).ToStringEntities(depth)}";
                    textList.Add(text);
                } catch (Exception) {
                    var text = $"{property.Name} : {property.GetType().Name}";
                    textList.Add(text);
                }
            }
            return string.Join(SEPARATOR, textList);
        }

        /// <summary>
        /// 実装クラスであるかを検証します。
        /// </summary>
        /// <param name="type">基底クラスの型</param>
        /// <param name="target">対象の型</param>
        /// <returns></returns>
        private static bool IsConcreteClassOf(this Type type, Type target)
        {
            if (type is null || target is null) {
                return false;
            }

            if (type == target) {
                return true;
            }

            if (type.BaseType == target) {
                return true;
            }

            foreach (var _interface in type.GetInterfaces())
            {
                if (_interface == target)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// クラスの値を文字列化します。
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <param name="depth">再帰レベル</param>
        /// <typeparam name="T">型</typeparam>
        /// <returns>文字列</returns>
        private static string ToStringEntities<T>(this T obj, int depth = 1) {
            if (obj is null) {
                return "null";
            }

            var type = obj.GetType();
            if (type.IsConcreteClassOf(typeof(IList))) {
                var parameters = obj as IList;
                var textList = new List<string>();
                foreach(var param in parameters) {
                    textList.Add(param.ToFormattedString(depth + 1));
                }
                return convertListToString(textList);
            }

            if (type.IsConcreteClassOf(typeof(IDictionary))) {
                var parameters = obj as IDictionary;
                var textList = new List<string>();
                foreach(var key in parameters.Keys) {
                    textList.Add($"{key.ToFormattedString(depth + 1)} : {parameters[key].ToFormattedString(depth + 1)}");
                }
                return convertListToString(textList);
            }

            return obj.ToFormattedString(depth + 1);
        }

        /// <summary>
        /// リストを文字列化します。
        /// </summary>
        /// <param name="textList">文字列リスト</param>
        /// <returns>文字列</returns>
        private static string convertListToString(List<string> textList) {
            if (textList.Count == 0) {
                return "{}";    
            } else {
                return $"{{ {string.Join(SEPARATOR, textList)} }}";    
            }
        }

        /// <summary>
        /// フォーマット後の文字列を返却します。
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <param name="depth">再帰レベル</param>
        /// <typeparam name="T">型</typeparam>
        /// <returns>文字列</returns>
        public static string ToFormattedString<T>(this T obj, int depth = 1)
        {
            if (obj is null) {
                return "null";
            }

            if (depth <= 0) {
                return obj.ToString();
            }

            if (depth > DEPTH) {
                return obj.ToString();
            }

            if (obj is Type) {
                return obj.ToString();
            }

            var type = obj.GetType();
            if (type == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (type == typeof(DateTime)) {
                DateTime datetime = (DateTime)(object)obj;
                return datetime.ToString("yyyy/MM/dd HH:mm:ss.fffffff");
            }

            if (type.IsPrimitive) {
                return obj.ToString();
            }

            if (type.IsEnum) {
                return obj.ToString();
            }

            if (type == typeof(string)) {
                return obj.ToString();
            }

            if (type == typeof(decimal)) {
                return obj.ToString();
            }

            return $"{{ {obj.ToStringProperties(depth)} }}";;
        }
    }
}