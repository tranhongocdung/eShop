using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace MVCWeb.Libraries
{
    public class WhiteSpaceFilter : Stream
    {
        private Stream _shrink;

        public WhiteSpaceFilter(Stream shrink, Func<string, string> filter)
        {
            _shrink = shrink;
        }


        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return true; } }
        public override void Flush() { _shrink.Flush(); }
        public override long Length { get { return 0; } }
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _shrink.Read(buffer, offset, count);
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _shrink.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            _shrink.SetLength(value);
        }
        public override void Close()
        {
            _shrink.Close();
        }

        private static readonly Regex reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n\r])\s{2,}");
        public override void Write(byte[] buffer, int offset, int count)
        {
            var data = new byte[count];
            Buffer.BlockCopy(buffer, offset, data, 0, count);
            var html = Encoding.Default.GetString(buffer);

            html = reg.Replace(html, string.Empty);
            html = html.Replace(Environment.NewLine, "\n");
            var outdata = Encoding.Default.GetBytes(html);
            _shrink.Write(outdata, 0, outdata.GetLength(0));
        }
    }

    public class WhitespaceFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            response.Filter = new WhiteSpaceFilter(response.Filter, s =>
            {
                s = Regex.Replace(s, @"\s+", " ");
                s = Regex.Replace(s, @"\s*\n\s*", "\n");
                s = Regex.Replace(s, @"\s*\>\s*\<\s*", "><");
                s = Regex.Replace(s, @"<!--(.*?)-->", ""); //Remove comments 

                // single-line doctype must be preserved 
                var firstEndBracketPosition = s.IndexOf(">");
                if (firstEndBracketPosition >= 0)
                {
                    s = s.Remove(firstEndBracketPosition, 1);
                    s = s.Insert(firstEndBracketPosition, ">");
                }
                return s;
            });
        }
    }
}