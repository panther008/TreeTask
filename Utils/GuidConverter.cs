using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTask.Utils
{
    public static class GuidConverter
    {
        public static Guid Base64UrlToGuid(ReadOnlySpan<char> base64Url)
        {
            Span<char> base64 = stackalloc char[24];

            for (int i = 0; i < base64Url.Length; i++)
            {
                base64[i] = base64Url[i] switch
                {
                    '-' => '+',
                    '_' => '/',
                    _ => base64Url[i]
                };
            }

            for (int i = base64Url.Length; i < 24; i++)
                base64[i] = '=';

            Span<byte> bytes = stackalloc byte[16];
            Convert.TryFromBase64Chars(base64, bytes, out _);

            return new Guid(bytes);
        }
    }
}
