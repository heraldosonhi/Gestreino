using System;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using DocumentFormat.OpenXml.EMMA;
using static Gestreino.Classes.SelectValues;
using Gestreino.Classes;

namespace JeanPiagetSGA
{
    public static class Converters
    {
        // Capitalize first character of each word
        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }


        // Count number of words in given string
        public static int WordCount(string text)
        {
            int wordCount = 0, index = 0;

            // skip whitespace until first word
            while (index < text.Length && char.IsWhiteSpace(text[index]))
                index++;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
            }
            return wordCount;
        }


        //Custom method to Replace special characters of string
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 255) sb.Append(text[i]);
                else sb.Append(s_Diacritics[text[i]]);
            }
            return sb.ToString();
        }

        private static readonly char[] s_Diacritics = GetDiacritics();
        private static char[] GetDiacritics()
        {
            char[] accents = new char[256];

            for (int i = 0; i < 256; i++) accents[i] = (char)i;

            accents[(byte)'á'] = accents[(byte)'à'] = accents[(byte)'ã'] = accents[(byte)'â'] = accents[(byte)'ä'] = 'a';
            accents[(byte)'Á'] = accents[(byte)'À'] = accents[(byte)'Ã'] = accents[(byte)'Â'] = accents[(byte)'Ä'] = 'A';

            accents[(byte)'é'] = accents[(byte)'è'] = accents[(byte)'ê'] = accents[(byte)'ë'] = 'e';
            accents[(byte)'É'] = accents[(byte)'È'] = accents[(byte)'Ê'] = accents[(byte)'Ë'] = 'E';

            accents[(byte)'í'] = accents[(byte)'ì'] = accents[(byte)'î'] = accents[(byte)'ï'] = 'i';
            accents[(byte)'Í'] = accents[(byte)'Ì'] = accents[(byte)'Î'] = accents[(byte)'Ï'] = 'I';

            accents[(byte)'ó'] = accents[(byte)'ò'] = accents[(byte)'ô'] = accents[(byte)'õ'] = accents[(byte)'ö'] = 'o';
            accents[(byte)'Ó'] = accents[(byte)'Ò'] = accents[(byte)'Ô'] = accents[(byte)'Õ'] = accents[(byte)'Ö'] = 'O';

            accents[(byte)'ú'] = accents[(byte)'ù'] = accents[(byte)'û'] = accents[(byte)'ü'] = 'u';
            accents[(byte)'Ú'] = accents[(byte)'Ù'] = accents[(byte)'Û'] = accents[(byte)'Ü'] = 'U';

            accents[(byte)'ç'] = 'c';
            accents[(byte)'Ç'] = 'C';

            accents[(byte)'ñ'] = 'n';
            accents[(byte)'Ñ'] = 'N';

            accents[(byte)'ÿ'] = accents[(byte)'ý'] = 'y';
            accents[(byte)'Ý'] = 'Y';

            return accents;
        }

        //Custom method to Replace ou/e acentuações of string
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        //Strip HTML (Remover tags html dos campos textarea)
        public static string StripHTML(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = Regex.Replace(str, "<[^>]*>", " ");
            }
            return str;
        }


        //RANDOM STRING :: MIGHT REMOVE
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool IsValidPhoto(string photo)
        {
            string[] allowedPhotoExtensions = { "jpg", "jpeg", "png", "gif" };

            bool IsValidPhoto = false;
            string format = photo.Split('.').Last().ToLower();
            //if(allowedPhotoExtensions.All(format.Contains)) IsValidPhoto = true;
            if (allowedPhotoExtensions.Any(s => format.Contains(s))) IsValidPhoto = true;
            return IsValidPhoto;
        }

        //Currency
        private static readonly Dictionary<string, CultureInfo> ISOCurrenciesToACultureMap =
        CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(c => new { c, new RegionInfo(c.Name).ISOCurrencySymbol })
            .GroupBy(x => x.ISOCurrencySymbol)
            .ToDictionary(g => g.Key, g => g.First().c, StringComparer.OrdinalIgnoreCase);

        public static string FormatCurrency(this decimal amount)
        {
            IFormatProvider FormatProvider = Configs.INST_MDL_GPAG_NOTA_DECIMAL == 1 ?
                new System.Globalization.CultureInfo("en-US") :
                new System.Globalization.CultureInfo("da-DK");
            return amount.ToString("#,##0.00", FormatProvider);
        }


        //##############################
        //NUMERARIO EM EXTENSO PT
        //##############################


        public static string toExtenso(decimal valor)
        {
            // if (valor <= 0 | valor >= 1000000000000000)
            if (valor >= 1000000000000000)
                return "Valor não suportado pelo sistema.";
            else if (valor == 0)
                return "Zero";
            else
            {
                string strValor = valor.ToString("000000000000000.00");
                string valor_por_extenso = string.Empty;
                string a = string.Empty;

                for (int i = 0; i <= 15; i += 3)
                {
                    valor_por_extenso += escreva_parte(Convert.ToDecimal(strValor.Substring(i, 3)));
                    if (i == 0 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                            valor_por_extenso += " Trilhão" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " e " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                            valor_por_extenso += " Trilhões" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " e " : string.Empty);
                    }
                    else if (i == 3 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                            valor_por_extenso += " Bilhão" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " e " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                            valor_por_extenso += " Bilhões" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " e " : string.Empty);
                    }
                    else if (i == 6 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                            valor_por_extenso += " Milhão" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " e " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                            valor_por_extenso += " Milhões" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " e " : string.Empty);
                    }
                    else if (i == 9 & valor_por_extenso != string.Empty)
                        if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                        {
                            if (valor_por_extenso == "Um")
                                valor_por_extenso = "Mil" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " e " : string.Empty);
                            else
                                valor_por_extenso += " Mil" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " e " : string.Empty);
                        }


                    if (i == 12)
                    {
                        if (valor_por_extenso.Length > 8)
                            if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "Bilhão" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "Milhão")
                                valor_por_extenso += " de";
                            else
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "Bilhões" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "Milhões" | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "Trilhões")
                                valor_por_extenso += " de";
                            else
                                    if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "Trilhões")
                                valor_por_extenso += " de";

                        /* if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                             valor_por_extenso += " KWANZA";
                         else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                             valor_por_extenso += " KWANZAS";*/

                        if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                            valor_por_extenso += " e ";
                    }

                    /*if (i == 15)
                        if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                            valor_por_extenso += " CENTIMO";
                        else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                            valor_por_extenso += " CENTIMOS";*/
                }
                return valor_por_extenso;
            }
        }

        static string escreva_parte(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }
                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "Cem" : "Cento";
                else if (a == 2) montagem += "Duzentos";
                else if (a == 3) montagem += "Trezentos";
                else if (a == 4) montagem += "Quatrocentos";
                else if (a == 5) montagem += "Quinhentos";
                else if (a == 6) montagem += "Seiscentos";
                else if (a == 7) montagem += "Setecentos";
                else if (a == 8) montagem += "Oitocentos";
                else if (a == 9) montagem += "Novecentos";

                if (b == 1)
                {
                    if (c == 0) montagem += ((a > 0) ? " e " : string.Empty) + "Dez";
                    else if (c == 1) montagem += ((a > 0) ? " e " : string.Empty) + "Onze";
                    else if (c == 2) montagem += ((a > 0) ? " e " : string.Empty) + "Doze";
                    else if (c == 3) montagem += ((a > 0) ? " e " : string.Empty) + "Treze";
                    else if (c == 4) montagem += ((a > 0) ? " e " : string.Empty) + "Quatorze";
                    else if (c == 5) montagem += ((a > 0) ? " e " : string.Empty) + "Quinze";
                    else if (c == 6) montagem += ((a > 0) ? " e " : string.Empty) + "Dezesseis";
                    else if (c == 7) montagem += ((a > 0) ? " e " : string.Empty) + "Dezessete";
                    else if (c == 8) montagem += ((a > 0) ? " e " : string.Empty) + "Dezoito";
                    else if (c == 9) montagem += ((a > 0) ? " e " : string.Empty) + "Dezenove";
                }
                else if (b == 2) montagem += ((a > 0) ? " e " : string.Empty) + "Vinte";
                else if (b == 3) montagem += ((a > 0) ? " e " : string.Empty) + "Trinta";
                else if (b == 4) montagem += ((a > 0) ? " e " : string.Empty) + "Quarenta";
                else if (b == 5) montagem += ((a > 0) ? " e " : string.Empty) + "Cinquenta";
                else if (b == 6) montagem += ((a > 0) ? " e " : string.Empty) + "Sessenta";
                else if (b == 7) montagem += ((a > 0) ? " e " : string.Empty) + "Setenta";
                else if (b == 8) montagem += ((a > 0) ? " e " : string.Empty) + "Oitenta";
                else if (b == 9) montagem += ((a > 0) ? " e " : string.Empty) + "Noventa";

                if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " e ";

                if (strValor.Substring(1, 1) != "1")


                    if (c == 1) montagem += "Um";
                    else if (c == 2) montagem += "Dois";
                    else if (c == 3) montagem += "Três";
                    else if (c == 4) montagem += "Quatro";
                    else if (c == 5) montagem += "Cinco";
                    else if (c == 6) montagem += "Seis";
                    else if (c == 7) montagem += "Sete";
                    else if (c == 8) montagem += "Oito";
                    else if (c == 9) montagem += "Nove";

                return montagem;
            }
        }

        public static string NotaExclusao(decimal notaexclusao, decimal notaobtida)
        {
            string result = string.Empty;

            if (notaobtida >= notaexclusao)
                result = "Admitido";
            else
                result = "Não Admitido";

            return result;
        }
        public static string LRESPautasFormatColor(string observacao)
        {
            string color = string.Empty;

            if (string.IsNullOrEmpty(observacao))
                color = "#eeede7";
            else if (observacao == "Aprovado" || observacao == "Admitido")
                color = "#EEFFEE";
            else
                color = "#F8E3E3";

            return color;
        }
        public static string GASchedulesColorSchemes(int type)
        {
            string color = "#81ecec"; //todas as semanas

            if (type == 2) //semana impar
                color = "#b8e994";
            else if (type == 3)
                color = "#ffbe76"; //semana par

            return color;
        }
        public static decimal FormatarDecimais(decimal? numeronullable, int casasDecimais, decimal escalaMaxima, decimal escalaMin)
        {
            decimal valor = numeronullable ?? 0;

            if (casasDecimais < 0)
                valor = valor;

            if (casasDecimais == 0)
            {
                // Para manter a parte inteira do número sem arredondamento
                /* if (numero >= 0)
                     numero = Math.Truncate(numero);
                 else
                     numero = Math.Ceiling(numero);*/
            }
            else
            {
                decimal fatorMultiplicativo = (decimal)Math.Pow(10, casasDecimais);
                decimal numeroMultiplicado = valor * fatorMultiplicativo;

                // Verifica se a parte decimal é maior ou igual a 0.5 para arredondar corretamente
                if (numeroMultiplicado - Math.Truncate(numeroMultiplicado) >= 0.5m)
                    valor = Math.Ceiling(numeroMultiplicado) / fatorMultiplicativo;
                else
                    valor = Math.Floor(numeroMultiplicado) / fatorMultiplicativo;
            }

            // Verifica se o número formatado excede a escala máxima
            if (valor > escalaMaxima)
                valor = escalaMaxima;

            if (valor < escalaMin)
                valor = escalaMin;

            return valor;
        }
        /*
        public static decimal FormatarDecimais(decimal numero, int casasDecimais, decimal escalaMaxima, decimal escalaMin)
        {
            if (casasDecimais < 0)
                numero = numero;

            if (casasDecimais == 0)
                numero = Math.Truncate(numero); // Retorna o valor truncado (sem casas decimais)
            else
                numero = Math.Round(numero, casasDecimais); // Retorna o valor arredondado com o número de casas decimais especificado

            // Verifica se o número formatado excede a escala máxima
            if (numero > escalaMaxima)
                numero = escalaMaxima;

            if (numero < escalaMin)
                numero = escalaMin;

            return numero;

        }*/
        public static string CapitalizeFirstLetter(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            char[] charArray = s.ToCharArray();
            charArray[0] = char.ToUpper(charArray[0]);
            return new string(charArray);
        }
        public static string GaPautaEscalaValorDecimais(int Decimal)
        {
            var step = string.Empty;
            switch (Decimal)
            {
                case 0:
                    step = "1";
                    break;
                case 1:
                    step = "0.1";
                    break;
                case 2:
                    step = "0.01";
                    break;
            }
            return step;
        }
        public static string DecodeAnoTipo(int tipo)
        {
            string tiponame = string.Empty;
            switch (tipo)
            {
                case 1: tiponame = "Dias"; break;
                case 2: tiponame = "Semanas"; break;
                case 3: tiponame = "Meses"; break;
                case 4: tiponame = "Anos"; break;
            }
            return tiponame;
        }
        public static string LimitCharacters(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // If text in shorter or equal to length, just return it
            if (text.Length <= length)
            {
                return text;
            }

            // Text is longer, so try to find out where to cut
            char[] delimiters = new char[] { ' ', '.', ',', ':', ';' };
            int index = text.LastIndexOfAny(delimiters, length - 3);

            if (index > (length / 2))
            {
                return text.Substring(0, index) + "...";
            }
            else
            {
                return text.Substring(0, length - 3) + "...";
            }
        }
        public static string GetFirstAndLastName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name; //caso for null
            }

            string[] nomes = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (nomes.Length == 0)
            {
                return name; //caso não tiver nome
            }
            else if (nomes.Length == 1)
            {
                return name; //caso não tiver o outro nome
            }

            string primeiroNome = nomes[0];
            string ultimoNome = nomes[nomes.Length - 1];

            return $"{primeiroNome[0]+"."} {ultimoNome}";
        }
        public static string GetFirstNChar(string entrada, int n)
        {
            if (entrada.Length > n)
            {
                return entrada.Substring(0, n) + "...";
            }
            else
            {
                return entrada;
            }
        }

        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

    }
}