/* 
class WeaponCard : Card{
    // member variable
    private int battlePoints;

    // member functions

    public WeaponCard(string cardType, string cardName, int bp) {
        this.type           = cardType;
        this.name           = cardName;
        this.battlePoints   = bp;
    }
    public int getBattlePoints(){
        return this.battlePoints;
    }
}
*/
namespace Quest{
    class WeaponCard : Card{
        // member variable
        private int battlePoints;

        // member functions

        public WeaponCard(string cardType, string cardName, int bp) {
            this.type           = cardType;
            this.name           = cardName;
            this.battlePoints   = bp;
        }
        public int getBattlePoints(){
            return this.battlePoints;
        }
    }
}