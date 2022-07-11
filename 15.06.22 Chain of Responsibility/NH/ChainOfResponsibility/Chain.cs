namespace ChainOfResponsibility
{
    public class Chain
    {
        public Employee Employee { get; init; }
        public FinanceEmployee FinanceEmployee { get; init; }
        public Director Director { get; init; }
        public CFO CFO { get; init; }
        public CEO CEO { get; init; }

        public Chain()
        {
            CEO = new CEO(null);
            CFO = new CFO(CEO);
            Director = new Director(CFO);
            FinanceEmployee = new FinanceEmployee(Director);
            Employee = new Employee(FinanceEmployee);
        }
    }
}
