using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using FastNet.TaskManager.DataContract;
using FastNet.TaskManager.Models;
using FastNet.TaskManager.QuartzNet;
using FastNet.TaskManager.Repository;

namespace FastNet.TaskManager.Controllers
{
    //[Authorize]
    public class TaskJobController : Controller
    {
        private readonly ILogger<TaskJobController> _logger;
        private QuartzRepository _yunYingRepository;
        private QuartzManager _quartzManager;
        public TaskJobController(ILogger<TaskJobController> logger, QuartzRepository yunYingRepository, QuartzManager quartzManager)
        {
            _logger = logger;
            _yunYingRepository = yunYingRepository;
            _quartzManager = quartzManager;
        }

        public async Task<IActionResult> Index()
        {
            var response = new List<TaskJobDto>();
            var list = _yunYingRepository.GetList<TaskJobInfo>();
            foreach (var item in list)
            {
                TaskJobDto dto = new TaskJobDto
                {
                    JobID = item.JobID,
                    JobName = item.JobName,
                    JobDesc = item.JobDesc,
                    JobClassName = item.JobClassName,
                    CronExpression = item.CronExpression,
                    ApiUrl = item.ApiUrl,
                    LastExecuteTime = item.LastExecuteTime
                };
                int status = await _quartzManager.GetState(item.JobName);
                dto.Status = status;
                if (status == (int)TriggerState.Normal)
                {
                    dto.StatusName = "运行中";
                }
                else if (status == (int)TriggerState.Paused)
                {
                    dto.StatusName = "暂停";
                }
                else if (status == (int)TriggerState.Complete)
                {
                    dto.StatusName = "已完成";
                }
                else if (status == (int)TriggerState.Error)
                {
                    dto.StatusName = "错误";
                }
                else if (status == (int)TriggerState.Blocked)
                {
                    dto.StatusName = "阻塞";
                }
                else if (status == (int)TriggerState.None)
                {
                    dto.StatusName = "";
                }
                response.Add(dto);
            }
            return View(response);
        }
        /// <summary>
        /// 任务添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(int id)
        {

            TaskJobInfo info = _yunYingRepository.GetModel<TaskJobInfo>(new { JobID = id });
            return View(info ?? new TaskJobInfo());
        }
        /// <summary>
        /// 任务添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Add(TaskJobInfo info)
        {
            AjaxResult ajaxResult = new AjaxResult();
            if (info.JobID == 0)
            {
                info.LastExecuteTime = DateTime.Now;
                var jobId = _yunYingRepository.Insert(info);
                if (jobId > 0)
                {
                    info.JobID = (int)jobId;
                    await _quartzManager.CreateJob(info);
                    ajaxResult.IsOk = true;
                    ajaxResult.Msg = "添加成功";
                }
                else
                {
                    ajaxResult.IsOk = false;
                    ajaxResult.Msg = "添加失败";
                }
            }
            else
            {
                var updateInfo = _yunYingRepository.GetModel<TaskJobInfo>(new { JobId = info.JobID });
                string oldJobName = updateInfo.JobName;
                updateInfo.JobName = info.JobName;
                updateInfo.JobDesc = info.JobDesc;
                updateInfo.JobClassName = info.JobClassName;
                updateInfo.CronExpression = info.CronExpression;
                updateInfo.ApiUrl = Convert.ToString(info.ApiUrl);
                if (_yunYingRepository.Update(updateInfo) > 0)
                {
                    await _quartzManager.DeleteJob(oldJobName);
                    await _quartzManager.CreateJob(updateInfo);
                    ajaxResult.IsOk = true;
                    ajaxResult.Msg = "更新成功";
                }
                else
                {
                    ajaxResult.IsOk = false;
                    ajaxResult.Msg = "更新失败";
                }
            }
            return Json(ajaxResult);
        }

        /// <summary>
        /// 任务添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            AjaxResult ajaxResult = new AjaxResult();
            var model = _yunYingRepository.GetModel<TaskJobInfo>(new { JobID = id });
            if (model != null)
            {
                var flag = _yunYingRepository.Delete<TaskJobInfo>(new { JobID = id }) > 0;
                if (flag)
                {
                    await _quartzManager.DeleteJob(model.JobName);
                    ajaxResult.IsOk = true;
                    ajaxResult.Msg = "删除成功";
                }
                else
                {
                    ajaxResult.IsOk = false;
                    ajaxResult.Msg = "删除失败";
                }
            }
            else
            {
                ajaxResult.IsOk = false;
                ajaxResult.Msg = "删除失败";

            }
            return Json(ajaxResult);
        }


        /// <summary>
        /// 启用/暂停
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SwitchStatus(SwitchStatusRequest switchRequest)
        {
            AjaxResult ajaxResult = new AjaxResult();
            TaskJobInfo info = _yunYingRepository.GetModel<TaskJobInfo>(new { JobID = switchRequest.JobId });
            if (info == null)
            {
                ajaxResult.IsOk = false;
                ajaxResult.Msg = $"作业不存在[jobid={switchRequest.JobId}]";
                return Json(ajaxResult);
            }
            await _quartzManager.CreateJob(info);
            if (switchRequest.Status == 1)
            {
                await _quartzManager.ResumeJob(info.JobName);
            }
            else if (switchRequest.Status == 2)
            {
                await _quartzManager.PauseJob(info.JobName);
            }
            else if (switchRequest.Status == 3)
            {
                await _quartzManager.DeleteJob(info.JobName);
                await _quartzManager.CreateJob(info);
            }
            ajaxResult.IsOk = true;
            ajaxResult.Msg = "操作成功";
            return Json(ajaxResult);
        }
    }
}
