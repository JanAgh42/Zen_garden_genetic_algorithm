using System.Diagnostics;

namespace ZenGarden.src.logic
{
    // singleton class
    class Counter
    {
        private static Counter instance = null!;
        private readonly Stopwatch _counter;

        private Counter()
        {
            _counter = new Stopwatch();
        }

        // returns the existing Counter instance (or creates one if null)
        public static Counter Get()
        {
            if(instance == null) {
                instance = new Counter();
            }
            return instance;
        }

        // starts / restarts time measurement
        public void Start()
        {
            if (_counter.ElapsedMilliseconds > 0) {
                _counter.Restart();
            }
            else {
                _counter.Start();
            }
        }

        // stops time measurement
        public void Stop()
        {
            _counter.Stop();
        }

        // returns the number of elapsed milliseconds with a precision of 3 decimal places
        public double GetMilliseconds()
        {
            return _counter.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)) / 1000.0;
        }
    }
}