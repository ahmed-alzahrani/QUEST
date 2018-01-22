/* 
class AllyCard : Card{
    // member variables

    private int battlePoints;
    private string special;

    // member functions
    public AllyCard(string cardType, string cardName, int bp, string specialSkill){
        type            = cardType;
        name            = cardName;
        battlePoints    = bp;
        special         = specialSkill;

    }

    public int getBattlePoints(){
        return this.battlePoints;
    }

    public bool hasSpecial(){
        return (this.special != "");
    }

    // static void executeSpecial() {} execute a specific card's given special ability
}
*/
namespace Quest{
   class AllyCard : Card{
        // member variables

        private int battlePoints;
        private string special;

        // member functions
        public AllyCard(string cardType, string cardName, int bp, string specialSkill){
            type            = cardType;
            name            = cardName;
            battlePoints    = bp;
            special         = specialSkill;
        }

        public int getBattlePoints(){
            return this.battlePoints;
        }

        public bool hasSpecial(){
            return (this.special != "");
        }
        // static void executeSpecial() {} execute a specific card's given special ability
    } 
}