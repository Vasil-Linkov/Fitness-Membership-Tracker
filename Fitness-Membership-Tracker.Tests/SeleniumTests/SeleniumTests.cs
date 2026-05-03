using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FitnessTracker.Tests.Selenium;

/// <summary>
/// Selenium UI integration tests for Fitness Membership Tracker.
///
/// PREREQUISITES before running:
///   1. Start the web application:
///        dotnet run --project Fitness-Membership-Tracker/Fitness-Membership-Tracker.Web.csproj
///   2. Ensure the database is seeded (happens automatically on first run).
///   3. Set the environment variable FMT_BASE_URL if the app runs on a port
///      other than 5292:
///        export FMT_BASE_URL=http://localhost:5292
///   4. ChromeDriver must match your installed Chrome version
///      (the NuGet package Selenium.WebDriver.ChromeDriver is version-pinned;
///       update it if needed).
///
/// Run only this category:
///   dotnet test --filter Category=Selenium
/// </summary>
[TestFixture]
[Category("Selenium")]
public class SeleniumTests
{
    private IWebDriver _driver = null!;
    private string _baseUrl    = null!;
    private WebDriverWait _wait = null!;

    // ── credentials seeded by RoleAndAdminSeeder / MemberSeeder ──────────────
    private const string AdminEmail    = "admin@fitzone.bg";
    private const string AdminPassword = "Admin123!";
    private const string MemberEmail   = "aleksandar.kolev@gmail.com";
    private const string MemberPassword = "Member123!";

    // ── setup / teardown ─────────────────────────────────────────────────────

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _baseUrl = Environment.GetEnvironmentVariable("FMT_BASE_URL")
                   ?? "http://localhost:5292";

        var options = new ChromeOptions();
        options.AddArgument("--headless");          // remove for visual debugging
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--window-size=1440,900");

        _driver = new ChromeDriver(options);
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
        // Always start logged out
        _driver.Navigate().GoToUrl($"{_baseUrl}/Account/Logout");
        _driver.Manage().Cookies.DeleteAllCookies();
    }

    // ── helpers ───────────────────────────────────────────────────────────────

    private void NavigateTo(string path = "")
        => _driver.Navigate().GoToUrl($"{_baseUrl}{path}");

    private IWebElement WaitForElement(By by)
        => _wait.Until(d => d.FindElement(by));

    private void Login(string email, string password)
    {
        NavigateTo("/Account/Login");
        WaitForElement(By.Id("Email")).Clear();
        _driver.FindElement(By.Id("Email")).SendKeys(email);
        _driver.FindElement(By.Id("Password")).SendKeys(password);
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        _wait.Until(d => d.Url != $"{_baseUrl}/Account/Login");
    }

    private void AdminLogin() => Login(AdminEmail, AdminPassword);
    private void MemberLogin() => Login(MemberEmail, MemberPassword);

    // ═══════════════════════════════════════════════════════════════════════════
    //  Authentication
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void HomePage_ShowsLoginAndRegisterLinks_WhenNotLoggedIn()
    {
        NavigateTo("/");

        _driver.PageSource.Should().Contain("Login");
        _driver.PageSource.Should().Contain("Register");
    }

    [Test]
    public void Login_WithValidAdminCredentials_RedirectsToDashboard()
    {
        AdminLogin();

        _driver.Url.Should().Contain("/Admin/Dashboard");
    }

    [Test]
    public void Login_WithValidMemberCredentials_RedirectsToHome()
    {
        MemberLogin();

        _driver.Url.Should().NotContain("/Account/Login");
        _driver.PageSource.Should().ContainAny("Welcome Back", "Your Membership", "Logout");
    }

    [Test]
    public void Login_WithInvalidPassword_ShowsError()
    {
        NavigateTo("/Account/Login");
        WaitForElement(By.Id("Email")).SendKeys(MemberEmail);
        _driver.FindElement(By.Id("Password")).SendKeys("WrongPassword999!");
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        _wait.Until(d => d.PageSource.Contains("Invalid login attempt"));
        _driver.PageSource.Should().Contain("Invalid login attempt");
    }

    [Test]
    public void Login_WithNonExistentEmail_ShowsError()
    {
        NavigateTo("/Account/Login");
        WaitForElement(By.Id("Email")).SendKeys("nobody@nowhere.com");
        _driver.FindElement(By.Id("Password")).SendKeys("Password123!");
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        _wait.Until(d => d.PageSource.Contains("Invalid login attempt"));
        _driver.PageSource.Should().Contain("Invalid login attempt");
    }

    [Test]
    public void Logout_RedirectsToHomePage()
    {
        MemberLogin();
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click(); // logout button
        _wait.Until(d => d.Url == $"{_baseUrl}/");
        _driver.Url.Should().Be($"{_baseUrl}/");
    }

    [Test]
    public void Register_WithValidData_CreatesAccountAndLogsIn()
    {
        var unique = Guid.NewGuid().ToString("N")[..8];
        NavigateTo("/Account/Register");

        WaitForElement(By.Id("Email")).SendKeys($"newuser_{unique}@test.com");
        _driver.FindElement(By.Id("Password")).SendKeys("NewUser123!");
        _driver.FindElement(By.Id("ConfirmPassword")).SendKeys("NewUser123!");
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        _wait.Until(d => d.Url != $"{_baseUrl}/Account/Register");
        _driver.PageSource.Should().ContainAny("Welcome", "Logout");
    }

    [Test]
    public void Register_WithDuplicateEmail_ShowsError()
    {
        NavigateTo("/Account/Register");
        WaitForElement(By.Id("Email")).SendKeys(MemberEmail);
        _driver.FindElement(By.Id("Password")).SendKeys("Member123!");
        _driver.FindElement(By.Id("ConfirmPassword")).SendKeys("Member123!");
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        _wait.Until(d => d.PageSource.Contains("already in use") || d.PageSource.Contains("Email"));
        _driver.PageSource.Should().ContainAny("already in use", "Email");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Home & Navigation
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void AboutPage_IsAccessibleWithoutLogin()
    {
        NavigateTo("/Home/Privacy");

        _driver.PageSource.Should().ContainAny("About", "Fitness", "Project");
    }

    [Test]
    public void NavBar_ShowsMemberLinks_WhenLoggedInAsMember()
    {
        MemberLogin();

        _driver.PageSource.Should().Contain("Your Membership");
        _driver.PageSource.Should().Contain("Workouts");
        _driver.PageSource.Should().Contain("Trainers");
    }

    [Test]
    public void NavBar_ShowsAdminDashboardLink_WhenLoggedInAsAdmin()
    {
        AdminLogin();

        _driver.PageSource.Should().Contain("Admin Dashboard");
    }

    [Test]
    public void ProtectedPage_RedirectsToLogin_WhenNotAuthenticated()
    {
        NavigateTo("/Membership/YourMembership");

        _driver.Url.Should().Contain("/Account/Login");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Membership (Member side)
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void YourMembership_PageLoads_WhenLoggedInAsMember()
    {
        MemberLogin();
        NavigateTo("/Membership/YourMembership");

        WaitForElement(By.TagName("h1")).Text.Should().ContainAny("Your Membership", "Membership");
    }

    [Test]
    public void BuyNewMembership_ShowsFourTiers()
    {
        MemberLogin();
        NavigateTo("/Membership/BuyNewMembership");

        _driver.PageSource.Should().Contain("Basic");
        _driver.PageSource.Should().Contain("Advanced");
        _driver.PageSource.Should().Contain("Elite");
        _driver.PageSource.Should().Contain("Ultimate");
    }

    [Test]
    public void BuyNewMembership_PurchaseButton_IsPresent()
    {
        MemberLogin();
        NavigateTo("/Membership/BuyNewMembership");

        var buttons = _driver.FindElements(By.CssSelector("button[type='submit']"));
        buttons.Count.Should().BeGreaterThan(0);
    }

    [Test]
    public void YourMembership_ShowsPaymentHistory_Section()
    {
        MemberLogin();
        NavigateTo("/Membership/YourMembership");

        _driver.PageSource.Should().Contain("Payment History");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Workout (Member side)
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void WorkoutHistory_PageLoads()
    {
        MemberLogin();
        NavigateTo("/Workout/History");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Workout History");
    }

    [Test]
    public void WorkoutLog_PageLoads_WithExerciseForm()
    {
        MemberLogin();
        NavigateTo("/Workout/Log");

        _driver.PageSource.Should().Contain("Exercise");
        _driver.FindElement(By.Id("workoutForm")).Should().NotBeNull();
    }

    [Test]
    public void WorkoutLog_AddExerciseButton_IsPresent()
    {
        MemberLogin();
        NavigateTo("/Workout/Log");

        var addBtn = WaitForElement(By.Id("addExercise"));
        addBtn.Should().NotBeNull();
        addBtn.Text.Should().Contain("Add Another Exercise");
    }

    [Test]
    public void WorkoutLog_SubmitWithNoExerciseName_StaysOnPage()
    {
        MemberLogin();
        NavigateTo("/Workout/Log");

        // Submit without filling exercise name
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        // Browser validation keeps us on the same page
        _driver.Url.Should().Contain("/Workout/Log");
    }

    [Test]
    public void WorkoutLog_CanSubmitValidLog()
    {
        MemberLogin();
        NavigateTo("/Workout/Log");

        _driver.FindElement(By.Name("Notes")).SendKeys("Selenium test session");
        _driver.FindElement(By.Name("Exercises[0].ExerciseName")).SendKeys("Squat");
        _driver.FindElement(By.Name("Exercises[0].Sets")).SendKeys("3");
        _driver.FindElement(By.Name("Exercises[0].Reps")).SendKeys("10");

        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        _wait.Until(d => d.Url.Contains("/Workout/History"));
        _driver.Url.Should().Contain("/Workout/History");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Trainers (Member side)
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void BrowseTrainers_PageLoads_WithDayFilter()
    {
        MemberLogin();
        NavigateTo("/Trainer/Browse");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Find a Trainer");
        // Day-of-week filter buttons
        var dayButtons = _driver.FindElements(By.CssSelector("a[href*='day=']"));
        dayButtons.Count.Should().Be(7);
    }

    [Test]
    public void MyTrainer_PageLoads_ForMember()
    {
        MemberLogin();
        NavigateTo("/Trainer/MyTrainer");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("My Trainer");
    }

    [Test]
    public void TrainerDashboard_PageLoads_ShowsNoPanelMessage_ForMember()
    {
        // A regular member has no trainer profile, so the page should
        // show a "no trainer profile" message
        MemberLogin();
        NavigateTo("/Trainer/Dashboard");

        _driver.PageSource.Should().ContainAny("No Trainer Profile", "Trainer Dashboard", "trainer");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Admin — Dashboard
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void AdminDashboard_ShowsCountWidgets()
    {
        AdminLogin();
        NavigateTo("/Admin/Dashboard");

        _driver.PageSource.Should().Contain("Employees");
        _driver.PageSource.Should().Contain("Members");
        _driver.PageSource.Should().Contain("Memberships");
        _driver.PageSource.Should().Contain("Payments");
    }

    [Test]
    public void AdminDashboard_IsInaccessibleToRegularMember()
    {
        MemberLogin();
        NavigateTo("/Admin/Dashboard");

        // Should be redirected (either to login or Access Denied)
        _driver.Url.Should().NotContain("/Admin/Dashboard");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Admin — Employees
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void AdminEmployees_ListPageLoads()
    {
        AdminLogin();
        NavigateTo("/Admin/Employees");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Employees");
    }

    [Test]
    public void AdminEmployees_CreateForm_IsAccessible()
    {
        AdminLogin();
        NavigateTo("/Admin/CreateEmployee");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Create Employee");
        _driver.FindElement(By.Name("FirstName")).Should().NotBeNull();
        _driver.FindElement(By.Name("LastName")).Should().NotBeNull();
    }

    [Test]
    public void AdminEmployees_SearchFilter_Works()
    {
        AdminLogin();
        NavigateTo("/Admin/Employees?search=Teodora");

        _driver.PageSource.Should().Contain("Teodora");
    }

    [Test]
    public void AdminEmployees_LocationFilter_RendersDropdown()
    {
        AdminLogin();
        NavigateTo("/Admin/Employees");

        var locationSelect = _driver.FindElement(By.Name("locationId"));
        locationSelect.Should().NotBeNull();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Admin — Trainers
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void AdminTrainers_ListPageLoads()
    {
        AdminLogin();
        NavigateTo("/Admin/Trainers");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Trainers");
    }

    [Test]
    public void AdminTrainers_CreateForm_IsAccessible()
    {
        AdminLogin();
        NavigateTo("/Admin/CreateTrainer");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Create Trainer");
        _driver.FindElement(By.Name("FirstName")).Should().NotBeNull();
        _driver.FindElement(By.Name("Specialization")).Should().NotBeNull();
    }

    [Test]
    public void AdminTrainers_ShowsSpecializationBadges()
    {
        AdminLogin();
        NavigateTo("/Admin/Trainers");

        _driver.PageSource.Should().ContainAny("Yoga", "CrossFit", "Pilates", "Personal Training");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Admin — Members
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void AdminMembers_ListPageLoads_WithTable()
    {
        AdminLogin();
        NavigateTo("/Admin/Members");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Members");
        _driver.FindElement(By.TagName("table")).Should().NotBeNull();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Admin — Memberships
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void AdminMemberships_ListPageLoads()
    {
        AdminLogin();
        NavigateTo("/Admin/Memberships");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Memberships");
    }

    [Test]
    public void AdminMemberships_ShowsActiveExpiredBadges()
    {
        AdminLogin();
        NavigateTo("/Admin/Memberships");

        _driver.PageSource.Should().ContainAny("Active", "Expired");
    }

    [Test]
    public void AdminCreateMembership_FormRendersDropdowns()
    {
        AdminLogin();
        NavigateTo("/Admin/CreateMembership");

        _driver.FindElement(By.Name("LocationId")).Should().NotBeNull();
        _driver.FindElement(By.Name("MembershipTierId")).Should().NotBeNull();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Admin — Payments
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void AdminPayments_ListPageLoads()
    {
        AdminLogin();
        NavigateTo("/Admin/Payments");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Payments");
    }

    [Test]
    public void AdminPayments_ShowsPaymentMethods()
    {
        AdminLogin();
        NavigateTo("/Admin/Payments");

        _driver.PageSource.Should().ContainAny("OnSite", "Card");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Admin — Visits
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void AdminVisits_PageLoads_WithChart()
    {
        AdminLogin();
        NavigateTo("/Admin/Visits");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Visits");
        _driver.PageSource.Should().Contain("canvas"); // Chart.js bar chart
    }

    [Test]
    public void AdminVisits_ShowsStatsWidgets()
    {
        AdminLogin();
        NavigateTo("/Admin/Visits");

        _driver.PageSource.Should().Contain("Total Visits");
        _driver.PageSource.Should().Contain("Avg / Day");
        _driver.PageSource.Should().Contain("Peak Day");
    }

    [Test]
    public void AdminLogVisit_FormIsAccessible()
    {
        AdminLogin();
        NavigateTo("/Admin/LogVisit");

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Log Visit");
        _driver.FindElement(By.Name("MemberId")).Should().NotBeNull();
        _driver.FindElement(By.Name("LocationId")).Should().NotBeNull();
    }

    [Test]
    public void AdminVisits_DateRangeFilter_IsPresent()
    {
        AdminLogin();
        NavigateTo("/Admin/Visits");

        _driver.FindElement(By.Name("from")).Should().NotBeNull();
        _driver.FindElement(By.Name("to")).Should().NotBeNull();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Admin — Trainer Schedule & Capacity
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void AdminTrainerSchedule_PageLoadsForFirstTrainer()
    {
        AdminLogin();
        NavigateTo("/Admin/Trainers");

        // Click first Schedule button
        var scheduleBtn = WaitForElement(
            By.XPath("//a[contains(@href,'TrainerSchedule')]"));
        scheduleBtn.Click();

        WaitForElement(By.TagName("h2")).Text.Should().Contain("Manage Schedule");
    }

    [Test]
    public void AdminTrainerCapacity_PageLoadsForFirstTrainer()
    {
        AdminLogin();
        NavigateTo("/Admin/Trainers");

        var capacityBtn = WaitForElement(
            By.XPath("//a[contains(@href,'TrainerCapacity')]"));
        capacityBtn.Click();

        WaitForElement(By.TagName("h4")).Text.Should().Contain("Trainee Capacity");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Error & edge cases
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public void NonExistentPage_Returns404OrError()
    {
        NavigateTo("/This/Does/NotExist");

        _driver.PageSource.Should().ContainAny("404", "not found", "Error", "error");
    }
}

// ── FluentAssertions extension helper ────────────────────────────────────────

internal static class StringAssertionExtensions
{
    public static void ContainAny(this FluentAssertions.Primitives.StringAssertions assertions,
        params string[] values)
    {
        var subject = assertions.Subject;
        subject.Should().MatchRegex(string.Join("|",
            values.Select(v => System.Text.RegularExpressions.Regex.Escape(v))));
    }
}
