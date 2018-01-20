/*
class FoeCard : Card {
    // member variables
    private int minBP;
    private int maxBP;

    // member functions

    public FoeCard(string cardType, string cardName, int min, int max) {
        type        = cardType;
        name        = cardName;
        minBP       = min;
        maxBP       = max;
    }

    public int getMinBP(){
        return this.minBP;
    }

    public int getMaxBP(){
        return this.maxBP;
    }
}
*/
namespace Quest{
    class FoeCard : Card {
        // member variables
        private int minBP;
        private int maxBP;

        // member functions

        public FoeCard(string cardType, string cardName, int min, int max) {
            type        = cardType;
            name        = cardName;
            minBP       = min;
            maxBP       = max;
        }

        public int getMinBP(){
            return this.minBP;
        }

        public int getMaxBP(){
            return this.maxBP;
        }
    }
}