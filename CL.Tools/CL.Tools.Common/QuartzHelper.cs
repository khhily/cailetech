using Quartz;
using System;

namespace CL.Tools.Common
{
    public class QuartzHelper : IJob
    {
        public IScheduler scheduler = null;

        public Quartz.Collection.HashSet<ITrigger> triggersForJob = new Quartz.Collection.HashSet<ITrigger>();

        public QuartzHelper()
        {

        }

        public void AddTrigger(string jobname, string jobgroup, string cronExpression, Action action)
        {
            //CronTrigger时间配置格式说明与实例
            //http://www.cnblogs.com/knowledgesea/p/4705796.html
            //http://blog.csdn.net/foamflower/article/details/4260645

            JobDataMap datamap = new JobDataMap();
            datamap.Add("action", action);
            ITrigger trigger = TriggerBuilder.Create().WithIdentity(jobname, jobgroup).WithCronSchedule(cronExpression).StartNow().UsingJobData(datamap).Build();
            triggersForJob.Add(trigger);
        }

        public void Start()
        {
            IJobDetail job = JobBuilder.Create(typeof(QuartzHelper)).WithIdentity("jobkey", "group").Build();
            ISchedulerFactory sf = new Quartz.Impl.StdSchedulerFactory();
            scheduler = sf.GetScheduler();
            scheduler.ScheduleJob(job, triggersForJob, true);
            scheduler.Start();
        }

        public void End()
        {
            if (scheduler != null) scheduler.Shutdown(false);
        }

        public void ReStart()
        {
            End();
            Start();

        }

        public void Execute(IJobExecutionContext context)
        {
            Action action = context.MergedJobDataMap.Get("action") as Action;
            action();
        }
    }
}
