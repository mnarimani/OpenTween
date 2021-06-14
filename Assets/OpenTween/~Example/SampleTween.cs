using Unity.Mathematics;
using UnityEngine;

namespace OpenTween
{
    public class SampleTween : MonoBehaviour
    {
        [SerializeField] int tweenDuration = 10;
        [SerializeField] int cellWidth = 10;
    
        private void Awake()
        {
            for (int i = 0; i < cellWidth; i++)
            {
                for (int j = 0; j < cellWidth; j++)
                {
                    var startPosition = new Vector3(-cellWidth / 2 + i, 0, -cellWidth / 2 + j);
                    var targetPosition = new Vector3(startPosition.x, 10, startPosition.z);
                    var cube = new GameObject();
    
                    cube.transform.SetParent(transform);
                    cube.transform.position = startPosition;

                    Tween<float3> move = UnityHelpers.DOMove(cube.transform);
                    move.Duration = tweenDuration;
                    move.Start = startPosition;
                    move.End = targetPosition;
                    UnityEngine.Debug.Log($"From: {startPosition} - To: {targetPosition}");
                    // move.Play();
                }
            }
        }
    }
}