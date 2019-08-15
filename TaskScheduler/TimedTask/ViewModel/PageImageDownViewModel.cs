using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

//----------------------------------------------------------------*/
// 版权所有：
// 文 件 名：PageIamgeDownViewModel.cs
// 功能描述：
// 创建标识：m.sh.lin0328@163.com 2014/8/30 14:31:07
// 修改描述：
//----------------------------------------------------------------*/
namespace TimedTask.ViewModel
{
    public class PageImageDownViewModel : ViewModelBase
    {
        /// <summary>
        /// 初始化 PageIamgeDownViewModel
        /// </summary>
        public PageImageDownViewModel()
        {
            if (!IsInDesignMode)
            {

            }
        }
        #region 属性

        private string _StatusInfo;
        /// <summary> 状态栏信息 </summary>
        public string StatusInfo
        {
            get { return _StatusInfo; }
            set
            {
                this._StatusInfo = value;
                base.RaisePropertyChanged("StatusInfo");
            }
        }
        #endregion

        #region 命令

        public RelayCommand<string> _IamgeDownCommand;
        /// <summary> 下载命令 </summary>
        public RelayCommand<string> ImageDownCommand
        {
            get
            {
                if (_IamgeDownCommand == null)
                {
                    _IamgeDownCommand = new RelayCommand<string>((url) =>
                    {
                        ImageDown(url);
                    });
                }
                return _IamgeDownCommand;
            }
        }
        #endregion

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="url"></param>
        private void ImageDown(string url)
        {
            if (String.IsNullOrEmpty(url))
                this.StatusInfo = "网址不能为空！";

            string html = String.Empty;//
            System.Collections.Generic.List<string> list = null;
            string savePath = Model.PM.StartPath + "\\Down\\Img\\";

            #region 进度

            Control.Loading load = new Control.Loading(() =>
            {
                try
                {
                    html = Utility.HtmlHelper.Instance.GetHtml(url);
                    html = Utility.HtmlHelper.Instance.GetBody(html);
                    Utility.HtmlHelper.Instance.FixUrl(url, html);
                    list = Utility.HtmlHelper.Instance.GetImgLinks(html);
                }
                catch (Exception ex)
                {
                    Utility.Log.SaveLog("ViewModel PageIamgeDownViewModel ImageDown url[" + url + "]", ex.ToString());
                    return;
                }
            });
            load.Msg = "正在加载。。。";
            load.Start();
            load.ShowDialog();
            #endregion

            Helper.Instance.DeleteFolder(savePath);
            Helper.Instance.CreateFolder(savePath);
            int yesNum = 0;
            int noNum = 0;
            if (list != null && list.Count > 0)
            {
                Utility.DownFile downFile = new Utility.DownFile();
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    string suffix = String.Empty;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!list[i].Contains("."))
                            continue;

                        suffix = list[i].Substring(list[i].LastIndexOf("."));
                        if (downFile.DownloadFileByWebClient(list[i], savePath + Guid.NewGuid().ToString() + suffix))
                        {
                            yesNum++;
                            this.StatusInfo = "共有图片链接" + list.Count + "个，下载成功" + yesNum + ",失败" + noNum + "";
                            continue;
                        }
                        noNum++;
                    }
                    System.Diagnostics.Process.Start("explorer.exe", savePath);
                });
                this.StatusInfo = "共有图片链接" + list.Count + "个，下载成功" + yesNum + ",失败" + noNum + "";
            }
            else
            {
                this.StatusInfo = "没有发现图片链接的存在！";
            }
        }
    }
}
