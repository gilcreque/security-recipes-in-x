# C&#35;

Security Recipes in C# aims to provide easily digestible and correct implementations of common 
security operations to .NET developers, using primitives built into the .NET framework
whenever possible. This "book" will not teach you how to write your own encryption algorithms,
hashing algorithms, etc., but will teach you how to apply existing crypto primitives in a safe
and correct way. 

## Table of Contents

1. [Hashing][hash]

## Style

The code style used in this book is roughly similar to the Microsoft default style, with some
minor deviations. The style is as follows:

1. Indents are 4 spaces, always. No tabs.
1. Braces go on the next line for namespaces, classes, and method declarations. For everything
   else, braces go on the same line.
1. No spaces between method name and parenthesis.
1. `else`/`catch`/etc. go on the same line as the closing brace for the previous block
1. Use `var` whenever possible. Only provide explicit types if necessary for compilation
   or if it would significantly elucidate the API/some other pattern.

C# 6 features are used whenever possible&mdash;if you need to compile with an older compiler,
you'll have to do the transliteration yourself.

## Structure

1. Each chapter should be composed of sections for different concepts covered in the chapter. Each
section should be presented in two ways:
    1. As a Markdown file, with code in fenced code blocks (with the proper highlighter applied)
    1. As a ScriptCS .csx file that omits the prose. 
    1. This is done so that readers can clone the repo and see output almost immediately. 
1. If a file is needed as part of the procedure, it should be included in the repository, so that
  internet access is not required beyond the initial download.
1. Each section should include a Markdown file that contains a definition list of any terms that
   are used in the section that aren't expected to be things readers already know. See the
   [definition file][def] for the hashing section for an example.

[hash]: ./hashing/readme.md
[def]: ./hashing/definitions.md