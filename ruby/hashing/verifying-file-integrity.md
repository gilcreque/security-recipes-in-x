We'll need to use Ruby's `Digest` for this section, so let's suck that in right now.

```ruby
require 'digest'
```

Typically, file integrity is verified by using a straight up hash function. Many distributors still use weakened functions like
MD5 or SHA-1, though more and more are switching to the SHA-2 family. The good news is that Ruby provides a common API
for all hash functions, so demonstrating one will be sufficient to demonstrate all of them. We'll use a [picture of
my cat Zooey][zooey] as our demo file.

Using a trusted external source, we've determined the SHA-256 digest of the image, seen below. The string is a hex representation
of the raw binary digest&mdash;typically, digests are represented this way, though you may also see them as Base64.

```ruby
expected_digest = "13592018079f2e42318432b4249f66a8e67f38c770be023e6dacac2cac1a201a"
```

The obvious way to compute the SHA-256 hash is to use `Digest::SHA256.hexdigest`.
This will compute the hash for a given string.

```ruby
file = File.read('zooey-picture.jpg')
digest_string = Digest::SHA256.hexdigest(file)
```

This works just fine, but we're reading the entire file into memory all at once
to compute our hash. This could especially be a problem if the file is coming
from an unknown place, such as a user upload. Ruby also provides a method that
allows us to compute the hash on the file without first reading it ourselves.

```ruby
file = Digest::SHA256.file('zooey-picture.jpg')
digest_string = file.hexdigest
```

Internally this method reads our file in fixed-length chunks and updates the
digest incrementally.

Now we can compare our computed digest against the known good digest. It's important to note that when
comparing hashes, we want to use what's known as a constant-time comparison. Constant-time comparison means
that regardless of whether the digests are equal or not, the comparison takes the same amount of time. This
means that attackers can't simply feed in files and use timing differences to determine how many bits of
the hash are correct.

See Coda Hale's [excellent article][coda] about timing attacks for more information. The comparison Coda illustrates
in Python is very standard, and we'll do the same thing here.

```ruby
puts "Computed digest: #{digest_string}."
puts "Expected digest: #{expected_digest}."

if digest_string.bytesize != expected_digest.bytesize
  puts "Digest lengths don't match, was the correct hash function used?"
else
  # We can use XOR here because `char` is convertible to `int`, and `a XOR a` is always 0. Thus,
  # if we OR together all of the XORs, we should end up with 0 if there were no differences.

  x = 0
  digest_bytes = digest_string.unpack("C#{digest_string.length}")

  expected_digest.each_byte do |byte|
    x |= byte ^ digest_bytes.shift
  end

  puts x == 0 ? "Digests match!" : "Digest mismatch!"
end
```

If you run this code, you should see the output of `"Digests match!"` on your console. Try changing
the hash function or replacing the file to see how the output changes. You can even make small
changes to the file (use a hex editor to change 1 byte, or even 1 bit!) and see how minimal
changes cascade into huge changes to the hash. You can see the complete code in the accompanying
[Ruby file][rb].

An important thing to note about verifying file integrity using hash functions is that it **only** verifies that
the contents of the file that you have probably (consider collisions!) match the contents that produced the digest
provided by your upstream. It is still possible for the contents to be malicious: if an attacker has compromised
your transport, or has compromised the host server, they can replace both the file and the digest at the same time.
Verifying that the contents are not malicious can only be done by manual inspection, though using asymmetric signatures
and a web of trust (to be discussed in later chapters) can help.

[ruby digest]: http://ruby-doc.org/stdlib-2.2.0/libdoc/digest/rdoc/Digest.html
[zooey]: zooey-picture.jpg
[coda]: https://codahale.com/a-lesson-in-timing-attacks/
[rb]: verifying-file-integrity.rb
