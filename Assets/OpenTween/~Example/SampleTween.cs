using System;
using System.Collections;
// using OpenTween;
using DG.Tweening;
using UnityEngine;

public class SampleTween : MonoBehaviour
{
    [SerializeField] int tweenDuration = 10;
    [SerializeField] int cellWidth = 10;
    private FPSDebugger fPSDebugger;

    private void Awake()
    {
        StartTest();
    }

    private void StartTest()
    {
        fPSDebugger = new FPSDebugger();
        for (int i = 0; i < cellWidth; i++)
        {
            for (int j = 0; j < cellWidth; j++)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cube.transform.SetParent(transform);

                var startPosition = new Vector3(-cellWidth / 2 + i, 0, -cellWidth / 2 + j);
                var targetPosition = new Vector3(startPosition.x, 10, startPosition.z);

                cube.transform.position = startPosition;

                TweenWithOpenTween(startPosition, targetPosition, cube);
            }
        }
        StartCoroutine(StopDebugger());
    }

    private IEnumerator StopDebugger()
    {
        yield return new WaitForSeconds(tweenDuration);
        fPSDebugger.Stop();
    }

    private void TweenWithOpenTween(Vector3 startPosition, Vector3 targetPosition, GameObject cube)
    {
        // Debug.Log($"{startPosition} -> {targetPosition}");
        // var tween = UnityHelpers.DOMove(cube.transform);
        // tween.Duration = tweenDuration;
        // tween.Start = startPosition;
        // tween.End = targetPosition;
        // tween.Ease = Ease.Linear;
        cube.transform.DOMove(targetPosition, tweenDuration).SetEase(Ease.Linear);
    }

    private void Update()
    {
        fPSDebugger.Update(Time.deltaTime);
    }
}