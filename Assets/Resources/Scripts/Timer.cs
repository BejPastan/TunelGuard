using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    int hour = 0;
    int minute = 0;
    Days day = 0;

    [SerializeField]
    TextMeshProUGUI timeText;
    [SerializeField]
    int tickTime = 1;
    [SerializeField]
    Animator dayCourtine;


    [SerializeField]
    Button startNewDayButton;

    //start time
    public int startHour = 6;
    public int endHour = 22;

    //start day
    public void StartDay()
    {
        hour = startHour;
        minute = 0;
        startNewDayButton.interactable = false;
        OnStartOfDay();
        dayCourtine.SetBool("Day", true);
        Debug.Log("Start day");
        if (day == Days.End)
        {
            day = Days.Monday;
        }
        TimerTick();
    }

    //end day
    private void EndDay()
    {
        CloseDay();
    }

    private async Task CloseDay()
    {
        DisplayTime();
        await Task.Delay(500);
        dayCourtine.SetBool("Day", false);
        await Task.Delay(1500);
        day = day + 1;
        DisplayTime();
        startNewDayButton.interactable = true;
    }

    //start timer
    public void StartGame()
    {
        //subscribe to event StartDay from Supply
        OnStartOfDay += FindObjectOfType<Supplies>().StartDay;
        OnEndOfDay += EndDay;
        DisplayTime();
    }

    //change time
    private async Task TimerTick()
    {
        DisplayTime();
        minute = minute + 15;
        if (minute == 60)
        {
            minute = 0;
            hour = hour + 1;
        }
        if (hour == endHour)
        {
            await Task.Delay(tickTime);
            OnEndOfDay();
            return;
        }
        await Task.Delay(tickTime);
        if (Application.isPlaying)
            TimerTick();
    }

    //void to diplay time
    private void DisplayTime()
    {
        timeText.text = day + ", " + hour.ToString("00") + ":" + minute.ToString("00");
    }


    //event for end of day
    public delegate void EndOfDay();
    public static event EndOfDay OnEndOfDay;

    //event for start of day
    public delegate void StartOfDay();
    public static event StartOfDay OnStartOfDay;
    
}

enum Days
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday,
    End
}