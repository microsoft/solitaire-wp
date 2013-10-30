Solitaire XNA Example
=====================

This example application demonstrates how to use XNA Game Studio 4.0 on
Windows Phone 7 using 2D Solitaire card textures.

![Screenshot](https://github.com/nokia-developer/solitaire-wp/raw/master/doc/screenshots/xnasolitaire_2.png)

The example has been developed with Microsoft Visual Studio 2012 Express for
Windows Phone and tested to work on Windows Phone 7 and Windows Phone 8.


Building and deploying with Microsoft Visual Studio
-------------------------------------------------------------------------------

1. Open the SLN file: File > Open Project, select the file `XNASolitaire.sln`.
2. Select the target, either emulator or device.
3. Press F5 to build the project and run it on the selected target.


Important files and classes
-------------------------------------------------------------------------------

* `Game1.cs`: Main game class derived from Microsoft.Xna.Framework.Game. In this 
  class the game content is loaded, game update requests are received, and the 
  drawing is handled.
* `Card.cs`: A single Solitaire card.
* `Deck.cs`, `SourceDeck.cs`, and `TargetDeck.cs`: The decks of cards.


Related documentation
-------------------------------------------------------------------------------

Getting Started Guide:
http://create.msdn.com/en-us/home/getting_started

Learn About Windows Phone 7 Development:
http://msdn.microsoft.com/fi-fi/ff380145

App Hub, develop for Windows Phone:
http://create.msdn.com

Game Development:
http://create.msdn.com/en-us/education/gamedevelopment


Version history
-------------------------------------------------------------------------------

* Version 1.0: The first version.
