using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexLabs.Glimpse.Linq2Sql
{
    public static class StateChangeHandler
    {
        static DateTime tsStart = DateTime.Now;
        public static void OnStateChange(Object o, StateChangeEventArgs stateChanged)
        {
            // linq seems to switch between open and close only
            if (stateChanged.OriginalState == ConnectionState.Open && stateChanged.CurrentState == ConnectionState.Closed)
            {
                // write the time between open => close
                var timeSpan = DateTime.Now.Subtract(tsStart);
                if (timeSpan.TotalSeconds > 0)
                    LogItemHandler.ConnectionClosed(timeSpan);
            }
            else if (stateChanged.OriginalState == ConnectionState.Closed && stateChanged.CurrentState == ConnectionState.Open)
            {
                tsStart = DateTime.Now;
            }
        }
    }
}
