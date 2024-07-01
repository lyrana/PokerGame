/*
Mission: 
    Create a function that will take in a hand of 5 cards and return the hand's score 
        See https://docs.google.com/document/d/1SeBrrEV51m7IVDi2XLZkwtG8WlPVIVitFIQyBTmjem8/edit for scoring details

Clarifying Questions:
    1. Can I trust that I get 5 cards? Yes
    2. Are all cards non-null and valid cards? Yes
Notes:
    My program will assume that Jack, Queen, King, and Ace are numbers 11, 12, 13 and 14, respectively
*/

namespace ConsoleApp
{
    class PokerProgram
    {
        // This dictionary will be a helper, we can use the dictionary's count intelligently to understand the hand that we have. If the count is 5 we know unfortunately that we don't have any pairs...
        static Dictionary<int, int> myDict = new Dictionary<int, int>();
        static bool IsContinuous = false;
        static Suit handSuit = new Suit();
        //we set to true but will return false if invalidated
        static bool AllSameSuit = true;
        static int[] cardValueArray = new int[5];

        static void Main(string[] args)
        {
            Card[] hand = CreateHandFromArgs(args);
            Console.WriteLine("Your hand's score is " + CalculateHand(hand));
        }

        static int CalculateHand(Card[] hand)
        {       
            UnderstandHand(hand);

            //Now that we have our helpers set up, let's start evaluating from highest scoring to lowest:

            //Royal Flush:
            if(AllSameSuit && IsContinuous && cardValueArray[0] == 10) return 1;

            // Straight Flush:
            if(AllSameSuit && IsContinuous) return 2;

            // Flush:
            if(AllSameSuit) return 5;

            // Straight:
            if(IsContinuous) return 6;

            //Could be Four of a Kind or a Full House, we need to understand more:
            if(myDict.Count == 2) {
                // Four of a Kind:
                if(myDict.ContainsValue(4)){return 3; }
                // Full House:
                else{return 4;}
            }

            //Could be Three of a Kind or Two Pair, we need to understand more:
            if (myDict.Count == 3) {
                // Three of a Kind:
                if(myDict.ContainsValue(3)){return 7; }
                // Two Pair:
                else { return 8; }
            }

            //One pair condition: 
            if (myDict.Count == 4)
            {
                return 9;
            }

            //Sadly we have nada, final case returns 10 for High Card, the worst scoring hand
            return 10;
        }

        static void UnderstandHand(Card[] hand){

            //How can we find out as much info as possible in ONE pass over the hand? Let's create our dictionary and evaluate the SameSuit boolean:

            //Let's store the Suit info of the first card for comparison:
            handSuit = hand[0].Suit;

            for(int i = 0; i < 5; i++){
                // All same suit?
                if (hand[i].Suit == handSuit) {
                    handSuit = hand[i].Suit;
                }
                else { AllSameSuit = false; }
               
                // Creating our dictionary
                if(myDict.TryGetValue(hand[i].Value, out int val)){
                    myDict[hand[i].Value] += 1;
                }
                else
                {
                    myDict.Add(hand[i].Value, 1);
                }

                //Adding values to our value array as we iterate over the cards
                cardValueArray[i] = hand[i].Value;               
            }

            //Still need to sort the array, although we only need to do this step if our dict count is 5, indicating we have 5 unique values!
            if(myDict.Count == 5){
                Array.Sort(cardValueArray);
                //now that we're sorted we can simple check if the last value minus the first value equals 4 to see if we're consecutive! 
                if(cardValueArray[4] - cardValueArray[0] == 4)
                {
                    IsContinuous = true;
                }
            }
        }


        //This function assumes that we received 5 args 'correctly' in this format: "<Card value Card Suite>" i.e. ["1 Spades", "4 Spades", "3 Diamonds", "3 Hearts", "14 Spades"]
        static Card[] CreateHandFromArgs(string[] args)
        {
            Card[] hand = new Card[5];
            for (int i = 0; i <= 4; i++)
            {
                string[] subs = args[i].Split(" ");
                Card card = new Card();
                card.Value = Int32.Parse(subs[0]);
                card.Suit = (Suit)Enum.Parse(typeof(Suit), subs[1]);
                hand[i] = card;

            }
            return hand;
        }

    }
    public enum Suit
    {
        Hearts = 1,
        Diamonds = 2,
        Clubs = 3,
        Spades = 4
    }

    public class Card
    {
        public int Value { get; set; }
        public Suit Suit { get; set; }
    }
}