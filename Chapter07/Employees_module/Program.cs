// See https://aka.ms/new-console-template for more information

string operation = args.Length > 0 ? args[0] : string.Empty;
string name = args.Length > 1 ? args[1] : string.Empty;

switch (operation.ToLower())
{
    case "pay":
        CalculatePay(name);
        break;
    case "basepay":
        SumOfBasePays();
        break;
}

void CalculatePay(string name)
{
    double taxRate = GetTaxRate();
    double pay = Employees.CalculatePay(name, taxRate);
    Console.WriteLine(DescribeResult(name, pay));
}

double GetTaxRate()
{
    Console.Write("Input tax rate: ");
    string? taxRate = Console.ReadLine();
    double.TryParse(taxRate, out double result);
    return result;
}

string DescribeResult(string name, double pay) => $"Name : {name}, Pay : {pay}";

void SumOfBasePays() => Console.WriteLine(Employees.SumOfBasePays());

struct Employees
{
    static string[] employees = new [] { "EmployeeA", "EmployeeB", "EmployeeC", "ParttimeD", "ParttimeE", "ParttimeF" };
    static double[] basePays = new [] { 400, 300, 250, 1, 1, 1.5 };
    static bool[] hourlys = new [] { false, false, false, true, true, true };
    static int[] timeCards = new [] { 0, 0, 0, 120, 120, 120 };

    public static double CalculatePay(string name, double taxRate) => IsHourly(name) ? CalculateHourlyPayFor(name, taxRate) : CalculatePayFor(name, taxRate);
    static bool IsHourly(string name)
    {
        int index = Array.IndexOf<string>(employees, name);
        return index >= 0 ? hourlys[index] : false;
    }

    static double CalculateHourlyPayFor(string name, double taxRate)
    {
        int index = Array.IndexOf<string>(employees, name);
        double basePay = index >= 0 ? basePays[index] * timeCards[index] : 0;
        return basePay - (basePay * taxRate);
    }

    static double CalculatePayFor(string name, double taxRate)
    {
        int index = Array.IndexOf<string>(employees, name);
        double basePay = index >= 0 ? basePays[index] : 0;
        return basePay - (basePay * taxRate);
    }

    public static double SumOfBasePays() => employees.Select((name, index) => IsHourly(name) ? 0 : basePays[index]).Sum();
}