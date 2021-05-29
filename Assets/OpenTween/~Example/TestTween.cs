using System;
using System.Diagnostics;
using DG.Tweening;
using OpenTween;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Ease = OpenTween.Ease;
using Tween = OpenTween.Tween;

public class TestTween : MonoBehaviour
{
    /*[SerializeField] private Text _startup, _fps;

    private Transform[] _testTransforms;
    private bool _track;
    private float _timer;
    private int _count;

    private void Awake()
    {
        _testTransforms = new Transform[64000];
        for (var i = 0; i < _testTransforms.Length; i++)
        {
            _testTransforms[i] = new GameObject("Test").transform;
            _testTransforms[i].position = new Vector3(-10, 0, 0);
        }
    }

    public void RunOpenTween()
    {
        _timer = 0;
        _count = 0;
        _track = true;

        // Stopwatch stopwatch = OTGenericFloat();
        Stopwatch stopwatch = OTTransforms();

        if (_startup != null) _startup.text = stopwatch.ElapsedMilliseconds.ToString();

        Debug.Log(stopwatch.ElapsedMilliseconds);
    }

    public void RunDOTween()
    {
        _timer = 0;
        _count = 0;
        _track = true;

        // Stopwatch stopwatch = DOTweenGenericFloat();
        Stopwatch stopwatch = DOTransforms();

        if (_startup != null) _startup.text = stopwatch.ElapsedMilliseconds.ToString();

        Debug.Log(stopwatch.ElapsedMilliseconds);
    }

    private void Update()
    {
        if (!_track)
            return;

        _timer += Time.deltaTime;
        _count++;
        if (_timer >= 10)
        {
            if (_fps != null)
                _fps.text = (_count / 10f).ToString();
            _timer = 0;
            _count = 0;
        }
    }

    private static Stopwatch OTGenericFloat()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < 64000; i++)
        {
            var tween = Tween.Create<float>();
            tween.Start = 0;
            tween.End = 10;
            tween.Duration = 60;
            tween.Ease = Ease.OutQuad;
            // tween.ValueUpdated += v => { };
            tween.Play();
        }

        stopwatch.Stop();
        return stopwatch;
    }

    private static Stopwatch DOTweenGenericFloat()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < 64000; i++)
        {
            float value = 0;
            DOTween.To(() => value, v => value = v, 10, 60)
                .SetEase(DG.Tweening.Ease.OutQuad);
        }

        stopwatch.Stop();
        return stopwatch;
    }

    private Stopwatch OTTransforms()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (Transform t in _testTransforms)
        {
            Tween<Vector3> move = UnityHelpers.DOMove(t);
            move.Duration = 60;
            move.Start = new Vector3(-10, 0, 0);
            move.End = new Vector3(10, 0, 0);
            move.Play();
        }

        stopwatch.Stop();
        return stopwatch;
    }

    private Stopwatch DOTransforms()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (Transform t in _testTransforms)
        {
            ShortcutExtensions.DOMove(t, new Vector3(10, 0, 0), 60).SetEase(DG.Tweening.Ease.OutQuad);
        }

        stopwatch.Stop();
        return stopwatch;
    }*/
}