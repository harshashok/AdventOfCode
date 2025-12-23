original source: [https://adventofcode.com/2025/day/1](https://adventofcode.com/2025/day/1)
## --- Day 1: Secret Entrance ---
The Elves have good news and bad news.

The good news is that they've discovered [project management](https://en.wikipedia.org/wiki/Project_management)! This has given them the tools they need to prevent their usual Christmas emergency. For example, they now know that the North Pole decorations need to be finished soon so that other critical tasks can start on time.

The bad news is that they've realized they have a <em>different</em> emergency: according to their resource planning, none of them have any time left to decorate the North Pole!

To save Christmas, the Elves need <em>you</em> to <em>finish decorating the North Pole by December 12th</em>.

Collect stars by solving puzzles.  Two puzzles will be made available on each day; the second puzzle is unlocked when you complete the first.  Each puzzle grants <em>one star</em>. Good luck!

You arrive at the secret entrance to the North Pole base ready to start decorating. Unfortunately, the <em>password</em> seems to have been changed, so you can't get in. A document taped to the wall helpfully explains:

"Due to new security protocols, the password is locked in the safe below. Please see the attached document for the new combination."

The safe has a dial with only an arrow on it; around the dial are the numbers <code>0</code> through <code>99</code> in order. As you turn the dial, it makes a small <em>click</em> noise as it reaches each number.

The attached document (your puzzle input) contains a sequence of <em>rotations</em>, one per line, which tell you how to open the safe. A rotation starts with an <code>L</code> or <code>R</code> which indicates whether the rotation should be to the <em>left</em> (toward lower numbers) or to the <em>right</em> (toward higher numbers). Then, the rotation has a <em>distance</em> value which indicates how many clicks the dial should be rotated in that direction.

So, if the dial were pointing at <code>11</code>, a rotation of <code>R8</code> would cause the dial to point at <code>19</code>. After that, a rotation of <code>L19</code> would cause it to point at <code>0</code>.

Because the dial is a circle, turning the dial <em>left from <code>0</code></em> one click makes it point at <code>99</code>. Similarly, turning the dial <em>right from <code>99</code></em> one click makes it point at <code>0</code>.

So, if the dial were pointing at <code>5</code>, a rotation of <code>L10</code> would cause it to point at <code>95</code>. After that, a rotation of <code>R5</code> could cause it to point at <code>0</code>.

The dial starts by pointing at <code>50</code>.

You could follow the instructions, but your recent required official North Pole secret entrance security training seminar taught you that the safe is actually a decoy. The actual password is <em>the number of times the dial is left pointing at <code>0</code> after any rotation in the sequence</em>.

For example, suppose the attached document contained the following rotations:

<pre>
<code>L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
</code>
</pre>

Following these rotations would cause the dial to move as follows:


 - The dial starts by pointing at <code>50</code>.
 - The dial is rotated <code>L68</code> to point at <code>82</code>.
 - The dial is rotated <code>L30</code> to point at <code>52</code>.
 - The dial is rotated <code>R48</code> to point at <code><em>0</em></code>.
 - The dial is rotated <code>L5</code> to point at <code>95</code>.
 - The dial is rotated <code>R60</code> to point at <code>55</code>.
 - The dial is rotated <code>L55</code> to point at <code><em>0</em></code>.
 - The dial is rotated <code>L1</code> to point at <code>99</code>.
 - The dial is rotated <code>L99</code> to point at <code><em>0</em></code>.
 - The dial is rotated <code>R14</code> to point at <code>14</code>.
 - The dial is rotated <code>L82</code> to point at <code>32</code>.

Because the dial points at <code>0</code> a total of three times during this process, the password in this example is <code><em>3</em></code>.

Analyze the rotations in your attached document. <em>What's the actual password to open the door?</em>


## --- Part Two ---
You're sure that's the right password, but the door won't open. You knock, but nobody answers. You build a snowman while you think.

As you're rolling the snowballs for your snowman, you find another security document that must have fallen into the snow:

"Due to newer security protocols, please use <em>password method 0x434C49434B</em> until further notice."

You remember from the training seminar that "method 0x434C49434B" means you're actually supposed to count the number of times <em>any click</em> causes the dial to point at <code>0</code>, regardless of whether it happens during a rotation or at the end of one.

Following the same rotations as in the above example, the dial points at zero a few extra times during its rotations:


 - The dial starts by pointing at <code>50</code>.
 - The dial is rotated <code>L68</code> to point at <code>82</code>; during this rotation, it points at <code>0</code> <em>once</em>.
 - The dial is rotated <code>L30</code> to point at <code>52</code>.
 - The dial is rotated <code>R48</code> to point at <code><em>0</em></code>.
 - The dial is rotated <code>L5</code> to point at <code>95</code>.
 - The dial is rotated <code>R60</code> to point at <code>55</code>; during this rotation, it points at <code>0</code> <em>once</em>.
 - The dial is rotated <code>L55</code> to point at <code><em>0</em></code>.
 - The dial is rotated <code>L1</code> to point at <code>99</code>.
 - The dial is rotated <code>L99</code> to point at <code><em>0</em></code>.
 - The dial is rotated <code>R14</code> to point at <code>14</code>.
 - The dial is rotated <code>L82</code> to point at <code>32</code>; during this rotation, it points at <code>0</code> <em>once</em>.

In this example, the dial points at <code>0</code> three times at the end of a rotation, plus three more times during a rotation. So, in this example, the new password would be <code><em>6</em></code>.

Be careful: if the dial were pointing at <code>50</code>, a single rotation like <code>R1000</code> would cause the dial to point at <code>0</code> ten times before returning back to <code>50</code>!

Using password method 0x434C49434B, <em>what is the password to open the door?</em>


