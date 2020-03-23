/*
login页面脚本
*/
var loginCallback = null;
layui.use(['form', 'layer'], function () {
    var form = layui.form,
        layer = layui.layer;

    //监听提交
    form.on('submit(login)', function (data) {
        return true;
    });
    //登录回调
    loginCallback = function (data) {
        if (data.IsOk) {
            location.href = '/Home';
        }
        else {
            layer.msg(data.Msg, { icon: 5 });
        }
    }
   
});
