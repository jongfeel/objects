// See https://aka.ms/new-console-template for more information

string[] employees = new [] { "EmployeeA", "EmployeeB", "EmployeeC" };
int[] basePays = new [] { 400, 300, 250 };

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
    float taxRate = GetTaxRate();
    float pay = CalculatePayFor(name, taxRate);
    Console.WriteLine(DescribeResult(name, pay));
}

float GetTaxRate()
{
    Console.Write("Input tax rate: ");
    string? taxRate = Console.ReadLine();
    float.TryParse(taxRate, out float result);
    return result;
}

float CalculatePayFor(string name, float taxRate)
{
    int index = Array.IndexOf<string>(employees, name);
    int basePay = index >= 0 ? basePays[index] : 0;
    return basePay - (basePay * taxRate);
}

string DescribeResult(string name, float pay) => $"Name : {name}, Pay : {pay}";

void SumOfBasePays() => Console.WriteLine(basePays.Sum());