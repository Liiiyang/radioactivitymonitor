using RadioactivityMonitor.Domain.Interface;

namespace RadioactivityMonitor.Tests
{
	public class TestSensor: ISensor
	{
        private double _nextMeasure;

        public TestSensor(double nextMeasure)
        {
            _nextMeasure = nextMeasure;
        }

        public void SetNextMeasure(double nextMeasure)
        {
            _nextMeasure = nextMeasure;
        }

        public double NextMeasure()
        {
            return _nextMeasure;
        }
    }
}

