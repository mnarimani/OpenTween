using System.Collections;
using UnityEngine;
using OpenTween;
using System;
public class TestTween : MonoBehaviour
{
    [SerializeField] Transform A;
    [SerializeField] Transform B;
    [SerializeField] float duration = 1f;
    private Vector3Tween vector3Tween;

    // Start is called before the first frame update
    void Start()
    {
        // transform.position = A.transform.position;
        // var tween = transform.DOMove(B.transform.position, 1f).SetLoops(-1, LoopType.Yoyo);
        vector3Tween = (Vector3Tween)Tween.To(A.transform.position, B.transform.position, duration, ScaleFuncs.QuarticEaseInOut);
        vector3Tween.OnUpdate += () =>
        {
            vector3Tween.StartValue = A.transform.position;
            vector3Tween.EndValue = B.transform.position;
            transform.position = vector3Tween.CurrentValue;
        };

        vector3Tween.OnComplete += () =>
        {
            vector3Tween.Start();
        };
    }
}
