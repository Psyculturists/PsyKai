EXTERNAL EnterFight(EnemyName)

-> START

== START == 
Hello, Ed! # Penguino
+ [Hello!]
    -> Talk
+ [I want to fight!]
    ~EnterFight("Penguino") 
    -> END
+ [Bye]
    -> END

== Talk ==
Hello, Señor Penguino. Are you practicing self care? # Ed
Of course! # Penguino
-> END