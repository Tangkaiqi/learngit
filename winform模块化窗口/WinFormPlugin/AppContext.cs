using PlugIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormPlugin
{
    /// <summary>
    /// 应用程序上下文对象类
    /// 作者：Zuowenjun
    /// 2016-3-26
    /// </summary>
    public class AppContext : IAppContext
    {

        internal static AppContext Current;

        internal Dictionary<string, Type> AppFormTypes
        {
            get;
            set;
        }

        public string AppName
        {
            get;
            private set;
        }

        public string AppVersion
        {
            get;
            private set;
        }

        public string SessionUserInfo
        {
            get;
            private set;
        }

        public string PermissionInfo
        {
            get;
            private set;
        }

        public Dictionary<string, object> AppCache
        {
            get;
            private set;
        }

        public System.Windows.Forms.Form AppFormContainer
        {
            get;
            private set;
        }


        public AppContext(string appName, string appVersion, string sessionUserInfo, string permissionInfo, Form appFormContainer)
        {
            this.AppName = appName;
            this.AppVersion = appVersion;
            this.SessionUserInfo = sessionUserInfo;
            this.PermissionInfo = permissionInfo;
            this.AppCache = new Dictionary<string, object>();
            this.AppFormContainer = appFormContainer;
        }

        public System.Windows.Forms.Form CreatePlugInForm(Type formType)
        {
            if (this.AppFormTypes.ContainsValue(formType))
            {
                return Activator.CreateInstance(formType) as Form;
            }
            else
            {
                throw new ArgumentOutOfRangeException(string.Format("该窗体类型{0}不在任何一个模块组件窗体类型注册列表中！", formType.FullName), "formType");
            }
        }

        public System.Windows.Forms.Form CreatePlugInForm(string formTypeName)
        {
            Type type = Type.GetType(formTypeName);
            return CreatePlugInForm(type);
        }
    }
}
