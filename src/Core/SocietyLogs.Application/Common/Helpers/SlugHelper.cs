using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SocietyLogs.Application.Common.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            // 1. Türkçe karakterleri İngilizceye çevir
            text = text.ToLower(new CultureInfo("tr-TR"));
            text = text.Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u")
                       .Replace("ş", "s").Replace("ö", "o").Replace("ç", "c");

            // 2. Geçersiz karakterleri sil (Sadece harf, sayı ve tire kalsın)
            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

            // 3. Birden fazla boşluğu tek tireye çevir "yazilim   kulubu" -> "yazilim-kulubu"
            text = Regex.Replace(text, @"\s+", "-").Trim();

            return text;
        }
    }
}
