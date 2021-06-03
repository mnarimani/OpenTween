public class FPSDebugger
{
    int frameCounter;
    float totalFPS;
    bool active;
    public FPSDebugger()
    {
        frameCounter = 0;
        totalFPS = 0;
        active = true;
        UnityEngine.Debug.Log("Fps Debugging started...");
    }

    public float GetAverageFPS()
    {
        return totalFPS / frameCounter;
    }

    public void Stop()
    {
        active = false;
        UnityEngine.Debug.Log($"Fps Debugging stopped with average FPS:{GetAverageFPS()}\nFrames:{frameCounter}");
    }

    public void Update(float deltaTime)
    {
        if (active)
        {
            totalFPS += (1 / deltaTime);
            frameCounter++;
        }
    }
}
