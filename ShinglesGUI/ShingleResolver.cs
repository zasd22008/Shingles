using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Shingles
{
    public class ShingleResolver:IShingleResolver
    {
        public double Calculate(string firstText, string secondText, int shingleSize)
        {

            var shingles1 = GenerateShingles(Canonize(firstText), shingleSize);
            var shingles2 = GenerateShingles(Canonize(secondText), shingleSize);

            return compare(shingles1, shingles2);
        }

        private IList<string> Canonize(string text)
        {
            var words = text.Split(' ').Select(w => w.Trim(' ', '\t')).Where(w => !String.IsNullOrWhiteSpace(w));

            var stopSymbols = new[] {".", ",", "!", "?", ":", ";", "-", "\n", "\r", "(", ")"};

            var stopWords = new[]
            {
                "это", "как", "так", "и", "в", "над",
                "к", "до", "не", "на", "но", "за",
                "то", "с", "ли",
                "а", "во", "от",
                "со", "для", "о",
                "же", "ну", "вы",
                "бы", "что", "кто",
                "он", "она"
            };

            return words.Where(w => !stopWords.Contains(w) && !stopSymbols.Contains(w)).ToList();
        }

        private IList<string> GenerateShingles(IList<string> words, int shingleSize)
        {
            var length = words.Count - shingleSize + 1;

            if (length <= 0)
                throw new InSufficientTextLengthException("Передан текст недостаточной длины для размера шингла");

            var shingles = new string[length];

            for (int i = 0; i < shingles.Length; i++)
                shingles[i] = getMD5Hash(String.Join(" ", getSubArray(words, i, shingleSize)));

            return shingles;
        }

        private string getMD5Hash(string str)
        {
            var md5Hasher = MD5.Create();
 
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
 
            var builder = new StringBuilder();
 
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(i.ToString("x2"));
            }

            return builder.ToString();
        }

        private IEnumerable<string> getSubArray(IList<string> words, int startIndex, int size)
        {
            var selected = new string[size];

            for (int i = 0; i < size; i++)
                selected[i] = words[i + startIndex];

            return selected;
        }

        private double compare(IList<string> shingles1, IList<string> shingles2)
        {
            int sameCount = shingles1.Count(shingles2.Contains);

            return sameCount * 2/(double)(shingles1.Count + shingles2.Count);
        }
    }
}
