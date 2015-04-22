using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = false)]
    public class TaskModule
    {
        [ScriptFunction(Name = "task_run")]
        public static Task TaskRun(dynamic callback)
        {
            if (callback != null)
            {
                return Task.Run(() => callback());
            }
            return null;
        }

        [ScriptFunction(Name = "task_await")]
        public static async void TaskAwait(Task t)
        {
            await t;
        }

        [ScriptFunction(Name = "task_delay")]
        public static Task TaskDelay(int ms)
        {
            return Task.Delay(ms);
        }

        [ScriptFunction(Name = "task_fromresult")]
        public static Task TaskFromResult(object result)
        {
            return Task.FromResult(result);
        }

        [ScriptFunction(Name = "task_source")]
        public static TaskCompletionSource<object> TaskSource()
        {
            return new TaskCompletionSource<object>();
        }

    }
}
