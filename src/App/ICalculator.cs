namespace App
{
    public interface ICalculator
    {
        int Sum(int a, int b);
        int Mult(int a, int b);
    }

    public class Calculator : ICalculator
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }

        public int Mult(int a, int b)
        {
            return a*b;
        }
    }
}
