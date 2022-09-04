using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class TestProgram: ConsoleProgram<TestRunner>
{
}

public interface ITestRunner
{
    public IEnumerable<Type> GetElementTypes();
    public IEnumerable<Type> GetUnitTypes();
}

public class TestRunner: MyValidatableObject, ITestRunner
{
    private readonly Assembly _assembly;

    public TestRunner(Assembly assembly)
    {
        _assembly = assembly;
    }

    public IEnumerable<Type> GetElementTypes() => _assembly.GetClassTypes().Where(t => t.IsExtendsFrom(typeof(TestingElement)));
    public IEnumerable<Type> GetUnitTypes() => _assembly.GetClassTypes().Where(t => t.IsExtendsFrom(typeof(TestingUnit)));
}