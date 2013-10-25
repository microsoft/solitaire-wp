Solitaire Game XNA Example
==========================

This example application demonstrates how to use XNA Game Studio 4.0 on
Windows Phone 7 using 2D Solitaire card textures.

The example has been developed with Microsoft Visual Studio 2010 Express for
Windows Phone and tested to work on Windows Phone 7.


PREREQUISITES
-------------------------------------------------------------------------------

- C# basics
- Development environment 'Microsoft Visual Studio 2010 Express for Windows 
  Phone'


LINKS
-------------------------------------------------------------------------------

Getting Started Guide:
http://create.msdn.com/en-us/home/getting_started

Learn About Windows Phone 7 Development:
http://msdn.microsoft.com/fi-fi/ff380145

App Hub, develop for Windows Phone:
http://create.msdn.com

Game Development:
http://create.msdn.com/en-us/education/gamedevelopment


IMPORTANT FILES/CLASSES
-------------------------------------------------------------------------------

Game1.cs: Main game class derived from Microsoft.Xna.Framework.Game. In this 
class the game content is loaded, game update requests are received, and the 
drawing is handled.

Card.cs: A single Solitaire card

Deck.cs, SourceDeck.cs, and TargetDeck.cs: Decks for the cards


Important classes: 
 * Texture2D
 * Microsoft.Xna.Framework.Game
 * Microsoft.Xna.Framework.Content.ContentManager
 * Microsoft.Xna.Framework.Input.Touch.TouchPanel


KNOWN ISSUES
-------------------------------------------------------------------------------

None.


BUILD & INSTALLATION INSTRUCTIONS
-------------------------------------------------------------------------------

Preparations
~~~~~~~~~~~~

Make sure you have the following installed:
 * Windows 7, may also work on Windows XP
 * Microsoft Visual Studio 2010 Express for Windows Phone
 * The Windows Phone Developer Tools January 2011 Update:
   http://download.microsoft.com/download/6/D/6/6D66958D-891B-4C0E-BC32-2DFC41917B11/WindowsPhoneDeveloperResources_en-US_Patch1.msp
 * Windows Phone Developer Tools Fix:
   http://download.microsoft.com/download/6/D/6/6D66958D-891B-4C0E-BC32-2DFC41917B11/VS10-KB2486994-x86.exe

Build on Microsoft Visual Studio
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

1. Open the SLN file:
   File > Open Project, select the file XNASolitaire.sln.
2. Select the 'Windows Phone 7 Emulator' target.
3. Press F5 to build the project and run it on the Windows Phone Emulator.


Deploy to Windows Phone 7
~~~~~~~~~~~~~~~~~~~~~~~~~

Preparations:
1. Register in the App Hub to get a Windows Live ID:
   http://create.msdn.com/en-us/home/membership
2. Install Zune for Windows Phone 7:
   http://www.zune.net/en-us/products/windowsphone7/default.htm
3. Register your device in your Windows Live account. 
   Select from Windows: Start > Windows Phone Developer Tools > Windows Phone 
   Developer Registration

Deploy:
1. Open the SLN file:
   File > Open Project, select XNASolitaire.sln file.
2. Connect the device to Windows via USB.
3. Select the 'Windows Phone 7 Device' target.
4. Press F5 to build the project and run it on your Windows device.

   
COMPATIBILITY
-------------------------------------------------------------------------------

- Windows Phone 7
- XNA Game Studio 4.0

Tested on: 
- HTC 7 Mozart
- LG Optimus 7 

Developed with:
- Microsoft Visual Studio 2010 Express for Windows Phone


CHANGE HISTORY
-------------------------------------------------------------------------------

1.0 First version