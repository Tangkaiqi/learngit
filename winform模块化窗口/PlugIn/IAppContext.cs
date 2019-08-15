using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlugIn
{
    /// <summary>
    /// 应用程序上下文对象接口
    /// 作用：用于收集应用程序必备的一些公共信息并共享给整个应用程序所有模块使用（含动态加载进来的组件）
    /// 作者：Zuowenjun
    /// 2016-3-26
    /// </summary>
    public interface IAppContext
    {
        /// <summary>
        /// 应用程序名称
        /// </summary>
        string AppName { get;}

        /// <summary>
        /// 应用程序版本
        /// </summary>
        string AppVersion { get; }

        /// <summary>
        /// 用户登录信息，这里类型是STRING，真实项目中为一个实体类
        /// </summary>
        string SessionUserInfo { get; }

        /// <summary>
        /// 用户登录权限信息，这里类型是STRING，真实项目中为一个实体类
        /// </summary>
        string PermissionInfo { get; }

        /// <summary>
        /// 应用程序全局缓存，整个应用程序（含动态加载的组件）均可进行读写访问
        /// </summary>
        Dictionary<string, object> AppCache { get; }

        /// <summary>
        /// 应用程序主界面窗体，各组件中可以订阅或获取主界面的相关信息
        /// </summary>
        Form AppFormContainer { get; }

        /// <summary>
        /// 动态创建在注册列表中的插件窗体实例
        /// </summary>
        /// <param name="formType"></param>
        /// <returns></returns>
        Form CreatePlugInForm(Type formType);

        /// <summary>
        /// 动态创建在注册列表中的插件窗体实例
        /// </summary>
        /// <param name="formTypeName"></param>
        /// <returns></returns>
        Form CreatePlugInForm(string formTypeName);

    }
}
