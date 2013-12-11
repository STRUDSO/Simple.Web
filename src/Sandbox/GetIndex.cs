using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace Sandbox
{
    using System.Threading.Tasks;
    using Simple.Web;
    using Simple.Web.Behaviors;
    [UriTemplate("/ChildAction")]
    public class GetChildAction : IOutput<RawHtml>, IGetAsync
    {
        private DateTime startDate;

        public RawHtml Output
        {
            get { return string.Format("<br /> This is a child action:{0} <br /> @Action(/ChildAction2)", startDate); }
        }

        public async Task<Status> Get()
        {
            startDate = DateTime.Now;
            var sw = new Stopwatch();
            sw.Start();
            await Task.Delay(3000);
            sw.Stop();

            return Status.OK;
        }
    }

    [UriTemplate("/ChildAction2")]
    public class GetChild2Action : IOutput<RawHtml>, IGet
    {
        private Guid value;

        public RawHtml Output
        {
            get { return string.Format("<br /> This is a child action2:{0}", value); }
        }

        public Status Get()
        {
            value = Guid.NewGuid();
            return Status.OK;
        }
    }

    [UriTemplate("/")]
    public class GetIndex : IGetAsync, IOutput<RawHtml>, IDisposable
    {
        public Task<Status> Get()
        {
            return DoLongRunningThing()
                .ContinueWith(t => Status.OK);
        }

        public RawHtml Output
        {
            get { return "<h2>Simple.Web is making your life easier.</h2> " +
                         "@Action(/ChildAction)"
                         + "@Action(/ChildAction)"
                         + "@Action(/ChildAction)"
                         + "@Action(/ChildAction)"
                         + "@Action(/ChildAction)"
                         + "@Action(/ChildAction)"
                         + "@Action(/ChildAction)"
                         ; 
            }
        }

        private Task DoLongRunningThing()
        {
            return Task.Factory.StartNew(() => { });
        }

        public void Dispose()
        {
            Trace.WriteLine("Disposing GetIndex");
        }
    }
}