using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Throttler
{
    public class Throttle
    {
        private List<Action> _queue= new List<Action>();
        private int _maximumRequestsInCurrentWindow = -1;
        private int _requestsInCurrentWindow = -1;
        private bool _queueRequests;

        private Timer _timer;

        public event EventHandler OnWindowClosed;

        public bool Accept(Action action)
        {
            if (_maximumRequestsInCurrentWindow < 0 || _requestsInCurrentWindow < _maximumRequestsInCurrentWindow)
            {
                // execute immediately
                _requestsInCurrentWindow++;
                action();
                return true;
            }
            else if (_queueRequests)
            {
                _queue.Add(action);
                return false;
            }
            else
            {
                throw new ThrottleException("Request cannot execute at the current time.");
            }
        }

        public void OpenWindow(int durationInMilliseconds, int maximumRequests)
        {
            _maximumRequestsInCurrentWindow = maximumRequests;
            _timer = new Timer(CloseWindow, null, durationInMilliseconds, 0);
        }

        private void CloseWindow(object? state)
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _maximumRequestsInCurrentWindow = -1;
                OnWindowClosed?.Invoke(this, new EventArgs());

                List<Action> current = new List<Action>(_queue);
                foreach (Action action in current)
                {
                    Accept(action);
                }
            }
        }

        public Throttle(bool queueRequests) 
        { 
            _queueRequests = queueRequests;
        }
    }

    public class ThrottleException : Exception
    {
        public ThrottleException(string message) : base(message) { }
    }
}
