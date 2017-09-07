using Metrics;
using Metrics.Core;
using System.Threading;

namespace SignalFxReporterExample
{
    class Program
    {
        private static TaggedMetricsContext context;
        private static Counter myCounter;
        private static Metrics.Timer myTimer;

        static void Main(string[] args)
        {
            InitializeSignalFx("SignalFxReporterExample");
            CollectData();
        }

        static void InitializeSignalFx(string contextName)
        {
            Metric.Config.WithReporting(Report => Report.WithSignalFxFromAppConfig());
            context = (TaggedMetricsContext)Metric.Context(contextName, (ctxName) => { return new TaggedMetricsContext(ctxName); });
            // Create a counter that has the dimension "api:login"
            myCounter = context.Counter("myCounter", Unit.Calls, tags: new MetricTags("api", "login"));
            // Create a timer
            myTimer = context.Timer("myTimer", Unit.Calls);
        }

        static void CollectData()
        {
            while (true)
            {
                myCounter.Increment();
                using (myTimer.NewContext())
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
