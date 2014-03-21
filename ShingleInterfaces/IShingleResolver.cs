namespace ShingleInterfaces
{
    public interface IShingleResolver
    {
        double CalculateShingles(string firstText, string secondText, int shingleSize);
    }
}
