using System;

namespace Shingles
{
    public interface IShingleResolver
    {
        double CalculateShingles(string firstText, string secondText, int shingleSize);
    }

    public class ShingleFactory
    {
        public IShingleResolver GetShingleResolver(bool isShingle, bool isSuperShingle, bool isMegaShingle)
        {
            if (isShingle)
                return new ShingleResolver();

            if (isSuperShingle)
                return new SuperShingleResolver();

            if (isMegaShingle)
                return new MegaShingleResolver();

            throw new Exception("ShingleFactory: class not found");
        }
    }
}
