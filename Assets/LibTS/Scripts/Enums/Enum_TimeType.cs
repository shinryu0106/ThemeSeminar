namespace LibTS
{
    /// <summary>
    /// 時間の種類
    /// </summary>
    /// <remarks>
    /// Default:フレームのブレありゲーム内時間(Time.deltaTime),
    /// FixedTime:フレームのブレなしゲーム内時間(Time.fixedDeltaTime),
    /// RealTime:現実時間(Time.unscaledDeltaTime)
    /// </remarks>
    public enum TimeType
    {
        Default,
        FixedTime,
        RealTime,
    }
}