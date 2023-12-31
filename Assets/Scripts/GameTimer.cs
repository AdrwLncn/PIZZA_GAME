using System.Collections;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public int Sec = 30;
    public int CurrentSec { get; private set; }
    public TextMeshProUGUI TimerText;

    void Start()
    {
        CurrentSec = Sec;
        StartCoroutine(ITimer());
    }
    
    IEnumerator ITimer()
    {
        while (true)
        {
            if (CurrentSec == 0)
            {
                TimerText.color = Color.white;
                CurrentSec = Sec;
                TimerText.text = CurrentSec.ToString();
                yield return new WaitForSeconds(1);
                continue;
            }
            if (CurrentSec == 6)
                TimerText.color = Color.red;
            CurrentSec -= 1;
            TimerText.text = CurrentSec.ToString();
            yield return new WaitForSeconds(1);
        }
    }
}
