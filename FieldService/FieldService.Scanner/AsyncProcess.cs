using System;
using System.Threading.Tasks;

namespace FieldService.Scanner
{
    public abstract class AsyncProcess
    {
        protected Task _task;
        protected TaskCompletionSource<bool> _stopping;

        protected abstract Task StartUp();
        protected abstract void DoWork();

        public void Start()
        {
            if (_task != null)
                return;

            _stopping = new TaskCompletionSource<bool>();
            _task = Task.Run(() => BackgroundProcess());
        }

        public void Stop()
        {
            if (_task == null)
                return;

            _stopping.SetResult(true);
            _task.Wait();
            _task = null;
            _stopping = null;
        }

        private async Task BackgroundProcess()
        {
            try
            {
                await StartUp();

                while (await ShouldContinue(500))
                {
                    DoWork();
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
        }

        private Task<bool> ShouldContinue(int timeout)
        {
            return Task.WhenAny(
                _stopping.Task.ContinueWith(_ => false),
                Task.Delay(timeout).ContinueWith(_ => true))
                .ContinueWith(t => t.Result.Result);
        }
    }
}
