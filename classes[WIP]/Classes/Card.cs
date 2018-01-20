/* 
abstract class Card{
    // member variables
    private string type;
    private string name;


    // member functions

    // 2 param constructor
    public Card(string cardType, string cardName){
        type = cardType;
        name = cardName;
    }

    public string getType(){
        return this.type;
    }

    public string getName(){
        return this.name;
    }
}
*/
namespace Quest{
    abstract class Card{
        // member variables
        private string type;
        private string name;


        // member functions

        // 2 param constructor
        public Card(string cardType, string cardName){
            type = cardType;
            name = cardName;
        }

        public string getType(){
            return this.type;
        }

        public string getName(){
            return this.name;
        }
    }
}