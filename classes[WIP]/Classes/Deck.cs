namespace Quest {
    class Deck{
        // member variables

        private string type;
        private int count;
        private Card[] discard;
        private Card[] deck;


        // member functions
        public Deck(string deckType, int deckCount, Card[] actualDeck, Card[] currentDiscard){
            type = deckType;
            count = deckCount;
            discard = currentDiscard;
            deck = actualDeck;
        }

        public string getType(){
            return this.type;
        }

        public int getCount(){
            return this.getCount;
        }

        public void decCount(){
            count -= 1;
        }

        // public Card drawNext(){}
    }

}

/*
class Deck{
    // member variables

    private string type;
    private int count;
    private Card[] discard;
    private Card[] deck;


    // member functions
    public Deck(string deckType, int deckCount, Card[] actualDeck, Card[] currentDiscard){
        type = deckType;
        count = deckCount;
        discard = currentDiscard;
        deck = actualDeck;
    }

    public string getType(){
        return this.type;
    }

    public int getCount(){
        return this.getCount;
    }

    public void decCount(){
        count -= 1;
    }

    // public Card drawNext(){}
}
*/
