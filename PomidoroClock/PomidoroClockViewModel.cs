using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PomidoroClock.ViewModel
{
    internal class PomidoroClockViewModel : INotifyPropertyChanged
    {
        DispatcherTimer Timer = new DispatcherTimer();
       
        DateTime workPeriod ;
        DateTime shortBreakPeriod;
        DateTime LongBreakPeriod;
        DateTime period;

        private const int _pomidoroWorkPeriodMinute = 25; 
        private const int _pomidoroShortBreakPeriodMinute = 5;
        private const int _pomidoroLongBreakPeriodMinute = 15;

        private readonly int _pomidoroNumForLongBreak = 4;
        private readonly int _pomidoroDayNumTask = 10;

        public PomidoroClockViewModel()
        {            
            IsPlayVisible = true;
            IsPauseVisible = false;

            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);           

            PlayCommand = new Command(OnPlay, CanPlay);
            PauseCommand = new Command(OnPause, CanPause);
            BackCommand = new Command(OnBack, CanBack);
            ResetCommand = new Command(OnReset);
            ForwardCommand = new Command(OnForward, CanForward);

            Result = $"{PomidoroCounter}/{_pomidoroDayNumTask} for day";

            CurrentPeriodLengthInMinute = _pomidoroWorkPeriodMinute;

            SetPeriod(CurrentPeriodLengthInMinute);

            IsBreak = false;            
        }

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
                Result = $"{PomidoroCounter}/{_pomidoroDayNumTask} for day";

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

            switch(periodLengthInMinute)
            {
                case 0:
                    PeriodType = $"Work {periodLengthInMinute} min";                    
                    break;
                case _pomidoroWorkPeriodMinute: PeriodType = $"Work {periodLengthInMinute} min";
                    break;
                case _pomidoroShortBreakPeriodMinute:   PeriodType = $"Break {periodLengthInMinute} min";
                    break;
                case _pomidoroLongBreakPeriodMinute:  PeriodType = $"Rest {periodLengthInMinute}  min";
                    break;
                default:  PeriodType = "";
                    break;
            }
        }

        private bool isBreak = false;
        public bool IsBreak
        {
            get
            {
                return isBreak;
            }
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
            get
            {
                return isPlayVisible;
            }
            set
            {
                if (isPlayVisible != value)
                {
                    isPlayVisible = value;
                    RaisePropertyChanged("IsPlayVisible");
                }
            }
        }

        private bool isPauseVisible = false;
        public bool IsPauseVisible
        {
            get
            {
                return isPauseVisible;
            }
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
            get
            {
                return currentPeriodLengthInMinute;
            }
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
            get
            {
                return pomidoroCounter;
            }
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
            get
            {
                return minute;
            }
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
            get
            {
                return second;
            }
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
            get
            {
                return result;
            }
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
            get
            {
                return periodType;
            }
            set
            {
                if (periodType != value)
                {
                    periodType = value;
                    RaisePropertyChanged("PeriodType");
                }
            }
        }


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

        private bool CanPlay()
        {
            return PomidoroCounter != _pomidoroDayNumTask;
        }

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

        private bool CanPause()
        {
            return true; 
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

        private bool CanBack()
        {
            return IsPlayVisible && (PomidoroCounter != _pomidoroDayNumTask); 
        }

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

        private bool CanForward()
        {
            return IsPlayVisible && (PomidoroCounter != _pomidoroDayNumTask); 
        }


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
            SetPeriod(_pomidoroWorkPeriodMinute);
            Result = $"{PomidoroCounter}/{_pomidoroDayNumTask} for day";
        }       

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
