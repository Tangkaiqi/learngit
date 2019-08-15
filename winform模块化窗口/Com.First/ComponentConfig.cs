using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlugIn;

namespace Com.First
{
    /// <summary>
    /// 组件信息注册类（每一个插件模块必需实现一个ICompoentConfig）
    /// 作者：Zuowenjun
    /// 2016-3-26
    /// </summary>
    public class CompoentConfig : ICompoentConfig
    {
        public static IAppContext AppContext;

        public void CompoentRegister(IAppContext context,out ICompoent compoent)
        {
            AppContext = context;
            var compoentInfo = new Compoent("Com.First", "V16.3.26.1.1");
            compoentInfo.AddFormTypes(typeof(Form1), typeof(Form2));//将认为需要用到的窗体类型添加到预注册列表中

            compoent = compoentInfo;//回传Compoent的实例
        }
    }
}
