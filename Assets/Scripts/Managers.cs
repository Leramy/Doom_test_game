using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(WeatherManager))]

[RequireComponent(typeof(ImageManager))]

[RequireComponent(typeof(AudioManager))]

public class Managers : MonoBehaviour
{
   public static WeatherManager Weather { get; private set; }
   public static ImageManager Image { get; private set; }
   public static AudioManager Audio { get; private set; }


    private List<IGameManager> _startSequence;

    private void Awake()
    {
        Weather = GetComponent<WeatherManager>();
        Image = GetComponent<ImageManager>();
        Audio = GetComponent<AudioManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Weather);
        _startSequence.Add(Image);
        _startSequence.Add(Audio);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        NetworkService network = new NetworkService();

        foreach (IGameManager manager in _startSequence)
            manager.Startup(network);

        yield return null;

        int numModules = _startSequence.Count;
        int numReady = 0;

        while(numReady < numModules)
        {
            int LastReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in _startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                    numReady++;
            }

            if (numReady > LastReady)
                Debug.Log("Progress: " + numReady + "/" + numModules);
            yield return null;
        }

        Debug.Log("All managers started up");
    }
}
