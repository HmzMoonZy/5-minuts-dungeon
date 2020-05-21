using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeginEndServer.Util
{
    /// <summary>
    /// 定时器(计时任务)
    /// </summary>
    public class MzTimer
    {
        public int id { get; set; }                //任务id
        public long delayTime { get; set; }         //延迟时间
        public long loopTime { get; set; }
        public bool isLoop { get; private set; }        //是否循环执行
        public Action action { get; private set; }      //触发的回调

        /// <summary>
        /// 设置一个定时任务
        /// </summary>
        /// <param name="id">任务的id</param>
        /// <param name="delayTime">任务触发的延迟</param>
        /// <param name="action">触发的任务</param>
        public MzTimer(int id, long delayTime, bool isLoop, Action action)
        {
            this.id = id;
            this.delayTime = delayTime;
            this.action = action;
            this.isLoop = isLoop;
            if (isLoop)
            {
                this.loopTime = delayTime;
            }
        }

        public void Trigger()
        {
            action();
        }

    }
}
