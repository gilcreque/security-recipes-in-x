# Ruby

Security Recipes in Ruby aims to provide easily digestible and correct implementations of common
security operations to Ruby developers, using libraries built into the Ruby distribution
whenever possible. This "book" will not teach you how to write your own encryption algorithms,
hashing algorithms, etc., but will teach you how to apply existing crypto primitives in a safe
and correct way.

## Table of Contents

1. [Hashing][hash]

## Style

The code style used in this book will attempt to follow the conventions popular
in the Ruby community. Github's Ruby style should serve our purposes.

https://github.com/styleguide/ruby

We won't attempt to be portable across all different versions of Ruby.  At the time
of this writing the code here was written using Ruby 2.2.2.

## Structure

1. Each chapter should be composed of sections for different concepts covered in the chapter. Each
section should be presented in two ways:
    1. As a Markdown file, with code in fenced code blocks (with the proper highlighter applied)
    1. As a Ruby .rb file that omits the prose.
    1. This is done so that readers can clone the repo and see output almost immediately.
1. If a file is needed as part of the procedure, it should be included in the repository, so that
  internet access is not required beyond the initial download.
1. Each section should include a Markdown file that contains a definition list of any terms that
   are used in the section that aren't expected to be things readers already know. See the
   [definition file][def] for the hashing section for an example.

[hash]: ./hashing/readme.md
[def]: ./hashing/definitions.md
