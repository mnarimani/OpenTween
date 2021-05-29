#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OpenTween
{
    public static class CodeGenerator
    {
        [MenuItem("Tools/OpenTween/Generate Classes")]
        public static void GenerateClasses()
        {
            string[] files = Directory.GetFiles(Application.dataPath, "OpenTween/Runtime/CodeGenerator.cs", SearchOption.AllDirectories);

            string parent = Path.GetDirectoryName(files[0]);

            GenerateAnimationTarget(parent);
        }

        private static void GenerateAnimationTarget(string parent)
        {
            string genPath = Path.Combine(parent, "OpenAnimationGen.cs");

            List<MethodInfo> methods = typeof(UnityHelpers)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => !m.IsSpecialName)
                .Where(m => m.GetParameters().Length == 1)
                .ToList();

            string targetClasses = string.Concat(methods.Select(m =>
            {
                string target = $"public {m.GetParameters()[0].ParameterType.FullName} Target;";
                string options = $"public TweenOptions<{m.ReturnType.GetGenericArguments()[0]}> Options;";
                string method = $"public ITween Play()\n{{ \nvar anim = Target.{m.Name}(); \nanim.CopyOptionsFrom(Options); \nanim.Play(); \nreturn anim; \n}} private void Reset() {{Options.ResetToDefault();}}";
                return $"\n[System.Serializable]\npublic sealed class {m.Name}Target : IAnimation\n{{\n{target}\n{options}\n{method}\n}}\n";
            }));

            string methodList = "internal static readonly string[] MethodList = { %NAMES% };"
                .Replace("%NAMES%", string.Join(",", methods.Select(GetMethodName).Distinct().Select(name => $"\"{name}\"")));

            string targetMapSwitchBody = string.Concat(methods.Select(m => $"case \"{GetMethodName(m)}\": return typeof({m.Name}Target);"));

            string nameToClass = "internal static System.Type GetTargetType(string methodName) { switch (methodName) { %SWITCH_BODY% default: return null; } }"
                .Replace("%SWITCH_BODY%", targetMapSwitchBody);

            string code = "namespace OpenTween { %TARGET_CLASSES%\npublic partial class OpenAnimation { \n#if UNITY_EDITOR\n %METHOD_LIST% %TARGET_MAP% \n#endif\n} }"
                .Replace("%METHOD_LIST%", methodList)
                .Replace("%TARGET_CLASSES%", targetClasses)
                .Replace("%TARGET_MAP%", nameToClass);

            File.WriteAllText(genPath, code);

            static string GetMethodName(MethodInfo m)
            {
                return m.GetParameters()[0].ParameterType.Name + "/" + m.Name;
            }
        }

        private static string GetWritten(Type t)
        {
            if (t.IsGenericType)
            {
                Type[] genericCount = t.GetGenericArguments();

                int index = t.Name.IndexOf("`", StringComparison.Ordinal);
                return t.Namespace + "." + t.Name.Substring(0, index) + "<" + string.Join(",", genericCount.Select(GetWritten)) + ">";
            }


            return t.FullName ?? "T";
        }

        private static IEnumerable<MethodInfo> GetAllMethods(this Type type)
        {
            foreach (var method in type.GetMethods())
            {
                yield return method;
            }

            if (type.IsInterface)
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    foreach (MethodInfo method in GetAllMethods(iface))
                    {
                        yield return method;
                    }
                }
            }
        }

        private static IEnumerable<EventInfo> GetAllEvents(this Type type)
        {
            foreach (EventInfo method in type.GetEvents())
            {
                yield return method;
            }

            if (type.IsInterface)
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    foreach (EventInfo method in GetAllEvents(iface))
                    {
                        yield return method;
                    }
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            foreach (var method in type.GetProperties())
            {
                yield return method;
            }

            if (type.IsInterface)
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    foreach (PropertyInfo method in GetAllProperties(iface))
                    {
                        yield return method;
                    }
                }
            }
        }
    }
}
#endif