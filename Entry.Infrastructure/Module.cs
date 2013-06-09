using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entry.Infrastructure
{
    public interface IModule
    {
        /// <summary>
        /// start up
        /// </summary>
        void Startup();

        /// <summary>
        /// the name of module ,it should be unique
        /// </summary>
        string Name { get; }

    }

    /// <summary>
    /// the interface defines a application module
    /// </summary>
    public interface IAppModule:IModule
    {
        /// <summary>
        /// the display name of module
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// initialize the module
        /// </summary>
        void Initialize();

        /// <summary>
        /// the icon uri 
        /// </summary>
        object IconContent { get; }

        /// <summary>
        /// process the request and send back the response 
        /// </summary>
        /// <returns></returns>
        Object ProcessRequest(IAppMessage request);
    }

    /// <summary>
    /// the base abstract module class,it implements the IModule as default
    /// </summary>
    public abstract class AppModule : IAppModule
    {
        ///// <summary>
        ///// constructor
        ///// </summary>
        ///// <param name="displayName">module's display name</param>
        ///// <param name="name">module's name</param>
        ///// <param name="iconUri">module's icon uri</param>
        //public Module(string displayName, string name, object icon)
        //{
        //    this.DisplayName = displayName;
        //    this.Name = name;
        //    this.IconContent = icon;
        //}

        #region IAppModule Members

        public string DisplayName
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public object IconContent
        {
            get;
            private set;
        }

        public virtual void Initialize()
        {
        }


        public virtual void Startup()
        {

        }

        public virtual object ProcessRequest(IAppMessage request)
        {
            return null;
        }

        #endregion
    }

    /// <summary>
    /// Module manager interface
    /// </summary>
    public interface IModuleManager
    {
        IEnumerable<IAppModule> GetAppModules();
        ISystemModule SystemModule { get; }
        IAppModule GetAppModule(string moduleName);
        bool HasModule(string moduleName);
    }


    /// <summary>
    /// the interface defines the system module
    /// </summary>
    public interface ISystemModule:IModule
    {
        /// <summary>
        /// initialize the system module with region
        /// </summary>
        void Initialize(IShellRegion region);

        /// <summary>
        /// process the request and send back the response 
        /// </summary>
        /// <returns></returns>
        Object ProcessRequest(ISystemMessage request);
    }
}
