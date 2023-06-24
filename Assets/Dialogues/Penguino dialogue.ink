EXTERNAL EnterFight()
// You can use "~ EnterFight()" to make the dialogue start a fight!

-> START

== START ==
// You can start writing here!
Hello friend!
+ [Talk.]
    -> TALK
+ [Fight.] # Ed
    Here we go!
    ~ EnterFight()
    -> END
+ [Leave.]
    Practice self care!
    -> END

== TALK ==
You're a great friend!
I loved you!
...
-> START

// Remember to keep choices to 3 at a time!

-> END