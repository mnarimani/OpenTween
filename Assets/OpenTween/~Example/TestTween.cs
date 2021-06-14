using System.Diagnostics;
using System.Globalization;
using DG.Tweening;
using OpenTween;
using OpenTween.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Ease = OpenTween.Ease;
using Tween = OpenTween.Tween;

public class TestTween : MonoBehaviour
{
    [SerializeField] private Text _startup, _fps;

    public int LoopCount = 64000;
    public float TweenDuration = 1;

    private Transform[] _testTransforms;
    private bool _track;
    private float _timer;
    private int _count;

    private void Awake()
    {
        _testTransforms = new Transform[LoopCount];
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

    public void RunJobTween()
    {
        _timer = 0;
        _count = 0;
        _track = true;

        Stopwatch stopwatch = OTJobFloat();
        // Stopwatch stopwatch = JobTransforms();

        if (_startup != null) _startup.text = stopwatch.ElapsedMilliseconds.ToString();

        Debug.Log(stopwatch.ElapsedMilliseconds);
    }

    public void RunDOTween()
    {
        _timer = 0;
        _count = 0;
        _track = true;

        Stopwatch stopwatch = DOTweenGenericFloat();
        // Stopwatch stopwatch = DOTransforms();

        if (_startup != null) _startup.text = stopwatch.ElapsedMilliseconds.ToString();

        Debug.Log(stopwatch.ElapsedMilliseconds);
    }

    private void Update()
    {
        if (!_track)
            return;

        _timer += Time.deltaTime;
        _count++;
        if (_timer >= TweenDuration / 4)
        {
            if (_fps != null)
                _fps.text = (_count / (TweenDuration / 4)).ToString(CultureInfo.InvariantCulture);
            _timer = 0;
            _count = 0;
        }
    }

    private Stopwatch OTGenericFloat()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < LoopCount; i++)
        {
            /*var tween = Tween.Create<float>();
            tween.Start = 0;
            tween.End = 10;
            tween.Duration = 60;
            tween.Ease = Ease.OutQuad;
            // tween.ValueUpdated += v => { };
            tween.Play();*/
        }

        stopwatch.Stop();
        return stopwatch;
    }

    private Stopwatch OTJobFloat()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < LoopCount; i++)
        {
            float value = 0;
            Tween.Create<float>().SetStart(0).SetEnd(10).SetDuration(TweenDuration).SetOnValueUpdated(f => value = f);
        }

        stopwatch.Stop();
        return stopwatch;
    }

    private Stopwatch DOTweenGenericFloat()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < LoopCount; i++)
        {
            float value = 0;
            DOTween.To(() => value, v => value = v, 10, TweenDuration)
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
            Tween<float3> move = UnityHelpers.DOMove(t);
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
    }
}