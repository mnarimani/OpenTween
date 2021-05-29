// using System;
// using System.Collections.Generic;
// using System.Runtime.CompilerServices;
// using Object = UnityEngine.Object;
//
// namespace OpenTween
// {
//     internal static class ComponentDelegateCache<T>
//     {
//         private static readonly Dictionary<string, Dictionary<Object, Action<T>>> MethodToValueChanged = new Dictionary<string, Dictionary<Object, Action<T>>>();
//         private static readonly Dictionary<string, Dictionary<Object, Func<T>>> MethodToStartEval = new Dictionary<string, Dictionary<Object, Func<T>>>();
//
//         public static Action<T> GetValueChanged<TComponent>(TComponent component, [CallerMemberName] string methodName = "")
//             where TComponent : Object
//         {
//             if (component == null)
//                 return null;
//
//             if (!MethodToValueChanged.TryGetValue(methodName, out Dictionary<Object, Action<T>> objectToDelegate))
//                 return null;
//
//             if (objectToDelegate == null)
//                 return null;
//
//             if (objectToDelegate.TryGetValue(component, out Action<T> action))
//                 return action;
//
//             return null;
//         }
//
//         public static Func<T> GetStartEval<TComponent>(TComponent component, [CallerMemberName] string methodName = "")
//             where TComponent : Object
//         {
//             if (component == null)
//                 return null;
//
//             if (!MethodToStartEval.TryGetValue(methodName, out Dictionary<Object, Func<T>> objectToDelegate))
//                 return null;
//
//             if (objectToDelegate == null)
//                 return null;
//
//             if (objectToDelegate.TryGetValue(component, out Func<T> func))
//                 return func;
//
//             return null;
//         }
//
//         public static void StoreValueChanged<TComponent>(TComponent component, Action<T> action, [CallerMemberName] string methodName = "")
//             where TComponent : Object
//         {
//             if (!MethodToValueChanged.TryGetValue(methodName, out Dictionary<Object, Action<T>> dictionary))
//             {
//                 dictionary = new Dictionary<Object, Action<T>>();
//                 MethodToValueChanged.Add(methodName, dictionary);
//             }
//
//             dictionary[component] = action;
//         }
//         
//         public static void StoreStartEval<TComponent>(TComponent component, Func<T> func, [CallerMemberName] string methodName = "")
//             where TComponent : Object
//         {
//             if (!MethodToStartEval.TryGetValue(methodName, out Dictionary<Object, Func<T>> dictionary))
//             {
//                 dictionary = new Dictionary<Object, Func<T>>();
//                 MethodToStartEval.Add(methodName, dictionary);
//             }
//
//             dictionary[component] = func;
//         }
//     }
// }