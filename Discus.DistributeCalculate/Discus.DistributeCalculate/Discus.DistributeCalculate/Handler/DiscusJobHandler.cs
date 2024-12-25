using DotXxlJob.Core.Model;
using DotXxlJob.Core;
using Newtonsoft.Json;

namespace Discus.DistributeCalculate.Handler
{
    [JobHandler("discusJobHandler")]
    public class DiscusJobHandler : AbstractJobHandler
    {
        public override async Task<ReturnT> Execute(JobExecuteContext context)
        {
            context.JobLogger.Log("receive demo job handler,parameter:{0}", context.JobParameter);
            context.JobLogger.Log("开始休眠10秒");
            context.JobLogger.Log("休眠10秒结束");
            return ReturnT.SUCCESS;
        }
    }
}
