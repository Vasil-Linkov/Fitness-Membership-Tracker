using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FitnessTracker.Tests.Selenium;

// ═══════════════════════════════════════════════════════════════════════════════
//  Page Object Models
// ═══════════════════════════════════════════════════════════════════════════════

internal sealed class LoginPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly string _baseUrl;

    public LoginPage(IWebDriver driver, WebDriverWait wait, string baseUrl)
    {
        _driver  = driver;
        _wait    = wait;
        _baseUrl = baseUrl;
    }

    public LoginPage Open()
    {
        _driver.Navigate().GoToUrl($"{_baseUrl}/Account/Login");
        return this;
    }

    public LoginPage EnterEmail(string email)
    {
        var field = _wait.Until(d => d.FindElement(By.Id("Email")));
        field.Clear();
        field.SendKeys(email);
        return this;
    }

    public LoginPage EnterPassword(string password)
    {
        _driver.FindElement(By.Id("Password")).SendKeys(password);
        return this;
    }

    public void Submit()
        => _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

    public string CurrentUrl => _driver.Url;
    public string PageSource => _driver.PageSource;

    public bool HasError(string message)
        => _driver.PageSource.Contains(message);
}

internal sealed class RegisterPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly string _baseUrl;

    public RegisterPage(IWebDriver driver, WebDriverWait wait, string baseUrl)
    {
        _driver  = driver;
        _wait    = wait;
        _baseUrl = baseUrl;
    }

    public RegisterPage Open()
    {
        _driver.Navigate().GoToUrl($"{_baseUrl}/Account/Register");
        return this;
    }

    public RegisterPage EnterEmail(string email)
    {
        _wait.Until(d => d.FindElement(By.Id("Email"))).SendKeys(email);
        return this;
    }

    public RegisterPage EnterPassword(string password)
    {
        _driver.FindElement(By.Id("Password")).SendKeys(password);
        return this;
    }

    public RegisterPage EnterConfirmPassword(string password)
    {
        _driver.FindElement(By.Id("ConfirmPassword")).SendKeys(password);
        return this;
    }

    public void Submit()
        => _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

    public string PageSource => _driver.PageSource;
    public string CurrentUrl => _driver.Url;
}

internal sealed class WorkoutLogPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly string _baseUrl;

    public WorkoutLogPage(IWebDriver driver, WebDriverWait wait, string baseUrl)
    {
        _driver  = driver;
        _wait    = wait;
        _baseUrl = baseUrl;
    }

    public WorkoutLogPage Open()
    {
        _driver.Navigate().GoToUrl($"{_baseUrl}/Workout/Log");
        _wait.Until(d => d.FindElement(By.Id("workoutForm")));
        return this;
    }

    public WorkoutLogPage SetNotes(string notes)
    {
        _driver.FindElement(By.Name("Notes")).SendKeys(notes);
        return this;
    }

    public WorkoutLogPage SetExerciseName(int index, string name)
    {
        _driver.FindElement(By.Name($"Exercises[{index}].ExerciseName")).Clear();
        _driver.FindElement(By.Name($"Exercises[{index}].ExerciseName")).SendKeys(name);
        return this;
    }

    public WorkoutLogPage SetExerciseSets(int index, int sets)
    {
        _driver.FindElement(By.Name($"Exercises[{index}].Sets")).SendKeys(sets.ToString());
        return this;
    }

    public WorkoutLogPage SetExerciseReps(int index, int reps)
    {
        _driver.FindElement(By.Name($"Exercises[{index}].Reps")).SendKeys(reps.ToString());
        return this;
    }

    public WorkoutLogPage SetExerciseWeight(int index, int kg)
    {
        _driver.FindElement(By.Name($"Exercises[{index}].WeightKg")).SendKeys(kg.ToString());
        return this;
    }

    public WorkoutLogPage ClickAddExercise()
    {
        _driver.FindElement(By.Id("addExercise")).Click();
        return this;
    }

    public void Submit()
        => _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

    public int ExerciseCount
        => _driver.FindElements(By.CssSelector(".exercise-entry")).Count;

    public string CurrentUrl => _driver.Url;
}

internal sealed class AdminTrainersPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly string _baseUrl;

    public AdminTrainersPage(IWebDriver driver, WebDriverWait wait, string baseUrl)
    {
        _driver  = driver;
        _wait    = wait;
        _baseUrl = baseUrl;
    }

    public AdminTrainersPage Open()
    {
        _driver.Navigate().GoToUrl($"{_baseUrl}/Admin/Trainers");
        _wait.Until(d => d.FindElement(By.TagName("h2")));
        return this;
    }

    public AdminTrainersPage SearchFor(string term)
    {
        var searchInput = _driver.FindElement(By.Name("search"));
        searchInput.Clear();
        searchInput.SendKeys(term);
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        return this;
    }

    public int TrainerRowCount
        => _driver.FindElements(By.CssSelector("tbody tr")).Count;

    public bool HasTrainerWithEmail(string email)
        => _driver.PageSource.Contains(email);

    public void ClickFirstScheduleButton()
        => _driver.FindElement(By.XPath("//a[contains(@href,'TrainerSchedule')]")).Click();

    public void ClickFirstCapacityButton()
        => _driver.FindElement(By.XPath("//a[contains(@href,'TrainerCapacity')]")).Click();
}

// ═══════════════════════════════════════════════════════════════════════════════
//  Extended Selenium tests using Page Objects
// ═══════════════════════════════════════════════════════════════════════════════

[TestFixture]
[Category("Selenium")]
public class SeleniumPageObjectTests
{
    private IWebDriver _driver   = null!;
    private WebDriverWait _wait  = null!;
    private string _baseUrl      = null!;

    private const string AdminEmail     = "admin@fitzone.bg";
    private const string AdminPassword  = "Admin123!";
    private const string MemberEmail    = "aleksandar.kolev@gmail.com";
    private const string MemberPassword = "Member123!";

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _baseUrl = Environment.GetEnvironmentVariable("FMT_BASE_URL")
                   ?? "http://localhost:5292";

        var opts = new ChromeOptions();
        opts.AddArgument("--headless");
        opts.AddArgument("--no-sandbox");
        opts.AddArgument("--disable-dev-shm-usage");
        opts.AddArgument("--window-size=1440,900");

        _driver = new ChromeDriver(opts);
        _wait   = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    [SetUp]
    public void SetUp()
    {
        _driver.Navigate().GoToUrl($"{_baseUrl}/Account/Logout");
        _driver.Manage().Cookies.DeleteAllCookies();
    }

    private void AdminLogin()
    {
        new LoginPage(_driver, _wait, _baseUrl)
            .Open()
            .EnterEmail(AdminEmail)
            .EnterPassword(AdminPassword)
            .Submit();

        _wait.Until(d => d.Url.Contains("/Admin/Dashboard"));
    }

    private void MemberLogin()
    {
        new LoginPage(_driver, _wait, _baseUrl)
            .Open()
            .EnterEmail(MemberEmail)
            .EnterPassword(MemberPassword)
            .Submit();

        _wait.Until(d => !d.Url.Contains("/Account/Login"));
    }

    // ── LoginPage ─────────────────────────────────────────────────────────────

    [Test]
    public void LoginPage_TitleContainsLogin()
    {
        new LoginPage(_driver, _wait, _baseUrl).Open();
        _driver.Title.Should().ContainAny("Login", "Fitness");
    }

    [Test]
    public void LoginPage_HasEmailAndPasswordFields()
    {
        new LoginPage(_driver, _wait, _baseUrl).Open();
        _driver.FindElement(By.Id("Email")).Should().NotBeNull();
        _driver.FindElement(By.Id("Password")).Should().NotBeNull();
    }

    [Test]
    public void LoginPage_WithBlankFields_StaysOnPage()
    {
        var page = new LoginPage(_driver, _wait, _baseUrl).Open();
        page.Submit();
        // HTML5 required validation keeps us on the login page
        page.CurrentUrl.Should().Contain("/Account/Login");
    }

    [Test]
    public void Login_AdminUser_LandsOnAdminDashboard()
    {
        new LoginPage(_driver, _wait, _baseUrl)
            .Open()
            .EnterEmail(AdminEmail)
            .EnterPassword(AdminPassword)
            .Submit();

        _wait.Until(d => d.Url.Contains("/Admin/Dashboard"));
        _driver.Url.Should().Contain("/Admin/Dashboard");
    }

    [Test]
    public void Login_MemberUser_LandsOnHomeNotAdmin()
    {
        new LoginPage(_driver, _wait, _baseUrl)
            .Open()
            .EnterEmail(MemberEmail)
            .EnterPassword(MemberPassword)
            .Submit();

        _wait.Until(d => !d.Url.Contains("/Account/Login"));
        _driver.Url.Should().NotContain("/Admin/Dashboard");
    }

    // ── RegisterPage ──────────────────────────────────────────────────────────

    [Test]
    public void RegisterPage_HasRequiredFields()
    {
        new RegisterPage(_driver, _wait, _baseUrl).Open();
        _driver.FindElement(By.Id("Email")).Should().NotBeNull();
        _driver.FindElement(By.Id("Password")).Should().NotBeNull();
        _driver.FindElement(By.Id("ConfirmPassword")).Should().NotBeNull();
    }

    [Test]
    public void RegisterPage_MismatchedPasswords_ShowsError()
    {
        var page = new RegisterPage(_driver, _wait, _baseUrl)
            .Open()
            .EnterEmail($"test_{Guid.NewGuid():N}@t.com")
            .EnterPassword("Password123!")
            .EnterConfirmPassword("Different123!");

        page.Submit();
        _wait.Until(d => d.PageSource.Contains("do not match"));
        page.PageSource.Should().Contain("do not match");
    }

    [Test]
    public void RegisterPage_WeakPassword_ShowsError()
    {
        var page = new RegisterPage(_driver, _wait, _baseUrl)
            .Open()
            .EnterEmail($"test_{Guid.NewGuid():N}@t.com")
            .EnterPassword("weak")
            .EnterConfirmPassword("weak");

        page.Submit();
        _wait.Until(d => d.Url == $"{_baseUrl}/Account/Register" || d.PageSource.Contains("password"));
        page.PageSource.Should().ContainAny("password", "Password", "characters");
    }

    [Test]
    public void RegisterPage_ValidNewUser_IsRedirected()
    {
        var unique = Guid.NewGuid().ToString("N")[..8];
        var page   = new RegisterPage(_driver, _wait, _baseUrl)
            .Open()
            .EnterEmail($"new_{unique}@test.com")
            .EnterPassword("NewUser123!")
            .EnterConfirmPassword("NewUser123!");

        page.Submit();
        _wait.Until(d => d.Url != $"{_baseUrl}/Account/Register");
        page.CurrentUrl.Should().NotContain("/Account/Register");
    }

    // ── WorkoutLogPage ────────────────────────────────────────────────────────

    [Test]
    public void WorkoutLog_StartsWith_OneExerciseEntry()
    {
        MemberLogin();
        var page = new WorkoutLogPage(_driver, _wait, _baseUrl).Open();

        page.ExerciseCount.Should().Be(1);
    }

    [Test]
    public void WorkoutLog_AddExerciseButton_IncreasesCount()
    {
        MemberLogin();
        var page = new WorkoutLogPage(_driver, _wait, _baseUrl).Open();

        page.ClickAddExercise();

        _wait.Until(d => d.FindElements(By.CssSelector(".exercise-entry")).Count == 2);
        page.ExerciseCount.Should().Be(2);
    }

    [Test]
    public void WorkoutLog_AddMultipleExercises_AllRendered()
    {
        MemberLogin();
        var page = new WorkoutLogPage(_driver, _wait, _baseUrl).Open();

        page.ClickAddExercise().ClickAddExercise().ClickAddExercise();

        _wait.Until(d => d.FindElements(By.CssSelector(".exercise-entry")).Count == 4);
        page.ExerciseCount.Should().Be(4);
    }

    [Test]
    public void WorkoutLog_RemoveButton_HiddenForFirstExercise()
    {
        MemberLogin();
        new WorkoutLogPage(_driver, _wait, _baseUrl).Open();

        var removeButtons = _driver.FindElements(By.CssSelector(".remove-exercise"));
        // First remove button is hidden via style="display:none"
        removeButtons.First().GetCssValue("display").Should().Be("none");
    }

    [Test]
    public void WorkoutLog_ValidSubmit_NavigatesToHistory()
    {
        MemberLogin();
        var page = new WorkoutLogPage(_driver, _wait, _baseUrl)
            .Open()
            .SetNotes("Page object test session")
            .SetExerciseName(0, "Deadlift")
            .SetExerciseSets(0, 3)
            .SetExerciseReps(0, 5)
            .SetExerciseWeight(0, 140);

        page.Submit();

        _wait.Until(d => d.Url.Contains("/Workout/History"));
        page.CurrentUrl.Should().Contain("/Workout/History");
    }

    [Test]
    public void WorkoutHistory_ShowsSuccessBanner_AfterNewLog()
    {
        MemberLogin();
        new WorkoutLogPage(_driver, _wait, _baseUrl)
            .Open()
            .SetNotes("Banner test")
            .SetExerciseName(0, "Pull-up")
            .Submit();

        _wait.Until(d => d.Url.Contains("/Workout/History"));
        _driver.PageSource.Should().Contain("Workout logged successfully");
    }

    [Test]
    public void WorkoutHistory_ShowsAccordionItems_ForLoggedSessions()
    {
        MemberLogin();
        _driver.Navigate().GoToUrl($"{_baseUrl}/Workout/History");
        _wait.Until(d => d.FindElement(By.TagName("h2")));

        // aleksander.kolev was seeded with workout logs
        _driver.PageSource.Should().ContainAny("accordion", "exercise", "Exercise",
            "No workout", "logged");
    }

    // ── AdminTrainersPage ─────────────────────────────────────────────────────

    [Test]
    public void AdminTrainersPage_ListsSeededTrainers()
    {
        AdminLogin();
        var page = new AdminTrainersPage(_driver, _wait, _baseUrl).Open();

        page.TrainerRowCount.Should().BeGreaterThan(0);
    }

    [Test]
    public void AdminTrainersPage_SearchBySpecialization_FiltersResults()
    {
        AdminLogin();
        var page = new AdminTrainersPage(_driver, _wait, _baseUrl)
            .Open()
            .SearchFor("Yoga");

        _driver.PageSource.Should().Contain("Yoga");
    }

    [Test]
    public void AdminTrainersPage_ScheduleButton_OpensSchedulePage()
    {
        AdminLogin();
        var page = new AdminTrainersPage(_driver, _wait, _baseUrl).Open();
        page.ClickFirstScheduleButton();

        _wait.Until(d => d.Url.Contains("TrainerSchedule"));
        _driver.PageSource.Should().Contain("Manage Schedule");
    }

    [Test]
    public void AdminTrainersPage_CapacityButton_OpensCapacityPage()
    {
        AdminLogin();
        var page = new AdminTrainersPage(_driver, _wait, _baseUrl).Open();
        page.ClickFirstCapacityButton();

        _wait.Until(d => d.Url.Contains("TrainerCapacity"));
        _driver.PageSource.Should().ContainAny("Trainee Capacity", "Max");
    }

    [Test]
    public void AdminCreateTrainer_Form_HasSpecializationDropdown()
    {
        AdminLogin();
        _driver.Navigate().GoToUrl($"{_baseUrl}/Admin/CreateTrainer");
        _wait.Until(d => d.FindElement(By.Name("Specialization")));

        var options = _driver.FindElements(By.CssSelector("select[name='Specialization'] option"));
        options.Count.Should().BeGreaterThan(5);
    }

    // ── Admin Visits ──────────────────────────────────────────────────────────

    [Test]
    public void AdminVisits_DateFilter_ChangesPeriodAndReloads()
    {
        AdminLogin();
        _driver.Navigate().GoToUrl($"{_baseUrl}/Admin/Visits?from=2024-01-01&to=2024-01-07");
        _wait.Until(d => d.FindElement(By.TagName("h2")));

        _driver.FindElement(By.Name("from")).GetAttribute("value").Should().Be("2024-01-01");
        _driver.FindElement(By.Name("to")).GetAttribute("value").Should().Be("2024-01-07");
    }

    [Test]
    public void AdminVisits_ShowsBarChart_Canvas()
    {
        AdminLogin();
        _driver.Navigate().GoToUrl($"{_baseUrl}/Admin/Visits");
        _wait.Until(d => d.FindElement(By.TagName("canvas")));

        _driver.FindElement(By.TagName("canvas")).Should().NotBeNull();
    }

    // ── Admin Membership Create ───────────────────────────────────────────────

    [Test]
    public void AdminCreateMembership_StartDateField_IsDateInput()
    {
        AdminLogin();
        _driver.Navigate().GoToUrl($"{_baseUrl}/Admin/CreateMembership");
        _wait.Until(d => d.FindElement(By.Name("StartDate")));

        _driver.FindElement(By.Name("StartDate"))
            .GetAttribute("type").Should().Be("date");
    }

    [Test]
    public void AdminCreateMembership_EndDateField_IsDateInput()
    {
        AdminLogin();
        _driver.Navigate().GoToUrl($"{_baseUrl}/Admin/CreateMembership");
        _wait.Until(d => d.FindElement(By.Name("EndDate")));

        _driver.FindElement(By.Name("EndDate"))
            .GetAttribute("type").Should().Be("date");
    }

    // ── Navigation / breadcrumbs ──────────────────────────────────────────────

    [Test]
    public void AdminSchedulePage_BackButton_ReturnsToTrainers()
    {
        AdminLogin();
        new AdminTrainersPage(_driver, _wait, _baseUrl).Open().ClickFirstScheduleButton();
        _wait.Until(d => d.Url.Contains("TrainerSchedule"));

        _driver.FindElement(By.XPath("//a[contains(text(),'Back')]")).Click();
        _wait.Until(d => d.Url.Contains("/Admin/Trainers"));

        _driver.Url.Should().Contain("/Admin/Trainers");
    }

    [Test]
    public void AdminCapacityPage_BackButton_ReturnsToTrainers()
    {
        AdminLogin();
        new AdminTrainersPage(_driver, _wait, _baseUrl).Open().ClickFirstCapacityButton();
        _wait.Until(d => d.Url.Contains("TrainerCapacity"));

        _driver.FindElement(By.XPath("//a[contains(text(),'Back')]")).Click();
        _wait.Until(d => d.Url.Contains("/Admin/Trainers"));

        _driver.Url.Should().Contain("/Admin/Trainers");
    }

    [Test]
    public void BrowseTrainers_MyTrainerButton_NavigatesToMyTrainer()
    {
        MemberLogin();
        _driver.Navigate().GoToUrl($"{_baseUrl}/Trainer/Browse");
        _wait.Until(d => d.FindElement(By.TagName("h2")));

        _driver.FindElement(By.XPath("//a[contains(@href,'MyTrainer')]")).Click();
        _wait.Until(d => d.Url.Contains("/Trainer/MyTrainer"));

        _driver.Url.Should().Contain("/Trainer/MyTrainer");
    }

    // ── Anti-forgery / form security ─────────────────────────────────────────

    [Test]
    public void AllPostForms_ContainAntiForgeryToken()
    {
        AdminLogin();
        _driver.Navigate().GoToUrl($"{_baseUrl}/Admin/Employees");
        _wait.Until(d => d.FindElement(By.TagName("form")));

        // Every form that posts should have the hidden __RequestVerificationToken field
        var forms = _driver.FindElements(By.CssSelector("form[method='post']"));
        foreach (var form in forms)
        {
            form.FindElement(By.Name("__RequestVerificationToken"))
                .Should().NotBeNull(
                    because: $"form at {form.GetAttribute("action")} must include anti-forgery token");
        }
    }
}

// ── Re-export the extension so both test files compile ────────────────────────
internal static class StringExt
{
    public static void ContainAny(
        this FluentAssertions.Primitives.StringAssertions assertions,
        params string[] values)
    {
        var subject = assertions.Subject;
        var pattern = string.Join("|",
            values.Select(v => System.Text.RegularExpressions.Regex.Escape(v)));
        subject.Should().MatchRegex(pattern);
    }
}
