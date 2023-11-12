using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GAMF.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent Report<TReportRow>(this IHtmlHelper<IEnumerable<TReportRow>> htmlHelper)
        {
            var properties = typeof(TReportRow).GetProperties().Select(property => new { PropertyInfo = property, DisplayAttribute = property.GetCustomAttribute<DisplayAttribute>()! }).Where(p => p.DisplayAttribute != null).ToArray();
            if (!properties.Any())
                return HtmlString.Empty;

            var rowParameter = Expression.Parameter(typeof(TReportRow));
            var propertyGetters = properties.Select(property => Expression.Lambda<Func<TReportRow, object?>>(Expression.Convert(Expression.Property(rowParameter, property.PropertyInfo), typeof(object)), rowParameter).Compile()).ToArray();

            var tbTable = new TagBuilder("table");
            tbTable.MergeAttribute("class", "table");

            var tbTHead = new TagBuilder("thead");
            var tbTHeadTr = new TagBuilder("tr");
            foreach (var property in properties)
            {
                var tbTH = new TagBuilder("th");
                tbTH.InnerHtml.Append(property.DisplayAttribute.GetName() ?? property.PropertyInfo.Name);
                tbTHeadTr.InnerHtml.AppendHtml(tbTH);
            }
            tbTHead.InnerHtml.AppendHtml(tbTHeadTr);
            tbTable.InnerHtml.AppendHtml(tbTHead);

            var tbBody = new TagBuilder("tbody");
            var rows = htmlHelper.ViewData.Model;
            foreach (var row in rows)
            {
                var tbTR = new TagBuilder("tr");
                foreach (var propertyGetter in propertyGetters)
                {
                    var tbTD = new TagBuilder("td");
                    tbTD.InnerHtml.Append(Format(propertyGetter(row)));
                    tbTR.InnerHtml.AppendHtml(tbTD);
                }
                tbBody.InnerHtml.AppendHtml(tbTR);
            }
            tbTable.InnerHtml.AppendHtml(tbBody);

            return tbTable;

            static string Format(object? value) => value switch
            {
                DateTime dateTime => dateTime.ToString("d"),
                DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("G"),
                _ => $"{value}"
            };
        }
    }
}
