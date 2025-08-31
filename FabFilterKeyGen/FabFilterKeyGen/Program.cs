namespace FabFilterKeyGen;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        
        string product = "Micro";
        string licensee = "esther";
        string license = KeyGen.GenerateLicense(product, licensee);
        
        Console.WriteLine(license);
        Console.ReadLine();
    }
}