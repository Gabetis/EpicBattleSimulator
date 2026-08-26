namespace FSDK
{
    using FSDK.Ads;
    using UnityEngine;

    public static class Runner
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void Run()
        {
            new GameObject("[AdsManager]", typeof(AdsManager));
        }
    }
}
