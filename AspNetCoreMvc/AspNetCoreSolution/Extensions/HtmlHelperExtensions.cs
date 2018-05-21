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
            string prefix = null;

            var VD = new ViewDataDictionary(helper.ViewData);

            if (isForCollection)
                prefix = GetItemHtmlPrefix(null, expression);
            else
                prefix = GetItemHtmlPrefix(VD.TemplateInfo.HtmlFieldPrefix, expression);

            VD.TemplateInfo.HtmlFieldPrefix = prefix;

            return helper.Partial(partialName, modelExplorer.Model, VD);
        }

        public static ModelExplorer GetModelExplorer<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            return ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);
        }

        public static string GetItemHtmlPrefix<TModel, TProperty>(string existingPrefix, Expression<Func<TModel, TProperty>> expression)
        {
            string prefix = existingPrefix;
            if (!String.IsNullOrEmpty(existingPrefix))
                prefix += "."; 
            prefix += ExpressionHelper.GetExpressionText(expression);
            return prefix;
        }
    }
}
