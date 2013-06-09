using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Collections.Concurrent;
using Library.Common.Extension;

namespace Entry.Infrastructure
{
    public class SysteMessage:ISystemMessage
    {
        public SysteMessage(string sourceModule,object body,SystemMessageType type)
        {
            this.SourceModule = sourceModule;
            this.Body = body;
            this.Extensions = new Dictionary<string, string>();
            this.Type = type;
        }

        public SysteMessage(string title, string sourceModule, object body, SystemMessageType type)
            : this(sourceModule, body, type)
        {
            this.Title = title;
        }

        public string SourceModule
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public IDictionary<string, string> Extensions
        {
            get;
            private set;
        }

        public object Body
        {
            get;
            private set;
        }

        public SystemMessageType Type
        { get; private set; }
    }

    public class AppMessage : IAppMessage
    {
        public AppMessage(string targetMoule,string sourceModule ,object body)
        {
            this.TargetModule = targetMoule;
            this.SourceModule = sourceModule;
            this.Extensions = new Dictionary<string, string>();
            this.Body = body;
        }
        public string TargetModule
        {
            get;
            private set;
        }

        public string SourceModule
        {
            get;
            private set;
        }

        public IDictionary<string, string> Extensions
        {
            get;
            private set;
        }

        public object Body
        {
            get;
            private set;
        }
    }

    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MessageManager
    {
        private ConcurrentQueue<IAppMessage> _appMsgQueue;
        private ConcurrentQueue<ISystemMessage> _sysMsgQueue;
        private ConcurrentQueue<ILogMessage> _logMsgQueue;
        private IModuleManager _moduleMgr;

        [ImportingConstructor]
        public MessageManager(IModuleManager moduleManager)
        {
            this._moduleMgr = moduleManager;
            //_appMsgQueue = new ConcurrentQueue<IAppMessage>();
            //_sysMsgQueue = new ConcurrentQueue<ISystemMessage>();
            //_logMsgQueue = new ConcurrentQueue<ILogMessage>();
        }

        public void SendSystemMessage(ISystemMessage message)
        {
            //_sysMsgQueue.Enqueue(message);
            this._moduleMgr.SystemModule.ProcessRequest(message);
        }

        public void SendAppMessage(IAppMessage message)
        {
            var targetModule = this._moduleMgr.GetAppModule(message.TargetModule);
            if (targetModule.IsNull())
                return;
            targetModule.ProcessRequest(message);

            //_appMsgQueue.Enqueue(message);
        }

        public void SendLogMessage(ILogMessage message)
        {
           // _logMsgQueue.Enqueue(message);
        }

    }

}
