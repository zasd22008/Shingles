using System;
using System.Collections.Generic;
using System.Linq;

namespace Shingles.Shingle
{
    public class MegaShingleResolver:SuperShingleResolver
    {
        public override double CalculateShingles(string firstText, string secondText, int shingleSize)
        {
            var hashes1 = GetMegaShingles(firstText, shingleSize);
            var hashes2 = GetMegaShingles(secondText, shingleSize);

            return compare(hashes1, hashes2);
        }

        private List<string> GetMegaShingles(string text, int shingleSize)
        {
            var superShingles = GetSuperShingles(text, shingleSize).OrderBy(s => s).ToList();

            var megaShingles = new List<string>();

            // размер мега шингла всегда 2 и точка

            for (int i = 0; i < superShinglesCount - 1; i++)
            {
                for (int j = i; j < superShinglesCount; j++)
                {
                    megaShingles.Add(GetHash(String.Join(" ", superShingles[i], superShingles[j]), hash_functions.First()));        
                }
            }

            return megaShingles;
        }
    }
}
