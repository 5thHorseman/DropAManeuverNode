# DropAManeuverNode
KSP Mod: Buttons to drop a maneuver node at orbital milestones

# LICENSE:
MIT (To match that of KSPCasher, whose Stock Toolbar code I used)

# ATTRIBUTION:
This project uses code for Stock Toolbar support from KSPCasher https://github.com/armazac/kspcasher
I read a lot of Kerbal Engineer Redux's code (but didn't actually copy anything) for An/Dn calculations. https://github.com/CYBUTEK/KerbalEngineer/

# CHANGELOG:
- 0.2:
  - UI "overhaul" so it's not quite so ugly. More to come.
    - Buttons grouped in 3 sections
    - Window locked in place near Toolbar button (actually, where the mouse was) to simulate it being a menu.
  - An/Dn placement (Thanks DMagic, Kerbal Engineer, and random Reddit dude), both absolute and relative to target.
- 0.1.2: Bugfix. There were multiple toolbar buttons that kept growing, and only the last worked. Fixed.
- 0.1.1: Bugfix. I had some "==" where I should have had "!=" :o
- 0.1: (Current) Initial build.
  - Stock Toolbar support.
  - Buttons to add a maneuver node at certain orbital milestones.
    
# BUGS:
- None known. Find some and tell me about them!
  
# TO DO:
- Save/load settings, if necessary.
- Blizzy's Toolbar support.
- "Modify Current Node" button or mode.
- Detect when things are not available (like no Ap due to escape) and disable the appropriate button.
- Detect when a maneuver node is already present (Within a few seconds?) and disable the appropriate button.
- Better UI
- Basic thrust options like "circularize" or "match tilt" or "kill velocity"
- "Undo" (may be difficult or impossible)
- Automation options, like "place a circularization burn at Ap whenever you leave atmosphere."
