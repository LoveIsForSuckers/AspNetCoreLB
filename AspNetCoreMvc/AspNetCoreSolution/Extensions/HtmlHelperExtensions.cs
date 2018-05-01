using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Linq.Expressions;

namespace AspNetCoreSolution.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent PartialFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string partialName, bool isForCollection = false)
        {
            var modelExplorer = helper.GetModelExplorer(expression);
            string prefix = ExpressionHelper.GetExpressionText(expression);

            var VD = new ViewDataDictionary(helper.ViewData);

            if (isForCollection)
            {
                VD.TemplateInfo.HtmlFieldPrefix = prefix;
            }
            else
            {
                if (!String.IsNullOrEmpty(VD.TemplateInfo.HtmlFieldPrefix))
                    VD.TemplateInfo.HtmlFieldPrefix += ".";

                VD.TemplateInfo.HtmlFieldPrefix += prefix;
            }
            
            return helper.Partial(partialName, modelExplorer.Model, VD);
        }

        public static ModelExplorer GetModelExplorer<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            return ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);
        }
    }
}
