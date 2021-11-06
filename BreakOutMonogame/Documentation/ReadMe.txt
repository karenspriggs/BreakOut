Things I changed:
- added respawning of blocks when the level is completed 
- added blocks that if you break them you die

Structural 
- removed hardcoded variables and instead based values off of things like viewport height/width
and variables to handle offset
- moved game journo bool to the ball class so it can be toggled by the paddle controller with
the x key
- made ball use a scoremanager to update the score and the lives, and then added it to the 
block manager to handle the level count
- scoremanager no longer has static properties or methods for the above thing to work well
- the thing for making a block in the block manager is now its own method so i can make 
death blocks but also separation of concern
- created death block class that inherited from monogameblock (not block so it could work
with blockmanager)
- created game ending class for win/loss

State Changes:
- None made 

Maintainability: 
- I think I made it more sustainable :/