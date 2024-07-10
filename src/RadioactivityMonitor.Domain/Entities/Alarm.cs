using RadioactivityMonitor.Domain.Interface;

namespace RadioactivityMonitor.Domain.Entities
{
    public class Alarm
    {
        /*
         *  Make LowThreshold and HighThreshold configurable outside of Alarm class based on SOLID principles
         */

        //private const double LowThreshold = 17;
        //private const double HighThreshold = 21;

        //Sensor _sensor = new Sensor();

        bool _alarmOn = false;
        private long _alarmCount = 0;
        private ISensor _sensor;
        private readonly int _alarmThreshold;
        private readonly AlarmSettings _settings;

        public Alarm(ISensor sensor, int alarmThreshold, AlarmSettings settings)
        {
            _sensor = sensor;
            _alarmThreshold = alarmThreshold;
            _settings = settings;
        }

        /*
         * Current code that does not reset the alarm when the value is within the threshold again 
         * The alarm turns on every time the value hits below or above the threshold, making the alarm turn on too frequently, therefore
         * set alarm turn on only when the alarmCount hits a certain configurable level
         */
        public void Check()
        {
            double value = _sensor.NextMeasure();

            if (value < _settings.LowThreshold || _settings.HighThreshold < value)
            {
                _alarmCount++;

                if (_alarmCount >= _alarmThreshold)
                {
                    _alarmOn = true;
                }
            }
            else
            {
                _alarmOn = false;
            }
        }



        public bool AlarmOn
        {
            get { return _alarmOn; }
        }

        /*
         * expose the alarmCount to check and ensure the counter is increasing correctly
         */
        public long AlarmCount
        {
            get { return _alarmCount; }
        }


        /*
         * Able to reset the alarm back to default
         * 
         */
        public void Reset()
        {
            _alarmOn = false;
            _alarmCount = 0;
        }
    }
}

