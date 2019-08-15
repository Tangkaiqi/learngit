using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlugIn;
using System.Windows.Forms;

namespace PlugIn
{
    /// <summary>
    /// 组件信息描述类
    /// 作者：Zuowenjun
    /// 2016-3-26
    /// </summary>
    public class Compoent : ICompoent
    {
        private List<Type> formTypeList = new List<Type>();

        public string CompoentName
        {
            get;
            private set;
        }

        public string CompoentVersion
        {
            get;
            private set;
        }

        public IEnumerable<Type> FormTypes
        {
            get
            {
                return formTypeList.AsEnumerable();
            }
        }

        public Compoent(string compoentName, string compoentVersion)
        {
            this.CompoentName = compoentName;
            this.CompoentVersion = compoentVersion;
        }

        public void AddFormTypes(params Type[] formTypes)
        {
            Type targetFormType = typeof(Form);
            foreach (Type formType in formTypes)
            {
                if (targetFormType.IsAssignableFrom(formType) && !formTypeList.Contains(formType))
                {
                    formTypeList.Add(formType);
                }
            }
        }
    }
}
