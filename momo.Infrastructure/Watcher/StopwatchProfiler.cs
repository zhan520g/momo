using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace momo.Infrastructure.Watcher
{
    public class StopwatchProfiler : IProfiler
    {
        private readonly Stopwatch _stopwatch;

        public StopwatchProfiler()
        {
            _stopwatch = new Stopwatch();
        }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public void Reset()
        {
            _stopwatch.Reset();
        }

        public void Restart()
        {
            _stopwatch.Restart();
        }

        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;
    }
}
