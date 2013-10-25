/*
 * Copyright (c) 2011 Nokia Corporation.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace XNASolitaire
{
    /// <summary>
    /// Main XNA Framework class Game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Cap between cards on top of each other
        public static int CARD_CAP = 30;

        // How much inflate deck touch area
        public static int DECK_TOUCH_ADDITION = CARD_CAP / 3;

        // List of cards
        List<Card> m_allCards = new List<Card>();

        // User Deck
        Deck m_deck = new Deck(10);
        Deck m_deck2 = new Deck(20);
        Deck m_deck3 = new Deck(30);
        Deck m_deck4 = new Deck(40);
        Deck m_deck5 = new Deck(50);
        Deck m_deck6 = new Deck(60);
        Deck m_deck7 = new Deck(70);

        // Target Decks
        TargetDeck m_targetdeck = new TargetDeck(100);
        TargetDeck m_targetdeck2 = new TargetDeck(200);
        TargetDeck m_targetdeck3 = new TargetDeck(300);
        TargetDeck m_targetdeck4 = new TargetDeck(400);
        
        // Source Decks
        SourceDeck m_sourceDeck = new SourceDeck(1000);
        WasteDeck m_wasteDeck = new WasteDeck(2000);

        // List of all decks
        List<Deck> m_deckList = new List<Deck>();

        // For deserialize (read) deck data to
        SolitaireDataList m_dataList = null;

        Random m_random = new Random();

        // Active card that is under user moving
        Card p_activeCard;

        // Z order counter
        static int m_rootZ = 0;

        // Background texture
        Texture2D m_background;

        // This view rectangle
        Rectangle m_rect;

        bool m_fromTombstoned = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Listening application tombstoning
            PhoneApplicationService.Current.Activated += new EventHandler<ActivatedEventArgs>(GameActivated);
            PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(GameDeactivated);
        }

        private int RandomNumber(int min, int max)
        {
            return m_random.Next(min, max);
        }

        /// <summary>
        /// Occurs when the game deactivated and tombstoned
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GameDeactivated(object sender, DeactivatedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("GameDeactivated");

            // Create data to serialize
            SolitaireDataList dataList = new SolitaireDataList();
            dataList.AddDeckData(m_deckList);

            // Serialize
            using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                isolatedStorageFile.DeleteFile("solitairexna.dat");

                using (IsolatedStorageFileStream fileStream
                    = isolatedStorageFile.CreateFile("solitairexna.dat"))
                {
                    XmlSerializer xmls = new XmlSerializer(typeof(SolitaireDataList));
                    xmls.Serialize(fileStream, dataList);
                    System.Diagnostics.Debug.WriteLine("Saved to solitairexna.dat");
                }

            }
        }

        /// <summary>
        /// Occurs when the game activated during return from tombstoned state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GameActivated(object sender, ActivatedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("GameActivated");

            m_dataList = null;

            // Deserialize
            using (IsolatedStorageFile isolatedStorageFile
                = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorageFile.FileExists("solitairexna.dat"))
                {
                    using (IsolatedStorageFileStream fileStream
                        = isolatedStorageFile.OpenFile("solitairexna.dat", FileMode.Open))
                    {
                        XmlSerializer xmls = new XmlSerializer(typeof(SolitaireDataList));
                        m_dataList = (SolitaireDataList)xmls.Deserialize(fileStream);
                        System.Diagnostics.Debug.WriteLine("Load from solitairexna.dat");
                        System.Diagnostics.Debug.WriteLine(m_dataList.m_list.Count());

                        m_dataList.ReadDeckDataToList(ref m_deckList, ref m_allCards);
                    }
                }
            }

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("Initialize");

            if (PhoneApplicationService.Current.StartupMode == StartupMode.Activate)
            {
                System.Diagnostics.Debug.WriteLine("StartupMode.Activate");
                m_fromTombstoned = true;
            }
            else if (PhoneApplicationService.Current.StartupMode == StartupMode.Launch)
            {
                System.Diagnostics.Debug.WriteLine("StartupMode.Launch");
            }


            p_activeCard = null;
            m_sourceDeck.p_wasteDeck = m_wasteDeck;

            // Screen rectangle
            m_rect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // Create all decks
            int cap = CARD_CAP;
            m_deck.Position = new Point(cap, 180);
            m_deckList.Add(m_deck);
            m_deck2.Position = new Point(cap * 2 + Card.CARD_WIDTH, 180);
            m_deckList.Add(m_deck2);
            m_deck3.Position = new Point(cap * 3 + Card.CARD_WIDTH * 2, 180);
            m_deckList.Add(m_deck3);
            m_deck4.Position = new Point(cap * 4 + Card.CARD_WIDTH * 3, 180);
            m_deckList.Add(m_deck4);
            m_deck5.Position = new Point(cap * 5 + Card.CARD_WIDTH * 4, 180);
            m_deckList.Add(m_deck5);
            m_deck6.Position = new Point(cap * 6 + Card.CARD_WIDTH * 5, 180);
            m_deckList.Add(m_deck6);
            m_deck7.Position = new Point(cap * 7 + Card.CARD_WIDTH * 6, 180);
            m_deckList.Add(m_deck7);

            m_targetdeck.Position = new Point(m_deck4.Position.X, cap);
            m_deckList.Add(m_targetdeck);
            m_targetdeck2.Position = new Point(m_deck5.Position.X, cap);
            m_deckList.Add(m_targetdeck2);
            m_targetdeck3.Position = new Point(m_deck6.Position.X, cap);
            m_deckList.Add(m_targetdeck3);
            m_targetdeck4.Position = new Point(m_deck7.Position.X, cap);
            m_deckList.Add(m_targetdeck4);

            m_sourceDeck.Position = new Point(m_deck.Position.X, cap);
            m_deckList.Add(m_sourceDeck);
            m_wasteDeck.Position = new Point(m_deck.Position.X + Card.CARD_WIDTH + cap, cap);
            m_deckList.Add(m_wasteDeck);

            base.Initialize();
        }

        /// <summary>
        /// Gets random card from the card list. Removes card from the list
        /// </summary>
        /// <returns></returns>
        private Card GetRandomCard()
        {
            int max = m_allCards.Count()-1;
            int random = RandomNumber(0, max);

            Card randCard = m_allCards[random];
            m_allCards.RemoveAt(random);
            return randCard;
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            System.Diagnostics.Debug.WriteLine("LoadContent");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            m_background = this.Content.Load<Texture2D>("background");

            // Create all cards
            for (int i = 1; i<14 ; i++)
                m_allCards.Add(new Card(Card.CardLandEnum.EClubs, i, nextZ(), this.Content));

            for (int i = 1; i < 14; i++)
                m_allCards.Add(new Card(Card.CardLandEnum.EDiamond, i, nextZ(), this.Content));

            for (int i = 1; i < 14; i++)
                m_allCards.Add(new Card(Card.CardLandEnum.ESpade, i, nextZ(), this.Content));

            for (int i = 1; i < 14; i++)
                m_allCards.Add(new Card(Card.CardLandEnum.EHeart, i, nextZ(), this.Content));


            m_deck.LoadBackground(this.Content);
            m_deck2.LoadBackground(this.Content);
            m_deck3.LoadBackground(this.Content);
            m_deck4.LoadBackground(this.Content);
            m_deck5.LoadBackground(this.Content);
            m_deck6.LoadBackground(this.Content);
            m_deck7.LoadBackground(this.Content);
            m_targetdeck.LoadBackground(this.Content);
            m_targetdeck2.LoadBackground(this.Content);
            m_targetdeck3.LoadBackground(this.Content);
            m_targetdeck4.LoadBackground(this.Content);
            m_sourceDeck.LoadBackground(this.Content);
            m_wasteDeck.LoadBackground(this.Content);

            if (!m_fromTombstoned)
            {
                // Create random decks
                Card tmpC = null;
                // Deck 1
                tmpC = GetRandomCard();
                tmpC.setTurned(true);
                m_deck.AddCard(tmpC);

                // Deck 2
                m_deck2.AddCard(GetRandomCard());
                tmpC = GetRandomCard();
                tmpC.setTurned(true);
                m_deck2.AddCard(tmpC);

                // Deck 3
                m_deck3.AddCard(GetRandomCard());
                m_deck3.AddCard(GetRandomCard());
                tmpC = GetRandomCard();
                tmpC.setTurned(true);
                m_deck3.AddCard(tmpC);

                // Deck 4
                m_deck4.AddCard(GetRandomCard());
                m_deck4.AddCard(GetRandomCard());
                m_deck4.AddCard(GetRandomCard());
                tmpC = GetRandomCard();
                tmpC.setTurned(true);
                m_deck4.AddCard(tmpC);

                // Deck 5
                m_deck5.AddCard(GetRandomCard());
                m_deck5.AddCard(GetRandomCard());
                m_deck5.AddCard(GetRandomCard());
                m_deck5.AddCard(GetRandomCard());
                tmpC = GetRandomCard();
                tmpC.setTurned(true);
                m_deck5.AddCard(tmpC);

                // Deck 6
                m_deck6.AddCard(GetRandomCard());
                m_deck6.AddCard(GetRandomCard());
                m_deck6.AddCard(GetRandomCard());
                m_deck6.AddCard(GetRandomCard());
                m_deck6.AddCard(GetRandomCard());
                tmpC = GetRandomCard();
                tmpC.setTurned(true);
                m_deck6.AddCard(tmpC);

                // Deck 7
                m_deck7.AddCard(GetRandomCard());
                m_deck7.AddCard(GetRandomCard());
                m_deck7.AddCard(GetRandomCard());
                m_deck7.AddCard(GetRandomCard());
                m_deck7.AddCard(GetRandomCard());
                m_deck7.AddCard(GetRandomCard());
                tmpC = GetRandomCard();
                tmpC.setTurned(true);
                m_deck7.AddCard(tmpC);

                // Source decks
                for (int i = 0; i < m_allCards.Count(); i++)
                {
                    m_sourceDeck.AddCard(GetRandomCard());
                    i--;
                }
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public static int nextZ()
        {
            return m_rootZ++;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Handle user touch to cards
            handleTouch();

            base.Update(gameTime);
        }

        /// <summary>
        /// Handle user touch events: Pressed, Moved, Released
        /// </summary>
        private void handleTouch()
        {
            // Handle all touch here
            TouchCollection touchCollection = TouchPanel.GetState();

            if (touchCollection.Count() > 0)
            {
                TouchLocation tl = touchCollection.First();

                if (tl.State == TouchLocationState.Pressed)
                {
                    // Handle deck touch and find active card
                    Card ret = null;
                    for (int di = 0; di < m_deckList.Count; di++)
                    {
                        ret = m_deckList[di].HandleTouch(tl);
                        if (ret != null)
                            break;
                    }
                    // Accept to select cards?
                    if (ret != null && ret.OwnerDeck.Type() != Deck.DeckType.ESourceDeck)
                    {
                        // Turn card and activate
                        if (!ret.IsTurned())
                        {
                            if (ret.ParentCard == null)
                            {
                                ret.setTurned(true);
                                p_activeCard = ret;
                            }
                        }
                        // Car is turned
                        // Set active card under move
                        else
                        {
                            p_activeCard = ret;
                        }
                    }

                }
                else if (tl.State == TouchLocationState.Moved)
                {
                    // If active card, move it
                    if (p_activeCard != null)
                    {
                        p_activeCard.handleTouch(tl);
                    }
                }
                else if (tl.State == TouchLocationState.Released)
                {
                    // Where active card was released?
                    if (p_activeCard != null)
                    {
                        // Accept moving cards only from target and source decks
                        Deck fromDeck = p_activeCard.OwnerDeck;
                        if (fromDeck != null && (fromDeck.Type() == Deck.DeckType.EUserDeck ||
                            fromDeck.Type() == Deck.DeckType.EWasteDeck))
                        {
                            // Find deck where card was released, accept target and source decks only
                            Deck toDeck = GetDeckUnderTouch(tl);
                            if (toDeck != null && (toDeck.Type() == Deck.DeckType.EUserDeck ||
                                toDeck.Type() == Deck.DeckType.ETargetDeck))
                            {
                                if (toDeck == fromDeck)
                                {
                                    // cancel move
                                    p_activeCard.cancelMove();
                                }
                                else
                                {
                                    // Check is this card move acceptable
                                    if (isAcceptedMove(p_activeCard, toDeck))
                                    {
                                        // Accept move
                                        fromDeck.RemoveCard(p_activeCard);
                                        toDeck.AddCard(p_activeCard);
                                    }
                                    else
                                    {
                                        // Cancel move
                                        p_activeCard.cancelMove();
                                    }
                                }
                            }
                            else
                            {
                                // Trying to move card between not acceptable decks
                                p_activeCard.cancelMove();
                            }
                        }
                        else
                        {
                            // Trying to move card between not acceptable decks
                            p_activeCard.cancelMove();
                        }
                        // Reset active card, no moving ongoing
                        p_activeCard = null;
                    }


                    int count = 0;
                    for (int i = 0; i < m_deckList.Count(); i++)
                    {
                        count += m_deckList[i].CardCount();
                    }
                }
            }
        }

        /// <summary>
        /// Solitaire game logic -check
        /// </summary>
        /// <param name="c"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private bool isAcceptedMove(Card c, Deck to)
        {
            // Accept card moves only to type 1 and 2 decks
            if (to.Type() != Deck.DeckType.EUserDeck && to.Type() != Deck.DeckType.ETargetDeck)
                return false;

            if (to.CardCount() > 0)
            {
                // Moving top of card
                Card toOnCard = to.GetLast();

                // Moving cards between card decks
                if (toOnCard.OwnerDeck.Type() == Deck.DeckType.EUserDeck)
                {
                    // Card decks must differ
                    if (c.OwnerDeck == toOnCard.OwnerDeck)
                        return false;
                    // Card can be top of one step greater card and different color
                    // Card can not be same color
                    if (c.CardLand() == toOnCard.CardLand() || toOnCard.CardId() != c.CardId() + 1 || c.IsBlack() == toOnCard.IsBlack())
                        return false;
                }
                else if (toOnCard.OwnerDeck.Type() == Deck.DeckType.ETargetDeck)
                {
                    // Cards must be in ascending order and same suite in 2 target deck
                    if (toOnCard.CardId() + 1 != c.CardId() || toOnCard.CardLand() != c.CardLand()) 
                        return false;
                }
            }
            else
            {
                // Moving top of empty deck

                // If there is no cards in the deck, then the first one must be King card in source decks 1
                if (to.CardCount() == 0 && c.CardId() != 13 && to.Type() == Deck.DeckType.EUserDeck)
                    return false;

                // Ace card must be the first card in foundation
                if (to.Type() == Deck.DeckType.ETargetDeck && to.CardCount() == 0 && c.CardId() != 1)
                    return false;

            }
            return true;
        }

        /// <summary>
        /// Returns the deck under touch location
        /// </summary>
        /// <param name="tl"></param>
        /// <returns></returns>
        private Deck GetDeckUnderTouch(TouchLocation tl)
        {
            Deck ret = null;
            // What deck is under touch?
            for (int di = 0; di < m_deckList.Count; di++)
            {
                Deck d = m_deckList[di];
                if (d.IsInTouch(tl))
                {
                    ret = d;
                    break;
                }
            }
            return ret;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // Background
            spriteBatch.Draw(m_background, m_rect, Color.White);

            // Decks
            for (int di = 0; di < m_deckList.Count; di++)
            {
                Deck d = m_deckList[di];
                d.Draw(this.spriteBatch);
            }

            // Active card
            if (p_activeCard != null)
            {
                // Draw active card and all its parent cards
                // on top of other cards
                p_activeCard.DrawMeAndParent(this.spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
