/* 
class TournamentCard : Card {
    // member variables

    private int shields;

    // member functions

    public TournamentCard(string cardType, string cardName, int cardShields){
        
        type        = cardType;
        name        = cardName; 
        shields     = cardShields;
    }

    public int getShields(){
        return this.shields;
    }
}
*/
namespace Quest{
    class TournamentCard : Card {
        // member variables

        private int shields;

        // member functions

        public TournamentCard(string cardType, string cardName, int cardShields){
        
            type        = cardType;
            name        = cardName; 
            shields     = cardShields;
        }

        public int getShields(){
            return this.shields;
        }
    }
}