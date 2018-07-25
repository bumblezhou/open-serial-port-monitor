using System;
using System.Configuration;
using System.IO;

namespace OpenSerialPortWindowsService
{
    public class Logger : IDisposable
    {
        /// <summary>
        /// 日志记录器的实例
        /// </summary>
        protected static Logger instance;
        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }

        /// <summary>
        /// 测井曲线数据，临时文件夹
        /// </summary>
        protected static string direction = System.Web.HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : System.Web.HttpContext.Current.Server.MapPath("~/bin/");

        /// <summary>
        /// 写流文件
        /// </summary>
        protected StreamWriter stream_writer = null;

        /// <summary>
        /// 构造一个日志记录器
        /// </summary>
        public Logger()
        {
            var openDebug = ConfigurationManager.AppSettings["open_debug"];
            if (!string.IsNullOrEmpty(openDebug) && openDebug != "false")
            {
                CreateDirectionAndFile();
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~Logger()
        {
            Dispose();
        }

        /// <summary>
        /// 创建日志文件
        /// </summary>
        protected void CreateDirectionAndFile()
        {
            //创建日志目录
            if (!System.IO.Directory.Exists(direction))
            {
                System.IO.Directory.CreateDirectory(direction);
            }

            //创建日志文件
            DateTime cur_time = System.DateTime.Now;
            string str_cur_time = cur_time.ToString("D");//取中文日期显示——年月日
            string file_path = direction + str_cur_time + ".txt";
            if (!System.IO.File.Exists(file_path))
            {
                stream_writer = new StreamWriter(file_path);
                stream_writer.AutoFlush = true;
            }
            else
            {
                try
                {
                    stream_writer = new StreamWriter(file_path, true);
                    stream_writer.AutoFlush = true;
                    stream_writer.WriteLine("\n\n===================================================================================================================");
                }
                catch (System.Exception ex)
                {
                    //打开文件流失败（被另外一个进程占用），创建不同文件名的日志
                    string file_path2 = direction + str_cur_time + cur_time.Ticks.ToString() + ".txt";
                    stream_writer = new StreamWriter(file_path2);
                    stream_writer.AutoFlush = true;
                }
            }
        }

        /// <summary>
        /// 输出说明文字,没有换行
        /// </summary>
        /// <param name="content"></param>
        public void PrintDescription(string description)
        {
            if (stream_writer == null)
            {
                return;
            }
            var openDebug = ConfigurationManager.AppSettings["open_debug"];
            if (!string.IsNullOrEmpty(openDebug) && openDebug != "false")
            {
                stream_writer.Write("{0,-50}", description);
            }
        }

        /// <summary>
        /// 输出经过时间,没有换行
        /// </summary>
        /// <param name="content"></param>
        public void PrintElapseTime(string elapsetime)
        {
            if (stream_writer == null)
            {
                return;
            }
            var openDebug = ConfigurationManager.AppSettings["open_debug"];
            if (!string.IsNullOrEmpty(openDebug) && openDebug != "false")
            {
                stream_writer.Write("{0,-30}", elapsetime);
            }
        }

        /// <summary>
        /// 输出当前时刻
        /// </summary>
        public void PrintTimeNow()
        {
            if (stream_writer == null)
            {
                return;
            }
            var openDebug = ConfigurationManager.AppSettings["open_debug"];
            if (!string.IsNullOrEmpty(openDebug) && openDebug != "false")
            {
                stream_writer.WriteLine("{0,-30}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));//-80:向左对齐,80个字符宽
            }
        }

        /// <summary>
        /// 输出一行内容
        /// </summary>
        /// <param name="content"></param>
        public void PrintLine(string content)
        {
            if (stream_writer == null)
            {
                return;
            }
            var openDebug = ConfigurationManager.AppSettings["open_debug"];
            if (!string.IsNullOrEmpty(openDebug) && openDebug != "false")
            {
                stream_writer.WriteLine("{0,-80}{1,-30}", content, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));//-80:向左对齐,80个字符宽
            }
        }

        /// <summary>
        /// 输出经过的时间
        /// </summary>
        /// <param name="description"></param>
        /// <param name="elapsetime"></param>
        public void PrintLine(string description, string elapsetime)
        {
            if (stream_writer == null)
            {
                return;
            }
            var openDebug = ConfigurationManager.AppSettings["open_debug"];
            if (!string.IsNullOrEmpty(openDebug) && openDebug != "false")
            {
                stream_writer.WriteLine("{0,-50}{1,-30}{2,-30}", description, elapsetime, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
        }

        #region IDisposable Members
        /// <summary>
        /// Destroys 
        /// </summary>
        public void Dispose()
        {
            //if (stream_writer != null)
            //{
            //    stream_writer.Close();
            //}
            instance = null;
        }
        #endregion
    }
}
