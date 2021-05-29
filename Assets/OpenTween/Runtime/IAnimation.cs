#if UNITASK
using Task = Cysharp.Threading.Tasks.UniTask;
using TaskCompletionSource = Cysharp.Threading.Tasks.AutoResetUniTaskCompletionSource;

#else
using Task = System.Threading.Tasks.Task;
using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource<bool>;

#endif

namespace OpenTween
{
    public interface IAnimation
    {
        ITween Play();
    }
}