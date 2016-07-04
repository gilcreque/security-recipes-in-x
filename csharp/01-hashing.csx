// "Much more than encryption algorithms, one-way hash functions are the workhorses of modern cryptography." - Bruce Schneier
//
// A few term definitions to begin with:
//
//    * message: the input data to the hash function
//    * digest: the output value of the function
//    * family: a group of hash functions derived from the same algorithm
// 
// One-way hash functions are probably the most versatile tools in the security/cryptography arsenal. They can be used
// for verifying the integrity of files or messages (as in checksums or HMAC), password verification as in bcrypt, scrypt, 
// or PBKDF2, proof of work as in the various cryptocurrencies, and even as sources of entropy for PRNGs. Their strength derives
// from the following properties:
//
//     * Computing a digest is quick for any given messages
//     * Inverting the function (that is, deriving the message from the digest) is infeasible, except by trying all messages
//     * A small change to the message should result in extensive changes to the digest
//     * It is infeasible to find collisions where two different messages generate the same digest
//
// As computing power has grown, one-way hashes have had to keep up--over the years, we've had to devise new
// hashing algorithms, as old ones are either mathematically broken, or broken simply by available computing power
// catching up. MD5 was designed in 1991 to replace a weakening MD4--it's been weakened itself since then, with major
// collision vulnerabilities appearing throughout the late '00s. Even SHA-1, long-recommended as a replacement for the
// weak MD class of algorithms, was found to have weaknesses. Modern usages should stick to the SHA-2 family, which includes
// SHA-224, SHA-256, SHA-384, SHA-512, SHA-512/224, and SHA-512/256.
//
// We'll cover examples here in roughly the same order of usages as above--that means starting with verifying the integrity
// of files or messages.

// We'll need these two namespaces pretty consistently for the rest of this chapter.
using System.Security.Cryptography;
using System.IO;

// Typically, files are verified by using a straight up hash function. Many distributors still use weakened functions like 
// MD5 or SHA-1, though more and more are switching to the SHA-2 family. The good news is that .NET provides a common API
// for all hash functions, so demonstrating one will be sufficient to demonstrate all of them. We'll use a picture of 
// my cat Zooey as our demo. Using a trusted external source, we've determined the SHA-256 digest of this image
// (01-hashing-zooey-picture.jpg) is "13592018079f2e42318432b4249f66a8e67f38c770be023e6dacac2cac1a201a". This is a hex
// representation of the raw binary digest--typically, digests are represented this way, though you may also see them
// as Base64.

const string expectedDigest = "13592018079f2e42318432b4249f66a8e67f38c770be023e6dacac2cac1a201a";

// The first thing we need to do is grab an instance of our hash function for use. .NET has a nice unified API for
// getting a usable instance of any hash function supported by the underlying crypto APIs. The `HashAlgorithm` class
// has a constructor function that takes a string and returns an instance of `HashAlgorithm`, the base class for all
// hash function implementations in .NET. MSDN has a nice table that gives us a list of supported names for `Create`
// at https://msdn.microsoft.com/en-us/library/wet69s13(v=vs.110).aspx. In this case, we'll pass "SHA-256" to match. 

var hash = HashAlgorithm.Create("SHA-256");

// All HashAlgorithm implementations are able to compute hashes of entire streams via the 
// `HashAlgorithm!ComputeHash(Stream inputStream)` method. Thus, we can simply open a stream to read the file, and pass
// it directly to our `HashAlgorithm` instance. Using the stream overload of `ComputeHash`, we get the computed digest
// back directly as a byte[]. We'll wrap the stream in a `using` statement so that it's disposed of for us after
// we're done with it.

byte[] digest;
using (var stream = File.OpenRead("01-hashing-zooey-picture.jpg")) {
    digest = hash.ComputeHash(stream);
}

// Now we need to get the byte[] digest into a hex string that we can use to compare against our known good hash.
// To do that, we'll use the `BitConverter` class, which has a `ToString` method that will convert byte arrays
// to dash-separated hex strings.

string digestString = BitConverter.ToString(digest);

// The default output from ToString looks like this: "13-59-20-18-07-9F-2E-42-31-84-32-B4-24-9F-66-A8-E6..."
// and is obviously a little inconvenient to work with. We'll use basic string methods to strip the -'s and
// lowercase the output.

digestString = digestString.Replace("-", string.Empty).ToLowerInvariant();

// Now we can compare our computed digest against the known good digest. It's important to note that when
// comparing hashes, we want to use what's known as a constant-time comparison. Constant-time comparison means
// that regardless of whether the digests are equal or not, the comparison takes the same amount of time. This
// means that attackers can't simply feed in files and use timing differences to determine how many bits of
// the hash are correct. See Coda Hale's excellent article about timing attacks at
// https://codahale.com/a-lesson-in-timing-attacks/ for more information. The comparison Coda illustrates
// in Python is very standard, and we'll do more or less the same thing here.

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

// If you run this file, you should see the output of "Digests match!" on your console. Try changing
// the hash function or replacing the file to see how the output changes. You can even make small
// changes to the file (use a hex editor to change 1 byte, or even 1 bit!) and see how minimal
// changes cascade into huge changes to the hash.