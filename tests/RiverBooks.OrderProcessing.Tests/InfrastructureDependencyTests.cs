using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit.Abstractions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace RiverBooks.OrderProcessing.Tests;

public class InfrastructureDependencyTests(ITestOutputHelper testOutputHelper)
{
  private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

  // A set of assemblies we're working in this test file.
  private static readonly Architecture _architecture = new ArchLoader()
    .LoadAssemblies(typeof(OrderProcessing.AssemblyInfo).Assembly)
    .Build();

  [Fact]
  public void DomainTypesShouldNotReferenceInfrastructure()
  {
    var domainTypes = Types().That()
      .ResideInNamespace("RiverBooks.OrderProcessing.Domain.*", useRegularExpressions: true)
      .As("OrderProcessing Domain Types");

    var infrastructureTypes = Types().That()
      .ResideInNamespace("RiverBooks.OrderProcessing.Infrastructure.*", useRegularExpressions: true)
      .As("OrderProcessing Infrastructure Types");

    var rule = domainTypes.Should().NotDependOnAny(infrastructureTypes);
    
    // PrintTypes(domainTypes, infrastructureTypes);

    rule.Check(_architecture);
  }
  
  // TODO: UseCases shouldn't depend on Infrastructure either.
  
  /// <summary>
  /// Used for debugging purposes
  /// </summary>
  private void PrintTypes(GivenTypesConjunctionWithDescription domainTypes,
    GivenTypesConjunctionWithDescription infrastructureTypes)
  {
    // Debugging - Inspect classes and their dependencies
    foreach (var domainClass in domainTypes.GetObjects(_architecture))
    {
      _testOutputHelper.WriteLine($"Domain Type: {domainClass.FullName}");
      foreach (var dependency in domainClass.Dependencies)
      {
        var targetType = dependency.Target;
        if (infrastructureTypes.GetObjects(_architecture).Any(infraClass => infraClass.Equals(targetType)))
        {
          _testOutputHelper.WriteLine($"  Depends on Infrastructure: {targetType.FullName}");
        }
      }
    }

    foreach (var iType in infrastructureTypes.GetObjects(_architecture))
    {
      _testOutputHelper.WriteLine($"Infrastructure Types: {iType.FullName}");
    }
  }
}
