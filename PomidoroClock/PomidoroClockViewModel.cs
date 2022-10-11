using System.ComponentModel;
using System.Windows.Threading;

namespace PomidoroClock.ViewModel;

public class PomidoroClockViewModel : INotifyPropertyChanged
{
    DispatcherTimer Timer = new DispatcherTimer();

    DateTime workPeriod;
    DateTime shortBreakPeriod;
    DateTime LongBreakPeriod;
    DateTime period;


    private const int _pomidoroWorkPeriodMinute = 25;
    private const int _pomidoroShortBreakPeriodMinute = 5;
    private const int _pomidoroLongBreakPeriodMinute = 15;
    private const int _pomidoroNumForLongBreak = 4;
    private const int _pomidoroDayNumTask = 10;

    /* test Dataset
    private const int _pomidoroWorkPeriodMinute = 3;
    private const int _pomidoroShortBreakPeriodMinute = 1;
    private const int _pomidoroLongBreakPeriodMinute = 2;
    private const int _pomidoroNumForLongBreak = 2;
    private const int _pomidoroDayNumTask = 3;
    */

    public PomidoroClockViewModel()
    {
        IsBackEnable = false;
        IsForwardEnable = false;

        IsPlayVisible = true;
        IsPauseVisible = false;

        Timer.Tick += new EventHandler(Timer_Click);
        Timer.Interval = new TimeSpan(0, 0, 1);

        PlayCommand = new Command(OnPlay, CanPlay);
        PauseCommand = new Command(OnPause);
        BackCommand = new Command(OnBack, CanBack);
        ResetCommand = new Command(OnReset);
        ForwardCommand = new Command(OnForward, CanForward);

        Result = GetPomidorosResult();

        CurrentPeriodLengthInMinute = _pomidoroWorkPeriodMinute;

        SetPeriod(CurrentPeriodLengthInMinute);

        IsBreak = false;
    }

    string GetPomidorosResult() => $"{PomidoroCounter}/{_pomidoroDayNumTask} pomidoros per day";

    private void CheckPeriod()
    {
        if (isBreak)
        {
            CurrentPeriodLengthInMinute = _pomidoroWorkPeriodMinute;
            IsBreak = false;
        }
        else
        {
            PomidoroCounter++;
            Result = GetPomidorosResult();

            if (PomidoroCounter == _pomidoroDayNumTask) //day work done
            {
                IsPauseVisible = false;
                IsPlayVisible = true;
                CurrentPeriodLengthInMinute = 0;
                Timer.Stop();
            }
            else
            {
                if (pomidoroCounter % _pomidoroNumForLongBreak == 0)
                {
                    CurrentPeriodLengthInMinute = _pomidoroLongBreakPeriodMinute;
                }
                else
                {
                    CurrentPeriodLengthInMinute = _pomidoroShortBreakPeriodMinute;
                }
                IsBreak = true;
            }
        }

        SetPeriod(CurrentPeriodLengthInMinute);
    }

    private void Timer_Click(object sender, EventArgs e)
    {
        period = period.AddSeconds(-1);
        Minute = period.Minute;
        Second = period.Second;

        if (period.Minute == 0 && period.Second == 0)
        {
            CheckPeriod();
        }
    }

    private void ForwardWind()
    {
        CheckPeriod();
    }

    private void SetPeriod(int periodLengthInMinute)
    {
        period = new DateTime(2000, 1, 1, 0, periodLengthInMinute, 0);
        Minute = period.Minute;
        Second = period.Second;
                
        PeriodType = periodLengthInMinute switch
        {            
            0 or _pomidoroWorkPeriodMinute => $"Work {periodLengthInMinute} min",
            _pomidoroShortBreakPeriodMinute => $"Break {periodLengthInMinute} min",
            _pomidoroLongBreakPeriodMinute =>  $"Rest {periodLengthInMinute}  min",
            _ => PeriodType = ""
        };

    }

    #region **Properties**

    private bool isBackEnable = false;
    public bool IsBackEnable
    {
        get => isBackEnable;       
        set
        {
            if (isBackEnable != value)
            {
                isBackEnable = value;
                RaisePropertyChanged("isBackEnable");
            }
        }
    }


    private bool isForwardEnable = false;
    public bool IsForwardEnable
    {
        get => isForwardEnable;
        set
        {
            if (isForwardEnable != value)
            {
                isForwardEnable = value;
                RaisePropertyChanged("isForwardEnable");
            }
        }
    }

    private bool isBreak = false;
    public bool IsBreak
    {
        get => IsBreak;        
        set
        {
            if (isBreak != value)
            {
                isBreak = value;
                RaisePropertyChanged("IsBreak");
            }
        }
    }

    private bool isPlayVisible = false;
    public bool IsPlayVisible
    {
        get => isPlayVisible;        
        set
        {
            if (isPlayVisible != value)
            {
                IsForwardEnable = value;
                IsBackEnable = value;
                isPlayVisible = value;
                RaisePropertyChanged("IsPlayVisible");
            }
        }
    }

    private bool isPauseVisible = false;
    public bool IsPauseVisible
    {
        get => isPauseVisible; 
        set
        {
            if (isPauseVisible != value)
            {
                isPauseVisible = value;
                RaisePropertyChanged("IsPauseVisible");
            }
        }
    }

    private int currentPeriodLengthInMinute = 0;
    public int CurrentPeriodLengthInMinute
    {
        get => currentPeriodLengthInMinute;        
        set
        {
            if (currentPeriodLengthInMinute != value)
            {
                currentPeriodLengthInMinute = value;
                RaisePropertyChanged("CurrentPeriodLengthInMinute");
            }
        }
    }

    private int pomidoroCounter = 0;
    public int PomidoroCounter
    {
        get => pomidoroCounter;        
        set
        {
            if (pomidoroCounter != value)
            {
                pomidoroCounter = value;
                RaisePropertyChanged("PomidoroCounter");
            }
        }
    }

    private int minute = 0;
    public int Minute
    {
        get => minute;        
        set
        {
            if (minute != value)
            {
                minute = value;
                RaisePropertyChanged("Minute");
            }
        }
    }

    private int second = 0;
    public int Second
    {
        get => second;        
        set
        {
            if (second != value)
            {
                second = value;
                RaisePropertyChanged("Second");
            }
        }
    }

    private string result = "";
    public string Result
    {
        get => result;        
        set
        {
            if (result != value)
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }
    }

    private string periodType = "";
    public string PeriodType
    {
        get => periodType;        
        set
        {
            if (periodType != value)
            {
                periodType = value;
                RaisePropertyChanged("PeriodType");
            }
        }
    }
    #endregion

    #region **Commands**
    //Play command
    public Command PlayCommand
    {
        get; set;
    }

    private void OnPlay()
    {
        IsPauseVisible = true;
        IsPlayVisible = false;
        Timer.Start();
    }

    private bool CanPlay() => PomidoroCounter != _pomidoroDayNumTask;
    
    //Pause command
    public Command PauseCommand
    {
        get; set;
    }

    private void OnPause()
    {
        IsPauseVisible = false;
        IsPlayVisible = true;
        Timer.Stop();
    }

    //Back command
    public Command BackCommand
    {
        get; set;
    }

    private void OnBack()
    {
        SetPeriod(CurrentPeriodLengthInMinute);
    }

    private bool CanBack() => IsPlayVisible && (PomidoroCounter != _pomidoroDayNumTask);
    
    //Forward command
    public Command ForwardCommand
    {
        get; set;
    }

    private void OnForward()
    {
        Timer.Stop();
        ForwardWind();
        IsPauseVisible = false;
        IsPlayVisible = true;
    }

    private bool CanForward() => IsPlayVisible && (PomidoroCounter != _pomidoroDayNumTask);
    
    //Reset command
    public Command ResetCommand
    {
        get; set;
    }

    private void OnReset()
    {
        PeriodType = "";
        IsPauseVisible = false;
        IsPlayVisible = true;
        Timer.Stop();
        PomidoroCounter = 0;
        IsBreak = false;
        CurrentPeriodLengthInMinute = _pomidoroWorkPeriodMinute;
        SetPeriod(CurrentPeriodLengthInMinute);
        Result = GetPomidorosResult();
    }

    #endregion

    #region **Events**
    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
    #endregion
}

