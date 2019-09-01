using System;
using System.Collections.Generic;
using System.Text;

namespace momo.Infrastructure.Watcher
{
    public interface IProfiler
    {
        /// <summary>Starts, or resumes, measuring elapsed time for an interval.</summary>
        void Start();

        /// <summary>Stops measuring elapsed time for an interval.</summary>
        void Stop();

        /// <summary>Stops time interval measurement and resets the elapsed time to zero.</summary>
        void Reset();

        /// <summary>
        /// Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.
        /// Restart = Reset + Start
        /// </summary>
        void Restart();

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in milliseconds.
        /// </summary>
        long ElapsedMilliseconds { get; }
    }
}
