#tool "xunit.runner.console&version=2.3.1"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var _target = Argument<string>("target", "Build");
var _configuration = Argument<string>("configuration", "Release");
var _verbosity = Argument<Cake.Core.Diagnostics.Verbosity>("verbosity", 0);
var _version = Argument<string>("packVersion", "0.0.1");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var _solutions = GetFiles("./**/*.sln");
var _solutionPaths = _solutions.Select(solution => solution.GetDirectory());

// TESTS
//string _unitTestsProjGlob = "./Tests/**/bin/" + _configuration + "/**/*.Tests.Unit.dll";

// BUILD
//var _artifactsDir = Directory("./artifacts");

///////////////////////////////////////////////////////////////////////////////
// Setup
///////////////////////////////////////////////////////////////////////////////
Setup(ctx =>
{
	var buildVersion = EnvironmentVariable("BUILD_NUMBER");
	if(string.IsNullOrWhiteSpace(buildVersion) == false)
	{
		_version = buildVersion;
	}
});

///////////////////////////////////////////////////////////////////////////////
// TARGETS TO DO THE WORK
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Description("Cleans all directories that are used during the build process.")
    .Does(() =>
{
    // Clean solution directories.
    foreach(var path in _solutionPaths)
    {
        Information("Cleaning {0}", path);
        CleanDirectories(path + "/**/bin");
        CleanDirectories(path + "/**/obj");
        //CleanDirectory(_artifactsDir);
    }
});

void XUnitTest(FilePathCollection testAssemblies, XUnit2Settings settings = null)
{
    CreateDirectory("./build");
    
    foreach(var testProject in testAssemblies){
        Verbose(testProject.FullPath);
    }

    settings = settings ?? new XUnit2Settings {
        Parallelism = ParallelismOption.All,
        XmlReport = true,
        NoAppDomain = false,
        OutputDirectory = "./build"};

    XUnit2(testAssemblies, settings);
}

Task("Restore")
    .Description("Restores all the NuGet packages that are used by the specified solution.")
    .Does(() =>
{
    //string rawPackageSources = EnvironmentVariable("PackageSources");
	string rawPackageSources = "https://api.nuget.org/v3/index.json";
    IList<string> packageSources = null;

    if (!string.IsNullOrEmpty(rawPackageSources))
    {
        packageSources = rawPackageSources.Split(';');
    }

    NuGetRestoreSettings nuGetRestoreSettings = new NuGetRestoreSettings {
        ArgumentCustomization = args => args.Append("-forceenglishoutput"),
        Source = packageSources,
    };

    // Restore all NuGet packages.
    foreach(var solution in _solutions)
    {
        Information("Restoring {0}...", solution);
        NuGetRestore(solution, nuGetRestoreSettings);
    }
});

Task("Build")
    .Description("Builds all the different parts of the project.")
    .IsDependentOn("Clean")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in _solutions)
    {
        Information("Building {0}", solution);
        var settings = new MSBuildSettings{
            Verbosity = _verbosity
        };

        settings.SetPlatformTarget(PlatformTarget.MSIL)
                .WithProperty("TreatWarningsAsErrors","false")
                .WithTarget("Build")
                .SetConfiguration(_configuration);

        MSBuild(solution, settings);
    }
});

//Task("Test-Unit")
//    .Description("Runs all unit tests.")
//   .Does(() => {
//      Information("Running unit tests...");
//      var testAssemblies = GetFiles(_unitTestsProjGlob);
//      XUnitTest(testAssemblies);
//  });


RunTarget(_target);