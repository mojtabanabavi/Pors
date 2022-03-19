using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Pors.Website.TagHelpers
{
    /// <summary>
    /// choose render mode style,
    /// <para>classic: regular dropdown select list</para>
    /// <para>Bootstrap4: HTML5 div with Bootstrap4 support</para>
    /// </summary>
    public enum RenderMode
    {
        /// <summary>
        /// regular dropdown list
        /// </summary>
        Classic = 0,

        /// <summary>
        /// HTML5 div with Bootstrap 4 support
        /// </summary>
        Bootstrap = 1,

        /// <summary>
        /// Render as form control
        /// </summary>
        FormControl = 2,
        /// <summary>
        /// HTML5 div with Bootstrap 5 support
        /// </summary>
        Bootstrap5 = 3
    }

    /// <summary>
    /// Number formats for different cultures.
    /// <para>https://github.com/unicode-cldr/cldr-core/blob/master/supplemental/numberingSystems.json</para>
    /// </summary>
    public static class NumberFormats
    {
        /// <summary>
        /// Receives a number in system format, and converts it to any other format.
        /// See <see cref="NumberFormats"/>
        /// </summary>
        /// <param name="number"></param>
        /// <param name="targetFormat"></param>
        /// <returns></returns>
        public static string ToNumberFormat(this int number, string targetFormat)
        {
            string _str = string.Empty;
            switch (targetFormat)
            {
                case NumberFormats.Default: _str = number.ToString("N0"); break;
                case NumberFormats.Hex: _str = number.ToString("X"); break;
                default:
                    var numberStr = number.ToString();
                    var newNum = string.Empty;

                    for (int i = 0; i < numberStr.Length; i++)
                        newNum += targetFormat.Split(' ')[int.Parse(numberStr[i].ToString())];

                    _str = string.Join("", newNum);
                    break;
            }

            return _str;
        }

        /// <summary>
        /// System default numbering format
        /// </summary>
        public const string Default = "default";

        /// <summary>
        /// 0123456789
        /// </summary>
        public const string Arabic = "0 1 2 3 4 5 6 7 8 9";

        /// <summary>
        /// Use hexadecimal numbering system
        /// </summary>
        public const string Hex = "hex";

        /// <summary>
        /// I II III IV V VI
        /// </summary>
        public const string Roman = "roman";

        /// <summary>
        /// ٠١٢٣٤٥٦٧٨٩
        /// </summary>
        public const string Hindi = "٠ ١ ٢ ٣ ٤ ٥ ٦ ٧ ٨ ٩";

        /// <summary>
        /// 𑁦𑁧𑁨𑁩𑁪𑁫𑁬𑁭𑁮𑁯
        /// </summary>
        public const string Brah = "𑁦 𑁧 𑁨 𑁩 𑁪 𑁫 𑁬 𑁭 𑁮 𑁯";

        /// <summary>
        /// ০১২৩৪৫৬৭৮৯
        /// </summary>
        public const string Beng = "০ ১ ২ ৩ ৪ ৫ ৬ ৭ ৮ ৯";

        /// <summary>
        /// ०१२३४५६७८९
        /// </summary>
        public const string Deva = "० १ २ ३ ४ ५ ६ ७ ८ ९";

        /// <summary>
        /// ۰۱۲۳۴۵۶۷۸۹
        /// </summary>
        public const string Farsi = "۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹";

        /// <summary>
        /// ０１２３４５６７８９
        /// </summary>
        public const string Fullwide = "０ １ ２ ３ ４ ５ ６ ７ ８ ９";

        /// <summary>
        /// ೦೧೨೩೪೫೬೭೮೯
        /// </summary>
        public const string Knda = "೦ ೧ ೨ ೩ ೪ ೫ ೬ ೭ ೮ ೯";

        /// <summary>
        /// ૦૧૨૩૪૫૬૭૮૯
        /// </summary>
        public const string Gujr = "૦ ૧ ૨ ૩ ૪ ૫ ૬ ૭ ૮ ૯";

        /// <summary>
        /// ੦੧੨੩੪੫੬੭੮੯
        /// </summary>
        public const string Guru = "੦ ੧ ੨ ੩ ੪ ੫ ੬ ੭ ੮ ੯";

        /// <summary>
        /// 〇一二三四五六七八九
        /// </summary>
        public const string Hanidec = "〇 一 二 三 四 五 六 七 八 九";

        /// <summary>
        /// ꧐꧑꧒꧓꧔꧕꧖꧗꧘꧙
        /// </summary>
        public const string Java = "꧐ ꧑ ꧒ ꧓ ꧔ ꧕ ꧖ ꧗ ꧘ ꧙";

        /// <summary>
        /// ០១២៣៤៥៦៧៨៩
        /// </summary>
        public const string Khmr = "០ ១ ២ ៣ ៤ ៥ ៦ ៧ ៨ ៩";

        /// <summary>
        /// ໐໑໒໓໔໕໖໗໘໙
        /// </summary>
        public const string Laoo = "໐ ໑ ໒ ໓ ໔ ໕ ໖ ໗ ໘ ໙";

        /// <summary>
        /// 0123456789
        /// </summary>
        public const string Latin = "0 1 2 3 4 5 6 7 8 9";

        /// <summary>
        /// 𝟎𝟏𝟐𝟑𝟒𝟓𝟔𝟕𝟖𝟗
        /// </summary>
        public const string Mathbold = "𝟎 𝟏 𝟐 𝟑 𝟒 𝟓 𝟔 𝟕 𝟖 𝟗";

        /// <summary>
        /// 𝟘𝟙𝟚𝟛𝟜𝟝𝟞𝟟𝟠𝟡
        /// </summary>
        public const string Mathborder = "𝟘 𝟙 𝟚 𝟛 𝟜 𝟝 𝟞 𝟟 𝟠 𝟡";

        /// <summary>
        /// 𝟶𝟷𝟸𝟹𝟺𝟻𝟼𝟽𝟾𝟿
        /// </summary>
        public const string Mathmono = "𝟶 𝟷 𝟸 𝟹 𝟺 𝟻 𝟼 𝟽 𝟾 𝟿";

        /// <summary>
        /// 𝟬𝟭𝟮𝟯𝟰𝟱𝟲𝟳𝟴𝟵
        /// </summary>
        public const string Mathanb = "𝟬 𝟭 𝟮 𝟯 𝟰 𝟱 𝟲 𝟳 𝟴 𝟵";

        /// <summary>
        /// 𝟢𝟣𝟤𝟥𝟦𝟧𝟨𝟩𝟪𝟫
        /// </summary>
        public const string Mathsans = "𝟢 𝟣 𝟤 𝟥 𝟦 𝟧 𝟨 𝟩 𝟪 𝟫";

        /// <summary>
        /// ൦൧൨൩൪൫൬൭൮൯
        /// </summary>
        public const string Mlym = "൦ ൧ ൨ ൩ ൪ ൫ ൬ ൭ ൮ ൯";

        /// <summary>
        /// ᠐᠑᠒᠓᠔᠕᠖᠗᠘᠙
        /// </summary>
        public const string Mong = "᠐ ᠑ ᠒ ᠓ ᠔ ᠕ ᠖  ᠗ ᠘ ᠙";

        /// <summary>
        /// ၀၁၂၃၄၅၆၇၈၉
        /// </summary>
        public const string Mymr = "၀ ၁ ၂ ၃ ၄ ၅ ၆ ၇ ၈ ၉";

        /// <summary>
        /// ႐႑႒႓႔႕႖႗႘႙
        /// </summary>
        public const string Mymrshan = "႐ ႑ ႒ ႓ ႔ ႕ ႖ ႗ ႘ ႙";

        /// <summary>
        /// ꧰꧱꧲꧳꧴꧵꧶꧷꧸꧹
        /// </summary>
        public const string Mymtlng = "꧰ ꧱ ꧲ ꧳ ꧴ ꧵ ꧶ ꧷ ꧸ ꧹";

        /// <summary>
        /// ߀߁߂߃߄߅߆߇߈߉
        /// </summary>
        public const string Nkoo = "߀ ߁ ߂ ߃ ߄ ߅ ߆ ߇ ߈ ߉";

        /// <summary>
        /// ᱐᱑᱒᱓᱔᱕᱖᱗᱘᱙
        /// </summary>
        public const string Olck = "᱐ ᱑ ᱒ ᱓ ᱔ ᱕ ᱖ ᱗ ᱘ ᱙";

        /// <summary>
        /// ୦୧୨୩୪୫୬୭୮୯
        /// </summary>
        public const string Orya = "୦ ୧ ୨ ୩ ୪ ୫ ୬ ୭ ୮ ୯";

        /// <summary>
        /// 𐒠𐒡𐒢𐒣𐒤𐒥𐒦𐒧𐒨𐒩
        /// </summary>
        public const string Osma = "𐒠 𐒡 𐒢 𐒣 𐒤 𐒥 𐒦 𐒧 𐒨 𐒩";

        /// <summary>
        /// ෦෧෨෩෪෫෬෭෮෯
        /// </summary>
        public const string Sinh = "෦ ෧ ෨ ෩ ෪ ෫ ෬ ෭ ෮ ෯";

        /// <summary>
        /// ᧐᧑᧒᧓᧔᧕᧖᧗᧘᧙
        /// </summary>
        public const string Talu = "᧐ ᧑ ᧒ ᧓ ᧔ ᧕ ᧖ ᧗ ᧘ ᧙";

        /// <summary>
        /// ௦௧௨௩௪௫௬௭௮௯
        /// </summary>
        public const string Tamldec = "௦ ௧ ௨ ௩ ௪ ௫ ௬ ௭ ௮ ௯";

        /// <summary>
        /// ౦౧౨౩౪౫౬౭౮౯
        /// </summary>
        public const string Telu = "౦ ౧ ౨ ౩ ౪ ౫ ౬ ౭ ౮ ౯";

        /// <summary>
        /// ๐๑๒๓๔๕๖๗๘๙
        /// </summary>
        public const string Thai = "๐ ๑ ๒ ๓ ๔ ๕ ๖ ๗ ๘ ๙";

        /// <summary>
        /// ༠༡༢༣༤༥༦༧༨༩
        /// </summary>
        public const string Tibt = "༠ ༡ ༢ ༣ ༤ ༥ ༦ ༧ ༨ ༩";

        /// <summary>
        /// ꘠꘡꘢꘣꘤꘥꘦꘧꘨꘩
        /// </summary>
        public const string Vaii = "꘠ ꘡ ꘢ ꘣ ꘤ ꘥ ꘦ ꘧ ꘨ ꘩";
    }

    /// <summary>
    /// Creates a pagination control
    /// </summary>
    public class PagingTagHelper : TagHelper
    {
        private IConfiguration Configuration { get; }
        private readonly ILogger _logger;

        /// <summary>
        /// A URL template for paging buttons
        /// e.g. 
        /// <![CDATA[?page={0}&pageSize={1}&q=test]]>
        /// </summary>
        private string UrlTemplate { get; set; }

        /// <summary>
        /// <para>ViewContext property is not required to be passed as parameter, it will be assigned automatically by the tag helper.</para>
        /// <para>View context is required to access TempData dictionary that contains the alerts coming from backend</para>
        /// </summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; } = null;

        /// <summary>
        /// Creates a pagination control
        /// </summary>
        public PagingTagHelper(IConfiguration configuration, ILogger<PagingTagHelper> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        #region Settings

        /// <summary>
        /// current page number.
        /// <para>default: 1</para>
        /// <para>example: p=1</para>
        /// </summary>
        public int PageNo { get; set; } = 1;

        /// <summary>
        /// how many items to get from db per page per request
        /// <para>default: 10</para>
        /// <para>example: pageSize=10</para>
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Total count of records in the db
        /// <para>default: 0</para>
        /// </summary>
        public int TotalRecords { get; set; } = 0;

        /// <summary>
        /// if count of pages is too much, restrict shown pages count to specific number
        /// <para>default: 10</para>
        /// </summary>
        public int? MaxDisplayedPages { get; set; }

        /// <summary>
        /// name of the settings section in appSettings.json
        /// <param>default: "default"</param>
        /// </summary>
        public string SettingsJson { get; set; } = "default";

        /// <summary>
        /// Force adding url path to the navigation url
        /// in some scenarios when the page is under some area/subFolder
        /// The navigation links are pointing to the home page.
        /// To force adding url path enable this property
        /// </summary>
        public bool? FixUrlPath { get; set; } = true;

        #endregion

        #region Page size navigation
        /// <summary>
        /// A list of dash delimitted numbers for page size dropdown. 
        /// default: "10-25-50"
        /// </summary>
        public string PageSizeDropdownItems { get; set; }

        #endregion

        #region QueryString

        /// <summary>
        /// Query string paramter name for current page.
        /// <para>default: p</para>
        /// <para>exmaple: p=1</para>
        /// </summary>
        public string QueryStringKeyPageNo { get; set; }

        /// <summary>
        /// Query string parameter name for page size
        /// <para>default: s</para>
        /// <para>example: s=25</para>
        /// </summary>
        public string QueryStringKeyPageSize { get; set; }

        #endregion

        #region Display settings

        /// <summary>
        /// Show drop down list for different page size options
        /// <para>default: true</para>
        /// <para>options: true, false</para>
        /// </summary>
        public bool? ShowPageSizeNav { get; set; }

        /// <summary>
        /// Show a three dots after first page or before last page 
        /// when there is a gap in pages at the beginnig or end
        /// </summary>
        public bool? ShowGap { get; set; }

        /// <summary>
        /// Show/hide First-Last buttons
        /// <para>default: true, if set to false and total pages > max displayed pages it will be true</para>
        /// </summary>
        public bool? ShowFirstLast { get; set; }

        /// <summary>
        /// Show/hide Previous-Next buttons
        /// <para>default: true</para>
        /// </summary>
        public bool? ShowPrevNext { get; set; }

        /// <summary>
        /// Show or hide total pages count
        /// <para>default: true</para>
        /// </summary>
        public bool? ShowTotalPages { get; set; }

        /// <summary>
        /// Show or hide total records count
        /// <para>default: true</para>
        /// </summary>
        public bool? ShowTotalRecords { get; set; }
        #endregion

        #region Texts
        /// <summary>
        /// The text to display at page size dropdown list label
        /// <para>default: Page size </para>
        /// </summary>
        public string TextPageSize { get; set; }


        /// <summary>
        /// Text to show on the "Go To First" Page button
        /// <para>
        /// <![CDATA[default: &laquo;]]></para>
        /// </summary>
        public string TextFirst { get; set; }

        /// <summary>
        /// Text to show on "Go to last page" button
        /// <para>
        /// <![CDATA[default: &raquo;]]></para>
        /// </summary>
        public string TextLast { get; set; }

        /// <summary>
        /// Next button text
        /// <para>
        /// <![CDATA[default: &rsaquo;]]></para>
        /// </summary>
        public string TextNext { get; set; }

        /// <summary>
        /// previous button text
        /// <para>
        /// <![CDATA[default: &lsaquo;]]></para>
        /// </summary>
        public string TextPrevious { get; set; }

        /// <summary>
        /// Display text for total pages label
        /// <para>default: page</para>
        /// </summary>
        public string TextTotalPages { get; set; }

        /// <summary>
        /// Display text for total records label
        /// <para>default: records</para>
        /// </summary>
        public string TextTotalRecords { get; set; }

        /// <summary>
        /// The number display format for page numbers. Use a list of numbers splitted by space e.g. "0 1 2 3 4 5 6 7 8 9" or use one from a pre-defined numbers formats in :
        /// <see cref="LazZiya.TagHelpers.Utilities.NumberFormats"/>
        /// </summary>
        public string NumberFormat { get; set; }
        #endregion

        #region Screen Reader
        /// <summary>
        /// Text for screen readers only
        /// </summary>
        public string SrTextFirst { get; set; }

        /// <summary>
        /// text for screen readers only
        /// </summary>
        public string SrTextLast { get; set; }

        /// <summary>
        /// text for screenreaders only
        /// </summary>
        public string SrTextNext { get; set; }

        /// <summary>
        /// text for screen readers only
        /// </summary>
        public string SrTextPrevious { get; set; }

        #endregion

        #region Styling

        /// <summary>
        /// Select bootstrap version
        /// </summary>
        public RenderMode RenderMode { get; set; } = RenderMode.Bootstrap;

        /// <summary>
        /// add custom class to content div
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// css class for pagination div
        /// </summary>
        public string ClassPagingControlDiv { get; set; }

        /// <summary>
        /// css class for page count/record count div
        /// </summary>
        public string ClassInfoDiv { get; set; }

        /// <summary>
        /// styling class for page size div
        /// </summary>
        public string ClassPageSizeDiv { get; set; }

        /// <summary>
        /// pagination control class
        /// <para>default: pagination</para>
        /// </summary>
        public string ClassPagingControl { get; set; }

        /// <summary>
        /// class name for the active page
        /// <para>default: active</para>
        /// <para>examples: disabled, active, ...</para>
        /// </summary>
        public string ClassActivePage { get; set; }

        /// <summary>
        /// name of the class when jumping button is disabled.
        /// jumping buttons are prev-next and first-last buttons
        /// <param>default: disabled</param>
        /// <para>example: disabled, d-hidden</para>
        /// </summary>
        public string ClassDisabledJumpingButton { get; set; }

        /// <summary>
        /// css class for total records info
        /// <para>default: badge badge-light</para>
        /// </summary>
        public string ClassTotalRecords { get; set; }

        /// <summary>
        /// css class for total pages info
        /// <para>default: badge badge-light</para>
        /// </summary>
        public string ClassTotalPages { get; set; }

        /// <summary>
        /// css class for page link, use for styling bg and text colors
        /// </summary>
        public string ClassPageLink { get; set; }

        #endregion

        private int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);

        private class Boundaries
        {
            public int Start { get; set; }
            public int End { get; set; }
        }

        /// <summary>
        /// process creating paging tag helper
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetDefaults();

            if (TotalPages > 0)
            {
                var pagingControl = new TagBuilder("ul");
                pagingControl.AddCssClass($"{ClassPagingControl}");

                // show-hide first-last buttons on user options
                if (ShowFirstLast == true)
                {
                    ShowFirstLast = true;
                }

                UrlTemplate = CreatePagingUrlTemplate();

                if (ShowFirstLast == true)
                {
                    var first = CreatePagingLink(1, TextFirst, SrTextFirst, ClassDisabledJumpingButton);
                    pagingControl.InnerHtml.AppendHtml(first);
                }

                if (ShowPrevNext == true)
                {
                    var prevPage = PageNo - 1 <= 1 ? 1 : PageNo - 1;
                    var prev = CreatePagingLink(prevPage, TextPrevious, SrTextPrevious, ClassDisabledJumpingButton);
                    pagingControl.InnerHtml.AppendHtml(prev);
                }

                if (MaxDisplayedPages == 1)
                {
                    var numTag = CreatePagingLink(PageNo, null, null, ClassActivePage);
                    pagingControl.InnerHtml.AppendHtml(numTag);
                }
                else if (MaxDisplayedPages > 1)
                {
                    // Boundaries are the start-end currently displayed pages
                    var boundaries = CalculateBoundaries(PageNo, TotalPages, MaxDisplayedPages.Value);

                    string gapStr = "<li class=\"page-item border-0\">&nbsp;...&nbsp;</li>";

                    if (ShowGap == true && boundaries.End > MaxDisplayedPages)
                    {
                        // add page no 1
                        var num1Tag = CreatePagingLink(1, null, null, ClassActivePage);
                        pagingControl.InnerHtml.AppendHtml(num1Tag);

                        // Add gap after first page
                        pagingControl.InnerHtml.AppendHtml(gapStr);
                    }

                    for (int i = boundaries.Start; i <= boundaries.End; i++)
                    {
                        var numTag = CreatePagingLink(i, null, null, ClassActivePage);
                        pagingControl.InnerHtml.AppendHtml(numTag);
                    }

                    if (ShowGap == true && boundaries.End < TotalPages)
                    {
                        // Add gap before last page
                        pagingControl.InnerHtml.AppendHtml(gapStr);

                        // add last page
                        var numLastTag = CreatePagingLink(TotalPages, null, null, ClassActivePage);
                        pagingControl.InnerHtml.AppendHtml(numLastTag);
                    }
                }

                if (ShowPrevNext == true)
                {
                    var nextPage = PageNo + 1 > TotalPages ? TotalPages : PageNo + 1;
                    var next = CreatePagingLink(nextPage, TextNext, SrTextNext, ClassDisabledJumpingButton);
                    pagingControl.InnerHtml.AppendHtml(next);
                }

                if (ShowFirstLast == true)
                {
                    var last = CreatePagingLink(TotalPages, TextLast, SrTextLast, ClassDisabledJumpingButton);
                    pagingControl.InnerHtml.AppendHtml(last);
                }

                var pagingControlDiv = new TagBuilder("div");
                pagingControlDiv.AddCssClass($"{ClassPagingControlDiv}");
                pagingControlDiv.InnerHtml.AppendHtml(pagingControl);

                output.TagName = "div";
                output.Attributes.SetAttribute("class", $"{Class}");
                output.Content.AppendHtml(pagingControlDiv);

                if (ShowPageSizeNav == true)
                {
                    var psDropdown = CreatePageSizeControl();

                    var psDiv = new TagBuilder("div");
                    psDiv.AddCssClass($"{ClassPageSizeDiv}");
                    psDiv.InnerHtml.AppendHtml(psDropdown);

                    output.Content.AppendHtml(psDiv);
                }

                if (ShowTotalPages == true || ShowTotalRecords == true)
                {
                    var infoDiv = AddDisplayInfo();

                    output.Content.AppendHtml(infoDiv);
                }

            }
        }

        /// <summary>
        /// This method will assign the values by checking three places
        /// 1- Property value if set from HTML code
        /// 2- Default values in appSettings.json
        /// 3- Hard coded default value in code
        /// </summary>
        private void SetDefaults()
        {
            var _settingsJson = SettingsJson ?? "default";

            _logger.LogInformation($"----> PagingTagHelper SettingsJson: {SettingsJson} - {_settingsJson}");

            MaxDisplayedPages = MaxDisplayedPages == null ? int.TryParse(Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:max-displayed-pages"], out int _dp) ? _dp : 10 : MaxDisplayedPages;

            PageSizeDropdownItems = PageSizeDropdownItems ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:page-size-dropdown-items"] ?? "10-25-50";

            QueryStringKeyPageNo = QueryStringKeyPageNo ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:query-string-key-page-no"] ?? "page";

            QueryStringKeyPageSize = QueryStringKeyPageSize ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:query-string-key-page-size"] ?? "pageSize";

            ShowGap = ShowGap == null ?
                bool.TryParse(Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:show-gap"], out bool _sg) ? _sg : true : ShowGap;

            ShowFirstLast = ShowFirstLast == null ?
                bool.TryParse(Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:show-first-last"], out bool _sfl) ? _sfl : true : ShowFirstLast;

            ShowPrevNext = ShowPrevNext == null ? bool.TryParse(Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:show-prev-next"], out bool _sprn) ? _sprn : true : ShowPrevNext;

            ShowPageSizeNav = ShowPageSizeNav == null ? bool.TryParse(Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:show-page-size-nav"], out bool _spsn) ? _spsn : true : ShowPageSizeNav;

            ShowTotalPages = ShowTotalPages == null ? bool.TryParse(Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:show-total-pages"], out bool _stp) ? _stp : true : ShowTotalPages;

            ShowTotalRecords = ShowTotalRecords == null ? bool.TryParse(Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:show-total-records"], out bool _str) ? _str : true : ShowTotalRecords;

            NumberFormat = NumberFormat ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:number-format"] ?? NumberFormats.Default;

            TextPageSize = TextPageSize ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:text-page-size"];

            TextFirst = TextFirst ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:text-first"] ?? "&laquo;";

            TextLast = TextLast ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:text-last"] ?? "&raquo;";

            TextPrevious = TextPrevious ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:text-previous"] ?? "&lsaquo;";

            TextNext = TextNext ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:text-next"] ?? "&rsaquo;";

            TextTotalPages = TextTotalPages ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:text-total-pages"] ?? "pages";

            TextTotalRecords = TextTotalRecords ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:text-total-records"] ?? "records";

            SrTextFirst = SrTextFirst ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:sr-text-first"] ?? "First";

            SrTextLast = SrTextLast ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:sr-text-last"] ?? "Last";

            SrTextPrevious = SrTextPrevious ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:sr-text-previous"] ?? "Previous";

            SrTextNext = SrTextNext ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:sr-text-next"] ?? "Next";

            Class = Class ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:class"] ?? "row";

            ClassActivePage = ClassActivePage ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:class-active-page"] ?? "active";

            ClassDisabledJumpingButton = ClassDisabledJumpingButton ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:class-disabled-jumping-button"] ?? "disabled";

            ClassInfoDiv = ClassInfoDiv ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:class-info-div"] ?? "col-2";

            ClassPageSizeDiv = ClassPageSizeDiv ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:class-page-size-div"] ?? "col-1";

            ClassPagingControlDiv = ClassPagingControlDiv ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:class-paging-control-div"] ?? "col";

            ClassPagingControl = ClassPagingControl ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:class-paging-control"] ?? "pagination";

            ClassTotalPages = ClassTotalPages ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:class-total-pages"] ?? (RenderMode == RenderMode.Bootstrap ? "badge badge-light" : "badge bg-light text-dark");

            ClassTotalRecords = ClassTotalRecords ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:class-total-records"] ?? (RenderMode == RenderMode.Bootstrap ? "badge badge-dark" : "badge bg-dark");

            ClassPageLink = ClassPageLink ?? Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:class-page-link"] ?? "";

            FixUrlPath = FixUrlPath == null ?
                bool.TryParse(Configuration[$"lazziya:pagingTagHelper:{_settingsJson}:fix-url-path"], out bool _fPath) ? _fPath : true : FixUrlPath;

            _logger.LogInformation($"----> PagingTagHelper - " +
                $"{nameof(PageNo)}: {PageNo}, " +
                $"{nameof(PageSize)}: {PageSize}, " +
                $"{nameof(TotalRecords)}: {TotalRecords}, " +
                $"{nameof(TotalPages)}: {TotalPages}, " +
                $"{nameof(QueryStringKeyPageNo)}: {QueryStringKeyPageNo}, " +
                $"{nameof(QueryStringKeyPageSize)}: {QueryStringKeyPageSize}, " +
                $"");
        }

        private TagBuilder AddDisplayInfo()
        {
            var infoDiv = new TagBuilder("div");
            infoDiv.AddCssClass($"{ClassInfoDiv}");

            var txt = string.Empty;
            if (ShowTotalPages == true)
            {
                infoDiv.InnerHtml.AppendHtml($"<span class=\"{ClassTotalPages}\">{TotalPages.ToNumberFormat(NumberFormat)} {TextTotalPages}</span>");
            }

            if (ShowTotalRecords == true)
            {
                infoDiv.InnerHtml.AppendHtml($"<span class=\"{ClassTotalRecords}\">{TotalRecords.ToNumberFormat(NumberFormat)} {TextTotalRecords}</span>");
            }

            return infoDiv;
        }

        /// <summary>
        /// Calculate the boundaries of the currently rendered page numbers
        /// </summary>
        /// <param name="currentPageNo"></param>
        /// <param name="totalPages"></param>
        /// <param name="maxDisplayedPages"></param>
        /// <returns></returns>
        private Boundaries CalculateBoundaries(int currentPageNo, int totalPages, int maxDisplayedPages)
        {
            int _start, _end;

            int _gap = (int)Math.Ceiling(maxDisplayedPages / 2.0);

            if (maxDisplayedPages > totalPages)
                maxDisplayedPages = totalPages;

            if (totalPages == 1)
            {
                _start = _end = 1;
            }
            // << < 1 2 (3) 4 5 6 7 8 9 10 > >>
            else if (currentPageNo < maxDisplayedPages)
            {
                _start = 1;
                _end = maxDisplayedPages;
            }
            // << < 91 92 93 94 95 96 97 (98) 99 100 > >>
            else if (currentPageNo + maxDisplayedPages == totalPages)
            {
                _start = totalPages - maxDisplayedPages > 0 ? totalPages - maxDisplayedPages - 1 : 1;
                _end = totalPages - 2;
            }
            // << < 91 92 93 94 95 96 97 (98) 99 100 > >>
            else if (currentPageNo + maxDisplayedPages == totalPages + 1)
            {
                _start = totalPages - maxDisplayedPages > 0 ? totalPages - maxDisplayedPages : 1;
                _end = totalPages - 1;
            }
            // << < 91 92 93 94 95 96 97 (98) 99 100 > >>
            else if (currentPageNo + maxDisplayedPages > totalPages + 1)
            {
                _start = totalPages - maxDisplayedPages > 0 ? totalPages - maxDisplayedPages + 1 : 1;
                _end = totalPages;
            }

            // << < 21 22 23 34 (25) 26 27 28 29 30 > >>
            else
            {
                _start = currentPageNo - _gap > 0 ? currentPageNo - _gap + 1 : 1;
                _end = _start + maxDisplayedPages - 1;
            }

            return new Boundaries { Start = _start, End = _end };
        }

        private TagBuilder CreatePagingLink(int targetPageNo, string text, string textSr, string pClass)
        {
            var liTag = new TagBuilder("li");
            liTag.AddCssClass("page-item");

            var pageUrl = CreatePagingUrl(targetPageNo, PageSize);

            var aTag = new TagBuilder("a");
            aTag.AddCssClass($"page-link {ClassPageLink}");
            aTag.Attributes.Add("href", pageUrl);

            // If no text provided for screen readers
            // use the actual page number
            if (string.IsNullOrWhiteSpace(textSr))
            {
                var pageNoText = targetPageNo.ToNumberFormat(NumberFormat);

                aTag.InnerHtml.Append($"{pageNoText}");
            }
            else
            {
                liTag.MergeAttribute("area-label", textSr);
                aTag.InnerHtml.AppendHtml($"<span area-hidden=\"true\">{text}</span>");

                if (RenderMode == RenderMode.Bootstrap5)
                    aTag.InnerHtml.AppendHtml($"<span class=\"visually-hidden-focusable\">{textSr}</span>");
                else
                    aTag.InnerHtml.AppendHtml($"<span class=\"sr-only\">{textSr}</span>");
            }

            if (PageNo == targetPageNo)
            {
                liTag.AddCssClass($"{pClass}");
                aTag.Attributes.Add("tabindex", "-1");
                aTag.Attributes.Remove("class");
                aTag.AddCssClass($"page-link {pClass}");
                aTag.Attributes.Remove("href");
            }

            liTag.InnerHtml.AppendHtml(aTag);
            return liTag;
        }

        /// <summary>
        /// dropdown list for changing page size (items per page)
        /// </summary>
        /// <returns></returns>
        private TagBuilder CreatePageSizeControl()
        {
            var dropDownDiv = new TagBuilder("div");
            dropDownDiv.AddCssClass("dropdown");

            var dropDownBtn = new TagBuilder("button");
            dropDownBtn.AddCssClass("btn btn-light dropdown-toggle");
            dropDownBtn.Attributes.Add("type", "button");
            dropDownBtn.Attributes.Add("id", "pagingDropDownMenuBtn");

            if (RenderMode == RenderMode.Bootstrap5)
                dropDownBtn.Attributes.Add("data-bs-toggle", "dropdown");
            else
                dropDownBtn.Attributes.Add("data-toggle", "dropdown");
            dropDownBtn.Attributes.Add("aria-haspopup", "true");
            dropDownBtn.Attributes.Add("aria-expanded", "false");

            var psText = string.IsNullOrWhiteSpace(TextPageSize)
                ? $"{PageSize.ToNumberFormat(NumberFormat)}"
                : string.Format(TextPageSize, $"{PageSize.ToNumberFormat(NumberFormat)}");

            dropDownBtn.InnerHtml.Append(psText);

            var dropDownMenu = new TagBuilder("div");
            dropDownMenu.AddCssClass("dropdown-menu dropdown-menu-right");
            dropDownMenu.Attributes.Add("aria-labelledby", "pagingDropDownMenuBtn");

            var pageSizeDropdownItems = PageSizeDropdownItems.Split("-", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < pageSizeDropdownItems.Length; i++)
            {
                var n = int.Parse(pageSizeDropdownItems[i]);

                var pageUrl = $"{CreatePagingUrl(1, n)}";

                var option = new TagBuilder("a");
                option.AddCssClass("dropdown-item");
                option.Attributes.Add("href", pageUrl);

                option.InnerHtml.Append($"{n.ToNumberFormat(NumberFormat)}");

                if (n == PageSize)
                    option.AddCssClass("active");

                dropDownMenu.InnerHtml.AppendHtml(option);
            }

            dropDownDiv.InnerHtml.AppendHtml(dropDownBtn);
            dropDownDiv.InnerHtml.AppendHtml(dropDownMenu);

            return dropDownDiv;
        }

        /// <summary>
        /// edit the url for each page, so it navigates to its target page number
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private string CreatePagingUrl(int pageNo, int pageSize)
        {
            return string.Format(UrlTemplate, pageNo, pageSize);
        }


        /// <summary>
        /// edit the url for each page, so it navigates to its target page number
        /// </summary>
        /// <returns>a string with placeholders for page no and page size</returns>
        private string CreatePagingUrlTemplate()
        {
            var queryString = ViewContext.HttpContext.Request.QueryString.Value;

            var urlTemplate = string.IsNullOrWhiteSpace(queryString)
                ? $"{QueryStringKeyPageNo}=1&{QueryStringKeyPageSize}={PageSize}".Split('&').ToList()
                : queryString.TrimStart('?').Split('&').ToList();

            var qDic = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(QueryStringKeyPageNo, "{0}"),
                new Tuple<string, object>(QueryStringKeyPageSize, "{1}")
            };

            var excludedKeys = new List<string> { "X-Requested-With", "_", QueryStringKeyPageNo, QueryStringKeyPageSize };
            foreach (var item in urlTemplate)
            {
                var key = item.Split('=')[0];
                var value = item.Split('=')[1];

                if (!excludedKeys.Contains(key))
                {
                    qDic.Add(new Tuple<string, object>(key, value));
                }
            }

            var path = ViewContext.HttpContext.Request.Path;

            return FixUrlPath ?? true
                ? path + "?" + string.Join("&", qDic.Select(q => q.Item1 + "=" + q.Item2))
                : "?" + string.Join("&", qDic.Select(q => q.Item1 + "=" + q.Item2));
        }
    }
}
