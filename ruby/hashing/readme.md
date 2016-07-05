# Hashing

> "Much more than encryption algorithms, one-way hash functions are the workhorses of modern cryptography."
>
> &mdash; <cite>Bruce Schneier</cite>

One-way hash functions are probably the most versatile tools in the security/cryptography arsenal. They can be used
for verifying the integrity of files or messages (as in checksums or HMAC), password verification as in bcrypt, scrypt, 
or PBKDF2, proof of work as in the various cryptocurrencies, and even as sources of entropy for PRNGs. Their strength derives
from the following properties:

    * Computing a digest is quick for any given messages
    * Inverting the function (that is, deriving the message from the digest) is infeasible, except by trying all messages
    * A small change to the message should result in extensive changes to the digest
    * It is infeasible to find collisions where two different messages generate the same digest

As computing power has grown, one-way hashes have had to keep up&mdash;over the years, we've had to devise new
hashing algorithms, as old ones are either mathematically broken, or broken simply by available computing power
catching up. MD5 was designed in 1991 to replace a weakening MD4&mdash;it's been weakened itself since then, with major
collision vulnerabilities appearing throughout the late '00s. Even SHA-1, long-recommended as a replacement for the
weak MD class of algorithms, was found to have weaknesses. Modern usages should stick to the SHA-2 family, which includes
SHA-224, SHA-256, SHA-384, SHA-512, SHA-512/224, and SHA-512/256.

We'll cover examples here in roughly the same order of usages as above&mdash;that means starting with verifying the integrity
of files or messages in the [Verifying File Integrity](verifying-file-integrity.md) section of this chapter. Every section will
link to the next one, or you can use the table of contents here to navigate sections if you want to skip around.

## Table of Contents

1. [Verifying File Integrity](verifying-file-integrity.md)