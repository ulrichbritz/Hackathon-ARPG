// Scripts/Framework/GlobalSceneDirector.cs
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using AsyncRoutines;

/// <summary>
/// Global, allocation-free async scene controller using AsyncRoutines.
/// Attach once in your bootstrap scene. Survives scene loads.
/// </summary>
public sealed class GlobalSceneManager : RoutineManagerBehavior
{
    public static GlobalSceneManager I { get; private set; }

    [Header("Optional fade")]
    [SerializeField] private CanvasGroup fadeCanvas;   // optional; leave null if you don’t want fades
    [SerializeField] private float fadeDuration = 0.25f;

    // internal re-usable fields (avoid allocs)
    private readonly string[] _noScenes = Array.Empty<string>();
    private bool _isBusy;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        if (fadeCanvas != null)
        {
            fadeCanvas.blocksRaycasts = false;
            fadeCanvas.alpha = 0f;
        }
    }

    // -----------------------------
    // Public API (returns RoutineHandle so callers can cancel)
    // -----------------------------

    /// <summary> Switch to a scene in Single mode (unloads current). Optional fade & preload/activate split. </summary>
    public RoutineHandle SwitchSingle(string sceneName, bool withFade = true)
    {
        var r = SwitchSingleRoutine(sceneName, withFade);
        return Run(r);
    }

    /// <summary> Load a scene additively (keeps current). </summary>
    public RoutineHandle LoadAdditive(string sceneName, bool setActive = false, bool withFade = false)
    {
        var r = LoadAdditiveRoutine(sceneName, setActive, withFade);
        return Run(r);
    }

    /// <summary> Unload an additive scene by name. </summary>
    public RoutineHandle UnloadAdditive(string sceneName, bool withFade = false)
    {
        var r = UnloadAdditiveRoutine(sceneName, withFade);
        return Run(r);
    }

    /// <summary> Reload currently active scene (Single). </summary>
    public RoutineHandle ReloadActive(bool withFade = true)
    {
        var active = SceneManager.GetActiveScene().name;
        return SwitchSingle(active, withFade);
    }

    // -----------------------------
    // Routines
    // -----------------------------

    private async Routine SwitchSingleRoutine(string sceneName, bool withFade)
    {
        if (_isBusy) return;
        _isBusy = true;

        try
        {
            if (withFade) { await Fade(1f, fadeDuration); }

            // Begin asynchronous load with activation gating
            var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            loadOp.allowSceneActivation = false;

            // Wait until Unity reaches 0.9 progress (loaded, not activated)
            while (loadOp.progress < 0.89f) { await AsyncRoutines.RoutineExtensions.GetAwaiter(loadOp); }
            // Small safety yield
            await Routine.WaitForNextFrame();

            // Activate
            loadOp.allowSceneActivation = true;
            await AsyncRoutines.RoutineExtensions.GetAwaiter(loadOp);

            if (withFade) { await Fade(0f, fadeDuration); }
        }
        finally
        {
            _isBusy = false;
        }
    }

    private async Routine LoadAdditiveRoutine(string sceneName, bool setActive, bool withFade)
    {
        if (_isBusy) return;
        _isBusy = true;

        try
        {
            if (withFade) { await Fade(1f, fadeDuration * 0.5f); }

            var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            loadOp.allowSceneActivation = true;
            await AsyncRoutines.RoutineExtensions.GetAwaiter(loadOp);

            // Optionally set newly loaded scene active
            if (setActive)
            {
                var s = SceneManager.GetSceneByName(sceneName);
                if (s.IsValid()) { SceneManager.SetActiveScene(s); }
            }

            if (withFade) { await Fade(0f, fadeDuration * 0.5f); }
        }
        finally { _isBusy = false; }
    }

    private async Routine UnloadAdditiveRoutine(string sceneName, bool withFade)
    {
        if (_isBusy) return;
        _isBusy = true;

        try
        {
            if (withFade) { await Fade(1f, fadeDuration * 0.5f); }

            var s = SceneManager.GetSceneByName(sceneName);
            if (s.IsValid())
            {
                var unloadOp = SceneManager.UnloadSceneAsync(s);
                if (unloadOp != null) { await AsyncRoutines.RoutineExtensions.GetAwaiter(unloadOp); }
            }

            if (withFade) { await Fade(0f, fadeDuration * 0.5f); }
        }
        finally { _isBusy = false; }
    }

    // -----------------------------
    // Fade helper (no GC; time-based, frame-synced)
    // -----------------------------

    private async Routine Fade(float target, float duration)
    {
        if (fadeCanvas == null || duration <= 0f) return;

        fadeCanvas.blocksRaycasts = (target > 0f);
        float start = fadeCanvas.alpha;
        float t = 0f;
        float inv = duration > 0f ? 1f / duration : 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * inv;
            float a = Mathf.Lerp(start, target, EaseOutCubic(Mathf.Clamp01(t)));
            fadeCanvas.alpha = a;
            await Routine.WaitForNextFrame();
        }
        fadeCanvas.alpha = target;
    }

    private static float EaseOutCubic(float x)
    {
        float inv = 1f - x;
        return 1f - inv * inv * inv;
    }
}
