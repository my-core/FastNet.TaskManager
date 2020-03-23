/*公共JS类*/

layui.use(['form', 'layer'], function () {
    var form = layui.form,
        layer = layui.layer;
});
//显示当前时间
function ShowTime(obj) {
    var now_time = new Date();
    var years = now_time.getFullYear();
    var months = now_time.getMonth();
    var dates = now_time.getDate();
    var days = now_time.getDay();
    var today = years + "年" + (months + 1) + "月" + dates + "日";
    var weeks;
    if (days == 0)
        weeks = "星期日";
    if (days == 1)
        weeks = "星期一";
    if (days == 2)
        weeks = "星期二";
    if (days == 3)
        weeks = "星期三";
    if (days == 4)
        weeks = "星期四";
    if (days == 5)
        weeks = "星期五";
    if (days == 6)
        weeks = "星期六";
    var hours = now_time.getHours();
    var minutes = now_time.getMinutes();
    var seconds = now_time.getSeconds();
    var timer = hours;
    timer += ((minutes < 10) ? ":0" : ":") + minutes;
    timer += ((seconds < 10) ? ":0" : ":") + seconds;
    var doc = document.getElementById(obj);
    doc.innerHTML = today + " " + timer + " " + weeks;
    setTimeout("ShowTime('" + obj + "')", 1000);
}

//执行ajax
function DoAjax(url, data, method,callback) {
    ShowLoading();
    $.ajax({
        url: dealAjaxUrl(url),
        data: data,
        dataType: 'json',
        type: method,
        success: function (d) {
            if (callback != null)
                callback(d);
            CloseLoading();
        }
    });
}
//对每一个请求的URL进行处理，确保每一次请求都是新的,
//否则Url相同视为相同请求，浏览器则不执行，而是返回上一次请求结果（避免缓存问题)
function dealAjaxUrl(url) {
    var guid = new Date().getTime().toString(36);
    var ajaxurl;
    if (url.lastIndexOf("?") > -1)//url带参数
        ajaxurl = url + "&ajaxGuid=" + guid;
    else //url不带参数
        ajaxurl = url + "?ajaxGuid=" + guid;
   // ajaxurl += "&url=" + location.href; //加上URL参数，用于出现错误时返回上一页
    return ajaxurl;
}
//将json参数转换为Url参数
var JsonToParam = function (json) {
    if (json != null)
        var param = [];
    for (var key in json) {
        param.push(key + '=' + json[key]);
    }

    return param.join('&');
}

//是否存在指定函数 
function isExitsFunction(funcName) {
    try {
        if (typeof (eval(funcName)) == "function") {
            return true;
        }
    } catch (e) { }
    return false;
}

/*layer相关方法
=================================*/
var loadi;
//需关闭加载层时，执行layer.close(loadi)即可
function ShowLoading() {
    loadi = layer.load(0, { time: 15 * 1000 });//设定最长等待15秒 
}
//关闭加载层
function CloseLoading() {
    layer.close(loadi);
}
//加载iframe窗口
var win;
function ShowWindow(title,url,width,height) {
    win=  layer.open({
        type: 2,
        title: title,
        shadeClose: true,
        shade: 0.3,
        maxmin: true, //开启最大化最小化按钮
        area: [width + 'px', height + 'px'],
        content: url
    });
}
function ShowMaxWindow(title, url) {
    win = layer.open({
        type: 2,
        title: title,
        shadeClose: true,
        shade: 0.3,
        maxmin: false, //开启最大化最小化按钮
        content: url
        //success: function (layero, index) {
        //    //在回调方法中的第2个参数“index”表示的是当前弹窗的索引。
        //    //通过layer.full方法将窗口放大。
        //    layer.full(index);
        //}
    });
    layer.full(win);
}

//关闭iframe窗口
function CloseWindow(refresh) {
    layer.close(win);
    if (refresh) {
        location.reload();
    }
}


//添加、编辑操作后的回调方法(result对应Yandex.Web.Models.AjaxResult)
function Callback(result) {
    if (result.IsOk) {
        layer.alert(result.Msg, { icon: 1, shadeClose: false, title: '操作提示' }, function () {
            parent.CloseWindow(true);
        });
    }
    else {
        layer.alert(result.Msg, { icon: 2, shadeClose: true, title: '操作提示' });
    }
}
//删除操作后的回调(result对应Yandex.Web.Models.AjaxResult)
function DelCallback(result) {
    if (result.IsOk) {
        layer.alert(result.Msg, { icon: 1, shadeClose: false, title: '操作提示' }, function () {
            location.reload();
        });
    }
    else {
        layer.alert(result.Msg, { icon: 2, shadeClose: true, title: '操作提示' });
    }
}
//公用的删除数据方法
function Delete(url) {
    layer.confirm('确认是否要删除？', {
        btn: ['确认', '取消'] //按钮
    }, function () {
        //确认
        $.post(url, null, DelCallback);
    }, function () {
        //取消
    });
}

