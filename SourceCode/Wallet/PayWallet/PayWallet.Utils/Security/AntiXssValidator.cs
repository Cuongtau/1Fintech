using Microsoft.Security.Application;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PayWallet.Utils.Security
{
    public class AntiXssValidator
    {
        public static string[] XSSTagStringsForDoubleOpening = {"iframe","script","style","input","a","abbr",
            "acronym", "address", "applet", "area", "article","aside","audio","b","base","basefont","bdi","bdo",
            "big","blockquote","body","br","button","canvas","caption","center","cite","code","col","colgroup","datalist",
            "dd","del","details","dfn","dialog","dir","div","dl","dt","em","embed","fieldset","figcaption","figure","font",
            "footer","form","frame","frameset","h1","head","header","hr","html","i","img","ins","kbd","keygen","label","legend","li",
            "link","main","map","mark","menu","menuitem","meta","meter","nav","noframes","noscript","object","ol","optgroup","option",
            "output","p","param","picture","pre","progress","q","rp","rt","ruby","s","samp","section","select","small","source","span",
            "strike","strong","strong","sub","summary","sup","table","tbody","td","textarea","tfoot","th","thead","time","title","tr","track",
            "tt","u","ul","var","video","wbr"};

        public static string[] XSSHtmlEntities = {"&excl;","&#33;","&#x21;","U+00021", "&quot;", "&num;", "&percnt;", "&amp;", "&apos;", "&lpar;", "&rpar;", "&ast;", "&comma;",
        "&period;","&sol;","&colon;","&semi;","&quest;","&commat;","&lbrack;","&bsol;", "&rbrack;","&Hat","&lowbar;","&grave;", "&lbrace;","&vert;",
        "&rbrace;","&tilde;","&acute;","&lsquo;","&rsquo;","&sbquo;","&ldquo;","&rdquo;", "&#34;","&#x22;","U+00022","U+00023","&#x23;","&#35;","U+00026",
        "&#x26;","&#38;","&#39;","&#x27;","U+00027","U+00028","&#x28;","&#40;","&#41;","&#x29;","U+00029","&#58;","&#x3a;","U+0003A","&#59;","&#x3b;",
        "U+0003B","U+0007B","&#x7b;","&#123;","U+0007D","&#x7d;","&#125;","&lt;","&gt;","&#60;","&#62;"};

        /// <summary>
        /// Validate the words which contains potential XSS Attack.
        /// This will compare the encoded with sanitize html fragments, if it finds any differences means that it contains unsafe html tags.
        /// Here is the least of potential XSS attacks
        /// https://www.owasp.org/index.php/XSS_Filter_Evasion_Cheat_Sheet
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public bool Validate(string words)
        {
            if (string.IsNullOrWhiteSpace(words))
            {
                return false;
            }

            //this will protect all this script attack list https://www.owasp.org/index.php/XSS_Filter_Evasion_Cheat_Sheet            
            var sanitizedWords = Sanitizer.GetSafeHtmlFragment(words);
            var sanitizedWordsDecoded = HttpUtility.HtmlDecode(sanitizedWords);

            var checkWords = words.ToLower().Trim().Replace(" ", ""); //take out all space to protect < iframe
            //compare the sanitizedWord to check whether there is invalid tag
            if (sanitizedWordsDecoded.ToLower().Trim().Replace(" ", "") != checkWords)
            {
                return true;
            }

            //except this one, we should protect againts Double open angle brackets
            
            return (checkWords.StartsWith("<") || XSSHtmlEntities.Any(ent => checkWords.Contains(ent)) || XSSTagStringsForDoubleOpening.Any(tag => checkWords.Contains("<" + tag)));
        }

        public static string SanitizeUrl(string url)
        {
            return Regex.Replace(url, @"[^-A-Za-z0-9+&@#/%?=~_|!:,.;\(\)\s\p{L}+$]", "");
        }
    }
}