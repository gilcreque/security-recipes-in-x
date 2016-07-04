# Security Recipes in &lt;X&gt;

A collection of "how to's" on correctly applying common security primitives in various languages.

# Why?

When I was first getting started programming, reading books like ["Numerical Recipes in C"][numerical]
was how I learned many common algorithms. These days, numeric algorithms are available far and wide,
but information on correctly applying security/cryptography primitives seems hard to come by. Answers
on sites like [Stack Overflow][so] are too often subtly incorrect or incomplete, leading to a growing
body of bad security advice. A collection of **correct** implementations of common security needs that
encourages contribution and covers as many developers as possible seems long overdue.

# Style

`Security Recipes in <X>` aims to use a conversational, literate style, with code interspersed with
commentary, discussing both the algorithms being applied, and common mistakes that are made while
implementing these.

Languages with "notebook" support (either via [Jupyter][jupyter] or via their own implementations)
should use those as the primary method, as they provide a great out-of-the-box experience. Languages with
"scripting" support should use that&mdash;the goal is to minimize the amount of time that it takes to
see usable output after cloning the repository/downloading a particular "chapter"/modifying the code. 

If neither scripting support nor notebooks are available, use the most natural "build" system for 
the language! If possible, make it cross-platform&mdash;we should aim to include rather than 
exclude developers.

# Languages Represented

* [C#][csharp]

# License

Code portions of this project are licensed under the MIT license (see the [LICENSE][license] file).
Non-code portions are licensed under the [CC-BY-NC-SA 4.0][cc] license.

[numerical]: https://amzn.to/29hYNBn
[so]: https://stackoverflow.com
[jupyter]: https://jupyter.org
[license]: LICENSE.md
[cc]: https://creativecommons.org/licenses/by-nc-sa/4.0/
[csharp]: ./csharp/readme.md