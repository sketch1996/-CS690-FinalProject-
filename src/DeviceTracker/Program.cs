using DeviceTracker.Data;
using DeviceTracker.Services;
using DeviceTracker.UI;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var store = new DataStore();
var deviceService = new DeviceService(store);
var issueService = new IssueService(store);
var assignmentService = new AssignmentService(store);
var reportService = new ReportService(store);

var menu = new MenuHandler(deviceService, issueService, assignmentService, reportService);

Console.Clear();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(@"
  ██████╗ ███████╗██╗   ██╗██╗ ██████╗███████╗    ████████╗██████╗  █████╗  ██████╗██╗  ██╗███████╗██████╗
  ██╔══██╗██╔════╝██║   ██║██║██╔════╝██╔════╝    ╚══██╔══╝██╔══██╗██╔══██╗██╔════╝██║ ██╔╝██╔════╝██╔══██╗
  ██║  ██║█████╗  ██║   ██║██║██║     █████╗         ██║   ██████╔╝███████║██║     █████╔╝ █████╗  ██████╔╝
  ██║  ██║██╔══╝  ╚██╗ ██╔╝██║██║     ██╔══╝         ██║   ██╔══██╗██╔══██║██║     ██╔═██╗ ██╔══╝  ██╔══██╗
  ██████╔╝███████╗ ╚████╔╝ ██║╚██████╗███████╗        ██║   ██║  ██║██║  ██║╚██████╗██║  ██╗███████╗██║  ██║
  ╚═════╝ ╚══════╝  ╚═══╝  ╚═╝ ╚═════╝╚══════╝        ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝
                                           Data Center Device Management
");
Console.ResetColor();
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("  Loading data...");
Console.ResetColor();
Thread.Sleep(600);

menu.ShowMainMenu();

Console.Clear();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("\n  Goodbye! Data saved.\n");
Console.ResetColor();
