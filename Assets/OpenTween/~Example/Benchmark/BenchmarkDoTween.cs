using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
public class BenchmarkDoTween : MonoBehaviour
{
    [SerializeField] BenchmarkSetting setting;
    private FPSDebugger fPSDebugger;

    public void StartBenchmark()
    {
        StartTest();
    }

    private async UniTask StartTest()
    {
        fPSDebugger = new FPSDebugger();
        for (int i = 0; i < setting.cellWidth; i++)
        {
            for (int j = 0; j < setting.cellWidth; j++)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cube.transform.SetParent(transform);
                var startPosition = new Vector3(-setting.cellWidth / 2 + i, 0, -setting.cellWidth / 2 + j);
                var targetPosition = new Vector3(startPosition.x, 10, startPosition.z);
                cube.transform.position = startPosition;
                cube.transform.DOMove(targetPosition, setting.tweenDuration).SetEase(Ease.Linear);
            }
        }
        await UniTask.Delay(TimeSpan.FromSeconds(setting.tweenDuration));
        fPSDebugger.Stop();
    }

    private void Update()
    {
        fPSDebugger?.Update(Time.deltaTime);
    }
}