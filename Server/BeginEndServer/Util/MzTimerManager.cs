using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

/// <summary>
/// 计时器管理器, 定时触发设定好的MzTimer
/// </summary>

namespace BeginEndServer.Util
{
    public class MzTimerManager
    {
        //单例
        private static MzTimerManager instance;
        public static MzTimerManager _Instance
        {
            get
            {
                if (instance == null)
                    instance = new MzTimerManager();
                return instance;
            }
        }

        //计时器
        private Timer timer;
        //索引
        private ConcurrentInt taskIndex = new ConcurrentInt(-1);
        //线程安全的字典
        private ConcurrentDictionary<int, MzTimer> mzTimerDic = new ConcurrentDictionary<int, MzTimer>();
        //字典索引集
        private List<int> removeIndex = new List<int>();

        private MzTimerManager()
        {
            this.timer = new Timer(100);     //设定计时器的间隔.单位:ms
            timer.Elapsed += Timer_Elapsed; //达到设定的间隔时触发Timer_Elapsed事件
            //timer.Enabled = true;
        }

        /// <summary>
        /// 根据timer间隔调用
        /// </summary>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //TODO 可以优化
            lock (removeIndex)
            {
                MzTimer temp;
                //清空字典
                foreach (int index in removeIndex)
                {
                    mzTimerDic.TryRemove(index, out temp);
                    if (temp.isLoop)
                    {
                        AddTask(temp.id, temp.loopTime, true, temp.action);
                    }
                }

            }
            removeIndex.Clear();

            //执行
            foreach (MzTimer mzTimer in mzTimerDic.Values)
            {
                //delayTime = set + Ticks(Ticks是无限增大的时间间隔数(100ns),可用于精确表示时间)
                if (mzTimer.delayTime <= Sys.GetUtcTimeStamp())
                {
                    mzTimer.Trigger();
                    removeIndex.Add(mzTimer.id);
                }
            }
        }

        public void Start()
        {
            timer.Start();
        }

        /// <summary>
        /// 到达指定日期后触发(非循环)
        /// </summary>
        public void AddTask(DateTime date, Action ac)
        {
            //if (date < DateTime.Now)
            //    return;
            long delay = date.Ticks - DateTime.Now.Ticks;
            if (delay < 0)
                return;

            AddTask(delay, false, ac);
        }

        /// <summary>
        /// 经过延迟后触发或循环触发
        /// </summary>
        public void AddTask(long delayTime, bool isLoop, Action ac)
        {
            MzTimer mzTimer = new MzTimer(taskIndex.Add(), delayTime , isLoop, ac);
            //mzTimer.delayTime *= 10000000;
            mzTimer.delayTime += Sys.GetUtcTimeStamp();
            mzTimerDic.TryAdd(mzTimer.id, mzTimer);
        }

        public void AddTask(int id, long delayTime, bool isLoop, Action ac)
        {
            MzTimer mzTimer = new MzTimer(id, delayTime, isLoop, ac);
            //mzTimer.delayTime *= 10000000;
            mzTimer.delayTime += Sys.GetUtcTimeStamp();
            mzTimerDic.TryAdd(mzTimer.id, mzTimer);
        }
    }
}


