/*
index页面脚本
*/
layui.use('element', function () {
    var element = layui.element;
    var $ = layui.jquery;

    //===============左侧菜单事件================
    $('.layui-nav-tree .layui-nav-child a').on('click', function () {
        var id = $(this).attr('lay-id');
        var url = $(this).attr('lay-url');
        var title = $(this).text();
        //tab已经存在直接切换tab
        var isExist = $('li[lay-id="' + id + '"]').length>0;
        if (isExist) {
            element.tabChange('menu-tab', id);
            return;
        }
        //不存在，新增一个Tab项
        element.tabAdd('menu-tab', {
            title: title,
            id: id
        });
        var content = '<div class="layui-tab-item layui-show" id="tab_' + id + '"><iframe class="layui-iframe" frameborder="0" src="' + url + '"></iframe></div>';
        $('#iframe-body').append(content);
        element.tabChange('menu-tab', id);
    });
    
    //==============tabs事件===================
    $('.layui-pagetabs-prev').bind('click', function () {
        tabMove('left')
    });
    $('.layui-pagetabs-next').bind('click', function () {
        tabMove('right')
    });
    //显示隐藏tabs操作菜单
    $('.layui-pagetabs-down').bind('mouseover', function () {
        $('.layui-pagetabs-nav').show();
    }).bind('mouseout', function () {
        $('.layui-pagetabs-nav').hide();
    });
    var tabMove = function (moveFlag) {
        var tabWidth = $('.layui-tab-title').width(); //tab显示区域宽度
        var liWidth = 0; //tab标签页总宽度
        $('.layui-tab-title li').each(function (i, o) {
            liWidth = liWidth + $(o).outerWidth();
        });
        if (liWidth <= tabWidth)
            return;
        var left = Math.abs($('.layui-tab-title').position().left - 40);
        var right = liWidth - tabWidth - left;
        right = (right <= 0 ? 0 : right);

        var setLeft = 0;
        if (moveFlag == 'left') {
            if (left == 0)
                return;
            if (left > tabWidth) {
                setLeft = tabWidth - left;
            } else if (left < tabWidth) {
                setLeft = 0;
            }
        } else if (moveFlag == 'right') {
            if (right == 0)
                return;
            if (right > tabWidth) {
                setLeft = 0 - left - tabWidth;
            } else if (right < tabWidth) {
                setLeft = 0 - liWidth + tabWidth;
            }
        }
        $('.layui-tab-title').animate({
            left: setLeft + 'px'
        });
    }
    //监听选项卡删除
    element.on('tabDelete(menu-tab)', function (data) {
        var id = $(this).parent().attr('lay-id');
        $('#tab_' + id).remove();
    });
    //监听选项卡切换
    element.on('tab(menu-tab)', function (data) {
        var id = $(this).attr('lay-id');
        $('.layui-show').removeClass('layui-show');
        $('#tab_' + id).addClass('layui-show');
    });
});