using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlugIn
{
    /// <summary>
    /// 组件信息描述接口
    /// 作用：描述该组件（或称为模块，即当前程序集）的一些主要信息，以便应用程序可以动态获取到
    /// 作者：Zuowenjun
    /// 2016-3-26
    /// </summary>
    public interface ICompoent
    {
        /// <summary>
        /// 组件名称
        /// </summary>
        string CompoentName { get;}

        /// <summary>
        /// 组件版本，可实现按组件更新
        /// </summary>
        string CompoentVersion { get; }

        /// <summary>
        /// 向应用程序预注册的窗体类型列表
        /// </summary>
        IEnumerable<Type> FormTypes { get; }
    }
}
