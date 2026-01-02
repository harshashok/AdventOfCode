original source: [https://adventofcode.com/2025/day/2](https://adventofcode.com/2025/day/2)
## --- Day 2: Gift Shop ---
You get inside and take the elevator to its only other stop: the gift shop. "Thank you for visiting the North Pole!" gleefully exclaims a nearby sign. You aren't sure who is even allowed to visit the North Pole, but you know you can access the lobby through here, and from there you can access the rest of the North Pole base.

As you make your way through the surprisingly extensive selection, one of the clerks recognizes you and asks for your help.

As it turns out, one of the younger Elves was playing on a gift shop computer and managed to add a whole bunch of invalid product IDs to their gift shop database! Surely, it would be no trouble for you to identify the invalid product IDs for them, right?

They've even checked most of the product ID ranges already; they only have a few product ID ranges (your puzzle input) that you'll need to check. For example:

<pre>
<code>11-22,95-115,998-1012,1188511880-1188511890,222220-222224,
1698522-1698528,446443-446449,38593856-38593862,565653-565659,
824824821-824824827,2121212118-2121212124</code>
</pre>

(The ID ranges are wrapped here for legibility; in your input, they appear on a single long line.)

The ranges are separated by commas (<code>,</code>); each range gives its <em>first ID</em> and <em>last ID</em> separated by a dash (<code>-</code>).

Since the young Elf was just doing silly patterns, you can find the <em>invalid IDs</em> by looking for any ID which is made only of some sequence of digits repeated twice. So, <code>55</code> (<code>5</code> twice), <code>6464</code> (<code>64</code> twice), and <code>123123</code> (<code>123</code> twice) would all be invalid IDs.

None of the numbers have leading zeroes; <code>0101</code> isn't an ID at all. (<code>101</code> is a <em>valid</em> ID that you would ignore.)

Your job is to find all of the invalid IDs that appear in the given ranges. In the above example:


 - <code>11-22</code> has two invalid IDs, <code><em>11</em></code> and <code><em>22</em></code>.
 - <code>95-115</code> has one invalid ID, <code><em>99</em></code>.
 - <code>998-1012</code> has one invalid ID, <code><em>1010</em></code>.
 - <code>1188511880-1188511890</code> has one invalid ID, <code><em>1188511885</em></code>.
 - <code>222220-222224</code> has one invalid ID, <code><em>222222</em></code>.
 - <code>1698522-1698528</code> contains no invalid IDs.
 - <code>446443-446449</code> has one invalid ID, <code><em>446446</em></code>.
 - <code>38593856-38593862</code> has one invalid ID, <code><em>38593859</em></code>.
 - The rest of the ranges contain no invalid IDs.

Adding up all the invalid IDs in this example produces <code><em>1227775554</em></code>.

<em>What do you get if you add up all of the invalid IDs?</em>


## --- Part Two ---
The clerk quickly discovers that there are still invalid IDs in the ranges in your list. Maybe the young Elf was doing other silly patterns as well?

Now, an ID is invalid if it is made only of some sequence of digits repeated <em>at least</em> twice. So, <code>12341234</code> (<code>1234</code> two times), <code>123123123</code> (<code>123</code> three times), <code>1212121212</code> (<code>12</code> five times), and <code>1111111</code> (<code>1</code> seven times) are all invalid IDs.

From the same example as before:


 - <code>11-22</code> still has two invalid IDs, <code><em>11</em></code> and <code><em>22</em></code>.
 - <code>95-115</code> now has two invalid IDs, <code><em>99</em></code> and <code><em>111</em></code>.
 - <code>998-1012</code> now has two invalid IDs, <code><em>999</em></code> and <code><em>1010</em></code>.
 - <code>1188511880-1188511890</code> still has one invalid ID, <code><em>1188511885</em></code>.
 - <code>222220-222224</code> still has one invalid ID, <code><em>222222</em></code>.
 - <code>1698522-1698528</code> still contains no invalid IDs.
 - <code>446443-446449</code> still has one invalid ID, <code><em>446446</em></code>.
 - <code>38593856-38593862</code> still has one invalid ID, <code><em>38593859</em></code>.
 - <code>565653-565659</code> now has one invalid ID, <code><em>565656</em></code>.
 - <code>824824821-824824827</code> now has one invalid ID, <code><em>824824824</em></code>.
 - <code>2121212118-2121212124</code> now has one invalid ID, <code><em>2121212121</em></code>.

Adding up all the invalid IDs in this example produces <code><em>4174379265</em></code>.

<em>What do you get if you add up all of the invalid IDs using these new rules?</em>


