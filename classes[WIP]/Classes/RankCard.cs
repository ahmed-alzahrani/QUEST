/*
class RankCard : Card {
    // member variables
    private int battlePoints;

    // member functions
    public RankCard(string cardType, string cardName, int bp){
        type            = cardType;
        name            = cardName;
        battlePoints    = bp;
    }

    public int getBattlePoints(){
        return this.battlePoints;
    }
}
*/
namespace Quest{
    class RankCard : Card {
        // member variables
        private int battlePoints;

        // member functions
        public RankCard(string cardType, string cardName, int bp){
            type            = cardType;
            name            = cardName;
            battlePoints    = bp;
        }

        public int getBattlePoints(){
            return this.battlePoints;
        }
    }
}