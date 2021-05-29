using Cysharp.Threading.Tasks;
using OpenTween;
using UnityEngine;

public class TestSequenceTween : MonoBehaviour
{
    /*[SerializeField] Transform A;
    [SerializeField] Transform B;
    [SerializeField] float duration = 1f;
    
    async void Start()
    {
        transform.position = A.transform.position;
        var seq = Sequence.Create();
        seq.Append(transform.DOMove(B.transform.position, duration));
        seq.Append(transform.DOMove(A.transform.position, duration));
        //seq.DisposeOnComplete = false;
        seq.Play();

        await seq.AwaitCompletion();
        await UniTask.Delay(1000);

        var seq2 = Sequence.Create();
        seq.Rewind();
        await seq.AwaitRewindCompletion();
        Debug.Log("Rewind Completed");
    }*/
}