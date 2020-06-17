using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EasyAssetManagerCore.Model.CommonModel
{
   public static class HtmlHelperExtensions
    {
        public static HtmlString GetTableHeader2(this IHtmlHelper helper, string className, String title)
        {
            return new HtmlString(@"<h2 class=""" + className + @""">" + title + "</h2>");

        }

        public static HtmlString PageHeader(this IHtmlHelper helper, string header, String description = "")
        {
            return new HtmlString(@"<div class=""page-head"">
                                        <div class=""page-title"">
                                            <h1>" + header + @"<small>" + description + @"</small></h1>
                                        </div>
                                    </div>");
        }
        public static HtmlString PorlateTitle(this IHtmlHelper helper, string title)
        {
            return new HtmlString(@"<div class=""portlet-title"">
                                        <div class=""caption uppercase"">" + title + @"
                                        </div>
                                    </div>");
        }

        public static HtmlString ShowEntries(this IHtmlHelper helper, string controlId, int setdValue, dynamic dynamicProperty = null)
        {
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "30", Value = "30" });
            items.Add(new SelectListItem { Text = "40", Value = "40" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });

            items.Where(o => o.Value == setdValue.ToString()).FirstOrDefault().Selected = true;


            string input = string.Empty;
            var htmlContent = helper.DropDownList(controlId, items, dynamicProperty as object);

            using (var writer = new StringWriter())
            {
                htmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                input = writer.ToString();
            }


            return new HtmlString(@" <div class=""form-inline"">
            <div class=""form-group"">
                <label>Shows</label>
                " + input + @"
                <label> Entries</label>
            </div>
        </div>");
        }


        public static HtmlString Paging(this IHtmlHelper helper, string controlId, int totalRows, int filteredRows, int displayRows, int currentPage, int currentlyDisplayItemsCount, dynamic dynamicProperty = null)
        {
            var finalHtml = new HtmlString("");
            if (filteredRows > 0)
            {
                var count = filteredRows % displayRows == 0 ? filteredRows / displayRows : (filteredRows / displayRows) + 1;

                var items = new List<SelectListItem>();

                for (int i = 1; i <= count; i++)
                {
                    items.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }

                items.Where(o => o.Value == currentPage.ToString()).FirstOrDefault().Selected = true;


                var id = string.Format("CP{0:yyyyMMddhhmm}", DateTime.Now);
                string input = string.Empty;
                var htmlContent = helper.DropDownList(id, items, dynamicProperty as object);

                using (var writer = new StringWriter())
                {
                    htmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                    input = writer.ToString();
                }

                var disablePrevious = currentPage == 1 ? "disabled" : "";
                var disableNext = currentPage == count ? "disabled" : "";


                var from = ((currentPage - 1) * 10) + 1;
                var to = (from + currentlyDisplayItemsCount) - 1;

                finalHtml = new HtmlString(@"
                <div class=""row"">
                    <div class=""col-md-6"">
                        <div class=""form-inline"">
                            <div class=""form-group"">
                                <label class=""bold"">Total Data: " + totalRows + @"</label>
                                <label style=""margin-left:20px;"">Total Filtered Data: " + filteredRows + @"</label>
                                <label style=""margin-left:20px;"">Currenly Showing " + from + @" to " + to + @" of " + filteredRows + @" entries</label>
                            </div>
                        </div>
                    </div><div class=""col-md-6 text-right""><div class=""form-inline"">
                            <div class=""form-group"">
                                <button id=""btnPrevisous"" " + disablePrevious + @" class=""btn btn-default"" type=""button"">Prvious</button>
                                <input type=""hidden"" id=""" + controlId + @""" name=""" + controlId + @""" value=""" + currentPage + @""" />
                                " + input + @"
                                <button id=""btnNext"" " + disableNext + @" class=""btn  btn-default"" type=""button"">Next</button>
                            </div>
                        </div>
                    </div>
                </div>");
            }

            return finalHtml;
        }

        public static HtmlString InputFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression, dynamic dynamicProperty = null)
        {
            var y = string.Empty;
            string input = GetIHtmlContent(htmlHelper, expression);
            input = input.Replace("/>", "");
            input += GetDynamicPropertyString(dynamicProperty);
            input += " />";
            return new HtmlString(input);
        }

        public static HtmlString Input_FormGroup(this IHtmlHelper htmlHelper, string idName, object value, string label, int colLength, dynamic dynamicProperty = null)
        {
            string input = string.Empty;
            var htmlContent = htmlHelper.TextBox(idName, value, dynamicProperty as object);

            using (var writer = new StringWriter())
            {
                htmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                input = writer.ToString();
            }

            var finalHtml = @"<div class=""col-md-" + colLength + @""">
                        <div class=""form-group"">
                            <label class=""control-label"">" + label + @"</label>
                            " + input + @"
                        </div>
                    </div>";
            return new HtmlString(finalHtml);
        }

        public static HtmlString InputFor_FromGroup<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression, string label, int colLength, dynamic dynamicProperty = null)
        {
            var y = string.Empty;
            string input = GetIHtmlContent(htmlHelper, expression);
            input = input.Replace("/>", "");
            input += GetDynamicPropertyString(dynamicProperty);
            input += " />";

            var finalHtml = @"<div class=""col-md-" + colLength + @""">
                        <div class=""form-group"">
                            <label class=""control-label"">" + label + @"</label>
                            " + input + @"
                        </div>
                    </div>";

            return new HtmlString(finalHtml);
        }

        public static HtmlString DateTime_FromGroup(this IHtmlHelper htmlHelper, string idName, object value, string label, int colLength, bool showTime = false, dynamic dynamicProperty = null)
        {
            string val = string.Empty;
            var v = value.GetType();
            if (v.Name == "DateTime")
            {
                var d = Convert.ToDateTime(value);
                if (d != default(DateTime))
                {
                    if (showTime)
                    {
                        val = string.Format("{0:yyyy-MM-dd hh:mm}", d);
                    }
                    else
                    {
                        val = string.Format("{0:yyyy-MM-dd}", d);
                    }
                }
            }
            else
            {
                val = value.ToString();
            }

            string input = string.Empty;
            var htmlContent = htmlHelper.TextBox(idName, val, dynamicProperty as object);

            using (var writer = new StringWriter())
            {
                htmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                input = writer.ToString();
            }

            var finalHtml = @"<div class=""col-md-" + colLength + @""">
                                <div class=""form-group"">
                                    <label class=""control-label"">" + label + @"</ label >       
                                    <div class=""input-group date form_datetime"">
                                        " + input + @"
                                        <span class=""input-group-btn"">
                                            <button class=""btn default date-set"" type=""button"">
                                                <i class=""fa fa-calendar""></i>
                                            </button>
                                        </span>
                                    </div>
                                </div>
                            </div>";

            return new HtmlString(finalHtml);
        }


        public static HtmlString Select_FromGroup(this IHtmlHelper htmlHelper, string idName, string label, int colLength, IEnumerable<SelectListItem> items, dynamic dynamicProperty = null)
        {

            string input = string.Empty;
            var htmlContent = htmlHelper.DropDownList(idName, items, dynamicProperty as object);

            using (var writer = new StringWriter())
            {
                htmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                input = writer.ToString();
            }

            var finalHtml = @"<div class=""col-md-" + colLength + @""">
                        <div class=""form-group"">
                            <label class=""control-label"">" + label + @"</label>
                            " + input + @"
                        </div>
                    </div>";

            return new HtmlString(finalHtml);
        }


        public static HtmlString Status_Table(this IHtmlHelper helper, int statusId)
        {
            var status = (Status)statusId;
            var className = string.Empty;
            if (status == Status.Active)
            {
                className = "label-success";
            }
            else if (status == Status.Deactive)
            {
                className = "label-danger";
            }
            else if (status == Status.Suspended)
            {
                className = "label-warning";
            }

            return new HtmlString(@"<span class=""label label-sm " + className + @"""> " + status.ToString() + " </span>");
        }

        public static string PorletColor(this IHtmlHelper helper)
        {
            return "green-seagreen";
        }

        public static HtmlString Loader(this IHtmlHelper helper)
        {
            return new HtmlString(@"<div class=""row"" style=""padding-top:60px;padding-bottom:60px;"">
                                        <div class=""col-md-4 col-md-offset-4 text-center"">
                                            <div class=""loader-container"">
                                                <img src=""/images/spin.gif"" /> Loading
                                            </div>
                                        </div>
                                    </div>");
        }

        public static HtmlString GoodsDamagedLabel(this IHtmlHelper helper, bool isDamaged)
        {
            return isDamaged
                ? new HtmlString(@"<span class=""label label-sm label-danger  bold"">YES</span>")
                : new HtmlString(@"<span class=""label label-sm label-green bold"">NO</span>");
        }

        #region Private Functions
        private static string GetIHtmlContent<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            string input = string.Empty;
            var htmlContent = htmlHelper.TextBoxFor(expression);

            using (var writer = new StringWriter())
            {
                htmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                input = writer.ToString();
            }
            return input;
        }




        private static string GetDynamicPropertyString(dynamic dynamicProperty = null)
        {
            if (dynamicProperty != null)
            {
                var proparties = string.Empty;

                var type = dynamicProperty.GetType();
                foreach (var v in type.GetProperties())
                {
                    string attrName = v.Name.ToString();
                    if (attrName.StartsWith("data_"))
                    {
                        attrName = attrName.Replace("_", "-");
                    }
                    var attrValue = type.GetProperty(v.Name).GetValue(dynamicProperty, null);
                    proparties = string.Format(@" {0} {1}=""{2}""", proparties, attrName, attrValue);
                }

                return proparties;
            }
            return string.Empty;
        }

        #endregion
    }
}
