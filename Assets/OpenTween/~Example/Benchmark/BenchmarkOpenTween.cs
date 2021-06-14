using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using OpenTween;
using UnityEngine;
using UnityEngine.UI;

public class BenchmarkOpenTween : MonoBehaviour
{
    [SerializeField] BenchmarkSetting setting;
    [SerializeField] private Text _text;
    
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
                var cube = setting.useVisual ? GameObject.CreatePrimitive(PrimitiveType.Sphere):  new GameObject();
                cube.transform.SetParent(transform);

                var startPosition = new Vector3(-setting.cellWidth / 2 + i, 0, -setting.cellWidth / 2 + j);
                var targetPosition = new Vector3(startPosition.x, 10, startPosition.z);

                cube.transform.position = startPosition;

                var tween = UnityHelpers.DOMove(cube.transform);
                tween.Duration = setting.tweenDuration;
                tween.Start = startPosition;
                tween.End = targetPosition;
                tween.Ease = Ease.Linear;
            }
        }
        await UniTask.Delay(TimeSpan.FromSeconds(setting.tweenDuration));
        fPSDebugger.Stop();
        _text.text = fPSDebugger.GetAverageFPS().ToString();
    }

    private void Update()
    {
        fPSDebugger?.Update(Time.deltaTime);
    }
}