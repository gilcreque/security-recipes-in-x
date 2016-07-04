We'll need these two namespaces for this section, so let's suck them in right now.

```csharp
using System.Security.Cryptography;
using System.IO;
```

Typically, file integrity is verified by using a straight up hash function. Many distributors still use weakened functions like 
MD5 or SHA-1, though more and more are switching to the SHA-2 family. The good news is that .NET provides a common API
for all hash functions, so demonstrating one will be sufficient to demonstrate all of them. We'll use a [picture of 
my cat Zooey][zooey] as our demo file.

Using a trusted external source, we've determined the SHA-256 digest of the image, seen below. The string is a hex representation 
of the raw binary digest&mdash;typically, digests are represented this way, though you may also see them as Base64.

```csharp
const string expectedDigest = "13592018079f2e42318432b4249f66a8e67f38c770be023e6dacac2cac1a201a";
```

The first thing we need to do is grab an instance of our hash function for use. .NET has a nice unified API for
getting a usable instance of any hash function supported by the underlying crypto APIs. The `HashAlgorithm` class
has a constructor function that takes a string and returns an instance of `HashAlgorithm`, the base class for all
hash function implementations in .NET. MSDN has a [nice table][hash table] that gives us a list of supported names 
for `Create`. In this case, we'll pass `"SHA-256"` to match. 

```csharp
var hash = HashAlgorithm.Create("SHA-256");
```

All HashAlgorithm implementations are able to compute hashes of entire streams via the 
`HashAlgorithm!ComputeHash(Stream inputStream)` method. Thus, we can simply open a stream to read the file, and pass
it directly to our `HashAlgorithm` instance. Using the stream overload of `ComputeHash`, we get the computed digest
back directly as a `byte[]`. We'll wrap the stream in a `using` statement so that it's disposed of for us after
we're done with it.

```csharp
byte[] digest;
using (var stream = File.OpenRead("zooey-picture.jpg")) {
    digest = hash.ComputeHash(stream);
}
```

Now we need to get the `byte[]` digest into a hex string that we can use to compare against our known good hash.
To do that, we'll use the [`BitConverter`][bc] class, which has a `ToString` method that will convert byte arrays
to dash-separated hex strings.

```csharp
string digestString = BitConverter.ToString(digest);
```

The default output from ToString looks like this: 

```
"13-59-20-18-07-9F-2E-42-31-84-32-B4-24-9F-66-A8-E6..."
```

and is obviously a little inconvenient to work with. We'll use basic string methods to strip the -'s and
lowercase the output.

```csharp
digestString = digestString.Replace("-", string.Empty).ToLowerInvariant();
```

Now we can compare our computed digest against the known good digest. It's important to note that when
comparing hashes, we want to use what's known as a constant-time comparison. Constant-time comparison means
that regardless of whether the digests are equal or not, the comparison takes the same amount of time. This
means that attackers can't simply feed in files and use timing differences to determine how many bits of
the hash are correct. 

See Coda Hale's [excellent article][coda] about timing attacks for more information. The comparison Coda illustrates
in Python is very standard, and we'll do the same thing here.

```csharp
Console.WriteLine($"Computed digest: {digestString}.");
Console.WriteLine($"Expected digest: {expectedDigest}.");

if (digestString.Length != expectedDigest.Length) {
    Console.WriteLine("Digest lengths don't match, was the correct hash function used?");
} else {
    // We can use XOR here because `char` is convertible to `int`, and `a XOR a` is always 0. Thus,
    // if we OR together all of the XORs, we should end up with 0 if there were no differences.
    var x = 0;
    for (var i = 0; i < digestString.Length; i++) {
        x |= digestString[i] ^ expectedDigest[i]; 
    }

    Console.WriteLine(x == 0 ? "Digests match!" : "Digest mismatch!");
}
```

If you run this code, you should see the output of `"Digests match!"` on your console. Try changing
the hash function or replacing the file to see how the output changes. You can even make small
changes to the file (use a hex editor to change 1 byte, or even 1 bit!) and see how minimal
changes cascade into huge changes to the hash. You can see the complete code in the accompanying
[ScriptCS file][csx]. 

An important thing to note about verifying file integrity using hash functions is that it **only** verifies that
the contents of the file that you have probably (consider collisions!) match the contents that produced the digest
provided by your upstream. It is still possible for the contents to be malicious: if an attacker has compromised
your transport, or has compromised the host server, they can replace both the file and the digest at the same time.
Verifying that the contents are not malicious can only be done by manual inspection, though using asymmetric signatures
and a web of trust (to be discussed in later chapters) can help.

[hash table]: https://msdn.microsoft.com/en-us/library/wet69s13(v=vs.110).aspx
[zooey]: zooey-picture.jpg
[bc]: https://msdn.microsoft.com/en-us/library/system.bitconverter(v=vs.110).aspx
[coda]: https://codahale.com/a-lesson-in-timing-attacks/
[csx]: verifying-file-integrity.csx