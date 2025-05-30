- 7/10/2024  - Research done on horror games, gameplay mechanics and movement in unity, game 	engine chosen as unity. Ps1 graphics and development techniques will be key in order to 	make an effective horror game with artistic limitations.
- 8/10/24 - Comprehensive articles read, resident evil 7 "document file" used as inspiration for 	game dev techniques. Learned gameplay mechanics such as saferoom effect and fog effect 	which was used in silent hill 1 (helped with atmosphere and helped with performance) 	which will help with adding atmosphere in the game.
- 9/10/24 - Blender chosen as main modelling tool for now, researched videos on modelling 	techniques in order to emulate ps1 graphics and models for characters and first person 	view hand model.
- 11/10/24 - Project plan completed.
- 20/10/24 - Unity project established, git repository set-up, there were problems with 	understading which files and folders should be commited and added to the repository as 	the base 3D game file in Unity is already 1.5gb large. Effective Git-ignore used to add 	and push only files which are important and not unity resources.
- 21/10/24 - First commit made after struggling with git and unity as unity is unfamiliar. A 		base plane setup in the 3D world of the game as testing ground for movement, basic 	keybinds added in the project. 
- 21/10/24 - First person camera setup, player template setup.
- 23/10/24 - Unity project issues with unity6 version, reverted to unity 2022 version of the 	software. Had to restart my project. Diary.md included in the project now.
- 23/10/24 - First person X-axis and Z-axis movement setup.
- 24/10/24 - Mouse camera functionality from the first person perspective setup. Developing 	first map, testing movement around the map. Difficulties learning the intricacies of the 	geometry editing tools.
-01/11/24 - Blender 3D practice with character models.
-03/11/24 - Real life inspiration taken and resources gathered, in particular I have taken pictures of objects that can be used as textures in the game.
-10/11/24 - First model for the game created with the use of real life picture as textures.
- 17/11/24 - Break from development caused by other module assignments and unfortunate accident that lead to me breaking my nose.
- 17/11/24 - First 3D humanoid npc model created in blender, good practice. This will be my first enemy to code in the project.
- 18/11/24 - 3D final map fbx model imported after being modelled in blender, difficulties with texturing as textures would be inside out I fixed this by backface culling and recalculating normals within blender.
- 19/11/24 - Tweaks to the 3D map model.
- 25/11/24 - Functional Mesh colliders added allowing physics interactions on the map.
- 29/11/24 - Navigation mesh added to the game.
- 30/11/24 - Navigation mesh used to develop AI movement, the 3D humanoid model now follows the player around the map.
- 01/12/24 - Functional dummy doors added, asset used as a placeholder.
- 02/12/24 - Functional doors now fully in the game.
- 03/12/24 - Item interactions being developed.
- 05/12/24 - Inventory GUI added in the game.
- 06/12/24 - Functional Inventory added to the game with slots as an array to be filled by the player, the player can now interact with items and put them in his inventory.
- 08/12/24 - Lighting effects added to the map.
- 30/01/25 - Added Player health states code and graphical implementation as welll when player presses key "TAB" the health status is displayed.
- 31/01/25 - Added deviating health status, when hit by an "enemy" type game object the player looses health which then updates the graphic of health status to let the player know.
- 01/02/25 - Doors can now be opened with a key, key item interaction added, on screen message display added.
- 02/02/25 - AI enemy machine state added, only fundamentals work.
- 03/02/25 - Added working flashlight item, the flashlight is tricky to understand as it deals with light in the game engine, the light must help the player but cannot illuminate the environment too realistically as it will break the game emersion.
- 10/02/25 - Added Gun 3D game model that the player can pick up and store in their inventory. A player health system has been added to the game, GUI for the player status is displayed in the inventory, the player is now capable of using medkits found and stored in the inventory to heal themseleves.
- 25/02/25 - Personal medical difficulties have caused me to not be able to work on the game and have significantly halted my progress, I'm looking forward to being able to work on the game again soon.
- 26/02/25 - Reflection day, the game needs few more features to be playable as a video game. Until then I must hold out with user testing until I have 2 or more game mechanics to show to players in order to test them and pick one that seems most suitable to have in a game.
- 27/02/25 - Edited my second GUI visual page for the main menu of the game, the title right now is "HEAL".
- 27/02/25 - Game main menu added with; option, play and quit buttons present, a graphic title screen edited by me is present. 
- 27/02/25 - The buttons are now functional, the player is able to enter the game straight away upon pressing the play button.
- 28/02/25 - Functional gun has been added to the game alon with updated AI state and AI health states, upon picking up the gun model and storing it in players inventory, the player is now capable of pressing number 1 to holster and unholster the gun, gun model appears on the screen now, functionality and raycasting has been added to the gun so that the player can shoot the gun at the enemies and they will lose health and "die". 
- 29/02/25 - Menu SFX and background music added to the game menu with the help of code.
- 10/03/25 - Additional map modelling has ongoing.
- 14/03/25 - A new City map and additional hospital levels designed in blender soon to be added to the game.
- 15/03/25 - AI to player attack damage system added to the game.
- 16/03/25 - New Ai animations added to the game, the AI now have walking, attacking, idle and death animations to better indicate what their current state is.
- 18/03/25 - New dialogue page added, fade effects and a beginning story dialogue added which leads the player to the first level of the game.
- 19/03/25 - Adding an ammunition system for the weapons in the game, experiencing major difficulties to incorporate this system into the existing inventory and item interaction systems, there are limited resources I can look into for this as I have merged multitude of resources when developing the item interaction and inventory systems.
- 22/03/25 - Added final dialogue scene of the game.
- 25/03/25 - Added lighting to the HospitalEnd map and enemy layout fixed.
- 26/03/25 - Navigation mesh issues, rebaked the whole navigation mesh and revised enemy pathfinding.
- 29/03/25 - Added new evirnomnents so that players movement is restricted in the city map, helps with controlling where the player moves during gameplay.
- 30/03/25 - Fixed enemy AI player interaction where the Ai was able to attack the player without being in range.
- 01/04/25 - Major bugs fixed with the inventory, added presistent inventory systems so that the player retains items collected in the previous levels when changing levels
- 02/04/25 - Added new options UI pagae, added a pause screen for the player, added a game over screen when players hitcount is greater than 9
- 04/04/25 - Presistent ammo system added player now retains ammunition from previous levels when changing levels
- 05/04/25 - Inventory system improved and overhauled the player is able to now move items in the inventory and actully manage it, the player is capable of removing items from the inventory.
- 07/04/25 - Credits screen added in the game, menu bugs fixed, additional options page added.
- 09/04/25 - Inventory updated it is now permanent between maps, disabled camera movement and player actions while in the inventory aside from player movement, fixed a bug where the player would shoot when clicking on items in the inventory.
- 10/04/25 - inventory item icons updated, flashlight system now available in all maps.
- 20/04/25 - Footsteps system added for emersion.
- 22/04/25 - Sound system overhaul, music now available in all maps, inventory bug fixes.


