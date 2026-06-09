namespace cafe;

public enum EmploymentType
{
    PartTime,
    FullTime
}

public enum workingStatus
{
    Working,
    NotWorking
}

public enum Diligence
{
    VeryGood,
    Good,
    Normal,
    Bad
}

public abstract class Person : IDisplayable
{
    protected string name;
    protected string phone;

    public string Name => name;
    public string Phone => phone;

    public Person(string name, string phone)
    {
        this.name = name;
        this.phone = phone;
    }

    public abstract string Role();

    public virtual string Info()
    {
        return $"이름: {name}, 전화번호: {phone}, 고객/직원: {Role()}";
    }
}

public class Customer : Person
{
    private int point;

    public int Point => point;

    public Customer(string name, string phone, int point = 0) : base(name, phone)
    {
        this.point = point;
    }

    public void AddPoint(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("적립할 포인트를 다시 입력해주세요.");
        }

        point += amount;
    }

    public bool UsePoint(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("사용할 포인트를 다시 입력해주세요.");
        }

        if (point >= amount)
        {
            point -= amount;
            return true;
        }

        return false;
    }

    public override string Role()
    {
        return "고객";
    }

    public override string Info()
    {
        return base.Info() + $", 포인트: {point}";
    }
}

public class WorkSchedule : IDisplayable
{
    private DayOfWeek day;
    private int startHour;
    private int endHour;

    public DayOfWeek Day => day;
    public int StartHour => startHour;
    public int EndHour => endHour;

    public WorkSchedule(DayOfWeek day, int startHour, int endHour)
    {
        if (startHour < 0 || startHour > 23)
        {
            throw new ArgumentException("시작 시간은 0시부터 23시 사이여야 합니다.");
        }

        if (endHour <= startHour || endHour > 24)
        {
            throw new ArgumentException("종료 시간은 시작 시간보다 크고 24시 이하여야 합니다.");
        }

        this.day = day;
        this.startHour = startHour;
        this.endHour = endHour;
    }

    public bool IsWorkingAt(DateTime dateTime)
    {
        return dateTime.DayOfWeek == day && dateTime.Hour >= startHour && dateTime.Hour < endHour;
    }

    public string Info()
    {
        return $"{DayText()} {startHour:00}:00-{endHour:00}:00";
    }

    private string DayText()
    {
        if (day == DayOfWeek.Monday)
        {
            return "월요일";
        }

        if (day == DayOfWeek.Tuesday)
        {
            return "화요일";
        }

        if (day == DayOfWeek.Wednesday)
        {
            return "수요일";
        }

        if (day == DayOfWeek.Thursday)
        {
            return "목요일";
        }

        if (day == DayOfWeek.Friday)
        {
            return "금요일";
        }

        if (day == DayOfWeek.Saturday)
        {
            return "토요일";
        }

        return "일요일";
    }
}

public abstract class Employee : Person
{
    protected int pay;
    protected EmploymentType employmentType;
    protected Diligence diligence;
    private List<WorkSchedule> schedules;

    public int Pay => pay;
    public EmploymentType EmploymentType => employmentType;
    public Diligence Diligence => diligence;
    public List<WorkSchedule> Schedules => schedules;
    public workingStatus Status => CurrentStatus();

    public Employee(string name, string phone, int pay, EmploymentType employmentType) : base(name, phone)
    {
        this.pay = pay;
        this.employmentType = employmentType;
        diligence = Diligence.Normal;
        schedules = new List<WorkSchedule>();
    }

    public Employee(string name, string phone, int pay, EmploymentType employmentType, Diligence diligence) : base(name, phone)
    {
        this.pay = pay;
        this.employmentType = employmentType;
        this.diligence = diligence;
        schedules = new List<WorkSchedule>();
    }

// 한국어 처리
    public string GetEmploymentType()
    {
        return employmentType == EmploymentType.FullTime ? "정식직원" : "파트타이머";
    }

// 한국어 처리
    protected string GetDiligenceText()
    {
        if (diligence == Diligence.VeryGood)
        {
            return "매우 좋음";
        }

        if (diligence == Diligence.Good)
        {
            return "좋음";
        }

        if (diligence == Diligence.Normal)
        {
            return "보통";
        }

        return "나쁨";
    }

    public void AddSchedule(DayOfWeek day, int startHour, int endHour)
    {
        schedules.Add(new WorkSchedule(day, startHour, endHour));
    }

    public workingStatus CurrentStatus()
    {
        if (IsWorkingAt(DateTime.Now))
        {
            return workingStatus.Working;
        }

        return workingStatus.NotWorking;
    }

    public bool IsWorking()
    {
        return CurrentStatus() == workingStatus.Working;
    }

    public bool IsWorkingAt(DateTime dateTime)
    {
        foreach (var schedule in schedules)
        {
            if (schedule.IsWorkingAt(dateTime))
            {
                return true;
            }
        }

        return false;
    }

    public string GetWorkingStatusText(DateTime dateTime)
    {
        return IsWorkingAt(dateTime) ? "Working" : "NotWorking";
    }

    public string ScheduleText()
    {
        if (schedules.Count == 0)
        {
            return "등록된 근무 일정 없음";
        }

        List<string> result = new();

        foreach (var schedule in schedules)
        {
            result.Add(schedule.Info());
        }

        return string.Join(", ", result);
    }

    public void ChangeDiligence(Diligence diligence)
    {
        this.diligence = diligence;
    }

    public virtual int CalculatePay(int hours)
    {
        return pay * hours;
    }

    public override string Role()
    {
        return "직원";
    }

    public override string Info()
    {
        return base.Info() + $", {GetEmploymentType()}, 시급: {pay}, 성실도: {GetDiligenceText()}, 현재상태: {GetWorkingStatusText(DateTime.Now)}, 근무일정: {ScheduleText()}";
    }
}

public class Barista : Employee
{
    // 성실도 기본
    public Barista(string name, string phone, int pay, EmploymentType employmentType) : base(name, phone, pay, employmentType)
    {
    }

    // 성실도 선택 (Employee 에서 받음)
    public Barista(string name, string phone, int pay, EmploymentType employmentType, Diligence diligence) : base(name, phone, pay, employmentType, diligence)
    {
    }

    public override string Role()
    {
        return "바리스타";
    }
}

public class Manager : Employee
{
    // 성실도 기본
    public Manager(string name, string phone, int monthlySalary, EmploymentType employmentType) : base(name, phone, monthlySalary, employmentType)
    {
    }

    // 성실도 선택 (Employee 에서 받음)
    public Manager(string name, string phone, int monthlySalary, EmploymentType employmentType, Diligence diligence) : base(name, phone, monthlySalary, employmentType, diligence)
    {
    }

    public override string Role()
    {
        return "매니저";
    }

    public override int CalculatePay(int hours)
    {
        return pay / 30; // 매니저는 그냥 30일 나눠서 계산 (월급이라)
    }

    public override string Info()
    {
        return $"이름: {name}, 전화번호: {phone}, 고객/직원: {Role()}, {GetEmploymentType()}, 월급: {pay}, 성실도: {GetDiligenceText()}, 현재상태: {GetWorkingStatusText(DateTime.Now)}, 근무일정: {ScheduleText()}";
    }
}
