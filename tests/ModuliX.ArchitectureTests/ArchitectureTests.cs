using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace ModuliX.ArchitectureTests;




public class ArchitectureTests
{
    private const string DomainNamespace = "ModuliX.Identity.Domain";
    private const string ApplicationNamespace = "ModuliX.Identity.Application";
    private const string InfrastructureNamespace = "ModuliX.Identity.Infrastructure";
    private const string PersistenceNamespace = "ModuliX.Identity.Persistence";


    [Fact]
    public void Domain_Should_Not_Depend_On_Upper_Layers()
    {
        var result = Types.InAssembly(typeof(ModuliX.Identity.Domain.Models.AspNetUser).Assembly)
            .Should()
            .NotHaveDependencyOnAny(new[] { ApplicationNamespace, InfrastructureNamespace, PersistenceNamespace })
            .GetResult();

        result.IsSuccessful.Should().BeTrue("Domain must not depend on Application/Infrastructure/Persistence");
    }
    // [Fact]
    // public void Application_Should_Only_Depend_On_Domain_And_SharedKernel()
    // {
    //     var result = Types.InAssembly(typeof(ModuliX.Identity.API.AssemblyReference).Assembly)
    // .Should()
    // .NotHaveDependencyOn("ModuliX.Identity.Domain")
    // .GetResult();

    //     Assert.True(result.IsSuccessful, "Application layer depends on Infrastructure/Persistence!");
    // }
}

