using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlugIn
{
    /// <summary>
    /// 组件信息注册接口
    /// 作用：应用程序将会第一时间从程序集找到实现了该接口的类并调用其CompoentRegister方法，从而被动的收集该组件的相关信息
    /// 作者：Zuowenjun
    /// 2016-3-26
    /// </summary>
    public interface ICompoentConfig
    {
        void CompoentRegister(IAppContext context, out ICompoent compoent);
    }
}
