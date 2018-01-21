/* 
class AmourCard : Card {
    // member variables

    private int bid;

    // member funtions

    public AmourCard(string cardType, string cardName, int cardBid){
        type        = cardType;
        name        = cardName;
        bid         = cardBid;
    }

    public int getBid(){
        return this.bid;
    }
}
*/
namespace Quest {
    class AmourCard : Card {
        // member variables

        private int bid;

        // member funtions

        public AmourCard(string cardType, string cardName, int cardBid){
            type        = cardType;
            name        = cardName;
            bid         = cardBid;
        }

        public int getBid(){
            return this.bid;
        }

        public void printCard(){
            System.Console.WritLine("The type is... " + this.type);
            System.Console.WritLine("The name is... " + this.name);
            System.Console.WritLine("The bid is...." + this.bid.ToString());           
        }
    }
}