using System.Runtime.CompilerServices;

// There's some debate over using this attribute, but since we want to ensure modularity in our project
// we need to keep these types as internal.
[assembly: InternalsVisibleTo("RiverBooks.Books.Tests")]
namespace RiverBooks.Books;

public class AssemblyInfo
{ }
