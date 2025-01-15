using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAL.Analyzers.RequiredAuthAnalyzer;

public static class RoslynExtensions
{
    public static bool IsOrInheritFrom(this INamedTypeSymbol symbol, ITypeSymbol expectedType)
    {
        return SymbolEqualityComparer.Default.Equals(symbol, expectedType) || RoslynExtensions.InheritsFrom(symbol, expectedType);
    }

    private static bool InheritsFrom(INamedTypeSymbol symbol, ITypeSymbol type)
    {
        var baseType = symbol.BaseType;
        while (baseType != null)
        {
            if (SymbolEqualityComparer.Default.Equals(type, baseType))
            {
                return true;
            }

            baseType = baseType.BaseType;
        }

        return false;
    }
}
