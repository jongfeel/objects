// See https://aka.ms/new-console-template for more information

string[] employees = new [] { "EmployeeA", "EmployeeB", "EmployeeC", "ParttimeD", "ParttimeE", "ParttimeF" };
double[] basePays = new [] { 400, 300, 250, 1, 1, 1.5 };
bool[] hourlys = new [] { false, false, false, true, true, true };
int[] timeCards = new [] { 0, 0, 0, 120, 120, 120 };

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
    double pay = IsHourly(name) ? CalculateHourlyPayFor(name, taxRate) : CalculatePayFor(name, taxRate);
    Console.WriteLine(DescribeResult(name, pay));
}

bool IsHourly(string name)
{
    int index = Array.IndexOf<string>(employees, name);
    return index >= 0 ? hourlys[index] : false;
}

double CalculateHourlyPayFor(string name, double taxRate)
{
    int index = Array.IndexOf<string>(employees, name);
    double basePay = index >= 0 ? basePays[index] * timeCards[index] : 0;
    return basePay - (basePay * taxRate);
}

double GetTaxRate()
{
    Console.Write("Input tax rate: ");
    string? taxRate = Console.ReadLine();
    double.TryParse(taxRate, out double result);
    return result;
}

double CalculatePayFor(string name, double taxRate)
{
    int index = Array.IndexOf<string>(employees, name);
    double basePay = index >= 0 ? basePays[index] : 0;
    return basePay - (basePay * taxRate);
}

string DescribeResult(string name, double pay) => $"Name : {name}, Pay : {pay}";

void SumOfBasePays() => Console.WriteLine(basePays.Sum());