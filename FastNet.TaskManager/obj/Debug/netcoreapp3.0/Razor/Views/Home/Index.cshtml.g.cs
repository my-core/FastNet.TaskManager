#pragma checksum "E:\test\FastNet.TaskManager\FastNet.TaskManager\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4a564778b20058a7114f729dc86ca0d79c4bf7a8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\test\FastNet.TaskManager\FastNet.TaskManager\Views\_ViewImports.cshtml"
using FastNet.TaskManager;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\test\FastNet.TaskManager\FastNet.TaskManager\Views\_ViewImports.cshtml"
using FastNet.TaskManager.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4a564778b20058a7114f729dc86ca0d79c4bf7a8", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a41fff7946b35cb24cf09775ec5800eca9f72ed7", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/index.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 3 "E:\test\FastNet.TaskManager\FastNet.TaskManager\Views\Home\Index.cshtml"
  
    ViewBag.Title = "主页";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"layui-layout layui-layout-admin\">\r\n    <!-- 头部区域（可配合layui已有的水平导航） -->\r\n    <div class=\"layui-header\">\r\n        <div class=\"layui-logo\">任务调度平台</div>\r\n\r\n");
            WriteLiteral(@"    </div>

    <!-- tabs标签页 -->
    <div class=""layui-pagetabs"">
        <div class=""layui-icon layui-pagetabs-control layui-pagetabs-prev"">&#xe65a;</div>
        <div class=""layui-icon layui-pagetabs-control layui-pagetabs-next"">&#xe65b;</div>
        <div class=""layui-icon layui-pagetabs-control layui-pagetabs-down"">
            &#xe61a;
            <dl class=""layui-pagetabs-nav"">
                <dd>
                    <a href=""javascript:;"">关闭当前标签页</a>
                </dd>
                <dd>
                    <a href=""javascript:;"">关闭其它标签页</a>
                </dd>
                <dd>
                    <a href=""javascript:;"">关闭全部标签页</a>
                </dd>
            </dl>
        </div>
        <div class=""layui-tab"" lay-filter=""menu-tab"" lay-allowclose=""true"">
            <ul class=""layui-tab-title"" style=""left:0px;"">
                <li class=""layui-this"" lay-id=""0"">
                    <i class=""layui-icon"">&#xe68e;</i>我的桌面
                </li>
            </ul>");
            WriteLiteral(@"

        </div>
    </div>

    <!-- 左侧导航区域） -->
    <div class=""layui-side layui-bg-black"">
        <div class=""layui-side-scroll"">
            <ul class=""layui-nav layui-nav-tree"" lay-filter=""test"">
                <li class=""layui-nav-item"">
                    <!--<a class="""" href=""javascript:;"">任务管理</a>
        <dl class=""layui-nav-child"">
            <dd><a href=""javascript:;"" lay-id=""1"" lay-url=""/TaskJob"">作业列表</a></dd>
        </dl>-->
                    <a");
            BeginWriteAttribute("class", " class=\"", 3333, "\"", 3341, 0);
            EndWriteAttribute();
            WriteLiteral(@" href=""javascript:;"">作业列表</a>
                </li>
            </ul>
        </div>
    </div>

    <!-- 内容主体区域 -->
    <div id=""iframe-body"" class=""layui-body"">
        <!-- iframe页面内容 -->
        <div class=""layui-tab-item layui-show"" id=""tab_0"">
            <iframe class=""layui-iframe"" frameborder=""0"" src=""/TaskJob/Index""></iframe>
        </div>

    </div>

    <!-- 底部固定区域 -->
    <div class=""layui-footer"" style=""font-size:12px; text-align:center"">
        © fxt.com - 底部固定区域
    </div>
</div>
");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4a564778b20058a7114f729dc86ca0d79c4bf7a86147", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
