using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FireBaseInit : MonoBehaviour
{
    // Start is called before the first frame update
    public static FireBaseInit Instance;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            // StartCoroutine(WaitToLoad()); 
        });

    }
    private void Start()
    {
        // Debug.Log("FireBaseInit");
    }
}
