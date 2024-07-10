using NUnit.Framework;
using RadioactivityMonitor.Domain.Entities;

namespace RadioactivityMonitor.Tests
{
    [TestFixture]
    public class AlarmTest
	{
        private IConfigurationRoot _configuration;
        private AlarmSettings _alarmSettings;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            _alarmSettings = new AlarmSettings();
            _configuration.GetSection("AlarmSettings").Bind(_alarmSettings);
        }

        /*
         * 
         *  Setting a value that is between the low threshold and high threshold, the alarm should be be off by default
         */
        [Test]
        public void AlarmIsOffByDefault()
		{
            var sensor = new TestSensor(18);
            var alarm = new Alarm(sensor, 0, _alarmSettings);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.False);
            Assert.That(alarm.AlarmCount, Is.EqualTo(0));
        }

        /*
         * 
         *  Setting a value that is below the low threshold. The alarm should turn on. 
         */
        [Test]
        public void AlarmActivateWhenValueBelowLowThreshold()
        {
            var sensor = new TestSensor(16);
            var alarm = new Alarm(sensor, 1, _alarmSettings);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.True);
        }

        /*
         * 
         *  Setting a value that is exactly at low threshold, alarm should still be off
         */
        [Test]
        public void AlarmActivateWhenValueIsExactlyAtLowThreshold()
        {
            var sensor = new TestSensor(17);
            var alarm = new Alarm(sensor, 1, _alarmSettings);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.False);
        }

        /*
         * 
         *  Setting a value that is above the high threshold. The alarm should turn on
         */
        [Test]
        public void AlarmActivateWhenValueAboveHighThreshold()
        {
            var sensor = new TestSensor(22);
            var alarm = new Alarm(sensor, 1, _alarmSettings);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.True);
        }

        /*
         * 
         *  Setting a value that is exactly at high threshold, alarm should still be off
         */
        [Test]
        public void AlarmActivateWhenValueIsExactlyAtHighThreshold()
        {
            var sensor = new TestSensor(21);
            var alarm = new Alarm(sensor, 1, _alarmSettings);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.False);
        }

        /*
         * 
         *  The alarm does not go off when the alarm threshold is not hit
         */
        [Test]
        public void AlarmDoesNotActivateWhenAlarmThresholdNotHit()
        {
            var sensor = new TestSensor(22);
            var alarm = new Alarm(sensor, 2, _alarmSettings);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.False);

            sensor.SetNextMeasure(18);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.False);

        }

        /*
         * 
         *  The alarm goes off when the alarm threshold is hit
         */
        [Test]
        public void AlarmActivateWhenAlarmThresholdHit()
        {
            var sensor = new TestSensor(22);
            var alarm = new Alarm(sensor, 2, _alarmSettings);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.False);

            sensor.SetNextMeasure(18);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.False);

            sensor.SetNextMeasure(24);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.True);
            Assert.That(alarm.AlarmCount, Is.EqualTo(2));

        }


        /*
        * 
        *  Alarm count and alarm resets when reset function is hit
        */
        [Test]
        public void AlarmResetsCorrectly()
        {
            var sensor = new TestSensor(22);
            var alarm = new Alarm(sensor, 2, _alarmSettings);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.False);

            sensor.SetNextMeasure(18);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.False);

            sensor.SetNextMeasure(24);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.True);
            Assert.That(alarm.AlarmCount, Is.EqualTo(2));

            alarm.Reset();

            sensor.SetNextMeasure(16);
            alarm.Check();
            Assert.That(alarm.AlarmOn, Is.False);
            Assert.That(alarm.AlarmCount, Is.EqualTo(1));

        }

    }
}

