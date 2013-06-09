using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entry.Infrastructure
{
    #region Resource interfaces

    public interface IResourceManager
    {
        void RegisterResource(string resourcePath, string projectName = null);
    }

    #endregion

    #region Message Interfaces

    public interface IMessage
    {
        string SourceModule { get; }
        IDictionary<string, string> Extensions { get; }
    }

    public interface IBody
    {
        Object Body { get; }
    }

    public enum SystemMessageType
    {
        RenderContent
    }

    public interface ISystemMessage : IMessage, IBody,ITitle
    {
        SystemMessageType Type { get; }
    }

    public interface IAppMessage : IMessage, IBody
    {
        string TargetModule { get; }
    }

    public interface ILogMessage:ISystemMessage
    {

    }

    #endregion

    #region Region Interfaces

    public interface IShellRegion
    {
         void SetRegionContent(string regionName,object content);
    }

    #endregion

    public interface ITitle
    {
        string Title { get; }
    }
}
