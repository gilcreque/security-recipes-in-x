using System.Security.Cryptography;
using System.IO;

const string expectedDigest = "13592018079f2e42318432b4249f66a8e67f38c770be023e6dacac2cac1a201a";

var hash = HashAlgorithm.Create("SHA-256");

byte[] digest;
using (var stream = File.OpenRead("zooey-picture.jpg")) {
    digest = hash.ComputeHash(stream);
}

string digestString = BitConverter.ToString(digest);
digestString = digestString.Replace("-", string.Empty).ToLowerInvariant();

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