using System;
using System.Linq;
using Loby.Extensions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Pors.Website.TagHelpers
{
    public class PagerTagHelper : TagHelper
    {
        #region Members

        [ViewContext]
        public ViewContext ViewContext { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalItems { get; set; }
        public int MaxPages { get; set; } = 10;

        private string UrlTemplate { get; set; }
        private int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        #region Options

        #region Texts

        public string FirstPageText { get; set; } = "«";
        public string LastPageText { get; set; } = "»";
        public string NextPageText { get; set; } = "›";
        public string PreviousPageText { get; set; } = "‹";

        #endregion;

        #region Options

        public bool ShowGaps { get; set; } = true;
        public bool ShowFirstAndLastPages { get; set; } = true;
        public bool ShowPreviousAndNextPages { get; set; } = true;

        #endregion;

        #region Classes

        public string PagingContainerCssClass { get; set; } = "pagination";
        public string LinkContainerCssClass { get; set; } = "page-item";
        public string LinkCssClass { get; set; } = "page-link";
        public string ActiveLinkCssClass { get; set; } = "active";
        public string InActiveLinkCssClass { get; set; } = "disabled";
        public string JumpingLinkCssClass { get; set; } = "page-link";
        public string ActiveJumpingLinkCssClass { get; set; } = "active";
        public string InActiveJumpingLinkCssClass { get; set; } = "disabled";

        #endregion;

        #region Queries

        public string PageSizeQueryStringKey { get; set; } = "pageSize";
        public string CurrentPageQueryStringKey { get; set; } = "page";

        #endregion;

        #endregion;

        #endregion;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (TotalPages > 0)
            {
                var ul = new TagBuilder("ul");
                ul.AddCssClass(PagingContainerCssClass);

                UrlTemplate = CreateUrlTemplate();

                if (ShowFirstAndLastPages)
                {
                    ul.InnerHtml.AppendHtml(CreateNavigationLink(1, FirstPageText, true));
                }

                if (ShowPreviousAndNextPages)
                {
                    var page = CurrentPage - 1 > 1 ? CurrentPage - 1 : 1;

                    ul.InnerHtml.AppendHtml(CreateNavigationLink(page, PreviousPageText, true));
                }

                if (MaxPages == 1)
                {
                    ul.InnerHtml.AppendHtml(CreateNavigationLink(CurrentPage));
                }
                else
                {
                    var boundaries = CalculateBoundaries();
                    var endBound = boundaries.Item2;
                    var startBound = boundaries.Item1;
                    var gapElement = CreateGap();

                    if (ShowGaps && endBound > MaxPages)
                    {
                        ul.InnerHtml.AppendHtml(CreateNavigationLink(1));
                        ul.InnerHtml.AppendHtml(gapElement);
                    }

                    for (int page = startBound; page <= endBound; page++)
                    {
                        ul.InnerHtml.AppendHtml(CreateNavigationLink(page));
                    }

                    if (ShowGaps && endBound < TotalPages)
                    {
                        ul.InnerHtml.AppendHtml(gapElement);
                        ul.InnerHtml.AppendHtml(CreateNavigationLink(TotalPages));
                    }
                }

                if (ShowPreviousAndNextPages)
                {
                    var page = CurrentPage + 1 > TotalPages ? TotalPages : CurrentPage + 1;

                    ul.InnerHtml.AppendHtml(CreateNavigationLink(page, NextPageText, true));
                }

                if (ShowFirstAndLastPages)
                {
                    ul.InnerHtml.AppendHtml(CreateNavigationLink(TotalPages, LastPageText, true));
                }

                output.Content.AppendHtml(ul);
            }

            base.Process(context, output);
        }

        #region Ulilities

        private TagBuilder CreateNavigationLink(int page, string text = null, bool isJumpingLink = false)
        {
            var a = new TagBuilder("a");
            var li = new TagBuilder("li");
            var url = string.Format(UrlTemplate, page, PageSize);

            a.Attributes.Add("href", url);
            a.AddCssClass(LinkCssClass);
            a.InnerHtml.Append(text ?? page.ToString());

            if (CurrentPage == page && !isJumpingLink)
            {
                a.AddCssClass(ActiveLinkCssClass);
                li.AddCssClass(ActiveLinkCssClass);
            }
            else if (CurrentPage == page && isJumpingLink)
            {
                a.AddCssClass(InActiveJumpingLinkCssClass);
                li.AddCssClass(InActiveJumpingLinkCssClass);
            }

            li.InnerHtml.AppendHtml(a);
            li.AddCssClass(LinkContainerCssClass);

            return li;
        }

        private TagBuilder CreateGap()
        {
            var li = new TagBuilder("li");

            li.InnerHtml.AppendHtml("&nbsp;...&nbsp;");
            li.AddCssClass(LinkContainerCssClass);

            return li;
        }

        private string CreateUrlTemplate()
        {
            var path = ViewContext.HttpContext.Request.Path;
            var query = ViewContext.HttpContext.Request.Query;
            var queryItems = new Dictionary<string, string>()
            {
                {PageSizeQueryStringKey,"{1}" },
                {CurrentPageQueryStringKey,"{0}" },
            };

            foreach (var queryItem in query)
            {
                if (!queryItems.ContainsKey(queryItem.Key))
                {
                    queryItems.Add(queryItem.Key, queryItem.Value);
                }
            }

            var queryString = queryItems
                .Select(x => $"{x.Key}={x.Value}")
                .Join("&");

            var template = $"{path}?{queryString}";

            return template;
        }

        private Tuple<int, int> CalculateBoundaries()
        {
            int start = 0, end;
            int gaps = (int)Math.Ceiling(MaxPages / 2.0);

            if (MaxPages > TotalPages)
            {
                MaxPages = TotalPages;
            }

            if (TotalPages == 1)
            {
                start = end = 1;
            }
            else if (CurrentPage < MaxPages)
            {
                start = 1;
                end = MaxPages;
            }
            else if (CurrentPage + MaxPages == TotalPages)
            {
                end = TotalPages - 2;
                start = TotalPages - MaxPages > 0 ? TotalPages - MaxPages - 1 : 1;
            }
            else if (CurrentPage + MaxPages == TotalPages + 1)
            {
                end = TotalPages - 1;
                start = TotalPages - MaxPages > 0 ? TotalPages - MaxPages : 1;
            }
            else if (CurrentPage + MaxPages > TotalPages + 1)
            {
                end = TotalPages;
                start = TotalPages - MaxPages > 0 ? TotalPages - MaxPages + 1 : 1;
            }
            else
            {
                end = start + MaxPages - 1;
                start = CurrentPage - gaps > 0 ? CurrentPage - gaps + 1 : 1;
            }

            return Tuple.Create(start, end);
        }

        #endregion;
    }
}
