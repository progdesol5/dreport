using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Hangfire;
using Hangfire.SqlServer;
using Owin;

namespace Web
{
    public class Startup
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage("HangFireConnectionString", new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }

        public void Configuration(IAppBuilder app)
        {
            // Remove app.UseHangfireAspNet(GetHangfireServers);
            // Instead, use app.UseHangfireServer() to start the Hangfire server
            app.UseHangfireServer();

            app.UseHangfireDashboard();

            // Let's also create a sample background job
            //BackgroundJob.Enqueue(() => new SReport1().HangfireLoad());
            RecurringJob.AddOrUpdate(() => new SReport1().HangfireLoad(), "15 21 * * * ");
            RecurringJob.AddOrUpdate(() => new SReport2().HangfireLoad(), "45 21 * * * ");
            // ...other configuration logic
        }
    }
}