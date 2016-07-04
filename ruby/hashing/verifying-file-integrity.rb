require 'digest'

expected_digest = '13592018079f2e42318432b4249f66a8e67f38c770be023e6dacac2cac1a201a'

file = Digest::SHA256.file('zooey-picture.jpg')
digest_string = file.hexdigest

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

